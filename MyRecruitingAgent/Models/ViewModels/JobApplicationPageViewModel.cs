using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyRecruitingAgent.Models.ViewModels
{
    public class JobApplicationPageViewModel
    {
        public Recruiter RecruiterObj { get; set; }
        public Job JobObj { get; set; }
        public Candidate CandidateObj { get; set; }
        public CandidateQualification QualificationObj { get; set; }
        public CandidateSkill SkillsObj { get; set; }
        public CareerPage CareerPageObj { get; set; }
        public Questionair QuestionairObj { get; set; }
        public List<Answer> AnswerList { get; set; }


    }
}