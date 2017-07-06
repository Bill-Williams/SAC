using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SAC.Web.App_Start
{
    public static class Application
    {
        private static string _organizationName;
        public static string OrganizationName => _organizationName ?? (_organizationName = ConfigurationManager.AppSettings["OrganizationName"]);

        private static string _organizationEmail;
        public static string OrganizationEmail => _organizationEmail ?? (_organizationEmail = ConfigurationManager.AppSettings["OrganizationEmail"]);

        private static string _title;
        public static string Title => _title ?? (_title = ConfigurationManager.AppSettings["Title"]);

        private static string _webUrl;
        public static string WebUrl => _webUrl ?? (_webUrl = ConfigurationManager.AppSettings["WebUrl"]);

        private static string _disqusUrl;
        public static string DisqusUrl => _disqusUrl ?? (_disqusUrl = ConfigurationManager.AppSettings["DisqusUrl"]);

        private static string _facebookUrl;
        public static string FacebookUrl => _facebookUrl ?? (_facebookUrl = ConfigurationManager.AppSettings["FacebookUrl"]);
    }
}