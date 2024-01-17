using System.ComponentModel.DataAnnotations;

namespace MedicalRecords.Dto
{
    public class UserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
