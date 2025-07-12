using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Application.Dtos.WooCommerce
{
    public class WooCommerceCustomerDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public WooCommerceBillingDto Billing { get; set; }
    }
}
