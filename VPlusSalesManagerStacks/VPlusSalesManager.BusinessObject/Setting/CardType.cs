using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPlusSalesManager.BusinessObject.Production;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Setting
{
    [Table("VPlusSales.CardType")]
    public class CardType
    {
        public CardType()
        {
            CardCommissions = new HashSet<CardCommission>();
            CardProductions = new HashSet<Card>();
        }

        public int CardTypeId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 150 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Face Value is required! ")]
        public decimal FaceValue { get; set; }

        public Status Status { get; set; }

        public ICollection<CardCommission> CardCommissions { get; set; }

        public ICollection<Card> CardProductions { get; set; }

    }
}
