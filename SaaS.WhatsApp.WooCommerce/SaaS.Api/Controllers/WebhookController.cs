using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SaaS.Application.Dtos.WooCommerce;
using SaaS.Application.IServices.WhatsAppService;
using SaaS.Application.IServices.WooCommerce;
using SaaS.Domain.Entities;
using SaaS.Infrastructure.DbContexts;

namespace SaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IWooCommerceService _wooCommerceService;
        private readonly IWhatsAppService _whatsAppService;
        private readonly SaaSDbContext _context;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(IWooCommerceService wooCommerceService, IWhatsAppService whatsAppService, SaaSDbContext context, ILogger<WebhookController> logger)
        {
            _wooCommerceService = wooCommerceService;
            _whatsAppService = whatsAppService;
            _context = context;
            _logger = logger;
        }


        [HttpPost("woocommerce/order")]
        public async Task<IActionResult> HandleOrderWebhook([FromBody] JsonElement payload)
        {
            _logger.LogInformation("Received webhook payload: {Payload}", payload.GetRawText());
            // Safely access the X-WC-Webhook-Source header
            StringValues webhookSource;
            if (!Request.Headers.TryGetValue("X-WC-Webhook-Source", out webhookSource) || StringValues.IsNullOrEmpty(webhookSource))
            {
                return BadRequest("Missing or invalid X-WC-Webhook-Source header.");
            }

            // Extract clientId from the header (assuming it contains the clientId in the last segment)
            string source = webhookSource.FirstOrDefault();
            if (string.IsNullOrEmpty(source))
            {
                return BadRequest("Invalid X-WC-Webhook-Source header value.");
            }

            // Assuming the header contains a URL or string with clientId as the last segment
            if (!int.TryParse(source.Split('/').Last(), out int clientId))
            {
                return BadRequest("Invalid client ID in X-WC-Webhook-Source header.");
            }

            // Deserialize the WooCommerce order payload
            var order = JsonSerializer.Deserialize<WooCommerceOrderDto>(payload.GetRawText());
            if (order == null)
            {
                return BadRequest("Invalid order payload.");
            }

            // Map to WooOrder entity
            var wooOrder = new WooOrder
            {
                ClientId = clientId,
                WooOrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = order.DateCreated,
                TotalAmount = decimal.Parse(order.Total),
                CustomerName = $"{order.Billing?.FirstName} {order.Billing?.LastName}".Trim(),
                CustomerPhone = order.Billing?.Phone
            };

            // Validate required fields
            if (string.IsNullOrEmpty(wooOrder.CustomerPhone))
            {
                return BadRequest("Customer phone number is missing in the order payload.");
            }

            // Save the order to the database
            _context.WooOrders.Add(wooOrder);
            await _context.SaveChangesAsync();

            // Send WhatsApp message
            var message = $"New order #{order.Number} placed for {order.Total:C}.";
            await _whatsAppService.SendMessageAsync(clientId, wooOrder.CustomerPhone, message, order.Number);

            return Ok();
        }
    }
}
