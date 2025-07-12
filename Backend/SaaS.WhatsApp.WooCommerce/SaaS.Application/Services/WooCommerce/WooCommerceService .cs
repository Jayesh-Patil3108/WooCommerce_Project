using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SaaS.Application.Dtos.WooCommerce;
using SaaS.Application.IServices.WooCommerce;
using SaaS.Domain.Entities;
using SaaS.Infrastructure.DbContexts;

namespace SaaS.Application.Services.WooCommerce
{
    public class WooCommerceService : IWooCommerceService
    {
        private readonly HttpClient _httpClient;
        private readonly SaaSDbContext _context;
        public WooCommerceService(HttpClient httpClient, SaaSDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        

        public async Task<List<WooOrder>> GetOrdersAsync(int clientId)
        {
            var wooSetting = await _context.ClientWooSettings
                .FirstOrDefaultAsync(ws => ws.ClientId == clientId);
            if (wooSetting == null) throw new Exception("WooCommerce settings not found.");

            var url = $"{wooSetting.StoreUrl}/wp-json/wc/v3/orders?consumer_key={wooSetting.ConsumerKey}&consumer_secret={wooSetting.ConsumerSecret}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var wooOrders = JsonSerializer.Deserialize<List<WooCommerceOrderDto>>(content);

            var orders = wooOrders.Select(o => new WooOrder
            {
                ClientId = clientId,
                WooOrderId = o.Id,
                OrderNumber = o.Number,
                OrderDate = o.DateCreated,
                TotalAmount = decimal.Parse(o.Total),
                CustomerName = $"{o.Billing.FirstName} {o.Billing.LastName}",
                CustomerPhone = o.Billing.Phone
            }).ToList();

            return orders;
        }

        public async Task<List<WooCustomer>> GetCustomersAsync(int clientId)
        {
            var wooSetting = await _context.ClientWooSettings
                .FirstOrDefaultAsync(ws => ws.ClientId == clientId);
            if (wooSetting == null) throw new Exception("WooCommerce settings not found.");

            var url = $"{wooSetting.StoreUrl}/wp-json/wc/v3/customers?consumer_key={wooSetting.ConsumerKey}&consumer_secret={wooSetting.ConsumerSecret}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var wooCustomers = JsonSerializer.Deserialize<List<WooCommerceCustomerDto>>(content);

            var customers = wooCustomers.Select(c => new WooCustomer
            {
                ClientId = clientId,
                WooCustomerId = c.Id,
                Name = $"{c.FirstName} {c.LastName}",
                Email = c.Email,
                Phone = c.Billing.Phone
            }).ToList();

            return customers;
        }

        public async Task<List<WooProduct>> GetProductsAsync(int clientId)
        {
            var wooSetting = await _context.ClientWooSettings
                .FirstOrDefaultAsync(ws => ws.ClientId == clientId);
            if (wooSetting == null) throw new Exception("WooCommerce settings not found.");

            var url = $"{wooSetting.StoreUrl}/wp-json/wc/v3/products?consumer_key={wooSetting.ConsumerKey}&consumer_secret={wooSetting.ConsumerSecret}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var wooProducts = JsonSerializer.Deserialize<List<WooCommerceProductDto>>(content);

            var products = wooProducts.Select(p => new WooProduct
            {
                ClientId = clientId,
                WooProductId = p.Id,
                ProductName = p.Name,
                Price = decimal.Parse(p.Price)
            }).ToList();

            return products;
        }

    }
}
