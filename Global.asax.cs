using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PSMSData.Model;

namespace PSMS_Beheer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected virtual void Application_BeginRequest()
        {
            HttpContext.Current.Items["DataContext"] = new PSMSEntities();
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
        }
        protected virtual void Application_EndRequest()
        {
            var dataContext = HttpContext.Current.Items["DataContext"] as PSMSEntities;
            if (dataContext != null)
                dataContext.Dispose();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpContext.Current.Items["DataContext"] = new PSMSEntities();
        }
    }
}
