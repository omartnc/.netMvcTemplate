using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EventManagementSystem.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Metronic
            bundles.Add(new StyleBundle("~/Assets/Bundles/Styles/Metronic")
                .Include(
                    "~/Assets/Themes/Metronic/global/plugins/bootstrap/css/bootstrap.min.css"
                )
                .Include("~/Assets/plugins/font-awesome/css/font-awesome.min.css",
                    new CssRewriteUrlTransform())
            );

            bundles.Add(new ScriptBundle("~/Assets/Bundles/Scripts/Metronic")
                .Include("~/Assets/Themes/Metronic/global/plugins/jquery/jquery.min.js")
            );

            #endregion

            #region Web

            bundles.Add(new StyleBundle("~/Assets/Bundles/Styles/Web")
                .Include(
                    "~/Assets/plugins/bootstrap/css/bootstrap.min.css")
                .Include("~/Assets/plugins/font-awesome/css/font-awesome.min.css",
                    new CssRewriteUrlTransform())
            );

            bundles.Add(new ScriptBundle("~/Assets/Bundles/Scripts/Web")
                .Include("~/Assets/plugins/jquery/jquery.min.js")
            );

            bundles.Add(new ScriptBundle("~/Assets/Bundles/Scripts/Revolution")
                .Include("~/Assets/revolution/js/jquery.themepunch.tools.min.js")
            );

            bundles.Add(new ScriptBundle("~/Assets/Bundles/Scripts/Index")
                .Include("~/Assets/Scripts/index.js")
            );

            #endregion

        }
    }
}