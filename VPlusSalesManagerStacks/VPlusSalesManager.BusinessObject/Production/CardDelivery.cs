using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Production
{
    [Table("VPlusSales.CardDelivery")]
    public class CardDelivery
    {
        public long CardDeliveryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Item Id is required")]
        public long CardItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Id is required")]
        public int CardId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Batch Id must be 5 characters")]
        public string BatchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " Start Batch Number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = " Start Batch Number must be between 7 and 12 characters")]
        public string StartBatchNumber { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Stop Batch Number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Stop Batch Number must be between 7 and 12 characters")]
        public string StopBatchNumber { get; set; }

        [Required( ErrorMessage = "Quantity Delivered is required")]
        public int QuantityDelivered { get; set; }

        [Required(ErrorMessage = "Missing Quantity is required")]
        public int MissingQuantity { get; set; }

        [Required(ErrorMessage = "Defective Quantity is required")]
        public int DefectiveQuantity { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string DeliveryDate { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registration Date - Time is required")]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Registration Date must be between 1 and 35 characters")]
        public string TimeStampRegistered { get; set; }

        public int ApprovedBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Comment is required")]
        [StringLength(150, ErrorMessage = "Approval Comment must be between 1 and 150 characters")]
        public string ApproverComment { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Time Stamp Approved is required")]
        [StringLength(35)]
        public string TimeStampApproved { get; set; }

        public int RecievedBy { get; set; }

        public CardStatus Status { get; set; }

        public virtual CardItem CardItem { get; set; }
    }
}
