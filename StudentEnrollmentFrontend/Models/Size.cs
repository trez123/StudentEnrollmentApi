using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }

        [Display (Name ="Size Name")]
        public string? SizeName { get; set; }
    }
}
