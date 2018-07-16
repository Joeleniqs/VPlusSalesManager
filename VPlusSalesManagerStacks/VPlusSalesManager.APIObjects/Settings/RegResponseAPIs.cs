using System.Collections.Generic;
using VPlusSalesManager.APIObjects.Common;

namespace VPlusSalesManager.APIObjects.Settings
{
    public class SettingRegRespObj
    {
        public int SettingId;
        public APIResponseStatus Status;
    }

    #region Card Type Response Object
    public class CardTypeRespObj
    {
        public List<CardTypeItemObj> CardTypes;
        public APIResponseStatus Status;
    }

    public class CardTypeItemObj
    {
        public int CardTypeId;
        public string Name;
        public decimal FaceValue;
        public int CardStatus;
        public string CardStatusLabel;
    }
    #endregion

    #region Card Commission Response Object
    public class CardCommissionRespObj
    {
        public List<CardCommissionItemObj> CardCommissions;
        public APIResponseStatus Status;
    }

    public class CardCommissionItemObj
    {
        public int CardCommissionId;
        public int CardTypeId;
        public string CardTypeName;
        public decimal LowerAmount;
        public decimal UpperAmount;
        public decimal CommissionRate;
        public int Status;
        public string StatusLabel;
    }
    #endregion

    #region Beneficiary Response Object
    public class BeneficiaryRespObj
    {
        public List<BeneficiaryItemObj> Beneficiaries;
        public APIResponseStatus Status;
    }

    public class BeneficiaryItemObj
    {
        public int BeneficiaryId;
        public string FullName;
        public string MobileNumber;
        public string Address;
        public string EmailAddress;
        public string TimeStampRegistered;

        public int ApprovedBy;
        public string ApproverComment;
        public string TimeStampApproved;

        public int BeneficiaryType;
        public string BeneficiaryTypeLabel;

        public int Status;
        public string StatusLabel;

        public BeneficiaryAccountObj BeneficiaryAccount;

    }
    public class BeneficiaryAccountObj
    {
        public int BeneficiaryAccountId;
        public int BeneficiaryId;

        public decimal AvailableBalance;
        public decimal CreditLimit;
        public decimal LastTransactionAmount;
        public int LastTransactionType;
        public string LastTransactionTypeLabel;

    }
    #endregion

    #region Card Requisition Response Object
    public class CardRequisitionRegRespObj
    {
        public long CardRequisitionId;
        public APIResponseStatus Status;
    }

    public class CardRequisitionRespObj
    {
        public List<CardRequisitionObj> CardRequisitions;
        public APIResponseStatus Status;
    }

    public class CardRequisitionObj
    {
        public long CardRequisitionId;
        public string RequisitionTitle;
        public int BeneficiaryId;
        public string BeneficiaryName;
        public int TotalQuantityRequested;
        public int RequestedBy;
        public string TimeStampRequested;
        public int ApprovedBy;
        public string ApproverComment;
        public string TimeStampApproved;
        public int QuantityApproved;
        public int Status;
        public string StatusLabel;
        public List<CardRequisitionItemRespObj> CardRequisitionItems;
    }

    public class CardRequisitionItemRespObj
    {
        public long CardRequisitionItemId;
        public long CardRequisitionId;

        public int CardCommissionId;
        public int BeneficiaryId;
        public string BeneficiaryName;

        public int CardTypeId;
        public string CardTypeName;

        public decimal Amount;
        public int Quantity;
        public decimal CommissionRate;
        public int CommissionQuantity;
        public decimal CommissionAmount;
        public decimal ExcessBalance;
        public decimal UnitPrice;
        public int RequestedBy;

        public string TimeStampRequested;
        public int ApprovedBy;
        public int QuantityApproved;
        public string TimeStampApproved;

        public int Status;
        public string StatusLabel;
    }


    #endregion

    #region Beneficiary Payment Response Object

    public class BeneficiaryPaymentRegRespObj
    {
        public long PaymentId;
        public APIResponseStatus Status;
    }
    public class BeneficiaryAccountTransactionRegRespObj
    {
        public long TransactionId;
        public APIResponseStatus Status;
    }
    public class BeneficiaryPaymentRespObj
    {
        public APIResponseStatus Status;
        public List<BeneficiaryPaymentObj> BeneficiaryPayments { get; set; }
    }
    public class BeneficiaryPaymentObj
    {
        public long BeneficiaryPaymentId;
        public long BeneficiaryAccountTransactionId;
        public int BeneficiaryAccountId;
        public int BeneficiaryId;
        public decimal AmountPaid;
        public int PaymentSource;
        public string PaymentSourceName;
        public string PaymentReference;
        public string PaymentDate;
        public int RegisteredBy;
        public string TimeStampRegistered;
        public int Status;
        public string StatusLabel;
    }



