using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.BusinessObject.Setting;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Transaction
{
    [Table("VPlusSales.CardRequisitionItem")]
    public class CardRequisitionItem
    {
        public long CardRequisitionItemId { get; set; }
        public long CardRequisitionId { get; set; }

        [CheckNumber(0,ErrorMessage = "Card Commission Id is required")]
        public int CardCommissionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary Id  is required")]
        public int BeneficiaryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id  is required")]
        public int CardTypeId { get; set; }

        [Required(ErrorMessage = "Item's  Amount is required")]
        public decimal Amount { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Commission Rate is required")]
        public decimal CommissionRate { get; set; }

        [Required(ErrorMessage = "Commission Quantity is required")]
        public int CommissionQuantity { get; set; }

        [Required(ErrorMessage = "Commission Amount is required")]
        public decimal CommissionAmount { get; set; }

        [Required(ErrorMessage = "Excess Balance is required")]
        public decimal ExcessBalance { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        public decimal UnitPrice { get; set; }

        [CheckNumber(0, ErrorMessage = "Requesting Officer  is required")]
        public int RequestedBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Request Date - Time is required")]
        [StringLength(35)]
        public string TimeStampRequested { get; set; }

        public int ApprovedBy { get; set; }

        [Required(ErrorMessage = "Quantity Approved is required")]
        public int QuantityApproved { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Date - time Approved is required")]
        [StringLength(35)]
        public string TimeStampApproved { get; set; }

        public CardRequisitionStatus Status { get; set; }

        public virtual CardRequisition CardRequisition { get; set; }
    }
}
