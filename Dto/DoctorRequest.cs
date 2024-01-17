using System.ComponentModel.DataAnnotations;

namespace MedicalRecords.Dto
{
    public class DoctorRequest
    {
        [Required]
        public string DoctorName { get; set; }
        [Required]
        public string DepartmentName { get; set; }
    }
}
