using MimeKit;
using PetStoreApi.Domain;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace PetStoreApi.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
        Task SendAsync(MailMessage mailMessage);
    }
}
