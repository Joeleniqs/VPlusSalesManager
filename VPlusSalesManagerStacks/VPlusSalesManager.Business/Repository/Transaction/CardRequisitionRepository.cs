using PlugPortalManager.Admin.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPlusSalesManager.APIObjects.Common;
using VPlusSalesManager.APIObjects.Settings;
using VPlusSalesManager.Business.Core;
using VPlusSalesManager.Business.Infrastructure;
using VPlusSalesManager.Business.Infrastructure.Contract;
using VPlusSalesManager.BusinessObject.Setting;
using VPlusSalesManager.BusinessObject.Transaction;
using VPlusSalesManager.Common;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManager.Business.Repository.Transaction
{
    internal class CardRequisitionRepository
    {
        private readonly IVPlusSalesManagerRepository<CardRequisition> _repository;
        private readonly IVPlusSalesManagerRepository<CardRequisitionItem> _itemRepository;
        private readonly IVPlusSalesManagerRepository<CardCommission> _commissionRepository;
        private readonly IVPlusSalesManagerRepository<BeneficiaryAccount> _beneficiaryAccountRepository;
        private readonly IVPlusSalesManagerRepository<Beneficiary> _beneficiaryRepository;
        private readonly IVPlusSalesManagerRepository<BeneficiaryAccountTransaction> _transactRepository;


        private readonly VPlusSalesManagerUoWork _uoWork;

        public CardRequisitionRepository()
        {
            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<CardRequisition>(_uoWork);
            _itemRepository = new VPlusSalesManagerRepository<CardRequisitionItem>(_uoWork);
            _commissionRepository = new VPlusSalesManagerRepository<CardCommission>(_uoWork);
            _beneficiaryAccountRepository = new VPlusSalesManagerRepository<BeneficiaryAccount>(_uoWork);
            _beneficiaryRepository = new VPlusSalesManagerRepository<Beneficiary>(_uoWork);
            _transactRepository = new VPlusSalesManagerRepository<BeneficiaryAccountTransaction>(_uoWork);

        }

        public CardRequisition GetCardRequisition(long cardRequisitionId)
        {
            try
            {
                return getCardRequisitionInfo(cardRequisitionId) ?? new CardRequisition();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardRequisition();
            }
        }

        public List<CardRequisition> GetCardRequisitions()
        {
            try
            {
                var myItemList = _repository.GetAll().OrderBy(m => m.CardRequisitionId);
                if (!myItemList.Any()) return new List<CardRequisition>();
                var settings = myItemList.ToList();
                if (settings.IsNullOrEmpty()) { return new List<CardRequisition>(); }
                return settings;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardRequisition>();
            }
        }

        public CardRequisitionRegRespObj AddCardRequisition(RegCardRequisitionObj regObj)
        {
            var response = new CardRequisitionRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };

            try
            {
                if (regObj?.CardRequisitionItems == null || !regObj.CardRequisitionItems.Any())
                {
                    response.Status.Message.FriendlyMessage = "Empty Request Item! Please try again later";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (!EntityValidatorHelper.Validate(regObj, out var valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                if (regObj.CardRequisitionItems == null || !regObj.CardRequisitionItems.Any())
                {
                    response.Status.Message.FriendlyMessage = "Empty Request Item List! Please try again later";
                    response.Status.Message.TechnicalMessage = "Empty Request Item List! Please try again later";
                    return response;
                }

                foreach (var reqitem in regObj.CardRequisitionItems)
                {
                    if (!EntityValidatorHelper.Validate(reqitem, out var valItemResults))
                    {
                        var errorDetail = new StringBuilder();
                        if (!valResults.IsNullOrEmpty())
                        {
                            errorDetail.AppendLine("Following error occurred:");
                            valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                        }
                        else
                        {
                            errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                        }
                        response.Status.Message.FriendlyMessage = errorDetail.ToString();
                        response.Status.Message.TechnicalMessage = errorDetail.ToString();
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                }

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, HelperMethods.getRequesterRoles(), ref response.Status.Message))
                {
                    return response;
                }

                var collectedSalesList = regObj.CardRequisitionItems
                                               .OrderBy(SaleItem => SaleItem.CardTypeId)
                                               .ToList();

                if (!collectedSalesList.Any())
                {
                    response.Status.Message.FriendlyMessage = "Invalid Card Requisition Item list";
                    response.Status.Message.TechnicalMessage = "Invalid Card Requisition Item list";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var itemList = new List<CardRequisitionItem>();

                //store date for Concurrency...
                var nowDateTime = DateMap.CurrentTimeStamp();
                var nowDate = nowDateTime.Substring(0, nowDateTime.IndexOf(' '));
                var nowTime = nowDateTime.Substring(nowDateTime.IndexOf('-') + 1);

                //Check Item duplicate
                foreach (var reqitem in collectedSalesList)
                {
                    //Check CardTypeId and Kind Count
                    if (collectedSalesList.Where(s => s.CardTypeId == reqitem.CardTypeId).Count() > 1)
                    {
                        response.Status.Message.FriendlyMessage = $"You cannot add another {getCardTypeInfo(reqitem.CardTypeId).Name} as there exist a duplicate!";
                        response.Status.Message.TechnicalMessage = $"Duplicate Card Requisition Item With Respect to Card Type Name";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    //check total amount validation
                    if (reqitem.Amount != (reqitem.Quantity * getCardTypeInfo(reqitem.CardTypeId).FaceValue))
                    {
                        response.Status.Message.FriendlyMessage = $"The Item's Amount is InCorrect";
                        response.Status.Message.TechnicalMessage = $"The Item's Amount is InCorrect";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    var CommissionObj = CommissionOperation(reqitem.Amount, reqitem.CardTypeId);
                    if (CommissionObj == null)
                    {
                        response.Status.Message.FriendlyMessage = "Card Commission Must Be Considered! Please Contact Administrator";
                        response.Status.Message.TechnicalMessage = "Card Type Commission Error";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    if (reqitem.CommissionQuantity != CommissionObj.CommissionQuantity)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Card Commission Quantity Data";
                        response.Status.Message.TechnicalMessage = "Invalid Card Commission Quantity Data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    if (reqitem.ExcessBalance != CommissionObj.ExcessBalance)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Card Commission Excess Balance Data";
                        response.Status.Message.TechnicalMessage = "Invalid Card Commission Excess Balance Data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    if (reqitem.CommissionAmount != CommissionObj.CommissionAmount)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Card Commission Amount Data";
                        response.Status.Message.TechnicalMessage = "Invalid Card Commission Amount Data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    if (reqitem.CommissionRate != CommissionObj.CommissionRate)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Card Commission Rate Data";
                        response.Status.Message.TechnicalMessage = "Invalid Card Commission Rate Data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    itemList.Add(new CardRequisitionItem
                    {
                        BeneficiaryId = regObj.BeneficiaryId,
                        CardTypeId = reqitem.CardTypeId,
                        Amount = reqitem.Amount,
                        Quantity = reqitem.Quantity,
                        UnitPrice = getCardTypeInfo(reqitem.CardTypeId).FaceValue,
                        CommissionAmount = CommissionObj.CommissionAmount,
                        CommissionQuantity = CommissionObj.CommissionQuantity,
                        CommissionRate = CommissionObj.CommissionRate,
                        ExcessBalance = CommissionObj.ExcessBalance,
                        CardCommissionId = CommissionObj.CardCommissionId,
                        ApprovedBy = 0,
                        QuantityApproved = 0,
                        TimeStampApproved = " ",
                        RequestedBy = regObj.AdminUserId,
                        TimeStampRequested = nowDateTime,
                        Status = CardRequisitionStatus.Registered
                    });
                }

                //Requisition's total Quantity
                var totalQuantity = itemList.Sum(s => s.Quantity);

                var item = new CardRequisition
                {
                    RequisitionTitle = $"Card Requisition On {nowDate} At {nowTime}",
                    BeneficiaryId = regObj.BeneficiaryId,
                    TotalQuantityRequested = totalQuantity,
                    QuantityApproved = itemList.Sum(s => s.QuantityApproved),
                    RequestedBy = regObj.AdminUserId,
                    TimeStampRequested = nowDateTime,
                    ApproverComment = " ",
                    ApprovedBy = 0,
                    TimeStampApproved = " ",
                    CardRequisitionItems = itemList,
                    Status = CardRequisitionStatus.Registered


                };

                var added = _repository.Add(item);

                _uoWork.SaveChanges();
                if (added.CardRequisitionId < 1 || added.CardRequisitionItems.Count < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }



                response.Status.IsSuccessful = true;
                response.CardRequisitionId = added.CardRequisitionId;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }

            return response;

        }

        public CardRequisitionRegRespObj UpdateCardRequisition(EditCardRequisitionObj regObj)
        {
            var response = new CardRequisitionRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };
            try
            {
                if (regObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (!EntityValidatorHelper.Validate(regObj, out var valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                if (regObj.RequisitionItems == null || !regObj.RequisitionItems.Any())
                {
                    response.Status.Message.FriendlyMessage = "Empty Request Item List! Please try again later";
                    response.Status.Message.TechnicalMessage = "Empty Request Item List! Please try again later";
                    return response;
                }

                foreach (var reqitem in regObj.RequisitionItems)
                {
                    if (!EntityValidatorHelper.Validate(reqitem, out var valItemResults))
                    {
                        var errorDetail = new StringBuilder();
                        if (!valItemResults.IsNullOrEmpty())
                        {
                            errorDetail.AppendLine("Following error occurred:");
                            valItemResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                        }
                        else
                        {
                            errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                        }
                        response.Status.Message.FriendlyMessage = errorDetail.ToString();
                        response.Status.Message.TechnicalMessage = errorDetail.ToString();
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    if (reqitem.IsNewRecord && reqitem.IsUpdated && reqitem.IsDeleted)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Sales Item list";
                        response.Status.Message.TechnicalMessage = "Invalid Sales Item list";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    else if (reqitem.IsDeleted && reqitem.IsUpdated || reqitem.IsUpdated && reqitem.IsNewRecord || reqitem.IsDeleted && reqitem.IsNewRecord)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Sales Item list";
                        response.Status.Message.TechnicalMessage = "Invalid Sales Item list";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                }

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, HelperMethods.getRequesterRoles(), ref response.Status.Message))
                {
                    return response;
                }

                var updatedItemList = new List<CardRequisitionItem>();

                //Check Item duplicate
                var collectedUpdateList = regObj.RequisitionItems
                                               .OrderBy(SaleItem => SaleItem.CardTypeId)
                                               .ToList();

                if (!collectedUpdateList.Any())
                {
                    response.Status.Message.FriendlyMessage = "Invalid Card Requisition Item list";
                    response.Status.Message.TechnicalMessage = "Invalid Card Requisition Item list";
                    response.Status.IsSuccessful = false;
                    return response;
                }


                foreach (var reqitem in regObj.RequisitionItems)
                {
                    //Check CardTypeId Duplicate
                    if (collectedUpdateList.Where(s => s.CardTypeId == reqitem.CardTypeId).Count() > 1)
                    {
                        response.Status.Message.FriendlyMessage = $"You cannot add another \"{getCardTypeInfo(reqitem.CardTypeId).Name}\" as there exist a duplicate!";
                        response.Status.Message.TechnicalMessage = $"Duplicate card Requisition Item With Respect to Card Type Name";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                }

                var thisCardRequisition = getCardRequisitionInfo(regObj.CardRequisitionId);

                if (thisCardRequisition == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Information found for the specified CardRequisition Id";
                    response.Status.Message.TechnicalMessage = "No Card Requisition Information found!";
                    return response;
                }

                if (thisCardRequisition.Status != CardRequisitionStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This Card Requisition Is Not Valid For Update! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " Card Requisition Status is either already Approved/Issued/Closed!";
                    return response;
                }


                var newItems = regObj.RequisitionItems.FindAll(m => m.IsNewRecord);
                var updateItems = regObj.RequisitionItems.FindAll(m => m.IsUpdated);
                var deletedItems = regObj.RequisitionItems.FindAll(m => m.IsDeleted);

                if (deletedItems.Count == regObj.RequisitionItems.Count)
                {
                    response.Status.Message.FriendlyMessage = "Sorry You Can't Have An Empty Requisition";
                    response.Status.Message.TechnicalMessage = "User Attempted to delete All Items ";
                    return response;
                }

                var newItemList = new List<CardRequisitionItem>();
                var updatedItemsList = new List<CardRequisitionItem>();

                var errString = new StringBuilder();
                if (newItems.Any())
                {

                    foreach (var reqItem in newItems)
                    {
                        if (CheckInitialDuplicate(regObj.CardRequisitionId, reqItem.CardTypeId))
                        {
                            response.Status.Message.FriendlyMessage = $"You cannot add another {getCardTypeInfo(reqItem.CardTypeId).Name} as there exist a duplicate!";
                            response.Status.Message.TechnicalMessage = $"Duplicate Card Requisition Item With Respect to Card Type Name";
                            response.Status.IsSuccessful = false;
                            return response;
                        }

                        var CommissionObj = CommissionOperation(reqItem.Amount, reqItem.CardTypeId);
                        if (CommissionObj == null)
                        {
                            response.Status.Message.FriendlyMessage = "Card Commission Must Be Considered! Please Contact Administrator";
                            response.Status.Message.TechnicalMessage = "Card Type Commission Error";
                            response.Status.IsSuccessful = false;
                            return response;
                        }

                        newItemList.Add(new CardRequisitionItem
                        {
                            BeneficiaryId = regObj.BeneficiaryId,
                            CardTypeId = reqItem.CardTypeId,
                            Amount = reqItem.Amount,
                            Quantity = reqItem.Quantity,
                            UnitPrice = reqItem.UnitPrice,
                            CommissionAmount = CommissionObj.CommissionAmount,
                            CommissionQuantity = CommissionObj.CommissionQuantity,
                            CommissionRate = CommissionObj.CommissionRate,
                            ExcessBalance = CommissionObj.ExcessBalance,
                            CardCommissionId = CommissionObj.CardCommissionId,
                            ApprovedBy = 0,
                            QuantityApproved = 0,
                            TimeStampApproved = " ",
                            RequestedBy = regObj.AdminUserId,
                            TimeStampRequested = DateMap.CurrentTimeStamp(),
                            Status = CardRequisitionStatus.Registered
                        });
                    }
                }

                if (updateItems.Any())
                {

                    foreach (var reqItem in updateItems)
                    {
                        var thisItem = getCardRequisitionItem(reqItem.CardRequisitionItemId) ?? new CardRequisitionItem();

                        if (thisItem.CardRequisitionId != regObj.CardRequisitionId) { continue; }


                        if (reqItem.Quantity > 0)
                        {
                            //checking Item Amount
                            if (reqItem.Amount != (reqItem.Quantity * getCardTypeInfo(reqItem.CardTypeId).FaceValue))
                            {
                                response.Status.Message.FriendlyMessage = $"The Item's Amount is InCorrect";
                                response.Status.Message.TechnicalMessage = $"The Item's Amount is InCorrect";
                                response.Status.IsSuccessful = false;
                                return response;
                            }

                            var CommissionObj = CommissionOperation(reqItem.Amount, reqItem.CardTypeId);
                            if (CommissionObj == null)
                            {
                                response.Status.Message.FriendlyMessage = "Card Commission Must Be Considered! Please Contact Administrator";
                                response.Status.Message.TechnicalMessage = "Card Type Commission Error";
                                response.Status.IsSuccessful = false;
                                return response;
                            }

                            thisItem.CardTypeId = reqItem.CardTypeId > 0 ? reqItem.CardTypeId : thisItem.CardTypeId;
                            thisItem.CommissionAmount = CommissionObj.CommissionAmount;
                            thisItem.CommissionQuantity = CommissionObj.CommissionQuantity;
                            thisItem.CommissionRate = CommissionObj.CommissionRate;
                            thisItem.ExcessBalance = CommissionObj.ExcessBalance;
                            thisItem.Amount = reqItem.Amount;
                            thisItem.UnitPrice = reqItem.CardTypeId > 0 ? getCardTypeInfo(reqItem.CardTypeId).FaceValue : thisItem.CardTypeId;
                        }


                        thisItem.BeneficiaryId = regObj.BeneficiaryId > 0 ? regObj.BeneficiaryId : thisItem.BeneficiaryId;

                        updatedItemList.Add(thisItem);
                    }
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    if (newItemList.Any())
                    {
                        var retVal = _itemRepository.AddRange(newItemList).ToList();
                        _uoWork.SaveChanges();
                        if (retVal.Count() != newItemList.Count())
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Unable to update Card Requisition";
                            response.Status.Message.TechnicalMessage = "Add New Item Failed!";
                            return response;
                        }
                    }

                    if (updatedItemList.Any())
                    {
                        var retVal = _itemRepository.UpdateRange(updatedItemList).ToList();
                        _uoWork.SaveChanges();
                        if (retVal.Count() != updatedItemList.Count())
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Unable to update Card Requisition";
                            response.Status.Message.TechnicalMessage = "Update Item Failed!";
                            return response;
                        }
                    }
                    if (deletedItems.Any())
                    {
                        foreach (var reqItem in deletedItems)
                        {
                            var thisItem = getCardRequisitionItem(reqItem.CardRequisitionItemId) ?? new CardRequisitionItem();
                            if (thisItem.CardRequisitionId != regObj.CardRequisitionId) { continue; }
                            if (!deleteCardRequisitionItem(reqItem.CardRequisitionItemId))
                            {
                                errString.AppendLine($"Sales Requisition Item for Item {getCardTypeInfo(reqItem.CardTypeId).Name} can not be deleted");
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = errString.ToString();
                                response.Status.Message.TechnicalMessage = "Update Item Failed!";
                                return response;
                            }
                        }

                    }

                    thisCardRequisition.BeneficiaryId = regObj.BeneficiaryId > 0 ? regObj.BeneficiaryId : thisCardRequisition.BeneficiaryId;

                    var added = _repository.Update(thisCardRequisition);
                    _uoWork.SaveChanges();
                    if (added.CardRequisitionId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }
                    db.Commit();

                }

                //Now Compute And Update Total Quantity Requested Of Individual Items
                thisCardRequisition.TotalQuantityRequested = getCardRequisitionItems(thisCardRequisition.CardRequisitionId).Sum(s => s.Quantity);
                var commit = _repository.Update(thisCardRequisition);
                _uoWork.SaveChanges();
                if (commit.CardRequisitionId < 1)
                {

                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to Update Total Amount to database";
                    return response;
                }

                response.Status.IsSuccessful = true;
                response.CardRequisitionId = commit.CardRequisitionId;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            return response;
        }

        public CardRequisitionRegRespObj ApproveCardRequisition(ApproveCardRequisitionObj regObj)
        {
            var response = new CardRequisitionRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };

            try
            {
                if (regObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (!EntityValidatorHelper.Validate(regObj, out var valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, HelperMethods.getMgtExecutiveRoles(), ref response.Status.Message))
                {
                    return response;
                }

                var thisCardRequisition = getCardRequisitionInfo(regObj.CardRequisitionId);
                if (thisCardRequisition == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Information found for the specified Card Requisition Id";
                    response.Status.Message.TechnicalMessage = "No Card Requisition Information found!";
                    return response;
                }

                if (thisCardRequisition.Status != CardRequisitionStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This Sale Requisition Is Not Valid For Approval! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " Sales Requisition Status is either already Active/Issued/Retired!";
                    return response;
                }

                //var validReqs = getCardRequisitions().FindAll(req => req.Status == CardRequisitionStatus.Approved);

                //if (validReqs.Count() > 0)
                //{
                //    response.Status.Message.FriendlyMessage = "This Item Is Not Yet Valid For Approval! Please Try Again Later.";
                //    response.Status.Message.TechnicalMessage = "There is An Existing Approved Requisition That Is Not Yet Issued! Please Try Again Later!";
                //    return response;
                //}

                var thisCardRequisitionItems = getCardRequisitionItems(regObj.CardRequisitionId);
                if (!thisCardRequisitionItems.Any())
                {
                    response.Status.Message.FriendlyMessage = "Requisition Items Of This Requisition Is Empty!Please Try Again Later.";
                    response.Status.Message.TechnicalMessage = "Requisition Items Of This Requisition Is Empty!Please Try Again Later.!";
                    return response;
                }

                if (regObj.IsApproved && regObj.IsDenied || !regObj.IsApproved && !regObj.IsDenied)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Operation!Please Try Again Later.";
                    response.Status.Message.TechnicalMessage = "Is Approved and Is Denied Cannot be both true/false!Please Try Again Later.!";
                    return response;
                }

                if (regObj.IsApproved)
                {
                    #region Check Beneficiary Funds
                    //Check Beneficiary Account
                    var associatedBeneficiary = new BeneficiaryRepository().GetBeneficiary(thisCardRequisition.BeneficiaryId);
                    if (associatedBeneficiary == null)
                    {
                        response.Status.Message.FriendlyMessage = "Beneficiary is not existent!Please Try Again Later.";
                        response.Status.Message.TechnicalMessage = "Beneficiary is not existent!Please Try Again Later..!";
                        return response;
                    }

                    var associatedBeneficiaryAccount = GetBeneficiaryAccount(associatedBeneficiary.BeneficiaryAccountId);

                    if (associatedBeneficiaryAccount == null)
                    {
                        response.Status.Message.FriendlyMessage = "Beneficiary Account is not existent!Please Try Again Later.";
                        response.Status.Message.TechnicalMessage = "Beneficiary Account is not existent!Please Try Again Later..!";
                        return response;
                    }

                    if (associatedBeneficiaryAccount.AvailableBalance < thisCardRequisitionItems.Sum(amount => amount.Amount))
                    {
                        response.Status.Message.FriendlyMessage = "Beneficiary's Fund Is Insufficient!Please Try Again Later.";
                        response.Status.Message.TechnicalMessage = "Beneficiary's Fund Is Insufficient!Please Try Again Later..!";
                        return response;
                    }

                    #endregion

                    using (var db = _uoWork.BeginTransaction())
                    {

                        #region Debit Beneficiary Account

                        #region Beneficiary Account Transaction Operation
                        var newBeneficiaryTransaction = new BeneficiaryAccountTransaction
                        {
                            BeneficiaryAccountId = associatedBeneficiaryAccount.BeneficiaryAccountId,
                            BeneficiaryId = thisCardRequisition.BeneficiaryId,
                            PreviousBalance = associatedBeneficiaryAccount.AvailableBalance,
                            Amount = thisCardRequisitionItems.Sum(amt => amt.Amount),
                            NewBalance = (associatedBeneficiaryAccount.AvailableBalance - (thisCardRequisitionItems.Sum(amt => amt.Amount))),
                            TransactionType = TransactionType.Debit,
                            TransactionSource = TransactionSourceType.Card_Purchase,
                            Status = Status.Active,
                            RegisteredBy = regObj.AdminUserId,
                            TimeStampRegistered = DateMap.CurrentTimeStamp()
                        };

                        var transactionAdded = _transactRepository.Add(newBeneficiaryTransaction);
                        _uoWork.SaveChanges();
                        if (transactionAdded.BeneficiaryAccountTransactionId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Beneficiary Account Update

                        associatedBeneficiaryAccount.AvailableBalance = transactionAdded.NewBalance;
                        associatedBeneficiaryAccount.LastTransactionAmount = transactionAdded.Amount;
                        associatedBeneficiaryAccount.LastTransactionType = transactionAdded.TransactionType;
                        associatedBeneficiaryAccount.Status = Status.Active;
                        associatedBeneficiaryAccount.LastTransactionId = transactionAdded.BeneficiaryAccountTransactionId;
                        associatedBeneficiaryAccount.LastTransactionTimeStamp = DateMap.CurrentTimeStamp();


                        var acctAdded = _beneficiaryAccountRepository.Update(associatedBeneficiaryAccount);
                        _uoWork.SaveChanges();
                        if (acctAdded.BeneficiaryAccountId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #endregion

                        #region Card Requisition Item Update

                        if (thisCardRequisitionItems.Any())
                        {
                            thisCardRequisitionItems.ForEachx(item =>
                            {
                                item.QuantityApproved = item.Quantity + item.CommissionQuantity;
                                item.Status = CardRequisitionStatus.Approved;
                                item.ApprovedBy = regObj.AdminUserId;
                                item.TimeStampApproved = DateMap.CurrentTimeStamp();
                            });

                            var retVal = _itemRepository.UpdateRange(thisCardRequisitionItems);
                            _uoWork.SaveChanges();
                            if (retVal.Count() < thisCardRequisitionItems.Count())
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Unable to update Sales Requisition Items";
                                response.Status.Message.TechnicalMessage = "Add New Item Failed!";
                                return response;
                            }
                        }
                        #endregion

                        #region Card Requisition Update
                        thisCardRequisition.Status = CardRequisitionStatus.Approved;
                        thisCardRequisition.ApprovedBy = regObj.AdminUserId;
                        thisCardRequisition.ApproverComment = regObj.ApproverComment;
                        thisCardRequisition.TimeStampApproved = DateMap.CurrentTimeStamp();
                        thisCardRequisition.QuantityApproved = thisCardRequisitionItems.Sum(q => q.Quantity) + thisCardRequisitionItems.Sum(q => q.CommissionQuantity);

                        var added = _repository.Update(thisCardRequisition);
                        _uoWork.SaveChanges();
                        if (added.CardRequisitionId < 1)
                        {
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        db.Commit();

                        response.Status.IsSuccessful = true;
                        response.CardRequisitionId = added.CardRequisitionId;
                        response.Status.Message.FriendlyMessage = "Approval Succesful! ";
                    }
                }

                if (regObj.IsDenied)
                {
                    using (var db = _uoWork.BeginTransaction())
                    {

                        if (thisCardRequisitionItems.Any())
                        {
                            thisCardRequisitionItems.ForEachx(item =>
                            {
                                item.QuantityApproved = 0;
                                item.Status = CardRequisitionStatus.Denied;
                                item.ApprovedBy = regObj.AdminUserId;
                                item.TimeStampApproved = DateMap.CurrentTimeStamp();
                            });

                            var retVal = _itemRepository.UpdateRange(thisCardRequisitionItems);
                            _uoWork.SaveChanges();
                            if (retVal.Count() < thisCardRequisitionItems.Count())
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Unable to update Sales Requisition Items";
                                response.Status.Message.TechnicalMessage = "Add New Item Failed!";
                                return response;
                            }
                        }

                        thisCardRequisition.Status = CardRequisitionStatus.Denied;
                        thisCardRequisition.ApprovedBy = regObj.AdminUserId;
                        thisCardRequisition.ApproverComment = regObj.ApproverComment;
                        thisCardRequisition.QuantityApproved = 0;
                        thisCardRequisition.TimeStampApproved = DateMap.CurrentTimeStamp();

                        var added = _repository.Update(thisCardRequisition);
                        _uoWork.SaveChanges();
                        if (added.CardRequisitionId < 1)
                        {
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }

                        db.Commit();

                        response.Status.IsSuccessful = true;
                        response.CardRequisitionId = added.CardRequisitionId;
                        response.Status.Message.FriendlyMessage = "Requisition Denied Successfully! ";
                    }

                }
            }
            catch (DbEntityValidationException ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            return response;
        }

        public CardRequisitionRespObj LoadCardRequisitionByDate(LoadCardRequisitionByDateObj regObj)
        {
            var response = new CardRequisitionRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };

            try
            {
                if (regObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (!EntityValidatorHelper.Validate(regObj, out var valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, HelperMethods.getRequesterRoles(), ref response.Status.Message))
                {
                    return response;
                }


                var thisCardRequisition = getCardRequisitions();

                if (!thisCardRequisition.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Requisition  Information found!";
                    return response;
                }

                var CardRequisitionByDate = new List<CardRequisitionObj>();

                foreach (var m in thisCardRequisition)
                {
                    var dateCreated = m.TimeStampRequested;
                    var value = dateCreated.IndexOf(' ');
                    if (value > 0) { dateCreated = dateCreated.Substring(0, value); }
                    var realDate = DateTime.Parse(dateCreated);
                    if (realDate >= DateTime.Parse(regObj.BeginDate) && realDate <= DateTime.Parse(regObj.EndDate))
                    {
                        var CardRequisitionItemsList = new List<CardRequisitionItemRespObj>();

                        var thisCardRequisitionItems = getCardRequisitionItems(m.CardRequisitionId);
                        if (!thisCardRequisitionItems.Any() || thisCardRequisitionItems == null)
                        {
                            response.Status.Message.FriendlyMessage = "At Least One Card Requisition's Item Information was not found!";
                            response.Status.Message.TechnicalMessage = "At Least One Card Requisition's Item Information was not found!";
                            continue;
                        }

                        if (thisCardRequisitionItems != null && thisCardRequisitionItems.Any())
                        {
                            thisCardRequisitionItems.ForEachx(reqItem =>
                            {

                                CardRequisitionItemsList.Add(new CardRequisitionItemRespObj
                                {
                                    CardRequisitionItemId = reqItem.CardRequisitionItemId,
                                    CardTypeId = reqItem.CardTypeId,
                                    CardTypeName = new CardTypeRepository().GetCardType(reqItem.CardTypeId).Name,
                                    UnitPrice = reqItem.UnitPrice,
                                    Quantity = reqItem.Quantity,
                                    Amount = reqItem.Amount,
                                    QuantityApproved = reqItem.QuantityApproved,
                                    BeneficiaryId = reqItem.BeneficiaryId,
                                    BeneficiaryName = new BeneficiaryRepository().GetBeneficiary(reqItem.BeneficiaryId).Fullname,
                                    CardCommissionId = reqItem.CardCommissionId,
                                    CardRequisitionId = reqItem.CardRequisitionId,
                                    CommissionAmount = reqItem.CommissionAmount,
                                    CommissionQuantity = reqItem.CommissionQuantity,
                                    CommissionRate = reqItem.CommissionRate,
                                    ExcessBalance = reqItem.ExcessBalance,
                                    RequestedBy = reqItem.RequestedBy,
                                    TimeStampRequested = reqItem.TimeStampRequested,
                                    ApprovedBy = reqItem.ApprovedBy,
                                    TimeStampApproved = reqItem.TimeStampApproved,
                                    Status = (int)reqItem.Status,
                                    StatusLabel = reqItem.Status.ToString().Replace("_", " ")
                                });
                            });
                        }
                        CardRequisitionByDate.Add(new CardRequisitionObj
                        {
                            CardRequisitionId = m.CardRequisitionId,
                            RequisitionTitle = m.RequisitionTitle,
                            BeneficiaryId = m.BeneficiaryId,
                            BeneficiaryName = new BeneficiaryRepository().GetBeneficiary(m.BeneficiaryId).Fullname,

                            TotalQuantityRequested = m.TotalQuantityRequested,

                            RequestedBy = m.RequestedBy,
                            TimeStampRequested = m.TimeStampRequested,

                            ApprovedBy = m.ApprovedBy,
                            TimeStampApproved = m.TimeStampApproved,
                            ApproverComment = m.ApproverComment,
                            QuantityApproved = m.QuantityApproved,

                            Status = (int)m.Status,
                            StatusLabel = m.Status.ToString().Replace("_", " "),
                            CardRequisitionItems = CardRequisitionItemsList

                        });
                    }
                }


                response.Status.IsSuccessful = true;
                response.CardRequisitions = CardRequisitionByDate;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardRequisitionRespObj();
            }

        }

        public SettingRegRespObj DeleteCardRequisition(DeleteCardRequisitionObj regObj)
        {
            var response = new SettingRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };


            try
            {
                if (regObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (!EntityValidatorHelper.Validate(regObj, out var valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, HelperMethods.getRequesterRoles(), ref response.Status.Message))
                {
                    return response;
                }

                var thisCardRequisition = getCardRequisitionInfo(regObj.CardRequisitionId);
                if (thisCardRequisition == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Information found for the specified CardRequisition Id";
                    response.Status.Message.TechnicalMessage = "No Card Requisition Information found!";
                    return response;
                }

                if (thisCardRequisition.Status != CardRequisitionStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This Card Requisition Is Not Valid For Delete! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " Card Requisition Status is either already Active/Issued/Retired!";
                    return response;
                }

                thisCardRequisition.RequisitionTitle =
                    thisCardRequisition.RequisitionTitle + "_Deleted_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                thisCardRequisition.Status = CardRequisitionStatus.Deleted;

                var added = _repository.Update(thisCardRequisition);
                _uoWork.SaveChanges();
                if (added.CardRequisitionId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }


                response.Status.IsSuccessful = true;
                response.SettingId = (int)added.CardRequisitionId;

            }
            catch (DbEntityValidationException ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            return response;
        }

        public CardRequisitionRespObj LoadCardRequisitions(SettingSearchObj searchObj)
        {
            var response = new CardRequisitionRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };

            try
            {
                if (searchObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (!EntityValidatorHelper.Validate(searchObj, out var valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                if (!HelperMethods.IsUserValid(searchObj.AdminUserId, searchObj.SysPathCode, HelperMethods.getRequesterRoles(), ref response.Status.Message))
                {
                    return response;
                }



                var thisCardRequisitions = getCardRequisitions(searchObj);

                if (!thisCardRequisitions.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Requisition  Information found!";
                    return response;
                }

                var CardRequisitions = new List<CardRequisitionObj>();

                foreach (var m in thisCardRequisitions)
                {
                    var thisCardRequisitionItems = getCardRequisitionItems(m.CardRequisitionId);
                    if (!thisCardRequisitionItems.Any() || thisCardRequisitionItems == null)
                    {
                        response.Status.Message.FriendlyMessage = "At Least One Card Requisition's Item Information was not found!";
                        response.Status.Message.TechnicalMessage = "At Least One Card Requisition's Item Information was not found!";
                        continue;
                    }

                    var CardRequisitionItemsList = new List<CardRequisitionItemRespObj>();

                    if (thisCardRequisitionItems != null && thisCardRequisitionItems.Any())
                    {
                        thisCardRequisitionItems.ForEachx(reqItem =>
                        {

                            CardRequisitionItemsList.Add(new CardRequisitionItemRespObj
                            {
                                CardRequisitionItemId = reqItem.CardRequisitionItemId,
                                CardTypeId = reqItem.CardTypeId,
                                CardTypeName = new CardTypeRepository().GetCardType(reqItem.CardTypeId).Name,
                                UnitPrice = reqItem.UnitPrice,
                                Quantity = reqItem.Quantity,
                                Amount = reqItem.Amount,
                                QuantityApproved = reqItem.QuantityApproved,
                                BeneficiaryId = reqItem.BeneficiaryId,
                                BeneficiaryName = new BeneficiaryRepository().GetBeneficiary(reqItem.BeneficiaryId).Fullname,
                                CardCommissionId = reqItem.CardCommissionId,
                                CardRequisitionId = reqItem.CardRequisitionId,
                                CommissionAmount = reqItem.CommissionAmount,
                                CommissionQuantity = reqItem.CommissionQuantity,
                                CommissionRate = reqItem.CommissionRate,
                                ExcessBalance = reqItem.ExcessBalance,
                                RequestedBy = reqItem.RequestedBy,
                                TimeStampRequested = reqItem.TimeStampRequested,
                                ApprovedBy = reqItem.ApprovedBy,
                                TimeStampApproved = reqItem.TimeStampApproved,
                                Status = (int)reqItem.Status,
                                StatusLabel = reqItem.Status.ToString().Replace("_", " ")
                            });
                        });
                    }
                    CardRequisitions.Add(new CardRequisitionObj
                    {
                        CardRequisitionId = m.CardRequisitionId,
                        RequisitionTitle = m.RequisitionTitle,
                        BeneficiaryId = m.BeneficiaryId,
                        BeneficiaryName = new BeneficiaryRepository().GetBeneficiary(m.BeneficiaryId).Fullname,

                        TotalQuantityRequested = m.TotalQuantityRequested,

                        RequestedBy = m.RequestedBy,
                        TimeStampRequested = m.TimeStampRequested,

                        ApprovedBy = m.ApprovedBy,
                        TimeStampApproved = m.TimeStampApproved,
                        ApproverComment = m.ApproverComment,
                        QuantityApproved = m.QuantityApproved,

                        Status = (int)m.Status,
                        StatusLabel = m.Status.ToString().Replace("_", " "),

                        CardRequisitionItems = CardRequisitionItemsList

                    });
                }


                response.Status.IsSuccessful = true;
                response.CardRequisitions = CardRequisitions;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                response.Status.IsSuccessful = false;
                return response;
            }
        }

        public CardRequisition getCardRequisitionInfo(long cardRequisitionId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardRequisition\" WHERE  \"CardRequisitionId\" = {cardRequisitionId}";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardRequisition>(sql1).ToList();
                if (!agentInfos.Any() || agentInfos.Count != 1)
                {
                    return null;
                }
                return agentInfos[0];

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public CardRequisitionItem getCardRequisitionItem(long itemId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardRequisitionItem\" WHERE  \"CardRequisitionItemId\" = {itemId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardRequisitionItem>(sql1).ToList();
                if (!agentInfos.Any() || agentInfos.Count != 1)
                {
                    return null;
                }
                return agentInfos[0];

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public List<CardRequisitionItem> getCardRequisitionItems(long itemId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardRequisitionItem\" WHERE  \"CardRequisitionId\" = {itemId}");

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardRequisitionItem>(sql.ToString()).ToList();
                return !agentInfos.Any() ? new List<CardRequisitionItem>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardRequisitionItem>();
            }
        }

        private bool deleteCardRequisitionItem(long itemId)
        {
            try
            {
                var sql1 = $"DELETE  FROM  \"VPlusSales\".\"CardRequisitionItem\" WHERE  \"CardRequisitionItemId\" = {itemId};";

                return _repository.RepositoryContext().Database.ExecuteSqlCommand(sql1) > 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        private List<CardRequisition> getCardRequisitions(SettingSearchObj searchObj)
        {

            try
            {
                var sql = new StringBuilder();

                if (searchObj.Status == -2)
                {
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardRequisition\" WHERE  \"Status\" != {-100}");
                }
                else
                {
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardRequisition\" WHERE  \"CardRequisitionStatus\" = {searchObj.Status} AND \"Status\" = {-100}");
                }

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardRequisition>(sql.ToString()).ToList();

                return !agentInfos.Any() ? new List<CardRequisition>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardRequisition>();
            }
        }

        private List<CardRequisition> getCardRequisitions()
        {
            try
            {

                var sql = new StringBuilder();
                sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardRequisition\" WHERE \"Status\" != {0} AND \"Status\" != {3} AND \"Status\" != {-100}");


                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardRequisition>(sql.ToString()).ToList();

                return !agentInfos.Any() ? new List<CardRequisition>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardRequisition>();
            }
        }

        private CardType getCardTypeInfo(int itemId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardType\" WHERE  \"CardTypeId\" = {itemId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardType>(sql1).ToList();
                if (!agentInfos.Any() || agentInfos.Count != 1)
                {
                    return new CardType();
                }
                return agentInfos[0];

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardType();
            }
        }

        private bool CheckInitialDuplicate(long cardRequisitionId, int cardTypeId)
        {


            string sql = $"Select * From \"VPlusSales\".\"CardRequisitionItem\" WHERE \"CardRequisitionId\" = {cardRequisitionId} AND \"CardTypeId\" = {cardTypeId}";

            var duplicates = _itemRepository.RepositoryContext().Database.SqlQuery<CardRequisitionItem>(sql).ToList();



            if (!duplicates.Any() || duplicates.Count() < 1)
            {
                return false;
            }


            return true;
        }

        private List<CardCommission> GetCardCommissions(int cardTypeId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardCommission\" WHERE  \"CardTypeId\" = {cardTypeId} AND \"Status\" = {1} ");

                var agentInfos = _commissionRepository.RepositoryContext().Database.SqlQuery<CardCommission>(sql.ToString()).ToList();
                return !agentInfos.Any() ? new List<CardCommission>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardCommission>();
            }
        }

        private CommissionHelper CommissionOperation(decimal amount, int cardTypeId)
        {
            var CommissionObject = new CommissionHelper();

            var cardTypeData = getCardTypeInfo(cardTypeId);
            if (cardTypeData == null)
            {
                return CommissionObject;

            }

            var cardCommissionData = GetCardCommissions(cardTypeId);
            if (!cardCommissionData.Any() || cardCommissionData == null)
            {
                return CommissionObject;
            }

            var specificCommissionData = cardCommissionData.Find(commission =>
                                                                 commission.LowerAmount <= amount &&
                                                                 commission.UpperAmount >= amount);

            CommissionObject.CommissionRate = specificCommissionData.CommissionRate;
            CommissionObject.CommissionAmount = amount * (specificCommissionData.CommissionRate) / 100;
            CommissionObject.CardCommissionId = specificCommissionData.CardCommissionId;

            var commissionQuantity = (amount * (specificCommissionData.CommissionRate / 100)) / cardTypeData.FaceValue;

            var mantissa = commissionQuantity - Math.Truncate(commissionQuantity);
            if (mantissa > 0)
            {
                if (mantissa >= (decimal)0.5)
                {
                    CommissionObject.CommissionQuantity = (int)(Math.Truncate(commissionQuantity) + 1);
                    CommissionObject.ExcessBalance = mantissa * cardTypeData.FaceValue;
                }
                else if (mantissa < (decimal)0.5)
                {
                    CommissionObject.CommissionQuantity = (int)Math.Truncate(commissionQuantity);
                    CommissionObject.ExcessBalance = mantissa * cardTypeData.FaceValue;
                }
            }
            else
            {
                CommissionObject.CommissionQuantity = (int)commissionQuantity;
                CommissionObject.ExcessBalance = 0;
            }

            return CommissionObject;
        }

        private BeneficiaryAccount GetBeneficiaryAccount(int beneficiaryAccountId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryAccount\" WHERE  \"BeneficiaryAccountId\" = {beneficiaryAccountId}";

                var beneficiaryAccount = _beneficiaryAccountRepository.RepositoryContext().Database.SqlQuery<BeneficiaryAccount>(sql1).ToList();
                if (!beneficiaryAccount.Any() || beneficiaryAccount.Count != 1)
                {
                    return null;
                }
                return beneficiaryAccount[0];

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
    }
}
