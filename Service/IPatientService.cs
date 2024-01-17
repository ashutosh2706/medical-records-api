using MedicalRecords.Model;

namespace MedicalRecords.Service
{
    public interface IPatientService
    {
        Task AddPatient(Patient patient);
        Task DeletePatient(Patient patient);
        Task<Patient> GetByPatientID(int id);
        Task<Patient> GetPatientByMRN(string MRN);
        Task<IEnumerable<Patient>> GetPatients();
        Task<IEnumerable<Patient>> GetPatientsByName(string name);
        Task UpdatePatient(Patient patient);
    }
}