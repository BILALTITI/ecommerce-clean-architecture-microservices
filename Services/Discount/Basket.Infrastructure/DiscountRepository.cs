using Dapper;
using Discount.Core.Entites;
using Discount.Core.Repositories;
 using Microsoft.Extensions.Configuration;
using Npgsql;
 

namespace Discount.Infrastructure
{
    public class DiscountRepository : IdiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration ) {
        
        

            _configuration = configuration;


        }
        public async Task<Coupon> GetDiscount(string productName)
        {
await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:DiscountDbConnectionString"));                
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });


            if (coupon == null)
            {
                return new Coupon { ProductName = "No Discount Available For This Product", Amount = 0, Description = "No Discount Descreption" };
            }

            return coupon;

        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
           
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:DiscountDbConnectionString"));
            var affectedRows = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            return affectedRows > 0;

        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:DiscountDbConnectionString"));


            var affectedRows = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            return affectedRows > 0;


        }




        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:DiscountDbConnectionString"));

            

            var affectedRows = await connection.ExecuteAsync("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affectedRows > 0;


        }
    }
}
