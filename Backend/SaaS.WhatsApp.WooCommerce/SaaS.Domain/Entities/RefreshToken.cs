using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public DateTime? RevokedDate { get; set; }

        public bool IsRevoked => RevokedDate.HasValue;

        public virtual Client Client { get; set; }
    }
}
