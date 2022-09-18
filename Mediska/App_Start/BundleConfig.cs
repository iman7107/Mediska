using System.Web;
using System.Web.Optimization;

namespace Mediska
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            ""));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            ""));

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            ""));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          ""));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css"));
        }
    }
}
