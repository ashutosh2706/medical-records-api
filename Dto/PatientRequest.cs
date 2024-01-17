using System.ComponentModel.DataAnnotations;

namespace MedicalRecords.Dto
{
    public class PatientRequest
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        public int DoctorID { get; set; }
        [Required]
        public IFormFile Chart { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
