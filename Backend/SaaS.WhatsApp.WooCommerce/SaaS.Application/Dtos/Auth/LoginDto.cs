using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Application.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
    }
}
