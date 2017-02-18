using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XMWB.Controllers
{
    public class ImageController : Controller
    {
        public void Index(string path)
        {
            //string path = "F://222/0414_1.jpg";
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] byteData = new byte[fs.Length];
                fs.Read(byteData, 0, byteData.Length);

                fs.Close();
                Response.ContentType = "image/jpeg";
                Response.BinaryWrite(byteData);
                Response.Flush();

            }
            catch (Exception e)
            {
            }
        }
    }
}