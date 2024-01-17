using MedicalRecords.Dto;
using MedicalRecords.Model.Auth;
using MedicalRecords.Service;
using MedicalRecords.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MedicalRecords.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        
        [HttpPost]
        [Route("admin/login")]
        public async Task<IActionResult> Login([FromForm] UserRequest request)
        {
            try
            {
                bool authorized = await _authService.VerifyUser(new User(){ Username = request.Username, Password = request.Password });
                if (authorized)
                {
                    var token = GenerateToken();
                    return Ok($"Token: {token}");
                }
                return Unauthorized($"Login Failed: {request.Username}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("admin/register")]
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            try
            {
                User user = new User
                {
                    Username = request.Username,
                    Password = Utils.Hash(request.Password)
                };
                await _authService.AddUser(user);
                return Ok($"{user.Username} was added to admin role");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
