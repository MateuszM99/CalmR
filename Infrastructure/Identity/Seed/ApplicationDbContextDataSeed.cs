using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seed
{
    public class ApplicationDbContextDataSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Add roles supported
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // New admin user
            string adminUserName = "mateusz@test.com";
            var adminUser = new ApplicationUser { 
                UserName = adminUserName,
                Email = adminUserName,
                EmailConfirmed = true,
                FirstName = "Mateusz",
                LastName = "Administrator"
            };
            
            // Add new user and their role
            var response = await userManager.CreateAsync(adminUser, "Admin123=123");
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}