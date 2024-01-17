using MedicalRecords.Model;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Service
{

    public class PatientService : IPatientService
    {
        // Constructor Dependency Injection
        private readonly MyContext context;
        public PatientService(MyContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Patient>> GetPatients()
        {
            return await context.Patients.ToListAsync();
        }

        public async Task<Patient> GetByPatientID(int id)
        {
            return await context.Patients.FindAsync(id);
        }

        public async Task AddPatient(Patient patient)
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient patient)
        {
            context.Patients.Update(patient);
            await context.SaveChangesAsync();
        }

        public async Task DeletePatient(Patient patient)
        {
            context.Patients.Remove(patient);
            await context.SaveChangesAsync();
        }

        public async Task<Patient> GetPatientByMRN(string MRN)
        {
            // LINQ Query
            return await context.Patients.FirstOrDefaultAsync(p => p.MRN == MRN);
        }

        public async Task<IEnumerable<Patient>> GetPatientsByName(string name)
        {
            return await context.Patients.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name)).ToListAsync();
        }
    }
}
