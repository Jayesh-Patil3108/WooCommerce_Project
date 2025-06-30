using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Entities
{
    public class ClientWhatsAppSetting
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string PhoneNumberId { get; set; }

        public virtual Client? Client { get; set; }
    }
}
