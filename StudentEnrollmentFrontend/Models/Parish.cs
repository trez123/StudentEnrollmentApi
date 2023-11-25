using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class Parish
    {
        [Key]
        public int Id { get; set; }
        
        [Display (Name ="Size Name")]
        public string? ParishName { get; set; }
    }
}
