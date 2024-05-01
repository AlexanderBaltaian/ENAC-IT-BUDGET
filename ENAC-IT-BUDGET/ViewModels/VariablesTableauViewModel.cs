using ENAC_IT_BUDGET.Models.ENACIT_Budget;
using Enacit.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ENAC_IT_BUDGET.ViewModels
{
    public class VariablesTableauViewModel
    {
        public string OctroisCorrige {  get; set; }
        public string OctroisFormat { get; set; }
        public string TransertsFormat { get; set; }
        public string SumCommandesFormat { get; set; }
        public string SumBudgetFormat { get; set; }
        public decimal BudgetInitial { get; set; }
        public List<tb_commande> Commandes { get; set; }
        public List<tb_unit> UnitsAuth {  get; set; }
        public int Date {  get; set; }
        public string Unit {  get; set; }
        public CultureInfo MontantCulture { get; set; }
    }
}