using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class MealType
    {
        [Key]
        public int Id { get; set; }

        public string? MealTypeName { get; set; }
    }
}
