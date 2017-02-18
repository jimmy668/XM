using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XMWB.Models
{
    public class ResultModel
    {
        public string ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string Token { get; set; }
        public object Result { get; set; }
    }
}