using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAC.Web.Startup))]
namespace SAC.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
