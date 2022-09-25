using MimeKit;
using PetStoreApi.Domain;

namespace PetStoreApi.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
        Task<bool> SendAsync(MimeMessage mailMessage);
    }
}
