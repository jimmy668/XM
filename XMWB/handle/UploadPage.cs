using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using XMWB.Models;

namespace XMWB.handle
{
    public class UploadPage
    {
        //解析微信文章内容
        public static string Load(string url, string userid, string wh)
        {
            if (url.Contains("____"))
            {
                url = url.Replace("____", "==");
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.Method = "GET";
                request.Referer = url;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string html = reader.ReadToEnd();

                stream.Close();
                reader.Close();
                request.Abort();
                response.Close();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                Modify(doc.DocumentNode);


                string title = Regex.Match(html, @"<title>([\s\S]+)</title>").Groups[1].ToString();
                string time = doc.DocumentNode.SelectSingleNode("//em[@id='post-date']").InnerText;
                string author = doc.DocumentNode.SelectSingleNode("//a[@id='post-user']").InnerText;
                string content = doc.DocumentNode.SelectSingleNode("//div[@id='js_content']").OuterHtml;
                int i = 320;
                int.TryParse(wh, out i);
                content = content.Replace("670", (i * 0.87).ToString());
                content = content.Replace("502.5", "180");
                //if (content.Contains("<embed"))
                //{
                //    var embeds = doc.DocumentNode.SelectNodes("//embed");
                //    foreach (HtmlNode em in embeds)
                //    {
                //        string x = em.OuterHtml;
                //        if (x.Contains("width"))
                //        {
                //            string rs = x.Replace("width", "max-width");
                //            content.Replace(x, rs);
                //        }
                //    }
                //}

                //var iframes = doc.DocumentNode.SelectNodes("//iframe");
                //if (iframes != null)
                //{
                //    foreach (HtmlNode em in iframes)
                //    {
                //        string x = em.OuterHtml;

                //        string rs = x.Replace("width", "");
                //        content.Replace(x, rs);
                //    }
                //}




                var imgs = doc.DocumentNode.SelectNodes("//img");


                DateTime nowTime = DateTime.Now;
                Random rdm = new Random();
                string id = nowTime.ToString("yyyyMMddHHmmss") + rdm.Next(10001, 99999);//文章ID,（删除的时候用盘+用户ID+文章ID）


                if (imgs != null)
                {
                    string timeString = nowTime.ToString("hhmmss");

                    string savePath = ConfigurationManager.AppSettings["ARSavePath"] + userid + "/" + id + "/";
                    foreach (HtmlNode img in imgs)
                    {
                        string res = img.OuterHtml;
                        if (res.Contains("data-src"))
                        {
                            string rs = UploadIMG(img.Attributes["data-src"].Value, savePath);
                            if (rs != "")
                            {
                                content = content.Replace(res, ("<img src='/Image?path=" + rs + "'/>"));
                            }
                        }
                    }
                    // Random rdm = new Random();
                    // DateTime nowTime = DateTime.Now;
                    // string timeString = nowTime.ToString("hhmmss");
                    // string root = HttpContext.Current.Server.MapPath("/ImagesCF/");
                    // string path = nowTime.Year.ToString() + "/" + nowTime.Month.ToString() 
                    //     + "/" + nowTime.Day.ToString() + "/" + timeString + rdm.Next(10001, 99999) + "/";
                    // string savePath = root + path;

                    //foreach (HtmlNode img in imgs)
                    //{
                    //    string res = img.OuterHtml;
                    //    if (res.Contains("data-src"))
                    //    {
                    //        string rs = UploadIMG(img.Attributes["data-src"].Value, savePath);
                    //        if(rs != "")
                    //        {
                    //            content = content.Replace(res, ("<img src='../ImagesCF/" + path + rs + "'/>"));
                    //        }
                    //    }
                    //}
                }


                return Save(id, title, time, author, content, userid);
            }
            catch (Exception e)
            {
                return "4";
            }
        }

