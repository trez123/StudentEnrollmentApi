using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        public string? Starch { get; set; }

        public string? Beverage { get; set; }

        public string? Meat { get; set; }

        public string? Vegetable { get; set; }

        public int MealTypeId { get; set; }

        [ForeignKey("MealTypeId")]
        public virtual MealType? MealType { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? MenuImageFilePath { get; set; } = string.Empty;
    }
}
