using E_Commerece.API;
using E_Commerece.API.Features.Orders.Payment;
using E_Commerece.Application;
using E_Commerece.Infrastructure;
using ECommerce.API.Endpoints;
using Microsoft.Extensions.FileProviders;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Console.WriteLine("Environment: " + builder.Environment.EnvironmentName);

        Console.WriteLine(
            "Secret: " + builder.Configuration["Strip:Secretkey"]);

        Console.WriteLine(
            "Publishable: " + builder.Configuration["Strip:Publishablekey"]);

        Console.WriteLine(
            "Currency: " + builder.Configuration["Strip:Currency"]);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseCors("Frontend");

        await app.SeedDataAsync();
        app.UseHttpsRedirection();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
        //}
        app.UseHttpsRedirection();
        var filesPath = Path.Combine(builder.Environment.ContentRootPath, "Files");

        Directory.CreateDirectory(filesPath);

        //app.UseStaticFiles(new StaticFileOptions
        //{
        //    FileProvider = new PhysicalFileProvider(filesPath),
        //    RequestPath = "/Files"
        //});
        app.UseStaticFiles();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Minimal API Endpoints
        app.MapPaymentEndpoints();
        app.MapCreateOrderPaymentEndpoint();

        app.Run();
    }
}