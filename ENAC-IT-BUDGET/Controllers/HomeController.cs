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
using System.Globalization;
using System.Web.Helpers;

namespace ENAC_IT_BUDGET.Controllers
{
    [Authorize(Roles = "auth")]

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new VariablesTableauViewModel();
            viewModel.Date = DateTime.Now.Year;
            ViewBag.data = Session["unitnames"];
            return View(viewModel);
        }
        

        [HttpPost]
        public ActionResult Index(VariablesTableauViewModel viewModel) 
        {
           return RedirectToAction("Budget", viewModel);
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Budget(int? date, int? unit)
        {
            // Définit la culture personnalisée avec ' comme séparateur de groupe
            CultureInfo montantCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            montantCulture.NumberFormat.NumberGroupSeparator = "'";
            montantCulture.NumberFormat.NumberDecimalSeparator = ".";
            var email = Session["email"];

            var viewModelBudget = new VariablesTableauViewModel();
            var dbENACITBudget = new enacit_budget();
            var unitsAuth = dbENACITBudget.tb_unit_contact.Where(x => x.AdresseEmail == email.ToString()).Select(x => x.tb_unit).ToList();


            if (!unitsAuth.Any(x => x.NoUnit == unit))
            {
                ViewBag.message = "Accès refusé : Vous n'avez pas les droits d'accès sur ce budget.";
                return View("Error");
            }

            if (dbENACITBudget.tb_octroisunit.FirstOrDefault(x => x.NoUnit == unit && x.OctroisYear == date) != null)
            {

                // Email de la personne connectée
                var Email = dbENACITBudget.tb_unit_contact.Select(x => x.AdresseEmail).ToList();

                // Nom de l'unité
                var unitName = (dbENACITBudget.tb_unit.FirstOrDefault(x => x.NoUnit == unit)).NomUnit;

                // budget de l'unité
                var octroisCorrige = dbENACITBudget.tb_octroisunit.FirstOrDefault(x => x.NoUnit == unit && x.OctroisYear == date).OctroisCorrige;

                // Commandes de l'unité d'une certaine année 
                var commandes = dbENACITBudget.tb_commande.OrderBy(x => x.DateCommande).Where(x => x.NoUnit == unit && x.CommandeYear == date && x.tb_commandStatus.isValid == 1).ToList();
                // Somme des parts payées par transfert
                var sumTransferts = commandes.Sum(x => x.tb_commandepaiement?.PaiementParTransfert) ?? 0;

                // Somme des montants des commandes valides (Engagé)
                var sumCommandes = commandes.Sum(x => x.Montant);

                // Solde budget (budget + transfert - engagé)
                var sumBudget = octroisCorrige + sumTransferts - sumCommandes;

                var budgetInitial = octroisCorrige + sumTransferts;
                viewModelBudget.BudgetInitial = budgetInitial;
                // Format des valeurs
                var octroisFormat = octroisCorrige.ToString("N", montantCulture);
                var transertsFormat = sumTransferts.ToString("N", montantCulture);
                var sumCommandesFormat = sumCommandes.ToString("N", montantCulture);
                var sumBudgetFormat = sumBudget.ToString("N", montantCulture);
                viewModelBudget.OctroisCorrige = octroisFormat;
                viewModelBudget.Commandes = commandes;
                viewModelBudget.OctroisFormat = octroisFormat;
                viewModelBudget.TransertsFormat = transertsFormat;
                viewModelBudget.SumCommandesFormat = sumCommandesFormat;
                viewModelBudget.SumBudgetFormat = sumBudgetFormat;
                viewModelBudget.Date = (int)date;
                viewModelBudget.MontantCulture = montantCulture;
                //ViewBag.transfert = totalTransfert;
                //var units = dbENACITBudget.tb_unit.Select()
                ViewBag.Message = "Budget IT " + date + " - " + unitName;

                //viewModel.Budget = budget;
                return View(viewModelBudget);
            }
            else
            {
                ViewBag.Message = "Veuiller vérifier l'année et l'unité entrée.";
                return View("Error");

            }
        }
    }
}