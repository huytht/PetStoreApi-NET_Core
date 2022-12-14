using System.Security.Permissions;

namespace PetStoreApi.Configuration
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        //public string APIKey { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
