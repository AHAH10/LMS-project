﻿using System.Web;
using System.Web.Optimization;

namespace LMS_Project
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
          
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/Angular/angular.min.js",
                        "~/Scripts/Angular/MainAngular.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/angularcourses").Include(
                        "~/Scripts/Angular/Courses.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularsubjects").Include(
                        "~/Scripts/Angular/Subjects.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularschedules").Include(
                        "~/Scripts/checklist-model.js",
                        "~/Scripts/Angular/Schedules.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-Slate.css",
                      "~/Content/site.css"));
        }
    }
}
