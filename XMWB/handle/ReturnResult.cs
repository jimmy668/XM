using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using XMWB.Models;

namespace XMWB.handle
{
    public class ReturnResult
    {
        public static object Return(string errorcode, string errormsg, string token, object obj)
        {
            ResultModel res = new ResultModel();
            res.ErrorCode = errorcode;
            res.ErrorMsg = errormsg;
            res.Token = token;
            res.Result = obj;

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(res);
        }
    }
}