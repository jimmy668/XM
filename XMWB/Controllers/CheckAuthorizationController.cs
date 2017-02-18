using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XMWB.handle;
using XMWB.Models;

namespace XMWB.Controllers
{
    public class CheckAuthorizationController : Controller
    {
        //检查是否是vip用户
        public object Get(string cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return ReturnResult.Return("1", "cookie错误", null, null);
            }
            else
            {
                ObjSessionModel obj = handle.AnalyzeCookie.Analyze(cookie);

                if (obj == null)
                {
                    return ReturnResult.Return("1", "cookie错误", null, null);
                }
                else
                {
                    return handle.CheckAuthorization.Check(obj.userId);
                }
            }
        }
    }
}