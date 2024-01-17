using MedicalRecords.Model.Auth;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Model
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Chart> Charts { get; set; }
        public DbSet<User> Users { get; set; }
        
    }
}
