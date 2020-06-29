using MyRecruitingAgent.Models;
using MyRecruitingAgent.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyRecruitingAgent.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CareerPage(int Recruiter)
        {
            CareerPageViewModel Model = new CareerPageViewModel();
            ATSEntities db = new ATSEntities();
            try
            {
                using (db)
                {
                    if (Recruiter > 0)
                    {
                        var RecruiterObj = db.Recruiters.FirstOrDefault(x => x.RecId == Recruiter);
                        var CareerObj = db.CareerPages.FirstOrDefault(x => x.RecId == Recruiter);
                        var JobsList = db.Jobs.Where(x => x.RecId == Recruiter && x.IsPosted == true /*&& x.IsActive==true*/).ToList();

                        if (RecruiterObj != null)
                            Model.RecruiterObj = RecruiterObj;

                        if (CareerObj != null)
                            Model.CareerPageObj = CareerObj;

                        if (JobsList != null)
                            Model.JobsList = JobsList;
                    }

                }
            }
            catch (Exception ex)
            {

                string st = ex.Message;
            }


            return View(Model);
        }
        [HttpGet]
        public ActionResult JobApplicationPage(int Recruiter, int Job)
        {
            JobApplicationPageViewModel Model = new JobApplicationPageViewModel();
            try
            {
                ATSEntities db = new ATSEntities();


                if (Recruiter > 0)
                {
                    var CareerObj = db.CareerPages.FirstOrDefault(x => x.RecId == Recruiter);
                    var RecruiterObj = db.Recruiters.FirstOrDefault(x => x.RecId == Recruiter);
                    var JobObj = db.Jobs.FirstOrDefault(x => x.JobId == Job);

                    if (RecruiterObj != null)
                        Model.RecruiterObj = RecruiterObj;

                    if (JobObj != null)
                        Model.JobObj = JobObj;

                    if (CareerObj != null)
                        Model.CareerPageObj = CareerObj;
                }


            }
            catch (Exception ex)
            {

                string st = ex.Message;
            }

            return View(Model);
        }
        [HttpPost]
        public ActionResult JobApplicationPage(JobApplicationPageViewModel Model)
        {
            int Marks = 0;
            try
            {
               
                ATSEntities db = new ATSEntities();
                using (db)
                {

                    foreach (var item in Model.AnswerList)
                    {
                        var obj = db.Answers.FirstOrDefault(x => x.AnswerId == item.AnswerId);
                        if (obj.IsRight)
                        {
                            Marks += 1;
                        }
                    }

                    Model.CandidateObj.MarksObtained = Marks;
                    int totalanswer = Model.AnswerList.Count;
                    totalanswer = totalanswer / 2;
                    if (Marks >= totalanswer)
                    {
                        Model.CandidateObj.IsQualified = true;
                    }
                    else { Model.CandidateObj.IsQualified = false; }

                    Model.CandidateObj.ResumeSourceId = 1;
                    Model.CandidateObj.RecId = Model.RecruiterObj.RecId;

                    if (Model.CandidateObj.CanId > 0)
                    {
                        //edit case
                        //db code for saving date
                        Model.CandidateObj.UpdateDate = DateTime.Now;
                        db.Candidates.Attach(Model.CandidateObj);
                        db.Entry(Model.CandidateObj).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        //insert case
                        //db code for saving date
                        Model.CandidateObj.IsActive = true;
                        Model.CandidateObj.CreateDate = DateTime.Now;
                        db.Candidates.Add(Model.CandidateObj);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {

                string st = ex.Message;
            }
            return RedirectToAction("JobApplicationPage", "Home", new { Recruiter = Model.RecruiterObj.RecId, Job = Model.JobObj.JobId });
        }


    }
}