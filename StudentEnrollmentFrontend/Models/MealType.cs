using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class MealType
    {
        [Key]
        public int Id { get; set; }

        [Display (Name = "Meal Type Name")]
        public string? MealTypeName { get; set; }
    }
}
