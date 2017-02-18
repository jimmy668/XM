using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XMWB.Models;

namespace XMWB.handle
{
    public class CheckAuthorization
    {
        //检查是否是vip用户
        public static object Check(string userid)
        {
            TB_USER user = null;
            using (XMWBEntities entity = new XMWBEntities())
            {
                try
                {
                    user = entity.TB_USER.SingleOrDefault(t => t.CM_USERID == userid);
                }
                catch (Exception e)
                {
                    return ReturnResult.Return("4", "网络异常，请稍后再试", null, null);
                }
            }

            if (user == null)
            {
                return ReturnResult.Return("2", "不存在此用户", null, null);
            }
            else if (user.CM_EXPIRINGTIME == null || (user.CM_EXPIRINGTIME != null && user.CM_EXPIRINGTIME < DateTime.Now))
            {
                return ReturnResult.Return("3", "Vip过期", null, null);
            }
            else
            {
                return ReturnResult.Return("0", "权限正常", null, null);
            }
        }
    }
}