using JustEatIt.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data
{
    public class SeedDataIdentity
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                //Resolve ASP .NET Core Identity with DI help
                UserManager<CustomUser> userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<CustomUser>>();
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                // If not find the admin role, create it                
                IdentityRole adminRole = await roleManager.FindByNameAsync("Administrator");
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("Administrator");
                    var result = await roleManager.CreateAsync(adminRole);
                }

                // If not find the partner role, create it
                IdentityRole partnerRole = await roleManager.FindByNameAsync("Partner");
                if (partnerRole == null)
                {
                    partnerRole = new IdentityRole("Partner");
                    await roleManager.CreateAsync(partnerRole);
                }

                // If not find the customer role, create it
                IdentityRole customerRole = await roleManager.FindByNameAsync("Customer");
                if (customerRole == null)
                {
                    customerRole = new IdentityRole("Customer");
                    await roleManager.CreateAsync(customerRole);
                }

                // If not find the admin user, create it
                CustomUser admUser = await userManager.FindByNameAsync("csjusteatit@gmail.com");
                if (admUser == null)
                {
                    admUser = new CustomUser {
                        UserName = "csjusteatit@gmail.com",
                        Email = "csjusteatit@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(admUser, "justEatIt5$");
                    admUser = await userManager.FindByNameAsync("csjusteatit@gmail.com");

                    // Add the roles to admin
                    await userManager.AddToRoleAsync(admUser, "Administrator");
                }


            }
        }
    }
}
