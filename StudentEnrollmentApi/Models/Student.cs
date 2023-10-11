using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentApi.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public string? StudentName { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? StudentImageFilePath { get; set; } = string.Empty;

        public int ParishId { get; set; }

        public int ProgramId { get; set; }

        public int SizeId { get; set; }

        [ForeignKey("ParishId")]

        public virtual Parish? Parish { get; set; }

        [ForeignKey("ProgramId")]

        public virtual Course? Course { get; set; }

        [ForeignKey("SizeId")]

        public virtual Size? Size { get; set; }
    }
}
