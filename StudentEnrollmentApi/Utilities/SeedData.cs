using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StudentEnrollmentApi.Utilities
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> usermanager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await SeedRoles(roleManager);
            await SeedAdminUser(usermanager);
            await SeedEmployeeUser(usermanager);
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] {"Admin" , "Employee"};
            foreach(string role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager)
        {
            IdentityUser? adminUser = await userManager.FindByNameAsync("admin");

            if(adminUser == null)
            {
                IdentityUser admin = new()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com"
                };

                IdentityResult createAdmin = await userManager.CreateAsync(admin, "Admin123$");
                if(createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        public static async Task SeedEmployeeUser(UserManager<IdentityUser> userManager)
        {
            IdentityUser? employeeUser = await userManager.FindByNameAsync("employee1");
            if(employeeUser == null)
            {
                IdentityUser employee1 = new()
                {
                    UserName = "employee1",
                    Email = "employee1@gmail.com"
                };

                IdentityResult createEmployee = await userManager.CreateAsync(employee1, "Employee123$");
                if(createEmployee.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee1, "Employee");
                }
            }
        }
    }
}