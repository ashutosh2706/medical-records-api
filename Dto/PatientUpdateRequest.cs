using System.ComponentModel.DataAnnotations;

namespace MedicalRecords.Dto
{
    public class PatientUpdateRequest
    {
        [Required]
        public string MRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        [Required]
        public int DoctorID { get; set; }
        public IFormFile Chart { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
