using Final_Project.ViewModels.Basket;

namespace Final_Project.Services
{
    public interface IBasketService
    {
        int GetCount();
        void Increase(int id);
        void Decrease(int id);
        void Delete(int id);
        List<BasketVM> GetBasket();
    }
}
