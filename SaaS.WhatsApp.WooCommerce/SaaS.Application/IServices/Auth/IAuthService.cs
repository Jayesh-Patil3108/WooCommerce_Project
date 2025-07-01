using SaaS.Application.Dtos.Auth;

namespace SaaS.Application.IServices.Auth
{
    public interface IAuthService 
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);
    }
}
