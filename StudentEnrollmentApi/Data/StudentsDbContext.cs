using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentApi.Models;
using System.Collections.Generic;

namespace StudentEnrollmentApi.Data
{
    public class StudentsDbContext: IdentityDbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MealType> MealTyes { get; set; }
    }
}
