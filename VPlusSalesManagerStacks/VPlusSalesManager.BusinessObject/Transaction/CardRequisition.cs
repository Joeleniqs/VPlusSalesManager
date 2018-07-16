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
    [Table("VPlusSales.CardRequisition")]
    public class CardRequisition
    {
        public CardRequisition()
        {
            CardRequisitionItems = new HashSet<CardRequisitionItem>();
        }

        public long CardRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary Id  is required")]
        public int BeneficiaryId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Title is required")]
        [StringLength(150)]
        public string RequisitionTitle { get; set; }

        [CheckNumber(0,ErrorMessage = "Total Quantity  is required")]
        public int TotalQuantityRequested { get; set; }

        [CheckNumber(0, ErrorMessage = "Requesting Officer  is required")]
        public int RequestedBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Requisition Date - Time is required")]
        [StringLength(35)]
        public string TimeStampRequested { get; set; }

        public int ApprovedBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Comment is required")]
        [StringLength(150, ErrorMessage = "Approval Comment must be between 1 and 150 characters")]
        public string ApproverComment { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Date Approved is required")]
        [StringLength(35)]
        public string TimeStampApproved { get; set; }

        [Required(ErrorMessage = "Quantity Approved is required")]
        public int QuantityApproved { get; set; }

        public CardRequisitionStatus Status { get; set; }

        public virtual Beneficiary Beneficiary { get; set; }

        public ICollection<CardRequisitionItem> CardRequisitionItems { get; set; }
    }
}
