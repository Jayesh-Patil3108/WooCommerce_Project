using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaS.Application.Dtos.Auth;
using SaaS.Application.IServices.Auth;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SaaS.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IAuthService authService, IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _httpClient = httpClientFactory.CreateClient("SaaSApiClient");
            _logger = logger;
        }

        [BindProperty]
        public LoginDto LoginDto { get; set; } = new LoginDto();

        public string Message { get; set; }
        public bool LoginSuccess { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogWarning("ModelState invalid: {Errors}", Message);
                return Page();
            }

            try
            {
                var response = await _authService.LoginAsync(LoginDto);
                HttpContext.Session.SetString("Token", response.Token);
                HttpContext.Session.SetString("RefreshToken", response.RefreshToken);
                HttpContext.Session.SetString("ClientId", response.ClientId.ToString());
                HttpContext.Session.SetString("ExpiresAt", response.ExpiresAt?.ToString("o"));

                LoginSuccess = true;
                return RedirectToPage("/Orders");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for Email={Email}", LoginDto?.Email);
                Message = ex.Message;
                return Page();
            }
        }
    }
}