using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentEnrollmentApi.Data;
using StudentEnrollmentApi.Models;

namespace StudentEnrollmentApi.Utilities
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> usermanager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            StudentsDbContext context = serviceProvider.GetRequiredService<StudentsDbContext>();

            await SeedRoles(roleManager);
            await SeedAdminUser(usermanager);
            await SeedEmployeeUser(usermanager);
            await SeedDefaultData(context);
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] {"Admin" , "Chef"};
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
            IdentityUser? employeeUser = await userManager.FindByNameAsync("chef");
            if(employeeUser == null)
            {
                IdentityUser employee1 = new()
                {
                    UserName = "chef",
                    Email = "chef1@gmail.com"
                };

                IdentityResult createEmployee = await userManager.CreateAsync(employee1, "Chef123$");
                if(createEmployee.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee1, "Chef");
                }
            }
        }

        public static async Task SeedDefaultData(StudentsDbContext context)
        {
            Parish stAnn = new()
            {
                ParishName = "St. Ann"
            };

            Parish? parishSearch = await context.Parishes.FirstOrDefaultAsync(x => x.ParishName == stAnn.ParishName);

            if(parishSearch == null)
            {
                context.Parishes.Add(stAnn);
                await context.SaveChangesAsync();
            }

            Parish Trelawney = new()
            {
                ParishName = "Trelawney"
            };

            Parish? trelawneySearch = await context.Parishes.FirstOrDefaultAsync(x => x.ParishName == Trelawney.ParishName);

            if(trelawneySearch == null)
            {
                context.Parishes.Add(Trelawney);
                await context.SaveChangesAsync();
            }

            Course course = new()
            {
                CourseName = "Web Development"
            };

            Course? courseSearch = await context.Courses.FirstOrDefaultAsync(x => x.CourseName == course.CourseName);

            if(courseSearch == null)
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();
            };

            Size? size = new()
            {
                SizeName = "Small"
            };

            Size? sizeSearch = await context.Sizes.FirstOrDefaultAsync(x => x.SizeName == size.SizeName);

            if(sizeSearch == null)
            {
                context.Sizes.Add(size);
                await context.SaveChangesAsync();
            }

            MealType breakfat = new()
            {
                MealTypeName = "Breakfast"
            };

            MealType? breakfastSearch = await context.MealTyes.FirstOrDefaultAsync(x => x.MealTypeName == breakfat.MealTypeName);

            if(breakfastSearch == null)
            {
                context.MealTyes.Add(breakfat);
                await context.SaveChangesAsync();
            }

            MealType lunch = new()
            {
                MealTypeName = "Lunch"
            };

            MealType? lunchSearch = await context.MealTyes.FirstOrDefaultAsync(x => x.MealTypeName == lunch.MealTypeName);

            if(breakfastSearch == null)
            {
                context.MealTyes.Add(lunch);
                await context.SaveChangesAsync();
            }

            MealType dinner = new()
            {
                MealTypeName = "Lunch"
            };

            MealType? dinnerSearch = await context.MealTyes.FirstOrDefaultAsync(x => x.MealTypeName == dinner.MealTypeName);

            if(breakfastSearch == null)
            {
                context.MealTyes.Add(dinner);
                await context.SaveChangesAsync();
            }

        }
    }
}