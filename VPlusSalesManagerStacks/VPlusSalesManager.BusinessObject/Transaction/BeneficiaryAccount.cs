using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPlusSalesManager.BusinessObject.Setting;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Transaction
{
    [Table("VPlusSales.BeneficiaryAccount")]
    public class BeneficiaryAccount
    {
        public BeneficiaryAccount()
        {
            BeneficiaryAcccountTransactions = new HashSet<BeneficiaryAccountTransaction>();
        }

        public int BeneficiaryAccountId { get; set; }

        [Required(ErrorMessage = "Available Balance is required! ")]
        public decimal AvailableBalance { get; set; }

        [Required(ErrorMessage = "Credit Limit is required! ")]
        public decimal CreditLimit { get; set; }

        [Required(ErrorMessage = "Last Transaction Amount is required! ")]
        public decimal LastTransactionAmount { get; set; }

        public TransactionType LastTransactionType { get; set; }

        public long LastTransactionId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Transaction Date - Time is required")]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "Last Transaction Date must be between 10 and 35 characters")]
        public string LastTransactionTimeStamp { get; set; }

        public Status Status { get; set; }

        public ICollection<BeneficiaryAccountTransaction> BeneficiaryAcccountTransactions { get; set; }
    }
}
