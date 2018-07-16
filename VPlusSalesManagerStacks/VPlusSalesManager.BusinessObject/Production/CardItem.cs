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
    [Table("VPlusSales.CardItem")]
    public class CardItem
    {
        public long CardItemId { get; set; }

        public int CardId { get; set; }

        [CheckNumber(0, ErrorMessage = "CardType Id Is Required")]
        public int CardTypeId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Batch Id must be 5 characters")]
        public string BatchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Start  Number is required")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "Batch Start  Number must be between 7 and 10 characters")]
        public string BatchStartNumber { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Stop  Number is required")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "Batch Stop Number must be between 7 and 10 characters")]
        public string BatchStopNumber { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(500, ErrorMessage = "Defective Batch Number Has a maximum of 500 characters")]
        public string DefectiveBatchNumbers { get; set; }

        [Required(ErrorMessage = "Batch Quantity is required")]
        public int BatchQuantity { get; set; }

        [Required(ErrorMessage = "Missing Quantity is required")]
        public int MissingQuantity { get; set; }

        [Required(ErrorMessage = "Defective Quantity is required")]
        public int DefectiveQuantity { get; set; }

        [Required(ErrorMessage = "Delivered Quantity is required")]
        public int DeliveredQuantity { get; set; }

        [Required(ErrorMessage = "Available Quantity is required")]
        public int AvailableQuantity { get; set; }

        [Required(ErrorMessage = "Issued Quantity is required")]
        public int IssuedQuantity { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registration Date - Time is required")]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Registration Date must be between 1 and 35 characters")]
        public string TimeStampRegistered { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Delivery Date - Time is required")]
        [StringLength(35, ErrorMessage = "Delivery Date must be between 1 and 35 characters")]
        public string TimeStampDelivered { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Issued Date - Time is required")]
        [StringLength(35, ErrorMessage = "Issued Date must be between 1 and 35 characters")]
        public string TimeStampLastIssued { get; set; }

        public int RegisteredBy { get; set; }

        public CardStatus Status { get; set; }

        public virtual Card Card { get; set; }
    }
}
