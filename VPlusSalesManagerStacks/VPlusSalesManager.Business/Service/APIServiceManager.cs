using PlugPortalManager.Admin.DataManager;
using VPlusSalesManager.APIObjects.Settings;
using VPlusSalesManager.Business.Repository;
using VPlusSalesManager.Business.Repository.Production;
using VPlusSalesManager.Business.Repository.Transaction;

namespace VPlusSalesManager.Business.Service
{
    public class APIServiceManager
    {
        public static bool Migrate(out string msg)
        {
            return MigrationManager.Migrate(out msg);
        }

        #region Card Type
        public static SettingRegRespObj AddCardType(RegCardTypeObj regObj)
        {
            return new CardTypeRepository().AddCardType(regObj);
        }

        public static SettingRegRespObj UpdateCardType(EditCardTypeObj regObj)
        {
            return new CardTypeRepository().UpdateCardType(regObj);
        }

        public static SettingRegRespObj DeleteCardType(DeleteCardTypeObj searchObj)
        {
            return new CardTypeRepository().DeleteCardType(searchObj);
        }

        public static CardTypeRespObj LoadCardTypes(SettingSearchObj searchObj)
        {
            return new CardTypeRepository().LoadCardTypes(searchObj);
        }
        #endregion

        #region Card Commission Service
        public static SettingRegRespObj AddCardCommission(RegCardCommissionObj regObj)
        {
            return new CardCommissionRepository().AddCardCommission(regObj);
        }

        public static SettingRegRespObj UpdateCardCommission(EditCardCommissionObj regObj)
        {
            return new CardCommissionRepository().UpdateCardCommission(regObj);
        }

        public static SettingRegRespObj DeleteCardCommission(DeleteCardCommissionObj searchObj)
        {
            return new CardCommissionRepository().DeleteCardCommission(searchObj);
        }

        public static CardCommissionRespObj LoadCardCommissions(SettingSearchObj searchObj)
        {
            return new CardCommissionRepository().LoadCardCommissions(searchObj);
        }
        #endregion

        #region Beneficiary Service
        public static SettingRegRespObj AddBeneficiary(RegBeneficiaryObj regObj)
        {
            return new BeneficiaryRepository().AddBeneficiary(regObj);
        }

        public static SettingRegRespObj UpdateBeneficiary(EditBeneficiaryObj regObj)
        {
            return new BeneficiaryRepository().UpdateBeneficiary(regObj);
        }

        public static SettingRegRespObj DeleteBeneficiary(DeleteBeneficiaryObj searchObj)
        {
            return new BeneficiaryRepository().DeleteBeneficiary(searchObj);
        }
        public static SettingRegRespObj ApproveBeneficiary(ApproveBeneficiaryObj searchObj)
        {
            return new BeneficiaryRepository().ApproveBeneficiary(searchObj);
        }
        public static BeneficiaryRespObj LoadBeneficiaries(SettingSearchObj searchObj)
        {
            return new BeneficiaryRepository().LoadBeneficiaries(searchObj);
        }
        #endregion

        #region Beneficiary Payment Service
        public static BeneficiaryPaymentRegRespObj AddBeneficiaryPayment(RegBeneficiaryPaymentObj regObj)
        {
            return new BeneficiaryPaymentRepository().AddBeneficiaryPayment(regObj);
        }

        public static BeneficiaryPaymentRespObj LoadBeneficiaryPayments(SettingSearchObj searchObj)
        {
            return new BeneficiaryPaymentRepository().LoadBeneficiaryPayments(searchObj);
        }
        public static BeneficiaryPaymentRespObj LoadBeneficiaryPaymentsByDate(LoadBeneficiaryPaymentsByDateObj regObj)
        {
            return new BeneficiaryPaymentRepository().LoadBeneficiaryPaymentsByDate(regObj);
        }

        public static BeneficiaryAccountTransactionRespObj LoadBeneficiaryAccountTransactionsByDate(LoadBeneficiaryAccountTransactionsByDateObj searchObj)
        {
            return new BeneficiaryPaymentRepository().LoadBeneficiaryAccountTransactionsByDate(searchObj);
        }
      
        #endregion

        #region Card Requisition Service

        public static CardRequisitionRegRespObj AddCardRequisition(RegCardRequisitionObj newCardRequisition)
        {
            return new CardRequisitionRepository().AddCardRequisition(newCardRequisition);
        }

        public static CardRequisitionRegRespObj UpdateCardRequisition(EditCardRequisitionObj regObj)
        {
            return new CardRequisitionRepository().UpdateCardRequisition(regObj);
        }

        public static CardRequisitionRegRespObj ApproveCardRequisition(ApproveCardRequisitionObj regObj)
        {
            return new CardRequisitionRepository().ApproveCardRequisition(regObj);
        }

        public static SettingRegRespObj DeleteCardRequisition(DeleteCardRequisitionObj searchObj)
        {
            return new CardRequisitionRepository().DeleteCardRequisition(searchObj);
        }

        public static CardRequisitionRespObj LoadCardRequisitions(SettingSearchObj searchObj)
        {
            return new CardRequisitionRepository().LoadCardRequisitions(searchObj);
        }

