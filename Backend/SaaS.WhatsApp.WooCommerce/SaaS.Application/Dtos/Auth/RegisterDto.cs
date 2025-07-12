using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Application.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        public string CompanyName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public string? Role { get; set; } = "Client";
    }


    public class RegisterResponseDto
    {
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}