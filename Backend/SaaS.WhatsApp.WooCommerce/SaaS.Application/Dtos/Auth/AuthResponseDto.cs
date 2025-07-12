using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Application.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string? Token { get; set; }
        public string? Role { get; set; }
        public int? ClientId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
