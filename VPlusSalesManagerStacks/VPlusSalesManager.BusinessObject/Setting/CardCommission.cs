using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Setting
{
    [Table("VPlusSales.CardCommission")]
    public class CardCommission
    {

        public int CardCommissionId { get; set; }

        [CheckNumber(0,ErrorMessage = "CardType Id Is Required")]
        public int CardTypeId { get; set; }

        [Required(ErrorMessage = "Lower Amount is required! ")]
        public decimal LowerAmount { get; set; }

        [Required(ErrorMessage = "Upper Amount is required! ")]
        public decimal UpperAmount { get; set; }

        [Required(ErrorMessage = "Commission Rate is required! ")]
        public decimal CommissionRate { get; set; }

        public Status Status { get; set; }

        public virtual CardType CardType { get; set; }


    }
}
