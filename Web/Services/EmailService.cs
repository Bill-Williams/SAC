using Microsoft.AspNet.Identity;
using SAC.Domain;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SAC.Web.App_Start;

namespace SAC.Web.Services
{
    public class EmailService : IIdentityMessageService
    {
        string apiKey;
        SendGridClient client;

        public EmailService()
        {
            apiKey = System.Environment.GetEnvironmentVariable("SendGrid");
            client = new SendGridClient(apiKey);
        }

        public Task SendAsync(IdentityMessage message)
        {
            var msg = new SendGridMessage()
            {
                Subject = message.Subject,
                PlainTextContent = message.Body,
                HtmlContent = message.Body
            };
            msg.AddTo(new EmailAddress(message.Destination, null));
            return SendAsync(msg);
        }

        public Task SendAsync(SendGridMessage message)
        {
            message.From = new EmailAddress($"no.reply@{Application.OrganizationEmail}", Application.OrganizationName);
            return client.SendEmailAsync(message);
        }

        public async Task SendBlastAsync(string subject, string htmlBody)
        {
            using (var db = new SacContext())
            {
                var userEmails = from u in db.Users
                                 where u.EmailConfirmed
                                 select new EmailAddress() { Email = u.Email };

                foreach (var email in userEmails)
                {
                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress($"no.reply@{Application.OrganizationEmail}", Application.OrganizationName),
                        Subject = subject,
                        PlainTextContent = htmlBody,
                        HtmlContent = htmlBody,
                    };

                    msg.AddTo(email);
                    
                    await client.SendEmailAsync(msg);
                }
            }
        }
    }
}