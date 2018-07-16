using System.Web.Mvc;
using VPlusSalesManagerCloud.PortalCore;

namespace VPlusSalesManagerCloud.Controllers
{
    public class PortalMasterController : Controller
    {
         [System.Web.Mvc.AllowAnonymous]
        public ActionResult Init()
        {
            string msg;
            var myAccess = PortalInit.InitPortal(out msg);
            if (!myAccess)
            {
                ViewBag.ConfigMessage = "Portal Initialization Failed! Detail: " +
                                        (msg.Length > 0 ? msg : "Please Check Configuration Settings");
                return View();
            }
             return View();
        }
    }
}