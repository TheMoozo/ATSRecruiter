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
    
    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            this.Applicants = new HashSet<Applicant>();
            this.PostedJobs = new HashSet<PostedJob>();
        }
    
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public string JobCity { get; set; }
        public string JobState { get; set; }
        public string JobCountry { get; set; }
        public string JobIntro { get; set; }
        public Nullable<int> Experience { get; set; }
        public string Qualification { get; set; }
        public Nullable<int> QualificationType { get; set; }
        public string JobType { get; set; }
        public Nullable<double> JobSalary { get; set; }
        public string Currency { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsPosted { get; set; }
        public Nullable<bool> IsReposted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int RecId { get; set; }
        public int QuestionairId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Applicant> Applicants { get; set; }
        public virtual Questionair Questionair { get; set; }
        public virtual Recruiter Recruiter { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostedJob> PostedJobs { get; set; }
    }
}