        private static void Modify(HtmlNode htmlNode)
        {
            //处理视频节点
            var vqq = htmlNode.SelectNodes("//iframe");
            if (vqq != null)
            {
                foreach (var item in vqq)
                {
                    if (item.Attributes["data-src"] != null && item.Attributes["src"] == null)
                    {
                        item.Attributes.Add("src", item.Attributes["data-src"].Value);
                        if (item.Attributes["height"] != null)
                            item.Attributes["height"].Value = "100%";
                        if (item.Attributes["width"] != null)
                            item.Attributes["width"].Value = "100%";
                    }


                    ////string value = item.Attributes["data-src"].Value;
                    //string res = item.OuterHtml;
                    //if (res.Contains("data-src") && !res.Contains(" src"))
                    //{
                    //    string rs = item.Attributes["data-src"].Value;
                    //    if (rs != "")
                    //    {
                    //        string ss = res.Insert(res.IndexOf("iframe", StringComparison.OrdinalIgnoreCase) + 6, " src='" + rs + "' ");
                    //        //content = content.Replace(res, ss);
                    //    }
                    //    string h = item.Attributes["height"].Value;
                    //    if (h != "")
                    //    {

                    //    }
                    //    string w = item.Attributes["width"].Value;
                    //    if (w != "")
                    //    {

                    //    }
                    //}
                }
            }
        }

        public static string Save(string id, string title, string time, string author, string content, string userid)
        {
            using (XMWBEntities entity = new XMWBEntities())
            {
                TB_LSARTICLE ad = entity.TB_LSARTICLE.SingleOrDefault(t => t.CM_USERID == userid);
                if (ad == null)
                {

                    try
                    {
                        TB_LSARTICLE lsad = new TB_LSARTICLE();
                        lsad.CM_TITLE = title;
                        lsad.CM_TIME = DateTime.Now;
                        lsad.CM_AUTHOR = author;
                        lsad.CM_CONTENT = content;
                        lsad.CM_USERID = userid;
                        lsad.CM_ID = id;

                        entity.TB_LSARTICLE.Add(lsad);
                        entity.SaveChanges();

                        return handle.ObjToJson.ToJson(lsad);
                    }
                    catch (Exception e)
                    {
                        return "5";
                    }
                }
                else
                {
                    try
                    {
                        ad.CM_TITLE = title;
                        ad.CM_TIME = DateTime.Now;
                        ad.CM_AUTHOR = author;
                        ad.CM_CONTENT = content;

                        entity.SaveChanges();

                        return handle.ObjToJson.ToJson(ad);
                    }
                    catch (Exception e)
                    {
                        return "5";
                    }
                }

            }
        }

        public static string UploadIMG(string imgurl, string savePath)
        {
            try
            {
                string s = imgurl.Substring((imgurl.LastIndexOf("=") + 1));
                if (imgurl.Contains("&"))
                {
                    int start = (imgurl.IndexOf("=") + 1);
                    int end = imgurl.IndexOf("&");
                    s = imgurl.Substring(start, (end - start));
                }
                Random rdm = new Random();
                string imgname = rdm.Next(10001, 99999) + "." + s;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgurl);
                request.Method = "GET";
                request.AllowAutoRedirect = true;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                if (!System.IO.Directory.Exists(savePath))
                {
                    System.IO.Directory.CreateDirectory(savePath);
                }
                savePath += imgname;
                img.Save(savePath);
            }
            catch (Exception e)
            {
                return "";
            }

            return savePath;
            //string imgname = null;
            //try
            //{
            //    string s = imgurl.Substring((imgurl.LastIndexOf("=") + 1));
            //    if (imgurl.Contains("&"))
            //    {
            //        int start = (imgurl.IndexOf("=") + 1);
            //        int end = imgurl.IndexOf("&");
            //        s = imgurl.Substring(start, (end - start));
            //    }
            //    Random rdm = new Random();
            //    imgname = rdm.Next(10001, 99999) + "." + s;

            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgurl);
            //    request.Method = "GET";
            //    request.AllowAutoRedirect = true;

            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    Stream stream = response.GetResponseStream();
            //    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

            //    if (!System.IO.Directory.Exists(savePath))
            //    {
            //        System.IO.Directory.CreateDirectory(savePath);
            //    }

            //    img.Save((savePath + imgname));
            //}
            //catch (Exception e)
            //{
            //    return "";
            //}

            //return imgname;
        }
    }
}