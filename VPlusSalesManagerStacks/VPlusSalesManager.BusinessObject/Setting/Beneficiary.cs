using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.BusinessObject.Transaction;
using VPlusSalesManager.Common;

namespace VPlusSalesManager.BusinessObject.Setting
{
    [Table("VPlusSales.Beneficiary")]
    public class Beneficiary
    {
        public Beneficiary()
        {
           CardRequisitions = new HashSet<CardRequisition>();
        }

        public int BeneficiaryId { get; set; }
        public int BeneficiaryAccountId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Beneficiary Full Name is required")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Beneficiary Full Name must be between 3 and 80 characters")]
        public string Fullname { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Beneficiary Mobile Number is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid Beneficiary Mobile Number")]
        public string MobileNumber { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Beneficiary Email is required")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Company Name is required")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 150 characters")]
        public string Address { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registration Date - Time is required")]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "Registration Date must be between 10 and 35 characters")]
        public string TimeStampRegistered { get; set; }

        public int ApprovedBy { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Comment is required")]
        [StringLength(150, ErrorMessage = "Approval Comment must be between 1 and 150 characters")]
        public string ApproverComment { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Date - Time is required")]
        [StringLength(35, ErrorMessage = "Approval Date must be between 1 and 35 characters")]
        public string TimeStampApproved { get; set; }

        public BeneficiaryType BeneficiaryType { get; set; }

        public Status Status { get; set; }

        public BeneficiaryAccount BeneficiaryAccount { get; set; }
        public ICollection<CardRequisition> CardRequisitions { get; set; }

    }
}
