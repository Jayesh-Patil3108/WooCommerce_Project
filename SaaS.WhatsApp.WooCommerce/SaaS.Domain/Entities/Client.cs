using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Entities
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }


        [Required]
        public string CompanyName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }

        public virtual ICollection<ClientWooSetting> WooSettings { get; set; }
        public virtual ICollection<ClientWhatsAppSetting> WhatsAppSettings { get; set; }

    }
}
