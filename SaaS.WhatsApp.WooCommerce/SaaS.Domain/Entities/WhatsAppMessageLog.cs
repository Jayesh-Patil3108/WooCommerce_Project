using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Entities
{
    public class WhatsAppMessageLog
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string MessageBody { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        [Required]
        public string Status { get; set; } 

        public string? RelatedOrderNumber { get; set; }

        public virtual Client? Client { get; set; }
    }
}
