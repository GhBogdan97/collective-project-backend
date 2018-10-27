using DatabaseAccess.Data;
using DatabaseAccess.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
                var services = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var userManager = services.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                DbInitializer.Initialize(context, userManager, roleManager);

            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
