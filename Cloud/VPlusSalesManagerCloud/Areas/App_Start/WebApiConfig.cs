using System.Web.Http;

namespace VPlusSalesManagerCloud
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "PlugPortal/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
