using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XMWB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }

        //通过手机号码登录(1：参数为空,2：无该用户信息,其他：user的JSON)
        public string LoginByPhone(string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
            {
                return "1";
            }

            return handle.LoginByPhone.Login(phone, password);
        }
    }
}