using PlugPortalManager.Common;
using System.ComponentModel.DataAnnotations;


namespace VPlusSalesManager.APIObjects.Common
{
    public class AdminObj
    {
        [CheckNumber(0, ErrorMessage ="User Id is required")]
        public int AdminUserId { get; set; }

        [Required(ErrorMessage = "System Code is required", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 15, ErrorMessage = "Invalid System Code")]
        public string SysPathCode;
    }
    
}
