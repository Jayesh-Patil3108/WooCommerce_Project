using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Application.Dtos.WooCommerce
{
    public class WooCommerceOrderDto
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime DateCreated { get; set; }
        public string Total { get; set; }
        public WooCommerceBillingDto BBilling { get; set; }
    }

    public class WooCommerceBillingDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
