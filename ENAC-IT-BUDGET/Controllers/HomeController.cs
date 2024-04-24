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
            var octroisCorrige = dbENACITBudget.tb_octroisunit.FirstOrDefault(x => x.NoUnit == unit && x.OctroisYear == date).OctroisCorrige;

            // Commandes de l'unité d'une certaine année 
            var commandes = dbENACITBudget.tb_commande.Where(x => x.NoUnit == unit && x.CommandeYear == date && x.tb_commandStatus.isValid == 1).ToList();

            // Somme des parts payées par transfert
            var sumTransferts = commandes.Sum(x => x.tb_commandepaiement.PaiementParTransfert);

            // Somme des montants des commandes valides (Engagé)
            var sumCommandes = commandes.Sum(x => x.Montant);

            // Solde budget (budget + transfert - engagé)
            var sumBudget = octroisCorrige + sumTransferts - sumCommandes;

            ViewBag.octroisCorrige = octroisCorrige;
            ViewBag.sumTransferts = sumTransferts;
            ViewBag.sumCommandes = sumCommandes;
            ViewBag.sumBudget = sumBudget;
            //ViewBag.transfert = totalTransfert;
            //var units = dbENACITBudget.tb_unit.Select()
            ViewBag.Message = "Budget IT " + date + " - "  + unitName;

            //viewModel.Budget = budget;
            return View("Budget");
        }
    }
}