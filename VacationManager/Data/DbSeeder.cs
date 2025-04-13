using Microsoft.AspNetCore.Identity;
using VacationManager.Models;

namespace VacationManager.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "CEO", "TeamLead", "Developer", "Unassigned" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

           
            var ceoUser = await userManager.FindByEmailAsync("ceo@vacation.com");

            if (ceoUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "ceo@vacation.com",
                    Email = "ceo@vacation.com",
                    FirstName = "Admin",
                    LastName = "CEO",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "CEO");
                }
                else
                {
                    throw new Exception("Грешка при създаване на CEO: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
