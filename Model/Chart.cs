using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalRecords.Model
{
    [Table("Chart")]
    public class Chart
    {
        [Key]
        [Required]
        public string ChartID { get; set; }
        [Required]
        public string ChartName { get; set; }
    }
}
