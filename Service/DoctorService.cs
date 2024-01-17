using MedicalRecords.Model;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly MyContext _context;
        public DoctorService(MyContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorByID(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctor(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
