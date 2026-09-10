using System.Text.Json;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites;
using E_Commerece.Domain.Entites.Orders;
using E_Commerece.Domain.Entites.Products;
using E_Commerece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Infrastructure.DataSeeding
{

    public class CatalogDataSeeder(StoreDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying pending migrations...");
                    await dbContext.Database.MigrateAsync(cancellationToken);
                    logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("No pending migrations found.");
                }

                var rootPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmptyAsync<ProductBrand, int>(rootPath, "brands.json", cancellationToken);
                await SeedIfEmptyAsync<ProductType, int>(rootPath, "types.json", cancellationToken);
                await SeedIfEmptyAsync<Product, int>(rootPath, "products.json", cancellationToken);
                await SeedIfEmptyAsync<DeliveryMethod, int>(rootPath, "DelivaryMethod.json", cancellationToken);

                int result = await dbContext.SaveChangesAsync(cancellationToken);
                if (result > 0)
                    logger.LogInformation($"{result}records saved to the database.");
                else
                    logger.LogInformation("No new records were added to the database.");
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task SeedIfEmptyAsync<TEntity, TKey>(string rootPath, string fileName, CancellationToken ct = default) where TEntity : BaseEntity<TKey>
        {

            if (await dbContext.Set<TEntity>().AnyAsync(ct))
            {
                logger.LogInformation("{Table} already contains data.",
                    typeof(TEntity).Name);

                return;
            }

            var filePath = Path.Combine(rootPath, fileName);
            //logger.LogInformation("Root Path: {Root}", rootPath);
            //logger.LogInformation("File Path: {File}", filePath);
            //logger.LogInformation("Exists: {Exists}", File.Exists(filePath));

            if (!File.Exists(filePath))
            {
                logger.LogWarning("{File} not found.", fileName);
                return;
            }

            await using var stream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = await JsonSerializer.DeserializeAsync<List<TEntity>>
            (
                stream,
                options,
                ct
            );

            if (data is null || data.Count == 0)
            {
                logger.LogWarning("{File} is empty.", fileName);
                return;
            }

            await dbContext.Set<TEntity>().AddRangeAsync(data, ct);

            await dbContext.SaveChangesAsync(ct);

            logger.LogInformation(
                "{Count} {Entity} inserted.",
                data.Count,
                typeof(TEntity).Name
            );
        }

    }
}

