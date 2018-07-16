using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Transaction
{
    [Table("VPlusSales.BeneficiaryAccountTransaction")]
    public class BeneficiaryAccountTransaction
    {
        public long BeneficiaryAccountTransactionId { get; set; }

        [Required ( ErrorMessage = "Beneficiary Account Id is required! ")]
        public int BeneficiaryAccountId { get; set; }
      
        [Required( ErrorMessage = "Beneficiary Id is required! ")]
        public int BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Amount is required! ")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Previous Balance is required! ")]
        public decimal PreviousBalance { get; set; }

        [Required(ErrorMessage = "New Balance is required! ")]
        public decimal NewBalance { get; set; }

        public int RegisteredBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registered Date - Time is required")]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "Registered Date must be between 10 and 35 characters")]
        public string TimeStampRegistered { get; set; }

        public TransactionType TransactionType { get; set; }

        public TransactionSourceType TransactionSource { get; set; }

        public Status Status { get; set; }

        public virtual BeneficiaryAccount BeneficiaryAccount { get; set; }
    }
}
