namespace TestWebApplication
{
    using System;
    using System.Configuration;
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css").Include("~/Css/simplest.css"));

            bundles.Add(new ScriptBundle("~/scripts").Include(
                        "~/scripts/angular.js",
                        "~/scripts/angular-route.js",
                        "~/scripts/angular-resource.js",
                        "~/scripts/app/app.js",
                        "~/scripts/app/services/*.js",
                        "~/scripts/app/controllers/*.js"));

            BundleTable.EnableOptimizations = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("BundleTable.EnableOptimizations"));
        }
    }
}