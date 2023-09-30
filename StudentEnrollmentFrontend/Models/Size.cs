using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }

        public string? SizeName { get; set; }
    }
}
