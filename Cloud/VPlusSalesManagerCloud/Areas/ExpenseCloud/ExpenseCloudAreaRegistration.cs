using System.Web.Mvc;

namespace JetTestEngineCloud.Areas.ExpenseCloud
{
    public class ExpenseCloudAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ExpenseCloud";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ExpenseCloud_default",
                "ExpenseCloud/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}