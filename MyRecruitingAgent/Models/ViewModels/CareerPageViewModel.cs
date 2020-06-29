using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyRecruitingAgent.Models.ViewModels
{
    public class CareerPageViewModel
    {
        public CareerPageViewModel()
        {
            JobsList = new List<Job>();
        }
        public Recruiter RecruiterObj { get; set; }
        public CareerPage CareerPageObj { get; set; }
        public List<Job> JobsList { get; set; }
    }
}