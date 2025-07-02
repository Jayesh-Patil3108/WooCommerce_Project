using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaS.Application.IServices.WhatsAppService;
using SaaS.Application.IServices.WooCommerce;

namespace SaaS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IWooCommerceService _wooCommerceService; 
        private readonly IWhatsAppService _whatsAppService;

        public OrdersController(IWooCommerceService wooCommerceService, IWhatsAppService whatsAppService)
        {
            _wooCommerceService = wooCommerceService;
            _whatsAppService = whatsAppService;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            if (!int.TryParse(HttpContext.Items["ClientId"]?.ToString(), out int clientId))
            {
                return Unauthorized("Client ID not found in context.");
            }
            try
            {
                var orders = await _wooCommerceService.GetOrdersAsync(clientId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("{orderId}/send-message")]
        public async Task<IActionResult> SendOrderMessage(long orderId, [FromBody] string message)
        {

            var clientId = int.Parse(User.FindFirst("clientId")?.Value);
            var orders = await _wooCommerceService.GetOrdersAsync(clientId);
            var order = orders.FirstOrDefault(o => o.WooOrderId == orderId);
            if (order == null) return NotFound();

            await _whatsAppService.SendMessageAsync(clientId, order.CustomerPhone, message, order.OrderNumber);

            return Ok();
        }
    }
}
