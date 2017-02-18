using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XMWB.Models;

namespace XMWB.handle
{
    public class ReviseEditArt
    {
        public static string Revise(string id, string content, string advertid, string title, string date, string author)
        {
            using (XMWBEntities entity = new XMWBEntities())
            {
                TB_LSARTICLE at = entity.TB_LSARTICLE.SingleOrDefault(t => t.CM_ID == id);
                try
                {
                    if (at != null)
                    {
                        TB_ARTICLE ad = new TB_ARTICLE();
                        ad.CM_USERID = at.CM_USERID;
                        ad.CM_ADVMTID = string.IsNullOrWhiteSpace(advertid) ? "" : advertid;
                        ad.CM_AUTHOR = string.IsNullOrWhiteSpace(author) ? at.CM_AUTHOR : author;
                        ad.CM_CONTENT = content;
                        ad.CM_ID = at.CM_ID;
                        ad.CM_TIME = DateTime.Now;
                        ad.CM_TITLE = string.IsNullOrWhiteSpace(title) ? at.CM_TITLE : title;
                        ad.CM_TYPE = 2;
                        ad.CM_ISPASS = 0;

                        entity.TB_ARTICLE.Add(ad);
                        entity.Set<TB_LSARTICLE>().Remove(at);
                        entity.SaveChanges();
                        return "0";
                    }
                    else
                    {
                        return "3";
                    }
                }
                catch (Exception e)
                {
                    return "2";
                }
            }
        }
    }
}