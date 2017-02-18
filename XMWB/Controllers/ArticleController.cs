using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XMWB.Models;

namespace XMWB.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(string url)
        {
            ViewBag.Url = url;
            return View();
        }
        //解析微信文章内容
        public string Analysis(string url, string cookie, string wh)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "1";
            }

            if (string.IsNullOrWhiteSpace(cookie))
            {
                return "2";
            }
            else
            {
                ObjSessionModel obj = handle.AnalyzeCookie.Analyze(cookie);

                if (obj == null)
                {
                    return "3";
                }
                else
                {
                    return handle.UploadPage.Load(url, obj.userId, wh);
                }
            }
        }

        //提交编辑后的文章 1:文章ID为空，2：保存错误，3：不存在此文章
        [HttpPost]
        public string Save(ArtReviseModel obj)
        {
            if (string.IsNullOrWhiteSpace(obj.artid))
            {
                return "1";
            }
            obj.content = Server.UrlDecode(obj.content);

            return handle.ReviseEditArt.Revise(obj.artid, obj.content, obj.advid, obj.title, obj.date, obj.author);
        }

        public ActionResult Preview(string id, string avid)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new EmptyResult();
            }
            else
            {
                using (XMWBEntities entity = new XMWBEntities())
                {
                    TB_LSARTICLE at = entity.TB_LSARTICLE.SingleOrDefault(t => t.CM_ID == id);

                    if (at != null)
                    {
                        TB_ARTICLE ad = new TB_ARTICLE();
                        ad.CM_USERID = at.CM_USERID;
                        ad.CM_ADVMTID = string.IsNullOrWhiteSpace(avid) ? "" : avid;
                        ad.CM_AUTHOR = at.CM_AUTHOR;
                        ad.CM_CONTENT = at.CM_CONTENT;
                        ad.CM_ID = at.CM_ID;
                        ad.CM_TIME = DateTime.Now;
                        ad.CM_TITLE = at.CM_TITLE;
                        ad.CM_TYPE = 2;
                        ad.CM_ISPASS = 0;

                        entity.TB_ARTICLE.Add(ad);
                        entity.Set<TB_LSARTICLE>().Remove(at);
                        entity.SaveChanges();

                        return View(ad);
                    }
                    else
                    {
                        TB_ARTICLE ARTICLE = entity.TB_ARTICLE.SingleOrDefault(t => t.CM_ID == id);
                        if (ARTICLE != null)
                        {
                            TB_USER user = entity.TB_USER.SingleOrDefault(t => t.CM_USERID == ARTICLE.CM_USERID);
                            if (user != null)
                            {
                                if (user.CM_EXPIRINGTIME < DateTime.Now)
                                {
                                    ARTICLE = null;
                                }
                            }
                            else
                            {
                                ARTICLE = null;
                            }
                        }
                        return View(ARTICLE);
                    }
                }
            }
        }
    }
}