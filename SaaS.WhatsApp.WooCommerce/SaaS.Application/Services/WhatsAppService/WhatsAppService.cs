using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SaaS.Application.IServices.WhatsAppService;
using SaaS.Domain.Entities;
using SaaS.Infrastructure.DbContexts;

namespace SaaS.Application.Services.WhatsAppService
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient; 
        private readonly SaaSDbContext _context;

        public WhatsAppService(HttpClient httpClient, SaaSDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task SendMessageAsync(int clientId, string phoneNumber, string message, string relatedOrderNumber = null)
        {
            var whatsAppSetting = await _context.ClientWhatsAppSettings
                .FirstOrDefaultAsync(ws => ws.ClientId == clientId);
            if (whatsAppSetting == null) throw new Exception("WhatsApp settings not found.");

            var url = "https://waba.360dialog.io/v1/messages";
            var payload = new
            {
                to = phoneNumber,
                type = "text",
                text = new { body = message }
            };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsAppSetting.ApiKey);
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            // Log the message
            var messageLog = new WhatsAppMessageLog
            {
                ClientId = clientId,
                PhoneNumber = phoneNumber,
                MessageBody = message,
                SentAt = DateTime.UtcNow,
                Status = "Sent",
                RelatedOrderNumber = relatedOrderNumber
            };
            _context.WhatsAppMessageLogs.Add(messageLog);
            await _context.SaveChangesAsync();
        }
    }
}
