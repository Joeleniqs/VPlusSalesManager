using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Transaction
{
    [Table("VPlusSales.BeneficiaryPayment")]
    public class BeneficiaryPayment
    {
        public long BeneficiaryPaymentId { get; set; }

        public long BeneficiaryAccountTransactionId { get; set; }

        [Required( ErrorMessage = "Beneficiary Account Id is required! ")]
        public int BeneficiaryAccountId { get; set; }

        [Required(ErrorMessage = "Beneficiary Id is required! ")]
        public int BeneficiaryId { get; set; }

        [Required( ErrorMessage = "Amount is required! ")]
        public decimal AmountPaid { get; set; }

        public PaySource PaymentSource { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Source Name is required")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Payment Source Name must be between 3 and 80 characters")]
        public string PaymentSourceName { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Reference  is required")]
        [StringLength(18, MinimumLength = 3, ErrorMessage = "Payment Reference must be between 3 and 18 characters")]
        public string PaymentReference { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Date is required")]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "Payment Date must be between 10 and 35 characters")]
        public string PaymentDate { get; set; }

        public int RegisteredBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registered Date - Time is required")]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "Registered Date must be between 10 and 35 characters")]
        public string TimeStampRegistered { get; set; }

        public Status Status { get; set; }

        public virtual BeneficiaryAccountTransaction BeneficiaryAccountTransaction { get; set; }

    }
}