    #endregion

    #region Beneficiary Account Transaction Response Object

    public class BeneficiaryAccountTransactionRespObj
    {
        public APIResponseStatus Status;
        public List<BeneficiaryAccountTransactionObj> BeneficiaryAccountTransactions { get; set; }
    }

    public class BeneficiaryAccountTransactionObj
    {
        public int BeneficiaryAccountId;
        public int BeneficiaryId;
        public decimal Amount;
        public decimal PreviousBalance;
        public decimal NewBalance;
        public int RegisteredBy;
        public string TimeStampRegistered;
        public int TransactionType;
        public string TransactionTypeLabel;
        public int TransactionSource;
        public string TransactionSourceLabel;
        public int Status;
        public string StatusLabel;
    }
    #endregion

    #region Card Issuance Response Object
    public class CardIssuanceRegRespObj
    {
        public long CardIssuanceId;
        public APIResponseStatus Status;
    }

    public class CardIssuanceRespObj
    {
        public List<CardIssuanceObj> CardIssuance;
        public APIResponseStatus Status;
    }

    public class CardIssuanceObj
    {
        public long CardIssuanceId;
        public long CardRequisitionId;
        public string CardRequisitionLabel;
        public long CardRequisitionItemId;
        public int CardTypeId;
        public int BeneficiaryId;
        public string BeneficiaryLabel;
        public long CardItemId;
        public string BatchId;
        public string StartBatchNumber;
        public string StopBatchNumber;
        public int QuantityIssued;
        public int IssuedBy;
        public string TimeStampIssued;

    }

    #endregion

    #region Card Production Response Object

    public class CardRegRespObj
    {
        public long CardId;
        public APIResponseStatus Status;
    }

    public class CardRespObj
    {
        public List<CardObj> Cards;
        public APIResponseStatus Status;
    }

    public class CardObj
    {
        public int CardId;
        public int CardTypeId;
        public string CardTitle;

        public string BatchKey;
        public string StartBatchId;
        public string StopBatchId;
        public int NoOfBatches;
        public int QuantityPerBatch;
        public int TotalQuantity;

        public string TimeStampRegistered;

        public int Status;
        public string StatusLabel;
        public List<CardItemObj> CardItems;
    }
    public class CardItemObj
    {
        public long CardItemId;
        public int CardId;
        public int CardTypeId;

        public string BatchId;
        public string BatchStartNumber;
        public string BatchStopNumber;
        public string DefectiveBatchNumbers;
        public int BatchQuantity;
        public int DefectiveQuantity;

        public int MissingQuantity;
        public int DeliveredQuantity;
        public int AvailableQuantity;
        public int IssuedQuantity;

        public int RegisteredBy;
        public string TimeStampRegistered;
        public string TimeStampDelivered;
        public string TimeStampIssued;

        public int Status;
        public string StatusLabel;
    }
    #endregion

    #region Card Delivery Response Object
    public class CardDeliveryRegRespObj
    {
        public long CardDeliveryId;
        public APIResponseStatus Status;
    }

    public class CardDeliveryRespObj
    {
        public List<CardDeliveryObj> CardDeliveries;
        public APIResponseStatus Status;
    }

    public class CardDeliveryObj
    {
        public long CardDeliveryId;
        public long CardItemId;
        public int CardId;
        public string CardIdLabel { get; set; }
        public int CardTypeId;
        public string BatchId;
        public string StartBatchNumber;
        public string StopBatchNumber;
        public int QuantityDelivered;
        public int MissingQuantity;
        public int DefectiveQuantity;
        public string DeliveryDate;
        public string TimeStampRegistered;
        public int ApprovedBy;
        public string ApproverComment;
        public string TimeStampApproved;
        public int RecievedBy;
        public int Status;
        public string StatusLabel;
    }


    #endregion

    #region Sales Retirement Report
    public class SalesRetirementsReportRespObj
    {
        public SalesRetirementsStatisticsObj SalesRetirementsStatistics;
        public List<CardDataReportObj> CardReport;
        public List<SalesRetirementsReportObj> SalesRetirementsReport;
        public APIResponseStatus Status;
    }

    public class SalesRetirementsStatisticsObj
    {
        public string BestSellingCard;
        public string MostProfitableCard;
        public int NoOfSalesRetirement;

    }

    public class CardDataReportObj
    {
        public long CardTypeId;
        public int VolumeCollected;
        public int VolumeSold;
        public decimal TotalAmountSold;
    }

    public class SalesRetirementsReportObj
    {
        public long SalesRetirementId;
        public string RetirementCode;
        public decimal SingleCardAmount;
        public decimal SpecialPackAmount;
        public decimal GoldCardAmount;
        public decimal DiamondCardAmount;
        public decimal RetirementTotalAmount;
    }
    #endregion
}
