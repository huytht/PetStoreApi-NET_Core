using MimeKit;
using PetStoreApi.Data.Entity;
using System.Net.Mail;

namespace PetStoreApi.Domain
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string? Token { get; set; }
        public AppUser AppUser { get; set; }

        public Message(string to, string subject, string? content, string? token, AppUser user)
        {
            To = to;
            Subject = subject;
            Content = content;
            Token = token;
            AppUser = user;
        }
    }
}
