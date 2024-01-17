using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalRecords.Model
{
    [Table("Patient")]
    public class Patient
    {
        [Key]
        [Required]
        public int PatientID { get; set; }
        [Required]
        public string FirstName { get; set;}
        [Required]
        public string LastName { get; set;}
        [Required]
        public string MRN { get; set; }
        [Required]
        public string Address { get; set; }
        public int TotalVisits { get; set; } = 0;
        [Required]
        public string ChartID { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        // Required Navigation Property
        public Doctor Doctor { get; set; }

    }
}
