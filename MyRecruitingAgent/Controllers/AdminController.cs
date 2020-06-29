using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyRecruitingAgent.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password, bool IsRemember)
        {
            return RedirectToAction("Index", "Recruiter");
        }
        // GET: Reruiter
        public ActionResult Index()
        {
            return View();
        }
    }
}