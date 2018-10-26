using DatabaseAccess.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace collective_project_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                //var services = scope.ServiceProvider;
                var services = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
               
                //services.ServiceProvider.GetService<ApplicationDbContext>().Initialize();
                //services.ServiceProvider.GetService<UserManager<ApplicationUser>>().Seed();

                DbInitializer.Initialize(context);

            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
