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
        // Affichage de la date actuelle et des unités disponible à l'utilisateur
        public ActionResult Index()
        {
            var viewModel = new VariablesFormViewModel();
            viewModel.Date = DateTime.Now.Year;
            ViewBag.data = Session["unitnames"];
            return View(viewModel);
        }


        [HttpPost]
        //Action après la validation du formulaire
        public ActionResult Index(VariablesFormViewModel viewModel)
        {
            return RedirectToAction("Budget", viewModel);
        }
        public ActionResult About()
        {
            return View();
        }

        // Actions de la page "Contact"
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // Affiche la page "Budget" avec toutes les variables nécessaires et crée un message d'erreur si l'utilisateur n'a pas les droits d'accès
        public ActionResult Budget(int? date, int? unit)
        {
            // Définit la culture personnalisée avec ' comme séparateur de groupe
            CultureInfo montantCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            montantCulture.NumberFormat.NumberGroupSeparator = "'";
            montantCulture.NumberFormat.NumberDecimalSeparator = ".";
            string email =Convert.ToString(Session["email"]);

            var viewModelBudget = new VariablesTableauViewModel();
            var dbENACITBudget = new enacit_budget();

            // Récupération des unité ou la personne est de contact
            var unitsAuth = dbENACITBudget.tb_unit_contact.Where(x => x.AdresseEmail == email.ToString()).Select(x => x.tb_unit).ToList();

            // Vérification si l'utilisateur est ENAC-IT
            var isEnacitMember = dbENACITBudget.tb_user.Any(x => x.AdresseMail == email.ToString());


            // Vérification de l'utilisateur non ENACIT et non personne de contact
            if (!unitsAuth.Any(x => x.NoUnit == unit) && isEnacitMember == false)
            {
                ViewBag.errorMessage = "Accès refusé : Vous n'avez pas les droits d'accès sur ce budget.";
                return View("Error");
            }
            // Vérification de l'entrée. Si aucune date n'est rentrée ou le formulaire n'est pas validé, une erreur apparait
            if (dbENACITBudget.tb_octroisunit.FirstOrDefault(x => x.NoUnit == unit && x.OctroisYear == date) != null)
            {

                // Email de la personne connectée
                var Email = dbENACITBudget.tb_unit_contact.Select(x => x.AdresseEmail).ToList();

                // Nom de l'unité
                var unitName = (dbENACITBudget.tb_unit.FirstOrDefault(x => x.NoUnit == unit)).NomUnit;

                // budget de l'unité
                var octroisCorrige = dbENACITBudget.tb_octroisunit.FirstOrDefault(x => x.NoUnit == unit && x.OctroisYear == date).OctroisCorrige;

                // Commandes de l'unité d'une certaine année qui sont validée
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

                // Envoie des valeurs à notre viewmodel (VariablesTableauViewModel)
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
            // Erreur si aucune valeur n'est reçue
            else
            {
                ViewBag.errorMessage = "Veuiller vérifier l'année et l'unité entrée.";
                return View("Error");

            }
        }
    }
}