using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.BusinessObject.Production;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Transaction
{
    [Table("VPlusSales.CardIssuance")]
    public class CardIssuance
    {
        public long CardIssuanceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Requisition Id is required")]
        public long CardRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Requisition Item Id is required")]
        public long CardRequisitionItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary Id is required")]
        public int BeneficiaryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Item Id is required")]
        public long CardItemId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Id is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Batch Id must be 5 characters")]
        public string BatchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " Start Batch Number is required")]
        [StringLength(300, MinimumLength = 7, ErrorMessage = " Start Batch Number must be between 7 and 12 characters")]
        public string StartBatchNumber { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Stop Batch Number is required")]
        [StringLength(300, MinimumLength = 7, ErrorMessage = "Stop Batch Number must be between 7 and 12 characters")]
        public string StopBatchNumber { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity Issued is required")]
        public int QuantityIssued { get; set; }

        public int IssuedBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Issued Date - Time is required")]
        [StringLength(35, ErrorMessage = "Issued Date must be between 1 and 35 characters")]
        public string TimeStampIssued { get; set; }

        public virtual CardItem CardItem { get; set; }

        public virtual CardRequisitionItem CardRequisitionItem { get; set; }

    }
}
