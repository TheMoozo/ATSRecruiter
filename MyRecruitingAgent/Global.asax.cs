using MyRecruitingAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyRecruitingAgent
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Code 3
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Task task = new Task(() =>
            {
                while (true)
                {
                    ATSEntities db = new ATSEntities();
                    using (db)
                    {
                        var Records = db.DripEmails.Where(x => x.IsScheduled == true && x.ScheduledDateTime <= DateTime.Now && (x.IsSentSuccess == false || x.IsSentSuccess == null)).ToList();
                        foreach (var item in Records)
                        {
                            //Email Code here 
                            try
                            {
                                int canId = (int)item.CandidateId;
                                var candiate = db.Candidates.FirstOrDefault(x => x.CanId == canId);
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                mail.From = new MailAddress("recruiteragentproject@gmail.com");
                                mail.To.Add(candiate.Email);//candidate email
                                mail.Subject = item.Subject;
                                mail.Body = item.EmailBodyText;

                                SmtpServer.Port = 587;
                                SmtpServer.Credentials = new System.Net.NetworkCredential("recruiteragentproject", "newproject");
                                SmtpServer.EnableSsl = true;

                                SmtpServer.Send(mail);

                                item.IsSentSuccess = true;
                                //Update Record Code
                                db.DripEmails.Attach(item);
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                ////Update Record Code if error issent=false item update
                                //Update Record Code
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
}
