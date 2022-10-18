using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace BlazorTestApp.Server.Services
{
    public class EmailSender : IEmailSender
    {       
        
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                         IConfiguration configuration)
        {
            Options = optionsAccessor.Value;          
            Configuration = configuration;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.
        public IConfiguration Configuration;

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(Options.SendGridKey))
            {
                Options.SendGridKey = Configuration["SendGrid:APIKey"];
            }

            if (string.IsNullOrWhiteSpace(Options.SendGridKey))
            {
                Options.SendGridKey = Configuration["AppSettings:AppUserName"];
            }

            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        public  Task Execute(string apiKey, string subject, string message, string toEmail)
        {
           
            var client = new SendGridClient(apiKey);            
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("nord.ferro@gmail.com", "Password Recovery"),
                Subject = subject,
               
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
           
        }
    }
}
