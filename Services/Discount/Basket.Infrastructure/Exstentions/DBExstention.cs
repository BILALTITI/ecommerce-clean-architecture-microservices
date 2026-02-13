using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading;

namespace Discount.Infrastructure.Extensions
{
    public static class DBExtension
    {
        public static IHost MigrateDataBase<TContext>(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                ApplyMigration(config, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database migration failed");
            }

            return host;
        }

        private static void ApplyMigration<TContext>(
            IConfiguration configuration,
            ILogger<TContext> logger)
        {
            var retry = 5;

            while (retry > 0)
            {
                try
                {
                    using var connection = new NpgsqlConnection(
                        configuration.GetConnectionString("DiscountDbConnectionString"));

                    connection.Open();

                    connection.Execute(
                        "DROP TABLE IF EXISTS Coupon");

                    connection.Execute(
                        @"CREATE TABLE Coupon(
                            Id SERIAL PRIMARY KEY,
                            ProductName VARCHAR(24) NOT NULL,
                            Description TEXT,
                            Amount INT
                        )");
                                        connection.Execute(
                        @"INSERT INTO Coupon (ProductName, Description, Amount)
                          VALUES (@ProductName, @Description, @Amount)",
                        new
                        {
                            ProductName = "iPhone 15",
                            Description = "Initial discount seed",
                            Amount = 150
                        });

                    logger.LogInformation("Database migration completed successfully");
                    break;
                }
                catch (Exception ex)
                {
                    retry--;
                    logger.LogError(ex, "Error migrating database. Retries left: {Retry}", retry);
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
