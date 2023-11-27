using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentFrontend.Models
{
    public class MenuImageViewModel
    {
        public virtual Menu? Menu { get; set; }
        public byte[]? ImageBytes { get; set; }
        public string? ContentType { get; set; }
    }
}
