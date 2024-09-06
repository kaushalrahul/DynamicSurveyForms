using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoImplementation;
using DynamicFormRepos.DynamicFormRepoInterface;
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DynamicFormServices.DynamicFormServiceImplementation
{
    public class LoginService : ILoginService
    {
        private readonly string _validEmail = "test@example.com";
        private readonly string _validPassword = "Password123";
        private readonly IConfiguration _configuration;
        private readonly IFormRepo _formRepo;

        public LoginService(IConfiguration configuration, IFormRepo formRepo)
        {
            _configuration = configuration;
            _formRepo = formRepo;
        }


        public bool ValidateUser(string email, string password)
        {
            return _formRepo.ValidateUserCredentials(email, password);
        }

        public string GenerateJwtToken(string email)
        {
            // Retrieve the user from the repository
            var user = _formRepo.GetUserByEmail(email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Now that 'user' is a valid instance, you can access 'user.Id'
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Using user.Email for clarity
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserID", user.Id.ToString()), // Accessing the Id property of the instance
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

