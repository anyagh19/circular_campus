using Campus.Services.AuthenticationAPI.Data;
using Campus.Services.AuthenticationAPI.Model.Dtos;
using Campus.Services.AuthenticationAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Campus.Services.AuthenticationAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<bool> CreateCampus(CampusDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }
            var user = await _dbContext.Campuses.FirstOrDefaultAsync(u => u.CampusEmail == request.CampusEmail);
            if (user != null)
            {
                throw new InvalidOperationException("A campus with the same email already exists.");
            }
            var HashedPassword = PasswordHasher(request.CampusPassword);
            var newCampus = new CampusBranch
            {
                CampusName = request.CampusName,
                CampusLocation = request.CampusLocation,
                CampusEmail = request.CampusEmail,
                CampusPhone = request.CampusPhone,
                CampusCode = request.CampusCode,
                CampusPassword = HashedPassword
            };

            await _dbContext.Campuses.AddAsync(newCampus);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<LoginResponseDto> Login(LoginDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            if (request.Identifier.Contains("@"))
            {
                var campus = await _dbContext.Campuses.FirstOrDefaultAsync(c => c.CampusEmail == request.Identifier);
                if (campus == null)
                {
                    throw new InvalidOperationException("Campus not found.");
                }
                if (!VerifyPassword(campus.CampusPassword, request.Password))
                {
                    throw new UnauthorizedAccessException("Invalid password.");
                }
                var accessToken = GenerateAccessToken(campus);
                var refreshToken = GenerateRefreshToken();

                campus.RefreshToken = refreshToken;
                campus.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _dbContext.SaveChangesAsync();

                return new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Role = campus.Role
                };
            }
            else
            {
                return null;
            }
        }

        private string PasswordHasher(string password)
        {
            var hashedPassword = new PasswordHasher<object>().HashPassword(null, password);
            return hashedPassword;
        }

        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var checker = new PasswordHasher<CampusBranch>();
            var result = checker.VerifyHashedPassword(null, hashedPassword, providedPassword);
            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }
            return false;
        }
        private string GenerateAccessToken(CampusBranch campus)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, campus.CampusId.ToString()),
                new Claim(ClaimTypes.Name, campus.CampusName),
                new Claim(ClaimTypes.Role , campus.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler =  new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
