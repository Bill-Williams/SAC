using Microsoft.AspNet.Identity;
using SAC.Domain;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
            message.From = new EmailAddress("no.reply@email.southernarcherycircuit.org", "Southern Archery Circuit");
            return client.SendEmailAsync(message);
        }

        public Task SendCompleteBlastAsync(string callbackUrl)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no.reply@email.southernarcherycircuit.org", "Southern Archery Circuit"),
                Subject = "SAC Tournament Results",
                PlainTextContent = string.Format("Results for today's tournament are in.  Please go to <a href='{0}'>here</a> to see the results", callbackUrl),
                HtmlContent = string.Format("Results for today's tournament are in.  Please go to <a href='{0}'>here</a> to see the results", callbackUrl),
            };

            using (var db = new SacContext())
            {
                var userEmails = from u in db.Users
                                 where u.EmailConfirmed
                                 select new EmailAddress() { Email = u.Email };

                var emailList = userEmails.ToList();

                msg.AddTos(emailList);
            }
            return client.SendEmailAsync(msg);
        }
    }
}