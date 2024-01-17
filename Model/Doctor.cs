using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalRecords.Model
{
    [Table("Doctor")]
    public class Doctor
    {
        [Key]
        [Required]
        public int DoctorID { get; set; }
        [Required]
        public string DoctorName { get; set; }
        [Required]
        public string DeptName { get; set; }
    }
}
