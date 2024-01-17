using MedicalRecords.Model;

namespace MedicalRecords.Service
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task<Doctor> GetDoctorByID(int id);
        Task AddDoctor(Doctor doctor);
        Task DeleteDoctor(Doctor doctor);
    }
}