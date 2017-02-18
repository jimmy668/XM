using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace XMWB.handle
{
    public class ObjToJson
    {
        public static string ToJson(object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Serialize(obj);
        }
    }
}