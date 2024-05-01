using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENAC_IT_BUDGET.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View("404");
        }
        public ActionResult Unavailable()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}