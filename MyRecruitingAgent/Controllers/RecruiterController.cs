using Microsoft.Ajax.Utilities;
using MyRecruitingAgent.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyRecruitingAgent.Controllers
{
    public class RecruiterController : Controller
    {
        Recruiter GetRecruiter;
        // GET: Reruiter
        #region Rec
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password, bool IsRemember = false)
        {
            //Code 1 here
            Recruiter user = new Recruiter();
            ATSEntities db = new ATSEntities();
            user = db.Recruiters.FirstOrDefault(x => x.Password == Password && x.Username == UserName);
            if (user != null)
            {
                UserLoginData UserLogin = new UserLoginData()
                { Id = user.RecId, Password = user.Password, Username = user.Username, PlanId = user.UserPlanId };
                var UserData = JsonConvert.SerializeObject(UserLogin);
                FormsAuthentication.SetAuthCookie(UserData, false);
                return RedirectToAction("Jobs", "Recruiter");
            }
            ViewBag.Error = "Login Incorrect";
            return RedirectToAction("Login", "Recruiter");
        }
        [Authorize]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Recruiter");

        }
        #endregion

        #region Sign Up
        [HttpGet]
        public ActionResult SignUp(int id = 0)
        {
            Recruiter Newrecruiter = new Recruiter();
            return View(Newrecruiter);
        }
        [HttpPost]
        public ActionResult SignUp(FormCollection fm, Recruiter Newrecruiter)
        {
            try
            {
                ATSEntities db = new ATSEntities();
                using (db)
                {

                    Newrecruiter.AdminId = 1;
                    db.Recruiters.Add(Newrecruiter);
                    db.SaveChanges();

                }
                ModelState.Clear();
                //return Content("Signup Confimed");

            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return RedirectToAction("Login", "Recruiter");
        }
        #endregion

        #region Jobs

        [Authorize]
        public ActionResult Jobs()
        {
            try
            {
                ATSEntities db = new ATSEntities();
                using (db)
                {
                    //job list all
                    var currentUser = LoginHelper.GetCurrentLoginUser();
                    var Jobs = db.Jobs.Where(x => x.RecId == currentUser.Id).ToList();

                    //To Get Jobs List filter
                    //var Jobs = db.Jobs.Where(x => x.IsActive == true).ToList();

                    //To get object
                    //var JobObj = db.Jobs.FirstOrDefault(x => x.JobId == 1);//Where 1 is job id

                    //code for delete
                    //db.Jobs.Remove(JobObject);
                    //db.SaveChanges();

                    return View(Jobs);
                }

            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return View(new List<Job>());
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddJobs(int id = 0)
        {
            Job model = new Job();

            if (id > 0)
            {
                ATSEntities db = new ATSEntities();
                using (db)
                {
                    model = db.Jobs.FirstOrDefault(x => x.JobId == id);
                }
            }
            model.RecId = LoginHelper.GetCurrentLoginUser().Id;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddJobs(Job Model)
        {
            try
            {
                Model.QuestionairId = 1;
                ATSEntities db = new ATSEntities();
                using (db)
                {
                    if (Model.JobId > 0)
                    {
                        //edit case
                        //db code for saving date
                        Model.UpdateDate = DateTime.Now;
                        db.Jobs.Attach(Model);
                        db.Entry(Model).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        //insert case
                        //db code for saving date
                        Model.CreateDate = DateTime.Now;
                        db.Jobs.Add(Model);
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return RedirectToAction("Jobs", "Recruiter");
        }


        [HttpGet]
        public ActionResult PostJob(int id = 0)
        {

            if (id > 0)
            {
                Job model = new Job();
                ATSEntities db = new ATSEntities();
                using (db)
                {
                    model = db.Jobs.FirstOrDefault(x => x.JobId == id);
                    model.IsPosted = true;
                    model.UpdateDate = DateTime.Now;
                    db.Jobs.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Jobs", "Recruiter");
        }

        [HttpGet]
        public ActionResult RepostJob(int id = 0)
        {

            if (id > 0)
            {
                Job model = new Job();
                ATSEntities db = new ATSEntities();
                using (db)
                {
                    model = db.Jobs.FirstOrDefault(x => x.JobId == id);
                    model.IsReposted = true;
                    model.CreateDate = DateTime.Now;
                    model.UpdateDate = DateTime.Now;
                    db.Jobs.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Jobs", "Recruiter");
        }
        #endregion

        #region Applicants
        [Authorize]
        public ActionResult Applicants(int JobId = 0)
        {
            ATSEntities DB = new ATSEntities();
            using (DB)
            {
                //List of All Applicants
                // var Applicant = DB.Applicants.ToList();

                //To Get Applicant List Filters
                List<Applicant> Applicants = new List<Applicant>();
                if (JobId > 0)
                {
                    Applicants = DB.Applicants.Where(x => x.JobId == JobId).ToList();
                }
                else
                {
                    Applicants = DB.Applicants.ToList();
                }
                var currentUser = LoginHelper.GetCurrentLoginUser();
                var JobList = DB.Jobs.Where(x => x.RecId == currentUser.Id).ToList();
                ViewBag.JobList = JobList;
                //To get object
                //var ApplicantObj = DB.Applicants.Where(x => x.JobId == JobId);//Where 1 is job id

                //Code for saving or insert
                //DB.Jobs.Add(new Job());
                //DB.SaveChanges();

                //code for delete
                //DB.Jobs.Remove(ApplicantObj);
                //DB.SaveChanges();


                return View(Applicants);

            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult Applicants(Applicant model, int jobid)
        {
            ATSEntities DB = new ATSEntities();
            Job job = new Job();
            job.JobId = 1;
            using (DB)
            {
                if (job.JobId == 1)
                {
                    DB.Applicants.Add(model);
                    DB.SaveChanges();

                }
                else
                {
                    string st = "Enter the Jobid";
                }
                return View();

            }

        }
        #endregion

        #region Sourcing/AddCandidate
        [Authorize]
        [HttpGet]
        public ActionResult AddCandidate(int id = 0)
        {
            Candidate Model = new Candidate();
            if (id > 0)
            {
                ATSEntities db = new ATSEntities();
                Model = db.Candidates.FirstOrDefault(x => x.CanId == id);
            }
            Model.RecId = LoginHelper.GetCurrentLoginUser().Id;
            // Model.ResumeSourceId = LoginHelper.GetCurrentLoginUser().Id;
            return View(Model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCandidate(Candidate Model)
        {
            try
            {

                Model.RecId = LoginHelper.GetCurrentLoginUser().Id;
                Model.ResumeSourceId = 1;

                ATSEntities db = new ATSEntities();

                if (Model.CanId > 0)
                {
                    //edit case
                    //db code for saving date
                    Model.UpdateDate = DateTime.Now;
                    db.Candidates.Attach(Model);
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    //insert case
                    //db code for saving date
                    Model.CreateDate = DateTime.Now;
                    db.Candidates.Add(Model);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return RedirectToAction("Sourcing", "Recruiter");
        }

        [Authorize]
        public ActionResult Sourcing(string Search = "")
        {
            try
            {
                ATSEntities db = new ATSEntities();
                using (db)
                {

                    var currentUser = LoginHelper.GetCurrentLoginUser();
                    var Jobs = db.Jobs.Where(x => x.RecId == currentUser.Id).Select(x => new SelectListItem() { Text = x.JobName, Value = x.JobId.ToString() }).ToList();

                    ViewBag.Jobslist = Jobs;

                    var Sourcing = db.Candidates.Where(x => x.RecId == currentUser.Id && x.IsActive == true).ToList();
                    if (!string.IsNullOrEmpty(Search))
                    {
                        Sourcing = Sourcing.Where(x => x.FirstName.Contains(Search) || x.LastName.Contains(Search) || x.FatherName.Contains(Search)).Where(x => x.IsActive == true).ToList();
                    }
                    return View(Sourcing);
                }

            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            ViewBag.Search = Search;
            return View(new List<Candidate>());
        }
        public ActionResult AssignToJob(int CandidateId, int JobId)
        {
            ATSEntities db = new ATSEntities();
            using (db)
            {
                var jobObj = db.Jobs.FirstOrDefault(x => x.JobId == JobId);
                var CandidateObj = db.Candidates.FirstOrDefault(x => x.CanId == CandidateId);
                CandidateObj.IsActive = false;
                db.Candidates.Attach(CandidateObj);
                db.Entry(CandidateObj).State = EntityState.Modified;
                db.SaveChanges();

                if (jobObj != null && CandidateObj != null)
                {
                    Applicant ApplicantObj = new Applicant();
                    ApplicantObj.ApplicantFullName = CandidateObj.FirstName + " " + CandidateObj.LastName;
                    ApplicantObj.CanId = CandidateObj.CanId;
                    ApplicantObj.CreateDate = DateTime.Now;
                    ApplicantObj.IsShortlisted = true;
                    ApplicantObj.JobId = jobObj.JobId;
                    db.Applicants.Add(ApplicantObj);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Sourcing", "Recruiter");
        }

        #endregion

        #region Campaigns
        [Authorize]

        public ActionResult Campaigns(int JobId = 0)
        {
            ATSEntities db = new ATSEntities();
            try
            {
                var currentUser = LoginHelper.GetCurrentLoginUser();
                var Jobs = db.Jobs.Where(x => x.RecId == currentUser.Id).Select(x => new SelectListItem() { Text = x.JobName, Value = x.JobId.ToString() }).ToList();
                ViewBag.Jobslist = Jobs;

            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(DripEmail Model, int JobId)
        {
            ATSEntities db = new ATSEntities();
            try
            {
                if (Model.IsScheduled == true)
                {
                    var jobObj = db.Jobs.FirstOrDefault(x => x.JobId == JobId);
                    var ApplicantList = db.Applicants.Where(x => x.JobId == JobId).ToList();
                    foreach (var item in ApplicantList)
                    {
                        Model.ApplicantId = item.ApplicantId;
                        Model.CandidateId = item.CanId;
                        Model.RecId = LoginHelper.GetCurrentLoginUser().Id;

                        if (jobObj != null && ApplicantList.Count > 0)
                        {

                            //insert case
                            //db code for saving date
                            Model.CreatedDate = DateTime.Now;
                            db.DripEmails.Add(Model);
                            db.SaveChanges();

                        }

                    }
                }
                else
                {
                    //Save DripEmail
                    var jobObj = db.Jobs.FirstOrDefault(x => x.JobId == JobId);
                    var ApplicantList = db.Applicants.Where(x => x.JobId == JobId).ToList();
                    foreach (var item in ApplicantList)
                    {
                        Model.ApplicantId = item.ApplicantId;
                        Model.CandidateId = item.CanId;
                        Model.RecId = LoginHelper.GetCurrentLoginUser().Id;

                        if (jobObj != null && ApplicantList.Count > 0)
                        {

                            //insert case
                            //db code for saving date
                            Model.CreatedDate = DateTime.Now;
                            db.DripEmails.Add(Model);
                            db.SaveChanges();

                        }

                    }
                    //send email
                    Task task = new Task(() =>
                    {
                        while (true)
                        {

                            using (db)
                            {
                                var Records = db.DripEmails.Where(x => x.IsScheduled == true && x.ScheduledDateTime <= DateTime.Now && (x.IsSentSuccess == false || x.IsSentSuccess == null));
                                foreach (var item in Records)
                                {
                                    //Email Code here 
                                    try
                                    {
                                        int canid = (int)item.CandidateId;
                                        var candidate = db.Candidates.FirstOrDefault(x => x.CanId == canid);
                                        MailMessage mail = new MailMessage();
                                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                        mail.From = new MailAddress("recruiteragentproject@gmail.com");
                                        mail.To.Add(candidate.Email);
                                        mail.Subject = item.Subject;
                                        mail.Body = item.EmailBodyText;

                                        SmtpServer.Port = 587;
                                        SmtpServer.Credentials = new System.Net.NetworkCredential("recruiteragentproject", "newproject");
                                        SmtpServer.EnableSsl = true;

                                        SmtpServer.Send(mail);

                                        item.IsSentSuccess = true;
                                        //Update Record Code
                                        db.DripEmails.Attach(Model);
                                        db.Entry(Model).State = EntityState.Modified;
                                        db.SaveChanges();

                                    }
                                    catch (Exception ex)
                                    {
                                        ////Update Record Code if error issent=false item update
                                        item.IsSentSuccess = false;
                                        db.DripEmails.Attach(item);
                                        db.Entry(item).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }

                            Thread.Sleep(1000);
                        }
                    });
                    task.Start();
                }


            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return RedirectToAction("Campaigns", "Recruiter");

        }


        #endregion

        #region Questioner/Question Answer
        [Authorize]
        [HttpGet]
        public ActionResult Questioner()
        {

            var currentUser = LoginHelper.GetCurrentLoginUser();
            ATSEntities db = new ATSEntities();
            List<Questionair> Queslist = new List<Questionair>();
            using (db)
            {
                Queslist = db.Questionairs.Where(x => x.RecId == currentUser.Id).ToList();
                return View(Queslist);
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Questioner(Questionair Model)
        {
            Model.RecId = LoginHelper.GetCurrentLoginUser().Id;
            ATSEntities db = new ATSEntities();
            using (db)
            {
                if (Model.QuestionairId > 0)
                {

                    db.Questionairs.Attach(Model);
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Questionairs.Add(Model);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Questioner", "Recruiter");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Questions(int QuestionairId)
        {
            ViewBag.QuestionairId = QuestionairId;
            ATSEntities db = new ATSEntities();
            using (db)
            {
                var Questions = db.Questions.Where(x => x.QuestionairId == QuestionairId).ToList();

                return View(Questions);
            }

        }

        //public ActionResult AddQuestions(int QuestionairId, int QId = 0)
        //{
        //    //Model.QuestionairId = 1;
        //    var Question = new Question();
        //    ATSEntities db = new ATSEntities();
        //    if (QId > 0)
        //    {
        //        Question = db.Questions.FirstOrDefault(x => x.QuestionId == QId);

        //    }
        //    else
        //    {
        //        Question.QuestionId = QuestionairId;
        //    }
        //    return View();
        //}
        [HttpPost]
        [Authorize]
        public ActionResult AddQuestions(Question Model)
        {
            //Model.QuestionairId = 1;
            ATSEntities db = new ATSEntities();
            using (db)
            {
                if (Model.QuestionId > 0)
                {

                    db.Questions.Attach(Model);
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Questions.Add(Model);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Questions", "Recruiter", new { QuestionairId = Model.QuestionairId });
        }

        public ActionResult Answers(int QuestionId)
        {
            ViewBag.QuestionId = QuestionId;
            ATSEntities db = new ATSEntities();
            using (db)
            {
                var Answers = db.Answers.Where(x => x.QuestionId == QuestionId).ToList();

                return View(Answers);
            }
        }
        [HttpPost]
        public ActionResult AddAnswer(Answer Model)
        {
            ATSEntities db = new ATSEntities();
            using (db)
            {
                if (Model.AnswerId > 0)
                {
                    //Edit
                    db.Answers.Attach(Model);
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Answers.Add(Model);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Answers", "Recruiter", new { QuestionId = Model.QuestionId });
        }
        public ActionResult IsRightAnswers(int QuestionId, int AnswerId, bool Value)
        {

            ATSEntities db = new ATSEntities();
            using (db)
            {
                var Answer = db.Answers.FirstOrDefault(x => x.AnswerId == AnswerId);

                Answer.IsRight = Value;
                db.Answers.Attach(Answer);
                db.Entry(Answer).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Answers", "Recruiter", new { QuestionId = QuestionId });
        }
        #endregion

        #region Setting/RecSetting CareerPage
        [Authorize]
        public ActionResult Setting()
        {
            return View();
        }


        [Authorize]
        public ActionResult RecruiterSetting()
        {
            ATSEntities db = new ATSEntities();
            using (db)
            {
                var currentUser = LoginHelper.GetCurrentLoginUser();
                Recruiter recruiter = db.Recruiters.FirstOrDefault(x => x.RecId == currentUser.Id);
                return View(recruiter);
            }

        }
        [HttpPost, Authorize]
        public ActionResult RecruiterSetting(Recruiter Model, FormCollection fm, HttpPostedFileBase LogoFile)
        {
            ATSEntities db = new ATSEntities();
            using (db)
            {
                if (Model.RecId > 0)
                {
                    var currentUser = LoginHelper.GetCurrentLoginUser();
                    Recruiter recruiter = db.Recruiters.FirstOrDefault(x => x.RecId == currentUser.Id);

                    try
                    {
                        //Check if File is available.
                        if (LogoFile != null && LogoFile.ContentLength > 0)
                        {
                            //Save the File.
                            string DirectoryPath = Server.MapPath("~/Content/Images/RecLogo/" + Model.RecId + "/");
                            string filePath = DirectoryPath + LogoFile.FileName;
                            Model.Logo = LogoFile.FileName;
                            if (!System.IO.Directory.Exists(DirectoryPath))
                            {
                                System.IO.Directory.CreateDirectory(DirectoryPath);
                            }
                            if (System.IO.Directory.Exists(DirectoryPath))
                            {
                                LogoFile.SaveAs(filePath);
                            }
                        }
                    }
                    catch (Exception ex) { }


                    recruiter.Username = Model.Username;
                    recruiter.Password = Model.Password;
                    recruiter.CompanyName = Model.CompanyName;
                    recruiter.CompanyAddress = Model.CompanyAddress;
                    recruiter.City = Model.City;
                    recruiter.State = Model.State;
                    recruiter.Logo = Model.Logo;
                    //recruiter.ExternalLogo = Model.ExternalLogo;

                    db.Recruiters.Attach(recruiter);
                    db.Entry(recruiter).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("RecruiterSetting", "Recruiter");
            }

        }

        [Authorize]
        public ActionResult CareerPage()
        {
            var currentUser = LoginHelper.GetCurrentLoginUser();
            ATSEntities db = new ATSEntities();
            using (db)
            {
                var careerPageObj = db.CareerPages.FirstOrDefault(x => x.RecId == currentUser.Id);
                if (careerPageObj == null)
                {
                    careerPageObj = new CareerPage();
                    careerPageObj.RecId = currentUser.Id;
                }
                careerPageObj.CreatedDate = DateTime.Now;
                return View(careerPageObj);
            }

        }
        [HttpPost, Authorize]
        [ValidateInput(false)]
        public ActionResult CareerPage(CareerPage Model, HttpPostedFileBase LogoFile)
        {
            try
            {
                if (LogoFile != null && LogoFile.ContentLength > 0)
                {
                    //Save the File.
                    string DirectoryPath = Server.MapPath("~/Content/Images/ExternalLogo/" + Model.RecId + "/");
                    string filePath = DirectoryPath + LogoFile.FileName;
                    Model.ExternalLogo = LogoFile.FileName;
                    if (!System.IO.Directory.Exists(DirectoryPath))
                    {
                        System.IO.Directory.CreateDirectory(DirectoryPath);
                    }
                    if (System.IO.Directory.Exists(DirectoryPath))
                    {
                        LogoFile.SaveAs(filePath);
                    }
                }
            }
            catch (Exception ex) { }
            ATSEntities db = new ATSEntities();
            using (db)
            {
                if (Model.Id == 0)
                {
                    db.CareerPages.Add(Model);
                    db.SaveChanges();
                }
                else
                {
                    var DbCareerModel = db.CareerPages.FirstOrDefault(x => x.RecId == Model.RecId);
                    //Check if File is available.


                    if (!string.IsNullOrEmpty(Model.ExternalLogo) && DbCareerModel.ExternalLogo != Model.ExternalLogo)
                    {
                        DbCareerModel.ExternalLogo = Model.ExternalLogo;
                    }
                    if (!string.IsNullOrEmpty(Model.Intro) && DbCareerModel.Intro != Model.Intro)
                    {
                        DbCareerModel.Intro = Model.Intro;
                    }
                    if (!string.IsNullOrEmpty(Model.Description) && DbCareerModel.Description != Model.Description)
                    {
                        DbCareerModel.Description = Model.Description;
                    }

                    if (!string.IsNullOrEmpty(Model.Heading) && DbCareerModel.Heading != Model.Heading)
                    {
                        DbCareerModel.Heading = Model.Heading;
                    }
                    DbCareerModel.UpdateDate = DateTime.Now;

                    db.CareerPages.Attach(DbCareerModel);
                    db.Entry(DbCareerModel).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("CareerPage", "Recruiter");
        }
        #endregion

    }
}

