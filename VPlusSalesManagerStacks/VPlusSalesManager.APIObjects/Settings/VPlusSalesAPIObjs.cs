using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPlusSalesManager.APIObjects.Common;
using PlugPortalManager.Common;

namespace VPlusSalesManager.APIObjects.Settings
{

    #region Card Type
    public class RegCardTypeObj : AdminObj
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Face Value is required! ")]
        public decimal FaceValue { get; set; }

        public int Status { get; set; }

    }

    public class EditCardTypeObj : AdminObj
    {

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Face Value is required! ")]
        public decimal FaceValue { get; set; }

        public int Status { get; set; }
    }

    public class DeleteCardTypeObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }
    }
    #endregion

    #region Card Commission
    public class RegCardCommissionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(ErrorMessage = "Lower Amount is required")]
        public decimal LowerAmount { get; set; }

        [Required(ErrorMessage = "Upper Amount is required")]
        public decimal UpperAmount { get; set; }

        [Required(ErrorMessage = "Commission Rate is required")]
        public decimal CommissionRate { get; set; }

        public int Status { get; set; }

    }

    public class EditCardCommissionObj : AdminObj
    {

        [CheckNumber(0, ErrorMessage = "Card Commission Id is required")]
        public int CardCommissionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(ErrorMessage = "Lower Amount is required")]
        public decimal LowerAmount { get; set; }

        [Required(ErrorMessage = "Upper Amount is required")]
        public decimal UpperAmount { get; set; }

        [Required(ErrorMessage = "Commission Rate is required")]
        public decimal CommissionRate { get; set; }

        public int Status { get; set; }
    }

    public class DeleteCardCommissionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Commission Id is required")]
        public int CardCommissionId { get; set; }
    }
    #endregion

    #region Card Requisition
    public class RegCardRequisitionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Beneficiary Id is required")]
        public int BeneficiaryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Total Quantity Requested is required")]
        public int TotalQuantityRequested { get; set; }

        public List<CardRequisitionItemObj> CardRequisitionItems { get; set; }
    }

    public class CardRequisitionItemObj
    {

        [CheckNumber(0, ErrorMessage = "Card Type Id  is required")]
        public int CardTypeId { get; set; }

        [Required(ErrorMessage = "Item's  Amount is required")]
        public decimal Amount { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity Requested  is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Commission Rate is required")]
        public decimal CommissionRate { get; set; }

        [Required(ErrorMessage = "Commission Quantity is required")]
        public int CommissionQuantity { get; set; }

        [Required(ErrorMessage = "Commission Amount is required")]
        public decimal CommissionAmount { get; set; }

        [Required(ErrorMessage = "Excess Balance is required")]
        public decimal ExcessBalance { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        public decimal UnitPrice { get; set; }

    }

    public class EditCardRequisitionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Requisition Id is required")]
        public long CardRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary  Id is required")]
        public int BeneficiaryId { get; set; }

        public int TotalQuantityRequested { get; set; }

        public List<EditCardRequisitionItemObj> RequisitionItems { get; set; }

    }

    public class EditCardRequisitionItemObj
    {
        [CheckNumber(0, ErrorMessage = "Requisition Item Id is required")]
        public long CardRequisitionItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Requisition Id is required")]
        public long CardRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id  is required")]
        public int CardTypeId { get; set; }

        [Required( ErrorMessage = "Item's  Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Quantity Requested  is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Commission Rate is required")]
        public decimal CommissionRate { get; set; }

        [Required(ErrorMessage = "Commission Quantity is required")]
        public int CommissionQuantity { get; set; }

        [Required(ErrorMessage = "Commission Amount is required")]
        public decimal CommissionAmount { get; set; }

        [Required(ErrorMessage = "Excess Balance is required")]
        public decimal ExcessBalance { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        public decimal UnitPrice { get; set; }

        public bool IsNewRecord { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
    }

    public class ApproveCardRequisitionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Requisition Id is required")]
        public long CardRequisitionId { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Comment is required")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Approval Comment must be between 1 and 150 characters")]
        public string ApproverComment { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDenied { get; set; }
    }

    public class IssueCardRequisitionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Sales Requisition Id is required")]
        public long CardRequisitionId { get; set; }

        public List<IssueCardRequisitionItemObj> RequisitionItems { get; set; }
    }

    public class IssueCardRequisitionItemObj
    {
        [CheckNumber(0, ErrorMessage = "Requisition Item Id is required")]
        public long CardRequisitionItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity Requested  is required")]
        public int QuantityIssued { get; set; }

        [StringLength(350)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Issued Batch Range Is Required ")]
        public string BatchRange { get; set; }

        public bool IsIssued { get; set; }


    }

    public class LoadCardRequisitionByDateObj : AdminObj
    {

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string BeginDate { get; set; }

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }
    }

    public class DeleteCardRequisitionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Sales Requisition Id is required")]
        public long CardRequisitionId { get; set; }

    }
    #endregion

    #region Beneficiary
    public class RegBeneficiaryObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters")]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Beneficiary Mobile Number is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid Beneficiary Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Beneficiary Email is required")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Company Name is required")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 150 characters")]
        public string Address { get; set; }

        public int BeneficiaryType { get; set; }

        public int Status { get; set; }

    }

    public class EditBeneficiaryObj : AdminObj
    {

        [CheckNumber(0, ErrorMessage = "Beneficiary Id is required")]
        public int BeneficiaryId { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Full Name must be between 5 and 100 characters")]
        public string FullName { get; set; }

        
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid Beneficiary Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [StringLength(150, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 150 characters")]
        public string Address { get; set; }

        public int BeneficiaryType { get; set; }

        public int Status { get; set; }
    }

    public class DeleteBeneficiaryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Commission Id is required")]
        public int BeneficiaryId { get; set; }
    }

    public class ApproveBeneficiaryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Beneficiary Id is required")]
        public long BeneficiaryId { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Comment is required")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Approval Comment must be between 1 and 150 characters")]
        public string ApproverComment { get; set; }
    }
    #endregion

    #region Card Production

    public class RegCardObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Key is required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Batch Key must be 2 characters")]
        public string BatchKey { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Start Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Start Batch Id must be 5 characters")]
        public string StartBatchId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Stop Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Stop Batch Id must be 5 characters")]
        public string StopBatchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Number Of Batches Is Required")]
        public int NumberOfBatches { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity Per Batch Is Required")]
        public int QuantityPerBatch { get; set; }
    }

    public class EditCardObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card  Id is required")]
        public int CardId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Item Id is required")]
        public int CardItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(ErrorMessage = "Missing Quantity Found is required")]
        public int MissingQuantityFound { get; set; }

        [Required(ErrorMessage = "Missing Quantity Found is required")]
        public int DefectiveQuantityRectified { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = " Defective Batch Numbers! is required")]
        [StringLength(500, ErrorMessage = "Defective Batch Numbers! is required")]
        public string UpdatedDefectiveBatchNumbers { get; set; }
    }

    public class LoadCardByDateObj : AdminObj
    {

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string BeginDate { get; set; }

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }
    }

    public class DeleteCardObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Id is required")]
        public long CardId { get; set; }

    }

    #endregion

    #region Beneficiary Payment

    public class RegBeneficiaryPaymentObj : AdminObj
    {
        [CheckNumber(0,ErrorMessage = "Beneficiary Id is required")]
        public int BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Amount Paid is required")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment Source is required")]
        public int PaymentSource { get; set; }

        [Required(ErrorMessage = "Transaction Source Id is required")]
        public int TransactionSourceId { get; set; }

        [Required(ErrorMessage = "Payment Reference is required")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Invalid Payment Reference ")]
        public string PaymentReference { get; set; }

    }

    public class LoadBeneficiaryPaymentsByDateObj : AdminObj
    {

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string BeginDate { get; set; }

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }

        public int BeneficiaryId { get; set; }
    }

    public class LoadBeneficiaryAccountTransactionsByDateObj : AdminObj
    {

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string BeginDate { get; set; }

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }

        public int BeneficiaryId { get; set; }
    }

    public class DeleteBeneficiaryPaymentObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Beneficiary Payment Id is required")]
        public long BeneficiaryPaymentId { get; set; }
    }

    public class DeleteBeneficiaryAccountTransactionObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Beneficiary Account Transaction Id is required")]
        public long BeneficiaryAccountTransactionId { get; set; }

    }

    #endregion

    #region Card Delivery

    public class RegCardDeliveryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Item Id is required")]
        public long CardItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Id is required")]
        public int CardId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Id is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Batch Id must be 5 characters")]
        public string BatchId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = " Start Batch Number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = " Start Batch Number must be between 7 and 12 characters")]
        public string StartBatchNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Stop Batch Number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Stop Batch Number must be between 7 and 12 characters")]
        public string StopBatchNumber { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = " Defective Batch Numbers! is required")]
        [StringLength(500,ErrorMessage = "Defective Batch Numbers! is required")]
        public string DefectiveBatchNumbers { get; set; }

        [Required(ErrorMessage = "Quantity Delivered is required")]
        public int QuantityDelivered { get; set; }

        [Required(ErrorMessage = "Missing Quantity is required")]
        public int MissingQuantity { get; set; }

        [Required(ErrorMessage = "Defective Quantity is required")]
        public int DefectiveQuantity { get; set; }

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string DeliveryDate { get; set; }
    }

    public class EditCardDeliveryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Delivery Id is required")]
        public long CardDeliveryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Item Id is required")]
        public long CardItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Id is required")]
        public int CardId { get; set; }

        [CheckNumber(0, ErrorMessage = "Card Type Id is required")]
        public int CardTypeId { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = " Defective Batch Numbers is required")]
        [StringLength(500)]
        public string DefectiveBatchNumbers { get; set; }

        [Required(ErrorMessage = "Quantity Delivered is required")]
        public int QuantityDelivered { get; set; }

        [Required(ErrorMessage = "Missing Quantity is required")]
        public int MissingQuantity { get; set; }

        [Required(ErrorMessage = "Defective Quantity is required")]
        public int DefectiveQuantity { get; set; }

        [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string DeliveryDate { get; set; }

    }

    public class ApproveCardDeliveryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Delivery Id is required")]
        public long CardDeliveryId { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Approval Comment is required")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Approval Comment must be between 1 and 150 characters")]
        public string ApproverComment { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDefective { get; set; }
    }

    public class DeleteCardDeliveryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Card Delivery Id is required")]
        public int CardDeliveryId { get; set; }
    }

    public class LoadCardDeliveryByDateObj : AdminObj
    {
        [StringLength(35, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string BeginDate { get; set; }

        [StringLength(35, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }

        public int CardId { get; set; }
    }

    #endregion

    #region Card Issuance
    public class RegCardIssuanceObj : AdminObj
    {
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Id is required")]
        [StringLength(7, MinimumLength = 5, ErrorMessage = "Batch Id must be 5 characters")]
        public string BatchId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = " Start Batch Number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = " Start Batch Number must be between 7 and 12 characters")]
        public string StartBatchNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Stop Batch Number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Stop Batch Number must be between 7 and 12 characters")]
        public string StopBatchNumber { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity Issued is required")]
        public int QuantityIssued { get; set; }

    }

    public class EditCardIssuanceObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Sales Retirement Id is required")]
        public long CardIssuanceId { get; set; }

        public long VolumeSold { get; set; }

        public decimal TotalCashAmount { get; set; }

        public decimal TotalBankAmount { get; set; }

        public decimal UnReconciledAmount { get; set; }

        public decimal TotalSystemOvercharge { get; set; }

    }
    public class LoadCardIssuanceByDateObj : AdminObj
    {
        [StringLength(35, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string BeginDate { get; set; }

        [StringLength(35, ErrorMessage = "Invalid Date - Time registered")]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }

        public int CardRequisitionId { get; set; }
    }
    #endregion

    //#region Sales Retirement Report
    //public class GetSalesRetirementReportObj : AdminObj
    //{
    //    [CheckNumber(0, ErrorMessage = "Sales Retirement Id is required")]
    //    public long SalesRetirementId { get; set; }
    //}

    //public class GetSalesRetirementsReportByDateObj : AdminObj
    //{

    //    [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
    //    [DataType(DataType.DateTime)]
    //    public string BeginDate { get; set; }

    //    [StringLength(35, MinimumLength = 7, ErrorMessage = "Invalid Date - Time registered")]
    //    [DataType(DataType.DateTime)]
    //    public string EndDate { get; set; }
    //}
    //#endregion

    public class SettingSearchObj : AdminObj
    {
        public int Status { get; set; }
    }

}
