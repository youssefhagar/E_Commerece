using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites;
using E_Commerece.Domain.Entites.Identity;
using E_Commerece.Domain.Entites.Products;
using E_Commerece.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.DataSeeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> logger;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext,
            UserManager<ApplicationUser> userManager
            ,RoleManager<IdentityRole> roleManager,
            ILogger<IdentityDataSeeder> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            this.logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken cancellationToken = default)
        {

            try
            {

                #region Check if There is Any Pending Migrations
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying pending migrations...");
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                    logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("No pending migrations found.");
                }
                #endregion


                if (! await _roleManager.Roles.AnyAsync(cancellationToken))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if(!await _userManager.Users.AnyAsync(cancellationToken))
                {
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Youssef Hagr",
                        UserName = "youssef",
                        Email = "youssef@gmail.com",
                        PhoneNumber = "01214524055",
                    };
                    var res = await _userManager.CreateAsync(admin,"P@ssw0rd");

                    if (res.Succeeded)
                        await _userManager.AddToRoleAsync(admin, "Admin");
                    else
                    {
                        var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                        logger.LogError("Can't seed admin user. Errors: {Errors}", errors);
                    }
                    ;
                }

                
            }
            catch (Exception)
            {

                logger.LogError("Can not Seed Identity"); ;
            }

        }

        
    }
}
