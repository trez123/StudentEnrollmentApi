using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentApi.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
