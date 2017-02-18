using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XMWB.Models;

namespace XMWB.handle
{
    public class LoginByPhone
    {
        //通过手机号码登录
        public static string Login(string phone, string password)
        {
            using (XMWBEntities entity = new XMWBEntities())
            {
                TB_USER user = entity.TB_USER.SingleOrDefault(t => t.CM_PHONE == phone && t.CM_PASSWORD == password);

                if (user != null)
                {
                    return handle.DES.GetLoginString(user.CM_USERID);
                }
                else
                {
                    return "2";
                }
            }
        }
    }
}