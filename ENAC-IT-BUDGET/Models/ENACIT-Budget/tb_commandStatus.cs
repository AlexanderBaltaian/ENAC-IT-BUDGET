//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENAC_IT_BUDGET.Models.ENACIT_Budget
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_commandStatus
    {
        public string NoCommande { get; set; }
        public short isValid { get; set; }
        public short isIndexed { get; set; }
        public short isPaid { get; set; }
        public short isReceived { get; set; }
    
        public virtual tb_commande tb_commande { get; set; }
    }
}