using Microsoft.AspNetCore.Identity;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Persistence
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole { Name = Policies.Admin }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = Policies.Moderator }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = Policies.Owner }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = Policies.Roommate }).Wait();
            }
            if (!userManager.Users.Any())
            {
                var admin = new User
                {
                    Email = "admin@admin.com",
                    UserName = "Admin",
                };
                userManager.CreateAsync(admin, "Admin-12345678900").Wait();
                admin = userManager.FindByEmailAsync(admin.Email).Result;
                userManager.AddToRoleAsync(admin, Policies.Admin).Wait();
                userManager.AddToRoleAsync(admin, Policies.Moderator).Wait();
                userManager.AddToRoleAsync(admin, Policies.Owner).Wait();
                userManager.AddToRoleAsync(admin, Policies.Roommate).Wait();
            }
        }
    }
}