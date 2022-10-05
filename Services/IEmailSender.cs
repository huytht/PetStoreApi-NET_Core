using MimeKit;
using PetStoreApi.Domain;

namespace PetStoreApi.Services
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        bool Send(MimeMessage mailMessage);
    }
}
