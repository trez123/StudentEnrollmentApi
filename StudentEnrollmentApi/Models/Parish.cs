using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentApi.Models
{
    public class Parish
    {
        [Key]
        public int Id { get; set; }

        public string? ParishName { get; set; }
    }
}
