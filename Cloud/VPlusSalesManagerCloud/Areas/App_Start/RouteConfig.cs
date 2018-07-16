using System.Web.Mvc;
using System.Web.Routing;

namespace VPlusSalesManagerCloud
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PortalMaster", action = "Init", id = UrlParameter.Optional }
            );
        }
    }
}
