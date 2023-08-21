using System.Data;
using System.Web;
using System.Web.Optimization;

namespace NWTradersWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
            bundles.Add(new ScriptBundle("~/bundles/font-awesome").Include(
                      "~/lib/font-awesome/js/all.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/lib/bootstrap/css/bootstrap.min.css",
                      "~/lib/W3css/w3.css",
                      "~/lib/datatables/datatables.min.css",
                      "~/lib/font-awesome/css/all.min.css"
            ));

        }
    }
}
