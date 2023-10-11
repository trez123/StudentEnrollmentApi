using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StudentEnrollmentFrontend.Models
{
    public class StudentVM
    {
        [Key]
        public int Id { get; set; }

        public string? StudentName { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public IFormFile? StudntIdImageFile { get; set; }

        public List<SelectListItem>? ParishList { get; set; }

        public List<SelectListItem>? ProgramList { get; set; }

        public List<SelectListItem>? SizeList { get; set; }

        [Display(Name = "Parish")]
        public int SelectedParishId { get; set; }

        [Display(Name = "Program")]
        public int SelectedProgramId { get; set; }

        [Display(Name = "Shirt Size")]
        public int SelectedSizeId { get; set; }
    }
}