        public static CardRequisitionRespObj LoadCardRequisitionByDate(LoadCardRequisitionByDateObj searchObj)
        {
            return new CardRequisitionRepository().LoadCardRequisitionByDate(searchObj);
        }

        #endregion

        #region Card Production Service
        public static CardRegRespObj AddCard(RegCardObj newCard)
        {
            return new CardRepository().AddCard(newCard);
        }
        public static CardRegRespObj UpdateCard(EditCardObj newCard)
        {
            return new CardRepository().UpdateCard(newCard);
        }

        public static SettingRegRespObj DeleteCard(DeleteCardObj searchObj)
        {
            return new CardRepository().DeleteCard(searchObj);
        }

        public static CardRespObj LoadCards(SettingSearchObj searchObj)
        {
            return new CardRepository().LoadCards(searchObj);
        }

        public static CardRespObj LoadCardByDate(LoadCardByDateObj searchObj)
        {
            return new CardRepository().LoadCardByDate(searchObj);
        }
        #endregion

        #region Card Delivery Service
        public static CardDeliveryRegRespObj AddCardDelivery(RegCardDeliveryObj newCard)
        {
            return new CardDeliveryRepository().AddCardDelivery(newCard);
        }

        public static CardDeliveryRegRespObj UpdateCardDelivery(EditCardDeliveryObj newCardDelivery)
        {
            return new CardDeliveryRepository().UpdateCardDelivery(newCardDelivery);
        }
        public static CardDeliveryRespObj LoadCardDeliveriesByDate(LoadCardDeliveryByDateObj searchObj)
        {
            return new CardDeliveryRepository().LoadCardDeliveryByDate(searchObj);
        }
        public static CardDeliveryRegRespObj ApproveCardDelivery(ApproveCardDeliveryObj regObj)
        {
            return new CardDeliveryRepository().ApproveCardDelivery(regObj);
        }
        #endregion

        #region Card Issuance Service
        public static CardIssuanceRegRespObj AddCardIssuance(RegCardIssuanceObj newCard)
        {
            return new CardIssuanceRepository().AddCardIssuance(newCard);
        }
        public static CardIssuanceRespObj LoadCardIssuanceByDate(LoadCardIssuanceByDateObj searchObj)
        {
            return new CardIssuanceRepository().LoadCardIssuanceByDate(searchObj);
        }
        #endregion

        //#region Sales Retirement Service
        //public static SalesRetirementRegRespObj AddSalesRetirement(RegSalesRetirementObj newSalesRequisition)
        //{
        //    return new SalesRetirementRepository().AddSalesRetirement(newSalesRequisition);
        //}
        //public static SalesRetirementRegRespObj UpdateSalesRetirement(EditSalesRetirementObj regObj)
        //{
        //    return new SalesRetirementRepository().UpdateSalesRetirement(regObj);
        //}

        //public static SalesRetirementRegRespObj ApproveSalesRetirement(ApproveSalesRetirementObj regObj)
        //{
        //    return new SalesRetirementRepository().ApproveSalesRetirement(regObj);
        //}
        //public static SalesRetirementRespObj LoadSalesRetirements(SettingSearchObj searchObj)
        //{
        //    return new SalesRetirementRepository().LoadSalesRetirements(searchObj);
        //}

        //public static SettingRegRespObj DeleteSalesRetirement(DeleteSalesRetirementObj searchObj)
        //{
        //    return new SalesRetirementRepository().DeleteSalesRetirement(searchObj);
        //}
        //public static SalesRetirementRespObj LoadSalesRetirementByDate(LoadSalesRetirementByDateObj searchObj)
        //{
        //    return new SalesRetirementRepository().LoadSalesRetirementByDate(searchObj);
        //}
        //#endregion


        //#region Card Delivery Service
        //public static CardDeliveryRegRespObj AddCardDelivery(RegCardDeliveryObj newCardDelivery)
        //{
        //    return new CardDeliveryRepository().AddCardDelivery(newCardDelivery);
        //}

        //public static CardDeliveryRegRespObj ApproveCardDelivery(ApproveCardDeliveryObj CardDelivery)
        //{
        //    return new CardDeliveryRepository().ApproveCardDelivery(CardDelivery);
        //}

        //public static SettingRegRespObj DeleteCardDelivery(DeleteCardDeliveryObj searchObj)
        //{
        //    return new CardDeliveryRepository().DeleteCardDelivery(searchObj);
        //}

        //public static CardDeliveryRespObj LoadCardDeliveries(SettingSearchObj searchObj)
        //{
        //    return new CardDeliveryRepository().LoadCardDeliveries(searchObj);
        //}

        //public static CardDeliveryRespObj LoadCardDeliveryByDate(LoadCardDeliveryByDateObj searchObj)
        //{
        //    return new CardDeliveryRepository().LoadCardDeliveryByDate(searchObj);
        //}
        //#endregion

        //#region Sales Retirement Report Service

        //public static SalesRetirementsReportRespObj GetSalesRetirementReportByDate(GetSalesRetirementsReportByDateObj dateRequest)
        //{
        //    return new RetirementReportRepository().GetSalesRetirementsReportByDate(dateRequest);
        //}

        //#endregion

    }
}
