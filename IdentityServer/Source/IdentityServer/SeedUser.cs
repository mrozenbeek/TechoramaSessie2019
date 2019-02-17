using IdentityServer.Contexts;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class SeedUser
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                Console.WriteLine("Scope created");
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                Task.Run(() => EnsureSeedData(roleManager, userManager)).GetAwaiter().GetResult();
            }
        }

        private async static Task EnsureSeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            Console.WriteLine("Find AppAdmin");
            if (await roleManager.FindByNameAsync("AppAdmin") == null)
            {
                Console.WriteLine("Add AppAdmin");
                // add admin role
                var role = new IdentityRole { Name = "AppAdmin" };

                await roleManager.CreateAsync(role);
            }
            else
            {
                Console.WriteLine("AppAdmin Already exists");
            }

            if (await userManager.FindByNameAsync("Admin") == null)
            {
                Console.WriteLine("Add Admin");
                // Add test user
                var user = new ApplicationUser() { UserName = "Admin" };

                var result = await userManager.CreateAsync(user, "Admin1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "AppAdmin");
                }
            }
            else
            {
                Console.WriteLine("Admin Already exists");
            }
        }
    }
}
