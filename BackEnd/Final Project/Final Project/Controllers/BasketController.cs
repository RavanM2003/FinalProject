using Final_Project.DAL;
using Final_Project.Models;
using Final_Project.Services;
using Final_Project.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Final_Project.Controllers
{
    public class BasketController : Controller
    {
        private AppDbContext _context { get; }
        private readonly IBasketService _basketService;
        private readonly UserManager<AppUser> _userManager;
        public BasketController(AppDbContext context, IBasketService basketService, UserManager<AppUser> userManager) //<<bunu arxada CLR yaradir
        {
            _context = context;
            _basketService = basketService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Add(int? id)
        {

            if (id == null) return NotFound();
            var product = _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var products = CheckBasket();

            CheckItemInBasket(products, product.Id);

            Response.Cookies.Append("basketFinal", JsonConvert.SerializeObject(products));
            return RedirectToAction("ShowBasket", "Basket");
        }
        public IActionResult ShowBasket()

        {
            string basket = Request.Cookies["basketFinal"];

            List<BasketVM> products = new();

            if (basket == null) return RedirectToAction("index", "home");

            products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            foreach (var basketProduct in products)
            {
                var dbProduct = _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == basketProduct.Id);
                basketProduct.Name = dbProduct.Name;
                if (dbProduct.SalePrice != null)
                {
                    basketProduct.Price = (double)dbProduct.SalePrice;
                }
                else
                {
                    basketProduct.Price = (double)dbProduct.Price;
                }

                basketProduct.ImgUrl = dbProduct.ImageUrl;

            }

            Response.Cookies.Append("basketFinal", JsonConvert.SerializeObject(products));
            return View(products);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["basketFinal"];
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var deletedProduct = products.FirstOrDefault(p => p.Id == id);
            if (deletedProduct == null) return NotFound();
            products.Remove(deletedProduct);
            Response.Cookies.Append("basketFinal", JsonConvert.SerializeObject(products));
            return RedirectToAction("showbasket");
        }
        public IActionResult Increase(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["basketFinal"];
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var increaseProduct = products.FirstOrDefault(p => p.Id == id);
            if (increaseProduct == null) return NotFound();
            increaseProduct.BasketCount++;
            Response.Cookies.Append("basketFinal", JsonConvert.SerializeObject(products));
            return RedirectToAction("showbasket");


        }
        public IActionResult Decrease(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["basketFinal"];
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var decreaseProduct = products.FirstOrDefault(p => p.Id == id);
            if (decreaseProduct == null) return NotFound();
            decreaseProduct.BasketCount--;
            if (decreaseProduct.BasketCount == 0)
            {
                products.Remove(decreaseProduct);
            }
            Response.Cookies.Append("basketFinal", JsonConvert.SerializeObject(products));
            return RedirectToAction("showbasket");


        }
        private List<BasketVM> CheckBasket()
        {
            List<BasketVM> products;

            string basket = Request.Cookies["basketFinal"];

            if (basket == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            }
            return products;
        }
        private void CheckItemInBasket(List<BasketVM> products, int id)
        {

            var existproduct = products.FirstOrDefault(p => p.Id == id);
            if (existproduct == null)
            {
                BasketVM basketVM = new();
                basketVM.Id = id;
                basketVM.BasketCount = 1;


                products.Add(basketVM);
            }
            else
            {
                existproduct.BasketCount++;
            }
        }
        public async Task<IActionResult> Sale()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            Sale sale = new();

            sale.AppUserId = user.Id;
            var basket = Request.Cookies["basketFinal"];

            if (basket == null) return NotFound();


            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            double total = 0;
            foreach (var product in products)
            {

                SaleProduct saleProduct = new();
                saleProduct.Price = product.Price;
                saleProduct.ProductId = product.Id;
                saleProduct.SaleId = sale.Id;
                total += product.Price * product.BasketCount;

                sale.SaleProducts.Add(saleProduct);



            }
            sale.Total = total;
            sale.CreatedAt = DateTime.Now;
            _context.Sales.Add(sale);
            _context.SaveChanges();
            TempData["Success"] = "Successful Payment";


            return RedirectToAction("showbasket");


        }
    }
}
