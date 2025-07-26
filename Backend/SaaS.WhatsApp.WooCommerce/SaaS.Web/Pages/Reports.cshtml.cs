using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace SaaS.Web.Pages
{
    public class ReportsModel : PageModel
    {
        public List<ReportDto> Reports { get; set; }
        public string ErrorMessage { get; set; }
        public string ClientName { get; set; }

        public IActionResult OnGet()
        {
            string clientId = HttpContext.Session.GetString("ClientId") ?? "ClientA";
            ClientName = clientId == "ClientA" ? "Client A" : "Client B";

            try
            {
                Reports = new List<ReportDto>();
                if (clientId == "ClientA")
                {
                    Reports.AddRange(new[]
                    {
                        new ReportDto { Id = 1, Title = "Sales Report Q1", Date = DateTime.Now.AddMonths(-1), Status = "Completed" },
                        new ReportDto { Id = 2, Title = "Inventory Report", Date = DateTime.Now, Status = "Pending" }
                    });
                }
                else
                {
                    Reports.AddRange(new[]
                    {
                        new ReportDto { Id = 3, Title = "Revenue Report", Date = DateTime.Now.AddDays(-5), Status = "Completed" },
                        new ReportDto { Id = 4, Title = "Customer Analysis", Date = DateTime.Now, Status = "Pending" }
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public class ReportDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime Date { get; set; }
            public string Status { get; set; }
        }
    }
}