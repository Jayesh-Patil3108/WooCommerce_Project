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
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Generate and store refresh token
            var refreshToken = new RefreshToken
            {
                ClientId = client.ClientId,
                Token = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.Ticks,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                RevokedDate = null
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = "Bearer " + tokenString,
                Role = client.Role,
                ClientId = client.ClientId,
                RefreshToken = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiryDate,
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var StoreToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.RevokedDate == null && rt.ExpiryDate > DateTime.UtcNow).ConfigureAwait(false);
            if (StoreToken == null)
                throw new SecurityTokenException("Invalid or expired refresh token.");

            var client = await _context.Clients.FindAsync(StoreToken.ClientId);
            if (client == null)
                throw new UnauthorizedAccessException("Client not found.");

            // Revoke the old refresh token
            StoreToken.RevokedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            //Generate a new refresh token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
             {
                new Claim(ClaimTypes.Name, client.Email),
                new Claim("clientId", client.ClientId.ToString()),
                new Claim(ClaimTypes.Role, client.Role)
            }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var newToken = tokenHandler.CreateToken(tokenDescriptor);
            var newAccessToken = tokenHandler.WriteToken(newToken);

            var newRefreshToken = new RefreshToken
            {
                ClientId = client.ClientId,
                Token = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.Ticks,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                RevokedDate = null
            };
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = "Bearer " + newAccessToken,
                Role = client.Role,
                ClientId = client.ClientId,
                RefreshToken = newRefreshToken.Token
            };

        }
    }
}
