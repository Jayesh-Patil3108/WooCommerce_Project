using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SaaS.Web.Pages
{
    public class CustomersModel : PageModel
    {
        public List<CustomerDto> Customers { get; set; }
        public string ErrorMessage { get; set; }
        public string ClientName { get; set; }

        public IActionResult OnGet()
        {
            string clientId = HttpContext.Session.GetString("ClientId") ?? "ClientA";
            ClientName = clientId == "ClientA" ? "Client A" : "Client B";

            try
            {
                Customers = new List<CustomerDto>();
                if (clientId == "ClientA")
                {
                    Customers.AddRange(new[]
                    {
                        new CustomerDto { Id = 1, Name = "John Doe", Email = "john@example.com", Phone = "123-456-7890" },
                        new CustomerDto { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Phone = "098-765-4321" }
                    });
                }
                else
                {
                    Customers.AddRange(new[]
                    {
                        new CustomerDto { Id = 3, Name = "Alice Johnson", Email = "alice@example.com", Phone = "111-222-3333" },
                        new CustomerDto { Id = 4, Name = "Bob Brown", Email = "bob@example.com", Phone = "444-555-6666" }
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public class CustomerDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}