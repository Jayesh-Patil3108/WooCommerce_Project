using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.Application.Dtos.WooCommerce
{
    public class WooCommerceProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}
