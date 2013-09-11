using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceStack.Logging;
using ServiceStack.Logging.Elmah;
using Elmah;
using SmartSite.Core.Rendering;
using System.Reflection;
using SmartSite.Core.Routing;

namespace Klick.CMS 
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
			ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SmartSite.Core.Views.SmartSiteRazorViewEngine());
            ViewEngines.Engines.Add(new System.Web.Mvc.WebFormViewEngine());

			// set up smartsite styled routes
			SmartSiteRouteGenerator.SetUpRoutes(RouteTable.Routes, HttpContext.Current);
            
			RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Use custom controller factory so new controllers are compiled at runtime
            ControllerBuilder.Current.SetControllerFactory(new RuntimeControllerFactory(new RuntimeControllerActivator(new DefaultPathProvider())));

        }

        void Application_PostRequestHandlerExecute(Object sender, EventArgs e)
        {
            #if DEBUG
            
            #else
				// Get Elmah to log all our post requests			
				string postlogpath = System.Configuration.ConfigurationManager.AppSettings["PostLogPath"] ?? HttpContext.Current.Request.PhysicalApplicationPath + "/PostLogs/";
				Elmah.XmlFileErrorLog xmlErrorLog = new Elmah.XmlFileErrorLog(postlogpath);
                xmlErrorLog.Log(new Elmah.Error(new Exception("POST"), System.Web.HttpContext.Current));
			#endif
        }
    }
}
