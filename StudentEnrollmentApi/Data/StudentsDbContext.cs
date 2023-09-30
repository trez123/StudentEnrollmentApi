using Microsoft.EntityFrameworkCore;
using StudentEnrollmentApi.Models;

namespace StudentEnrollmentApi.Data
{
    public class StudentsDbContext: DbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
