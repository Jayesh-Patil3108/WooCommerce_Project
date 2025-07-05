using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaaS.Application.Dtos.Auth;
using SaaS.Infrastructure.DbContexts;
using System.Security.Claims;
using SaaS.Domain.Entities;
using SaaS.Application.IServices.Auth;

namespace SaaS.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SaaSDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(SaaSDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Clients.AnyAsync(x => x.Email == registerDto.Email))
                throw new Exception("Email already registered");

            var client = new Client
            {
                CompanyName = registerDto.CompanyName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "ClientUser"
            };

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return new RegisterResponseDto
            {
                Email = client.Email,
                Role = client.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == loginDto.Email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, client.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, client.Email),
                    new Claim("clientId", client.ClientId.ToString()),
                    new Claim(ClaimTypes.Role, client.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponseDto
            {
                Token = "Bearer " + tokenString,
                Role = client.Role,
                ClientId = client.ClientId
            };
        }
    }
}
