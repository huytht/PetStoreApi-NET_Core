using com.sun.org.apache.xml.@internal.serializer.utils;
using MailKit.Security;
using MimeKit;
using PetStoreApi.Configuration;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.Helpers;
using System.Net;
using System.Net.Mail;

namespace PetStoreApi.Services.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<EmailSender> _logger;
        private readonly DataContext _dataContext;

        public EmailSender(EmailConfiguration emailConfig, IWebHostEnvironment hostingEnvironment, ILogger<EmailSender> logger, DataContext dataContext)
        {
            _emailConfig = emailConfig;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _dataContext = dataContext;
        }

        public void SendEmail(Message message)
        {
            VerificationToken vToken = new VerificationToken();
            Guid newToken = Guid.NewGuid();
            vToken.Token = newToken.ToString();
            message.Token = newToken.ToString();
            vToken.AppUser = message.AppUser;
            vToken.IsVerify = false;

            var mailMessage = CreateEmailMessage(message);
            
            bool isSuccess = Send(mailMessage);

            vToken.IsSend = isSuccess;
            
            _dataContext.VerificationTokens.Add(vToken);
            _dataContext.SaveChanges();

        }
        public bool Send(MailMessage mailMessage)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
                smtp.Send(mailMessage);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                return false;
            }
            
        }
        private MailMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MailMessage();
            emailMessage.From = new MailAddress(_emailConfig.From);
            emailMessage.To.Add(new MailAddress(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.IsBodyHtml = true;

            string tempateFilePath = _hostingEnvironment.ContentRootPath + "/Templates/VerifyEmail.html";
            var bodyBuilder = new BodyBuilder { HtmlBody = File.ReadAllText(tempateFilePath).Replace("VERIFICATION_URL", string.Format("https://localhost:7277/api/user/verify/{0}", message.Token))};

            emailMessage.Body = bodyBuilder.HtmlBody;
            return emailMessage;
        }

    }

}
