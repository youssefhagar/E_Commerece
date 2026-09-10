using E_Commerece.Application.Common.Settings;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Service;
using E_Commerece.Application.Service.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerece.Application
{

    public static class ApplicationServiceRegister
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(
                x => { },
                typeof(ApplicationServiceRegister).Assembly);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.AddSingleton<ICachService, CachService>();

            services.Configure<StripSetting>(configuration.GetSection(StripSetting.SectionName));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    typeof(ApplicationServiceRegister).Assembly);
            });

            return services;
        }
    }
}

