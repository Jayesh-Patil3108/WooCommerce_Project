using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaaS.Web.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public OrdersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SaaSApiClient");
        }

        public List<OrderDto> Orders { get; set; }
        public int? TotalOrders { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? PendingOrders { get; set; }
        public string ErrorMessage { get; set; }

        //    public async Task<IActionResult> OnGetAsync()
        //    {
        //        var token = HttpContext.Session.GetString("Token");
        //        if (string.IsNullOrEmpty(token))
        //        {
        //            return RedirectToPage("/Login");
        //        }

        //        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //        _httpClient.DefaultRequestHeaders.Add("X-Refresh-Token", HttpContext.Session.GetString("RefreshToken"));

        //        try
        //        {
        //            var response = await _httpClient.GetAsync("api/orders");
        //            response.EnsureSuccessStatusCode();
        //            var content = await response.Content.ReadAsStringAsync();
        //            var orders = JsonSerializer.Deserialize<List<OrderDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //            Orders = orders;

        //            TotalOrders = orders?.Count ?? 0;
        //            TotalAmount = orders?.Sum(o => o.TotalAmount);
        //            PendingOrders = orders?.Count(o => o.Status == "Pending");
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorMessage = ex.Message;
        //        }

        //        return Page();
        //    }
        //}
        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login");
            }

            // Mock sample data while backend is offline
            Orders = new List<OrderDto>
            {
                new OrderDto { Id = 1, OrderNumber = "ORD1001", OrderDate = DateTime.Now.AddDays(-2), TotalAmount = 1299.99m, Status = "Completed" },
                new OrderDto { Id = 2, OrderNumber = "ORD1002", OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 799.50m, Status = "Pending" },
                new OrderDto { Id = 3, OrderNumber = "ORD1003", OrderDate = DateTime.Now.AddDays(-5), TotalAmount = 1599.00m, Status = "Completed" },
                new OrderDto { Id = 4, OrderNumber = "ORD1004", OrderDate = DateTime.Now.AddDays(-3), TotalAmount = 420.75m, Status = "Pending" }
            };

            TotalOrders = Orders.Count;
            TotalAmount = Orders.Sum(x => x.TotalAmount);
            PendingOrders = Orders.Count(x => x.Status == "Pending");

            return Page();
        }

        public class OrderDto
        {
            public int Id { get; set; }
            public long WooOrderId { get; set; }
            public string OrderNumber { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string Status { get; set; }
        }
    }
}
