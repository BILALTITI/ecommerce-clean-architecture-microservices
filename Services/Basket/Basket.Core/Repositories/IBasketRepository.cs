using Basket.Core.Entities;


namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {

        Task<ShoppingCart> GetBasket(string UserName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart cart);
        Task  DeleteBasket(string UserName);
    }
}
