using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminCategory;
using Final_Project.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            ProductVM productVM = new ProductVM();
            productVM.Products = _context.Products
                .Where(s => !s.IsDeleted)
                .Include(p=>p.Category)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Categories.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Product> pagination = new(productVM.Products, pageCount, page);

            return View(pagination);
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Products.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            var existProduct = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductFeatures)
                .FirstOrDefault(p => p.Id == id);
            if (existProduct == null) return NotFound();
            ViewBag.Category = new SelectList(_context.Categories.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            UpdateProductVM updateProductVM = new UpdateProductVM();
            updateProductVM.Name = existProduct.Name;
            updateProductVM.Color = existProduct.Color;
            updateProductVM.Price = existProduct.Price;
            updateProductVM.SalePrice = existProduct.SalePrice;
            updateProductVM.Description = existProduct.Description;
            updateProductVM.CategoryId = existProduct.CategoryId;
            updateProductVM.HowToUse = existProduct.HowToUse;
            updateProductVM.StockCount = existProduct.StockCount;
            updateProductVM.Volume = existProduct.Volume;
            return View(updateProductVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateProductVM updateProductVM)
        {
            if (!ModelState.IsValid) return View();
            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Name");
            var existProduct = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);
            if (existProduct == null) return NotFound();
            var existProductWithName = _context.Products.Any(p => p.Name.ToLower() == updateProductVM.Name.ToLower() && p.Id != id);

            if (existProductWithName)
            {
                ModelState.AddModelError("", "This product is exist");
                return View();
            }
            if (updateProductVM.Photos == null)
            {
                ModelState.AddModelError("", "Please enter the product photo");
                return View(updateProductVM);
            }
            List<ProductImage> productImages = new List<ProductImage>();


            foreach (var newPhoto in updateProductVM.Photos)
            {
                if (!newPhoto.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("", "Please choose the image file");
                    return View(updateProductVM);
                }
                if (newPhoto.Length / 1024 > 5000)
                {
                    ModelState.AddModelError("", "This photo length is bigger 5MB");
                    return View(updateProductVM);
                }
                var newUrl = Guid.NewGuid().ToString() + newPhoto.FileName;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    newPhoto.CopyTo(file);
                }

                ProductImage productImg = new ProductImage();
                productImg.ImageUrl = newUrl;
                productImg.AddedBy = User.Identity.Name;
                productImages.Add(productImg);
            }
            existProduct.UpdatedTime = DateTime.Now;
            existProduct.Name = updateProductVM.Name;
            existProduct.Price = updateProductVM.Price;
            existProduct.SalePrice = updateProductVM.SalePrice;
            existProduct.Description = updateProductVM.Description;
            existProduct.Color = updateProductVM.Color;
            existProduct.HowToUse = updateProductVM.HowToUse;
            existProduct.StockCount = updateProductVM.StockCount;
            existProduct.ProductImages = productImages;
            existProduct.CategoryId = updateProductVM.CategoryId;
            existProduct.AddedBy = User.Identity.Name;
            existProduct.Color = updateProductVM.Color;
            _context.SaveChanges();
            return RedirectToAction("index");


        }
        public IActionResult Create()
        {
            ViewBag.Catagories = new SelectList(_context.Categories.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateProductVM createProductVM)
        {
            ViewBag.Catagories = new SelectList(_context.Categories.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (_context.Products.Any(p => p.Name.ToLower() == createProductVM.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Name must be unique");
                return View();
            }
            foreach (var image in createProductVM.Images)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("Image", "only image");
                    return View();
                }
                else if (!image.IsLenghSuit(1000))
                {
                    ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                    return View();
                }
            }

            string fileName = Guid.NewGuid().ToString() + createProductVM.Images[0].FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createProductVM.Images[0].CopyTo(stream);
            }
            Product product = new()
            {
                ImageUrl = fileName,
                Name = createProductVM.Name,
                Description = createProductVM.Description,
                Color = createProductVM.Color,
                Price = createProductVM.Price,
                SalePrice = createProductVM.SalePrice,
                ProductCode = createProductVM.ProductCode,
                CategoryId = createProductVM.CategoryId,
                AddedBy = User.Identity.Name,
                StockCount = createProductVM.StockCount,
                HowToUse = createProductVM.HowToUse,
                Volume = createProductVM.Volume
            };

            List<ProductImage> productImages = new List<ProductImage>();
            for (int i = 0; i < createProductVM.Images.Count; i++)
            {
                string ExtrafileName = Guid.NewGuid().ToString() + createProductVM.Images[i].FileName;
                string Extrapath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", ExtrafileName);
                using (FileStream streamExtra = new FileStream(Extrapath, FileMode.Create))
                {
                    createProductVM.Images[i].CopyTo(streamExtra);
                }
                productImages.Add(new ProductImage
                {
                    ImageUrl = ExtrafileName,
                    ProductId = product.Id,
                    AddedBy = User.Identity.Name
                });
            }
            product.ProductImages = productImages;
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}
