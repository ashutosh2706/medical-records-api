using MedicalRecords.Model.Auth;

namespace MedicalRecords.Service
{
    public interface IAuthService
    {
        Task AddUser(User user);
        Task<bool> VerifyUser(User user);
    }
}