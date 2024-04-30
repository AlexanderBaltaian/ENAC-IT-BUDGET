using ENAC_IT_BUDGET.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ENAC_IT_BUDGET
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Response.Clear();
            Server.ClearError();
            var controller = new ErrorController();
            RouteData errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");


            if (exception is HttpException)
            {
                var httpException = exception as HttpException;
                Response.StatusCode = httpException.GetHttpCode();
                controller.ViewBag.errorMessage = $"Erreur : {(exception as HttpException).GetHttpCode()}";

                if (httpException.GetHttpCode() == 404)
                {
                    errorRoute.Values.Add("action", "NotFound");
                }
                else
                {
                    errorRoute.Values.Add("action", "Error");
                }
            }
            else
            {
                errorRoute.Values.Add("action", "Error");

                controller.ViewBag.errorMessage = $"{exception.GetType()}: {exception.Message}";
            }
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(Context), errorRoute));

        }
    }
}
