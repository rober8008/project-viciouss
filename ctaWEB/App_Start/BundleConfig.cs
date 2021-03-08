using System.Web;
using System.Web.Optimization;

namespace ctaWEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.4.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        //"~/Scripts/jquery-ui-1.9.2.custom.min.js",
                        "~/Scripts/jquery.bxslider.min.js",
                        "~/Scripts/jquery.easing.min.js",
                        "~/Scripts/jquery.flexslider.js",
                        "~/Scripts/jquery.magnific-popup.js",
                        "~/Scripts/jquery.parallax-1.1.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      //"~/Scripts/respond.js",
                      "~/Scripts/bootstrap-colorpicker.js",                      
                      "~/Scripts/hover-dropdown.js",
                      "~/Scripts/html5shiv.js",
                      "~/Scripts/link-hover.js",
                      "~/Scripts/mixitup.js",
                      "~/Scripts/superfish.js",
                      "~/Scripts/wow.min.js",
                      "~/Scripts/jquery.cslider.js",
                      "~/Scripts/spin.min.js",
                      "~/Scripts/modernizr.custom.28468.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/animate.css",
                      "~/Content/bootstrap-reset.css",
                      "~/Content/bootstrap-social.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bxslider.css",
                      "~/Content/component.css",
                      "~/Content/flexslider.css",
                      "~/Content/font-awesome.css",
                      "~/Content/magnific-popup.css",
                      //"~/Content/mixitup.css",
                      "~/Content/parallax-slider.css",
                      "~/Content/bootstrap-colorpicker.css",                      
                      //"~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/style-responsive.css",
                      "~/Content/superfish-vertical.css",
                      "~/Content/superfish.css",
                      "~/Content/theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/viciouss-watchlist-utils").Include(
                      "~/Scripts/viciouss-watchlist-utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/viciouss-symboldashboard-utils").Include(
                      "~/Scripts/viciouss-symboldashboard-utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                      "~/Scripts/knockout-3.4.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout/admin-report-types").Include(
                      "~/Scripts/KO/admin-report-types.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout/admin-report").Include(
                      "~/Scripts/KO/admin-report.js"));
        }
    }
}
