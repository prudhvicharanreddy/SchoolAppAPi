using Microsoft.AspNetCore.Identity;

namespace SchoolApplication.Models;


    public static class DatabaseSeeder
    {
        public static async Task SeedAdminUser(UserManager<User> userManager)
        {
            var admin = new User
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@school.com",
                UserName = "admin@school.com",
                Role = Role.Admin
            };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, "AdminPassword123!");
            }
        }
    }
