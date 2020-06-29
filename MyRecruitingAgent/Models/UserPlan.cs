//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyRecruitingAgent.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserPlan
    {
        public int UserPlanId { get; set; }
        public int PlanId { get; set; }
        public int RecId { get; set; }
        public Nullable<bool> IsSubscribed { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public bool IsPlanPurchased { get; set; }
        public Nullable<System.DateTime> Expiry { get; set; }
    
        public virtual Plan Plan { get; set; }
        public virtual Recruiter Recruiter { get; set; }
    }
}