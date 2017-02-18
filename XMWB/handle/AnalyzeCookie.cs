using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XMWB.Models;

namespace XMWB.handle
{
    public class AnalyzeCookie
    {
        //解析cookie
        public static ObjSessionModel Analyze(string cookie)
        {
            try
            {
                string res = handle.DES.Decode(cookie);
                ObjSessionModel obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjSessionModel>(res);
                obj.userId = obj.userId.Trim();

                return obj;
                //TimeSpan tm = DateTime.Now - DateTime.Parse(obj.loginTime);

                //if (tm.Minutes > 1440)
                //{
                //    return null;
                //}
                //else
                //{
                //    return obj;
                //}
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}