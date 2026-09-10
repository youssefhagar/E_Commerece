using E_Commerce_Infrastructure.DataSeeding;
using E_Commerece.Application.Contracts;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Identity;
using E_Commerece.Infrastructure.Data;
using E_Commerece.Infrastructure.DataSeeding;
using E_Commerece.Infrastructure.Repository;
using E_Commerece.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerece.Infrastructure
{
    public static class InfrastructureServiceRegister
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            AddJwtAuthentication(services, configuration);
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });


            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUserStore, UserStore>();
            
            //services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            });
            services.AddSingleton<ICachRepository, CachRepository>();
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // يمكنك تعديل شروط كلمة المرور هنا إن أردت
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
.AddRoles<IdentityRole>()
.AddRoleManager<RoleManager<IdentityRole>>()
//.AddSignInManager()                           
.AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;

        }


        private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var JwtSection = configuration.GetSection(JwtSetting.SectionNamne);
            var jWtSetting = JwtSection.Get<JwtSetting>();// To Create Object
            services.Configure<JwtSetting>(JwtSection);
            services.AddSingleton<IAcessTokenGenerator, JWTAccessTokenGenerator>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jWtSetting.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jWtSetting.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWtSetting.Secert)),

                        RequireAudience = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero,

                    };
                });


        }




    }
}
