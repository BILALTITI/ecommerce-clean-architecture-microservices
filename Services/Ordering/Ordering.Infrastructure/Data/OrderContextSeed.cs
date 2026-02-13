using Microsoft.Extensions.Logging;
using Ordering.Core.Entites;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(
            OrderContext orderContext,
            ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();

                logger.LogInformation("Order database seeded successfully.");
            }
            else
            {
                logger.LogInformation("Order database already contains data.");
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    // User Info
                    UserName = "bilal",
                    FIrstName = "Bilal",
                    LastName = "Altiti",
                    EmailAddress = "bilal@example.com",

                    // Address Info
                    AddressLine = "Amman - Jordan",
                    Country = "Jordan",
                    State = "Amman",
                    ZipCode = "11181",

                    // Payment Info
                    PaymentMethod = "Visa",
                    CardNumber = "4111111111111111", // demo only
                    CardHolderName = "Bilal Altiti",
                    CVV = "123",
                    CardExpiration = DateTime.UtcNow.AddYears(2),

                    // Order Info
                    TotalPrice = 250.00m
                }
            };
        }
    }
}
