namespace MedicalRecords.Dto
{
    public class PatientResponse
    {
        public string MR_No { get; set; }
        public string Patient_Name { get; set; }
        public string Address { get; set; }
        public int Visits { get; set; }
        public string Chart_ID { get; set; }
        public string Doctor_Name { get; set; }
        public string Department_Name { get; set; }
        public bool Active { get; set; }

    }
}
