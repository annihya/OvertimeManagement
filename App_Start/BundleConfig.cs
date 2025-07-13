using System.Web;
using System.Web.Optimization;

namespace OvertimeManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/adminlte").Include(
                    "~/Content/AdminLTE/bootstrap/css/bootstrap.min.css",
                    "~/Content/AdminLTE/bootstrap/css/bootstrap-datepicker.min.css",
                    "~/Content/AdminLTE/bootstrap/css/bootstrap-datetimepicker.min.css",
                    "~/Content/AdminLTE/css/AdminLTE.min.css",
                    "~/Content/AdminLTE/css/skins/skin-blue.min.css",
                    "~/Content/AdminLTE/plugins/font-awesome/css/font-awesome.min.css",
                    "~/Content/AdminLTE/plugins/ionicons/ionicons.min.css",
                    "~/Content/AdminLTE/plugins/daterangepicker/daterangepicker.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
                    "~/Scripts/jquery-3.7.0.js",
                    "~/Scripts/AdminLTE/plugins/moment/moment.js",
                    "~/Scripts/AdminLTE/bootstrap/js/bootstrap.min.js",
                    "~/Scripts/AdminLTE/bootstrap/js/bootstrap-datepicker.min.js",
                    "~/Scripts/AdminLTE/bootstrap/js/bootstrap-datetimepicker.min.js",
                    "~/Scripts/AdminLTE/js/adminlte.js",
                    "~/Scripts/AdminLTE/js/adminlte.min.js",
                    "~/Scripts/AdminLTE/plugins/chartjs/Chart.min.js",
                    "~/Scripts/AdminLTE/plugins/daterangepicker/daterangepicker.js"
                ));
        }
    }
}
