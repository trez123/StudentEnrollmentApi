using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StudentEnrollmentFrontend.Models
{
    public class MenuVM
    {
        [Key]
        public int Id { get; set; }

        public string? Starch { get; set; }

        public string? Beverage { get; set; }

        public string? Meat { get; set; }

        public string? Vegetable { get; set; }


        [Display(Name = "Menu Image")]
        public IFormFile? MenuIdImageFile { get; set; }

        public List<SelectListItem>? MealTypeList { get; set; }

        [Display(Name = "Meal Type")]
        public int SelectedMealTypeId { get; set; }
    }
}
