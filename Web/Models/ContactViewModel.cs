using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAC.Web.App_Start;

namespace SAC.Web.Models
{
    public class ContactViewModel
    {
        public string SacFacebookURL = $"{Application.FacebookUrl}/";

        public string FacebookImg = "~/Content/Images/facebook.png";
    }
}