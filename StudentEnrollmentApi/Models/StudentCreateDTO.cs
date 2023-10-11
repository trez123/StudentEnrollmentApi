using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentApi.Models
{
    public class StudentCreateDTO
    {
        public string? StudentName { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public int ParishId { get; set; }

        public int ProgramId { get; set; }

        public int SizeId { get; set; }

        public IFormFile? StudntIdImageFile { get; set; }
    }
}
