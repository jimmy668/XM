using System.Web;
using System.Web.Optimization;

namespace XMWB
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/common.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/checkCookie.js",
                        "~/Scripts/md5.js"
                        ));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/*.css"));
        }
    }
}
