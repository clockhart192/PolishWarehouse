using System.Web.Mvc;
using System.Web.Routing;

namespace PolishWarehouse
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        //protected void Application_BeginRequest()
        //{
        //    if (!Context.Request.IsSecureConnection
        //        && !Context.Request.Url.ToString().StartsWith("http://localhost:") // to avoid switching to https when local testing
        //        )
        //    {
        //        Response.Clear();
        //        Response.Status = "301 Moved Permanently";
        //        Response.AddHeader("Location", Context.Request.Url.ToString().Insert(4, "s"));
        //        Response.End();
        //    }
        //}
    }
}
