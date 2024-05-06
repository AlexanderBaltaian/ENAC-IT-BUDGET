using ENAC_IT_BUDGET.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.SqlClient;
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
            // Vérification si il y a des erreurs
            var exception = Server.GetLastError();
            Response.Clear();
            Server.ClearError();
            var controller = new ErrorController();
            RouteData errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");

            // Si c'est une erreur Http
            if (exception is HttpException)
            {
                var httpException = exception as HttpException;
                Response.StatusCode = httpException.GetHttpCode();
                controller.ViewBag.errorMessage = $"Erreur : {(exception as HttpException).GetHttpCode()}";

                // Si le code d'erreur est "404"
                if (httpException.GetHttpCode() == 404)
                {
                    errorRoute.Values.Add("action", "NotFound");
                }
                // Si c'est une autre erreur HTTP
                else
                {
                    errorRoute.Values.Add("action", "Error");
                }
            }
            // Si c'est une erreur de SQL ou Entity, si il y a une erreur, c'est une erreur de base de donnée
            else if (exception is SqlException || exception is EntityException)
            {
                var dbException = exception;
                if (exception is SqlException)
                {
                    dbException = exception as SqlException;
                }
                else if (exception is EntityException)
                {
                    dbException = exception as EntityException;
                }
                // Renvoi la page d'erreur "Unavailable"
                controller.ViewBag.errorMessage = dbException.Message;
                errorRoute.Values.Add("action", "Unavailable");
            }
            // Si c'est n'importe quelle autre erreur
            else
            {
                errorRoute.Values.Add("action", "Error");

                controller.ViewBag.errorMessage = $"{exception.GetType()}: {exception.Message}";
            }
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(Context), errorRoute));

        }
    }
}
