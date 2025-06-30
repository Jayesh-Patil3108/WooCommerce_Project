using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Entities
{
    public class ClientWooSetting
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        public string StoreUrl { get; set; }

        [Required]
        public string ConsumerKey { get; set; }

        [Required]
        public string ConsumerSecret { get; set; }

        public virtual Client? Client { get; set; }


    }
}
