using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENAC_IT_BUDGET.ViewModels
{
    public class VariablesTableauViewModel
    {
        public decimal Budget {  get; set; }
        public int Transfert { get; set; }
        public int Engage { get; set; }
        public int SoldeBudget { get; set; }
    }
}