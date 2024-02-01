using JokeJunction.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JokeJunction.DAL
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            InitializeRoles(roleManager);
            InitializeUsers(userManager);
        }

        private static void InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            // Додайте ваші ролі, якщо потрібно
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExists = roleManager.RoleExistsAsync(roleName).Result;

                if (!roleExists)
                {
                    var role = new IdentityRole
                    {
                        Name = roleName
                    };
                    var result = roleManager.CreateAsync(role).Result;
                }
            }
        }

        private static void InitializeUsers(UserManager<ApplicationUser> userManager)
        {
            // Додайте вашого адміністратора, якщо потрібно
            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };

            var adminUserExists = userManager.FindByEmailAsync(adminUser.Email).Result;

            if (adminUserExists == null)
            {
                var result = userManager.CreateAsync(adminUser, "AdminPassword123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }

}
