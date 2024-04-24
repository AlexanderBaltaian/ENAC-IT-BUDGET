using ENAC_IT_BUDGET.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ENAC_IT_BUDGET.Models.ENACIT_Budget;

namespace ENAC_IT_BUDGET.Controllers
{
    [Authorize(Roles = "auth")]

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new VariablesFormViewModel();
            if (ViewBag.data == null)
            {
                ViewBag.data = Session["unitnames"];
            }
            return View();
        }
        

        [HttpPost]
        public ActionResult Index(VariablesFormViewModel viewModel) 
        {
            return RedirectToAction("Budget", new { date = viewModel.Date, unit = viewModel.Unit });
        }
        public ActionResult About(int? date, string unit)
        {
            ViewBag.Message = date + unit;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Budget(int? date, int? unit)
        {
            var viewModelBudget = new VariablesTableauViewModel();
            var dbENACITBudget = new enacit_budget();
            // Email de la personne connectée
            var Email = dbENACITBudget.tb_unit_contact.Select(x => x.AdresseEmail).ToList();

            // Nom de l'unité
            var unitName = (dbENACITBudget.tb_unit.FirstOrDefault(x => x.NoUnit == unit)).NomUnit;

            // budget de l'unité
            var budget = dbENACITBudget.tb_octroisunit.FirstOrDefault(x => x.NoUnit == unit && x.OctroisYear == date).OctroisCorrige;

            // Numéro de commande de l'unité d'une certaine année
            var NoCommande = dbENACITBudget.tb_commande.Where(x => x.NoUnit == unit && x.CommandeYear == date).ToList();

            // Commandes valides de l'unité d'après NoCommande
            var commandeValide = dbENACITBudget.tb_commandStatus.Where(x => x.NoCommande == NoCommande && x.isValid == 1).ToList();
            //var transfert = dbENACITBudget.tb_commandepaiement.Where(x => x.NoCommande == commandeValide).ToList();
            //decimal totalTransfert = transfert.Sum(x => Convert.ToDecimal(x));

            viewModelBudget.Budget = budget;
            ViewBag.budget = budget;
            //ViewBag.transfert = totalTransfert;
            //var units = dbENACITBudget.tb_unit.Select()
            ViewBag.Message = "Budget IT " + date + " - "  + unitName;

            //viewModel.Budget = budget;
            return View("Budget");
        }
    }
}