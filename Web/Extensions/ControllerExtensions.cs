using SAC.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using SAC.Web.App_Start;

namespace SAC.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static string RenderPartialViewToEmailString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                var htmlSource = sw.GetStringBuilder().ToString();

                var pm = new PreMailer.Net.PreMailer(htmlSource, new Uri($"{Application.WebUrl}/"));

                var result = pm.MoveCssInline();

                return result.Html;
            }
        }
    }
}