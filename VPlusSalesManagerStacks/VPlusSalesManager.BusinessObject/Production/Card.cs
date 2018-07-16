using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.BusinessObject.Setting;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Production
{
    [Table("VPlusSales.Card")]
    public class Card
    {
        public Card()
        {
            CardItems = new HashSet<CardItem>();
        }

        public int CardId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Card Title is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Card Title  must be between 3 and 200 characters")]
        public string CardTitle { get; set; }

        [CheckNumber(0, ErrorMessage = "CardType Id Is Required")]
        public int CardTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Total Quantity Is Required")]
        public int TotalQuantity { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Key is required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Batch Key  must be 2 characters")]
        public string BatchKey { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Start Batch Id must be 5 characters")]
        public string StartBatchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Stop Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Stop Batch Id must be 5 characters")]
        public string StopBatchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Number Of Batches Is Required")]
        public int NumberOfBatches { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity Per Batch Is Required")]
        public int QuantityPerBatch { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registration Date - Time is required")]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Registration Date must be between 10 and 35 characters")]
        public string TimeStampRegistered { get; set; }

        public CardStatus Status { get; set; }

        public virtual CardType CardType { get; set; }

        public ICollection<CardItem> CardItems { get; set; }
    }
}
