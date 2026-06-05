
namespace PetFoodTrackerWebApp.AppHost
{
    public class AppHost
    {
        private static async Task Main(string[] args)
        {
            var builder = DistributedApplication.CreateBuilder(args);

            var server = builder.AddProject<Projects.PetFoodTrackerWebApp_Server>("server")
                .WithHttpHealthCheck("/health")
                .WithExternalHttpEndpoints();

            var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
                .WithReference(server)
                .WaitFor(server);

            server.PublishWithContainerFiles(webfrontend, "wwwroot");

            await builder.Build().RunAsync();
        }
    }
}



