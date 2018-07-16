using System;
using PlugPortalManager;
using XPLUG.WEBTOOLS;
using VPlusSalesManager.Business.Service;

namespace VPlusSalesManagerCloud.PortalCore
{
    public class PortalInit
    {
        public static bool InitPortal(out string msg)
        {
            try
            {

                if (!AdminPortalService.Migrate(out msg))
                {
                    msg = "Initialization Error: " + msg;
                    return false;
                }

                if (!APIServiceManager.Migrate(out msg))
                {
                    msg = "Initialization Error: " + msg;
                    return false;
                }

               
                msg = "";
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                msg = "Operation Error: " + ex.Message;
                return false;
            }

        }
    }
}