using E_Commerece.Domain.Contract;
using System.Security.Principal;

namespace E_Commerece.API
{
    public static class WebApplicationExtension
    {

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            await seeder.SeedDataAsync();

            var seeder2 = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await seeder2.SeedDataAsync();

            return app;
        }

    }
}
