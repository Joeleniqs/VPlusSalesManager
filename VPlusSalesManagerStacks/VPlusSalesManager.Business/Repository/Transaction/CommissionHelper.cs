using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPlusSalesManager.Business.Repository.Transaction
{
    public class CommissionHelper
    {
        public decimal CommissionAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public int CommissionQuantity { get; set; }
        public decimal ExcessBalance { get; set; }
        public int CardCommissionId { get; set; }
    }
}
