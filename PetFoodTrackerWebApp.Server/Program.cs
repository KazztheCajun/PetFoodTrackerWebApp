using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;

namespace PetFoodTrackerWebApp.Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();

            // Add services to the container.
            builder.Services.AddProblemDetails();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Register your dependencies directly into the Autofac ContainerBuilder
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }


            string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

            var api = app.MapGroup("/api");
            api.MapGet("weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");

            app.MapDefaultEndpoints();

            app.UseFileServer();

            app.Run();
        }
    }
}



record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
