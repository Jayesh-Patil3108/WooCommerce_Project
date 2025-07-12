using SaaS.Domain.Entities;

namespace SaaS.Application.IServices.WooCommerce
{
    public interface IWooCommerceService
    {
        Task<List<WooOrder>> GetOrdersAsync(int clientId);
        Task<List<WooCustomer>> GetCustomersAsync(int clientId);
        Task<List<WooProduct>> GetProductsAsync(int clientId);
    }
}
