using MimeKit;
using PetStoreApi.Domain;
using System.Net.Mail;

namespace PetStoreApi.Services
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        bool Send(MailMessage mailMessage);
    }
}
