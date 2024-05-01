using ENAC_IT_BUDGET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace ENAC_IT_BUDGET
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Account",
               url: "Account/{action}/{id}",
               defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
              name: "Error",
              url: "Error/{action}/{id}",
              defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional }
              );

            routes.MapRoute(
                name: "Budget",
                url: "{unit}/{date}",
                defaults: new { controller = "Home", action = "Budget"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );           

        }
    }
}
