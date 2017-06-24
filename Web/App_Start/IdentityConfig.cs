using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SAC.Web.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using SAC.Domain;
using System.Linq;

namespace SAC.Web
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
                From = new EmailAddress("azure_7569176bdd96be4db440d7fc26d127c8@azure.com","Southern Archery Circut"),
                Subject = message.Subject,
                PlainTextContent = message.Body,
                HtmlContent = message.Body
            };
            msg.AddTo(new EmailAddress(message.Destination, null));
            return client.SendEmailAsync(msg);
        }

        public Task SendCompleteBlastAsync(string callbackUrl)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("azure_7569176bdd96be4db440d7fc26d127c8@azure.com", "Southern Archery Circut"),
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

        private enum UnsubscribeGroup
        {
            Complete = 0
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, Guid>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
