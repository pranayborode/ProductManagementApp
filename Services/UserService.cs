using Microsoft.IdentityModel.Tokens;
using ProductManagementApp.Models;
using ProductManagementApp.Models.DTO;
using ProductManagementApp.Repository.Interfaces;
using ProductManagementApp.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(
            IUserRepository _userRepository, 
            IConfiguration _configuration)
        {
            this._userRepository = _userRepository;
            this._configuration = _configuration;

        }
        public int AddUser(User user)
        {
           return _userRepository.AddUser(user);
        }

        public int DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public int EditUser(User user)
        {
           return _userRepository.EditUser(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int userId)
        {
           return _userRepository.GetUserById(userId);
        }

        public LoginDTO Login(User user)
        {
            var _user = _userRepository.Login(user);

            if(_user != null)
            {
                var token = GenerateJWTToken(_user);

                return new LoginDTO
                {
                    Token = token,
                    UserId = _user.UserId,
                    UserName = _user.UserName,
                    Email = _user.Email,
                    Role = _user.Role,
                };
            }
            else
            {
                return null;
            }
        }

        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
                );
            var encodeToeken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToeken;
        }
    }
}
