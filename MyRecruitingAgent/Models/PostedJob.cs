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
    
    public partial class PostedJob
    {
        public int PostedJobId { get; set; }
        public int JobId { get; set; }
        public int PlatformId { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<int> PaymentId { get; set; }
        public string PaymentIdString { get; set; }
        public Nullable<int> PaymentType { get; set; }
    
        public virtual Job Job { get; set; }
        public virtual Platform Platform { get; set; }
    }
}
