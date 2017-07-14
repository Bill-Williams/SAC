using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using SAC.Web.Models;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Facebook;
using System.Net.Http;

namespace SAC.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: (id) => (Guid.Parse(id.GetUserId())))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            var mo = new MicrosoftAccountAuthenticationOptions()
            {
                ClientId = "69997243-9b41-4890-a449-3cdc91c02ac9",
                ClientSecret = System.Environment.GetEnvironmentVariable("sac-MS"),
                //Scope = { "User.ReadBasic.All" }
            };

            app.UseMicrosoftAccountAuthentication(mo);

            app.UseTwitterAuthentication(
               consumerKey: "gObyL8ird9nMbTlyEB0vh4mgA",
               consumerSecret: System.Environment.GetEnvironmentVariable("sac-Twitter"));

            //There are two keys for Facebook.  One is live and one is test.  They don't allow multi domain access from what I can tell.
#if DEBUG
            //TEST
            var fb = new FacebookAuthenticationOptions()
            {
                AppId = "438970499791540",
                AppSecret = System.Environment.GetEnvironmentVariable("sac-Facebook"),
                BackchannelHttpHandler = new HttpClientHandler(),
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name"
            };
#else
            // LIVE
            var fb = new FacebookAuthenticationOptions()
            {
               AppId = "424419464593148",
               AppSecret = System.Environment.GetEnvironmentVariable("sac-Facebook"),
               BackchannelHttpHandler = new HttpClientHandler(),
               UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name"
            };
#endif
            app.UseFacebookAuthentication(fb);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "65715418484-rthhdl9q054anp71utc9eh1ikno739sn.apps.googleusercontent.com",
                ClientSecret = System.Environment.GetEnvironmentVariable("sac-Google")
            });
        }
    }
}