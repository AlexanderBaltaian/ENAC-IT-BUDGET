using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENAC_IT_BUDGET.Controllers
{
    // Convrtôleur des erreurs
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            // Retourne la page 404
            return View("404");
        }
        public ActionResult Unavailable()
        {
            // Retourne la page DB non accèssible
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}