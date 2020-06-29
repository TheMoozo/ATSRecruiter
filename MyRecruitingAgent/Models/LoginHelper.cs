using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyRecruitingAgent.Models
{
    public class UserLoginData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PlanId { get; set; }
        

    }
    public class LoginHelper
    {
        public static UserLoginData GetCurrentLoginUser()
        {
            return JsonConvert.DeserializeObject<UserLoginData>(HttpContext.Current.User.Identity.Name);
        }

        //Code 4
    }
}