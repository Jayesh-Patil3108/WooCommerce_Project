using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace SaaS.Web.Pages
{
    public class WhatsAppInboxModel : PageModel
    {
        public List<MessageDto> Messages { get; set; }
        public string ErrorMessage { get; set; }
        public string ClientName { get; set; }

        public IActionResult OnGet()
        {
            string clientId = HttpContext.Session.GetString("ClientId") ?? "ClientA";
            ClientName = clientId == "ClientA" ? "Client A" : "Client B";

            try
            {
                Messages = new List<MessageDto>();
                if (clientId == "ClientA")
                {
                    Messages.AddRange(new[]
                    {
                        new MessageDto { Id = 1, Sender = "User1", Content = "Hello, how can I help?", Time = DateTime.Now.AddHours(-1) },
                        new MessageDto { Id = 2, Sender = "User2", Content = "Need support", Time = DateTime.Now }
                    });
                }
                else
                {
                    Messages.AddRange(new[]
                    {
                        new MessageDto { Id = 3, Sender = "User3", Content = "Order details?", Time = DateTime.Now.AddMinutes(-30) },
                        new MessageDto { Id = 4, Sender = "User4", Content = "Thanks!", Time = DateTime.Now }
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public class MessageDto
        {
            public int Id { get; set; }
            public string Sender { get; set; }
            public string Content { get; set; }
            public DateTime Time { get; set; }
        }
    }
}