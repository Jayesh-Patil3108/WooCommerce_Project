using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SaaS.Web.Pages
{
    public class ProductsModel : PageModel
    {
        public List<ProductDto> Products { get; set; }
        public string ErrorMessage { get; set; }
        public string ClientName { get; set; }

        public IActionResult OnGet()
        {
            string clientId = HttpContext.Session.GetString("ClientId") ?? "ClientA";
            ClientName = clientId == "ClientA" ? "Client A" : "Client B";

            try
            {
                Products = new List<ProductDto>();
                if (clientId == "ClientA")
                {
                    Products.AddRange(new[]
                    {
                        new ProductDto { Id = 1, Name = "Product A1", Price = 99.99m, Stock = 50 },
                        new ProductDto { Id = 2, Name = "Product A2", Price = 149.50m, Stock = 30 }
                    });
                }
                else
                {
                    Products.AddRange(new[]
                    {
                        new ProductDto { Id = 3, Name = "Product B1", Price = 199.99m, Stock = 40 },
                        new ProductDto { Id = 4, Name = "Product B2", Price = 79.75m, Stock = 20 }
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
        }
    }
}