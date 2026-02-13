using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Infrasture.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task DeleteBasket(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName is required");

            var existingBasket = await _distributedCache.GetStringAsync(userName);

            if (existingBasket == null)
                return; // nothing to delete

            await _distributedCache.RemoveAsync(userName);
        }


        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basketData = await _distributedCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basketData))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basketData);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            if (string.IsNullOrWhiteSpace(cart.UserName))
                throw new ArgumentException("UserName is required");

            // check if basket already exists
            var existingBasket = await _distributedCache.GetStringAsync(cart.UserName);

            if (existingBasket != null)
                throw new InvalidOperationException("Basket already exists");

            var basketJson = JsonSerializer.Serialize(cart);

            await _distributedCache.SetStringAsync(
                cart.UserName,
                basketJson,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

            return cart;
        }

    }
}
