using System.Web.Mvc;

namespace VPlusSalesManagerCloud.Areas.VPlusCloud
{
    public class TestEngineCloudAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TestEngineCloud";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TestEngineCloud_default",
                "TestEngineCloud/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}