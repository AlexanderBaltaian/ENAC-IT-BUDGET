using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ENAC_IT_BUDGET.Models.ENACIT_Budget;

namespace ENAC_IT_BUDGET.ViewModels
{
    public class VariablesFormViewModel
    {
        public int Date { get; set; }
        public int Unit {  get; set; }
        public List<string> Units { get; set; }
    }
    
    
}