using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentApi.Models
{
    public class MenuCreateDTO
    {
        public string? Starch { get; set; }

        public string? Beverage { get; set; }

        public string? Meat { get; set; }

        public string? Vegetable { get; set; }

        public int MealTypeId { get; set; }

        public IFormFile? MenuIdImageFile { get; set; }
    }
}
