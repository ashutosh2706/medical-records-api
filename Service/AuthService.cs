using MedicalRecords.Model;
using MedicalRecords.Model.Auth;
using MedicalRecords.Util;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Service
{
    public class AuthService : IAuthService
    {
        private readonly MyContext _context;
        public AuthService(MyContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyUser(User user)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(p => p.Username == user.Username);
            if (existing != null)
            {
                return existing.Password == Utils.Hash(user.Password);
            }
            return false;
        }
    }
}
