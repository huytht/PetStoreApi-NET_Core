﻿using com.sun.org.apache.xml.@internal.serializer.utils;
using MailKit.Security;
using MimeKit;
using PetStoreApi.Configuration;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        private readonly ISendGridClient _sendGridClient;
        private bool _isSuccess;

        public EmailSender(EmailConfiguration emailConfig, IWebHostEnvironment hostingEnvironment, ILogger<EmailSender> logger, DataContext dataContext, ISendGridClient sendGridClient)
        {
            _emailConfig = emailConfig;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _dataContext = dataContext;
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(Message message)
        {
            VerificationToken vToken = new VerificationToken();
            Guid newToken = Guid.NewGuid();
            vToken.Token = newToken.ToString();
            message.Token = newToken.ToString();
            vToken.AppUser = message.AppUser;
            vToken.IsVerify = false;

            //var mailMessage = CreateEmailMessage(message);

            //await SendAsync(mailMessage);

            vToken.IsSend = _isSuccess;

            await _dataContext.VerificationTokens.AddAsync(vToken);
            await _dataContext.SaveChangesAsync();

        }
        public async Task SendAsync(SendGridMessage mailMessage)
        {
            try
            {
                //var apiKey = _emailConfig.APIKey;
                //var client = new SendGridClient(apiKey);
                await _sendGridClient.SendEmailAsync(mailMessage);

                _isSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _isSuccess = false;
            }
           

        }
        private SendGridMessage CreateEmailMessage(Message message)
        {
            //var emailMessage = new MailMessage();
            //emailMessage.From = new MailAddress(_emailConfig.From);
            //emailMessage.To.Add(new MailAddress(message.To));
            //emailMessage.Subject = message.Subject;
            //emailMessage.IsBodyHtml = true;

            string tempateFilePath = _hostingEnvironment.ContentRootPath + "/Templates/VerifyEmail.html";
            var bodyBuilder = new BodyBuilder { HtmlBody = File.ReadAllText(tempateFilePath).Replace("VERIFICATION_URL", string.Format("https://localhost:7277/api/user/verify/{0}", message.Token)) };

            var emailMessage = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.From, _emailConfig.FromName),
                Subject = message.Subject,
                HtmlContent = bodyBuilder.HtmlBody
            };
            emailMessage.AddTo(message.To);

            return emailMessage; 
        }

    }

}
