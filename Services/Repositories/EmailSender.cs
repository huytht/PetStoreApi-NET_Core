using MimeKit;
using PetStoreApi.Configuration;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.Helpers;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using MailKit.Security;
using System.Net;

namespace PetStoreApi.Services.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmailSender> _logger;
        private readonly DataContext _dataContext;
        private bool _isSuccess;

        public EmailSender(EmailConfiguration emailConfig, IWebHostEnvironment hostingEnvironment, ILogger<EmailSender> logger, DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _emailConfig = emailConfig;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendEmailAsync(Message message)
        {
            VerificationToken vToken = new VerificationToken();
            Guid newToken = Guid.NewGuid();
            vToken.Token = newToken.ToString();
            message.Token = newToken.ToString();
            vToken.AppUser = message.AppUser;
            vToken.IsVerify = false;

            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);

            vToken.IsSend = _isSuccess;

            await _dataContext.VerificationTokens.AddAsync(vToken);
            await _dataContext.SaveChangesAsync();

        }
        public async Task SendAsync(MailMessage mailMessage)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
                smtp.Send(mailMessage);

                _isSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _isSuccess = false;
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
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            var bodyBuilder = new BodyBuilder { HtmlBody = File.ReadAllText(tempateFilePath).Replace("VERIFICATION_URL", string.Format("https://{0}/api/user/verify/{1}", host, message.Token)) };

            emailMessage.Body = bodyBuilder.HtmlBody;
            return emailMessage;
        }

    }

}
