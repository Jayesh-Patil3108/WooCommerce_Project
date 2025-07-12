namespace SaaS.Application.IServices.WhatsAppService
{
    public interface IWhatsAppService
    {
        Task SendMessageAsync(int clientId, string phoneNumber, string message, string relatedOrderNumber = null);
    }
}
