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
    
    public partial class tb_unit_contact
    {
        public short NoUnit { get; set; }
        public string RolePerson { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public string AdresseEmail { get; set; }
        public short ContactID { get; set; }
    
        public virtual tb_unit tb_unit { get; set; }
    }
}
