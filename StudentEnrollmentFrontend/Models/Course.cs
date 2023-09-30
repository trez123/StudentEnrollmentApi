using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string? CourseName { get; set; }
    }
}
