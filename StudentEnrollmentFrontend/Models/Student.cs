using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Display (Name ="Student Name")]
        public string? StudentName { get; set; }

        [Display (Name ="Email Address")]
        public string? EmailAddress { get; set; }
        
        [Display (Name ="Phone Number")]
        public string? PhoneNumber { get; set; }

        public int ParishId { get; set; }

        public int ProgramId { get; set; }

        public int SizeId { get; set; }

        public string? studentImageFilePath { get; set; } = string.Empty;
        public virtual Parish? Parish { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Size? Size { get; set; }
    }
}
