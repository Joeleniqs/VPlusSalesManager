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
using VPlusSalesManager.Business.Repository.Production;
using VPlusSalesManager.BusinessObject.Production;
using VPlusSalesManager.BusinessObject.Transaction;
using VPlusSalesManager.Common;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManager.Business.Repository.Transaction
{
    internal class CardIssuanceRepository
    {
        private readonly IVPlusSalesManagerRepository<CardIssuance> _repository;
        private readonly IVPlusSalesManagerRepository<CardItem> _cardItemRepository;
        private readonly IVPlusSalesManagerRepository<Card> _cardRepository;
        private readonly IVPlusSalesManagerRepository<CardRequisition> _requisitionRepository;
        private readonly IVPlusSalesManagerRepository<CardRequisitionItem> _requisitionItemRepository;
        private readonly VPlusSalesManagerUoWork _uoWork;

        public CardIssuanceRepository()
        {
            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<CardIssuance>(_uoWork);
            _cardItemRepository = new VPlusSalesManagerRepository<CardItem>(_uoWork);
            _cardRepository = new VPlusSalesManagerRepository<Card>(_uoWork);
            _requisitionRepository = new VPlusSalesManagerRepository<CardRequisition>(_uoWork);
            _requisitionItemRepository = new VPlusSalesManagerRepository<CardRequisitionItem>(_uoWork);
        }

        public CardIssuanceRegRespObj AddCardIssuance(RegCardIssuanceObj regObj)
        {
            var response = new CardIssuanceRegRespObj
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

                #region Parameters And Validation

                var associatedRequisition = new CardRequisitionRepository().getCardRequisitionInfo(regObj.CardRequisitionId);
                if (associatedRequisition == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Information Found";
                    response.Status.Message.TechnicalMessage = "No Card Requisition Information Found";
                    return response;
                }
                var associatedRequisitionItem = new CardRequisitionRepository().getCardRequisitionItem(regObj.CardRequisitionItemId);
                if (associatedRequisitionItem == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Item Information Found";
                    response.Status.Message.TechnicalMessage = "No Card Requisition Item Information Found";
                    return response;
                }
                var associatedRequisitionItems = new CardRequisitionRepository().getCardRequisitionItems(regObj.CardRequisitionId);
                if (associatedRequisitionItems == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Requisition Items Information Found";
                    response.Status.Message.TechnicalMessage = "No Card Requisition Items Information Found";
                    return response;
                }

                if (associatedRequisitionItem.Status != CardRequisitionStatus.Approved)
                {
                    response.Status.Message.FriendlyMessage = "Sorry! This Requisition Isn't Valid For Issuance!";
                    response.Status.Message.TechnicalMessage = "Sorry! This Requisition Isn't Valid For Issuance!";
                    return response;
                }

                var associatedCardItem = new CardRepository().getCardItemInfo(regObj.CardItemId);
                if (associatedCardItem == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Item Information Found";
                    response.Status.Message.TechnicalMessage = "No Card Item Information Found";
                    return response;
                }

                var associatedCardItems = new CardRepository().GetCardItems(associatedCardItem.CardId);
                if (!associatedCardItems.Any() || associatedCardItems == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Items Information Found";
                    response.Status.Message.TechnicalMessage = "No Card Items Information Found";
                    return response;
                }

                var associatedCard = new CardRepository().getCardInfo(associatedCardItem.CardId);
                if (associatedCard == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Production Information Found";
                    response.Status.Message.TechnicalMessage = "No Card Production Information Found";
                    return response;
                }

                if (!associatedCardItems.FindAll(cardItem => cardItem.AvailableQuantity > 0 && cardItem.Status == CardStatus.Available).Any()) {
                    response.Status.Message.FriendlyMessage = "Associated Card Item has no batch with available quantity";
                    response.Status.Message.TechnicalMessage = "Batch delivery not yet approved";
                    return response;
                }

                var MinimumBatchNumberAvailableValue = associatedCardItems
                                              .FindAll(cardItem => cardItem.AvailableQuantity > 0 && cardItem.Status == CardStatus.Available)
                                              .Min(card => card.AvailableQuantity);

                var ValidBatch = associatedCardItems
                                        .FindAll(cardItem => cardItem.AvailableQuantity == MinimumBatchNumberAvailableValue)
                                        .OrderBy(cardItem => cardItem.CardItemId)
                                        .FirstOrDefault();



                if (associatedCardItem.AvailableQuantity > MinimumBatchNumberAvailableValue)
                {
                    response.Status.Message.FriendlyMessage = $"Sorry There's Another Batch That Still Has {MinimumBatchNumberAvailableValue} {new CardTypeRepository().GetCardType(regObj.CardTypeId).Name} Available";
                    response.Status.Message.TechnicalMessage = $"Sorry There's Another Batch That Still Has {MinimumBatchNumberAvailableValue} {new CardTypeRepository().GetCardType(regObj.CardTypeId).Name} Available";
                    return response;
                }

                if (regObj.QuantityIssued < 1)
                {
                    response.Status.Message.FriendlyMessage = $"At Least One Card must be issued !";
                    response.Status.Message.TechnicalMessage = $"At Least One Card must be issued!";
                    return response;
                }

                if (regObj.QuantityIssued > MinimumBatchNumberAvailableValue)
                {
                    response.Status.Message.FriendlyMessage = $"Sorry Available Quantity is {MinimumBatchNumberAvailableValue}, Quantity Issued must be less!";
                    response.Status.Message.TechnicalMessage = $"Sorry Available Quantity is {MinimumBatchNumberAvailableValue}, Quantity Issued must be less!";
                    return response;
                }

                if (regObj.BatchId != ValidBatch.BatchId)
                {
                    response.Status.Message.FriendlyMessage = $"Sorry! The Right Batch To Issue From Is {ValidBatch.BatchId}";
                    response.Status.Message.TechnicalMessage = $"Sorry! The Right Batch To Issue From Is {ValidBatch.BatchId}";
                    return response;
                }
                var ValidBatchStopNumber = (Int32.Parse(ValidBatch.BatchStopNumber) - (ValidBatch.DefectiveQuantity + ValidBatch.MissingQuantity)).ToString();

                var ValidBatchStartNumber = ((Int32.Parse(ValidBatchStopNumber) + 1) - (ValidBatch.AvailableQuantity)).ToString();
                ErrorManager.LogApplicationError("inside Validation", $"", $"{ValidBatch.BatchStopNumber}");

                if (regObj.StopBatchNumber != ValidBatchStopNumber)
                {
                    response.Status.Message.FriendlyMessage = $"Sorry! The Right Stop Batch Number Is {ValidBatchStopNumber}";
                    response.Status.Message.TechnicalMessage = $"Sorry! The Right Stop Batch Number Is {ValidBatchStopNumber}";
                    return response;

                }

                if (regObj.StartBatchNumber != ValidBatchStartNumber)
                {
                    response.Status.Message.FriendlyMessage = $"Sorry! The Right Start Batch Number Is {ValidBatchStartNumber}";
                    response.Status.Message.TechnicalMessage = $"Sorry! The Right Start Batch Number Is {ValidBatchStartNumber}";
                    return response;
                }

                //store date for Concurrency...
                var nowDateTime = DateMap.CurrentTimeStamp();
                var nowDate = nowDateTime.Substring(0, nowDateTime.IndexOf(' '));
                var nowTime = nowDateTime.Substring(nowDateTime.IndexOf('-') + 1);

                #endregion

                #region Issuance Operation 
                ErrorManager.LogApplicationError("inside issuance ops", "", "");

                var associatedCardIssuance = GetIssuedListAssociatedWithRequisitionItem(regObj.CardRequisitionItemId);
                #region First Time Operation
                //First Time Issuance........................
                if (associatedCardIssuance.Count() < 1)
                {
                    using (var db = _uoWork.BeginTransaction())
                    {
                        ErrorManager.LogApplicationError("inside firsttime", "", "");

                        #region Card Issuance Operation
                        var newCardIssuance = new CardIssuance
                        {
                            CardRequisitionId = regObj.CardRequisitionId,
                            CardRequisitionItemId = regObj.CardRequisitionItemId,
                            CardTypeId = regObj.CardTypeId,
                            BeneficiaryId = regObj.BeneficiaryId,
                            CardItemId = regObj.CardItemId,
                            BatchId = ValidBatch.BatchId,
                            StartBatchNumber = ValidBatchStartNumber,
                            StopBatchNumber = ValidBatchStopNumber,
                            QuantityIssued = regObj.QuantityIssued,
                            IssuedBy = regObj.AdminUserId,
                            TimeStampIssued = nowDateTime,
                        };

                        var issuanceAdded = _repository.Add(newCardIssuance);
                        _uoWork.SaveChanges();
                        if (issuanceAdded.CardIssuanceId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Associated Card Item Operation
                        associatedCardItem.IssuedQuantity += issuanceAdded.QuantityIssued;
                        associatedCardItem.AvailableQuantity -= issuanceAdded.QuantityIssued;
                        associatedCardItem.TimeStampLastIssued = nowDateTime;
                        associatedCardItem.Status = CardStatus.Issued;

                        var cardItemAdded = _cardItemRepository.Update(associatedCardItem);
                        _uoWork.SaveChanges();
                        if (cardItemAdded.CardItemId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Associated Card Operation
                        associatedCard.Status = CardStatus.Issued;

                        var cardAdded = _cardRepository.Update(associatedCard);
                        _uoWork.SaveChanges();
                        if (cardAdded.CardId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Associated Card Requistion Item
                        if (associatedRequisitionItem.QuantityApproved != issuanceAdded.QuantityIssued)
                        {
                            associatedRequisitionItem.Status = CardRequisitionStatus.Issued;

                            var reqItemAdded = _requisitionItemRepository.Update(associatedRequisitionItem);
                            _uoWork.SaveChanges();
                            if (reqItemAdded.CardRequisitionItemId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        else if (associatedRequisitionItem.QuantityApproved == issuanceAdded.QuantityIssued)
                        {
                            associatedRequisitionItem.Status = CardRequisitionStatus.Closed;

                            var reqItemAdded = _requisitionItemRepository.Update(associatedRequisitionItem);
                            _uoWork.SaveChanges();
                            if (reqItemAdded.CardRequisitionItemId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        #endregion


                        db.Commit();

                        #region Associated Card Requisition Operation
                        if (associatedRequisitionItems.FindAll(cardItem => cardItem.Status == CardRequisitionStatus.Closed).Count() == associatedRequisitionItems.Count())
                        {
                            associatedRequisition.Status = CardRequisitionStatus.Closed;

                            var requisitionAdded = _requisitionRepository.Update(associatedRequisition);
                            _uoWork.SaveChanges();
                            if (requisitionAdded.CardRequisitionId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        else
                        {
                            associatedRequisition.Status = CardRequisitionStatus.Issued;

                            var requisitionAdded = _requisitionRepository.Update(associatedRequisition);
                            _uoWork.SaveChanges();
                            if (requisitionAdded.CardRequisitionId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        #endregion

                        db.Commit();

                        response.Status.IsSuccessful = true;
                        response.CardIssuanceId = issuanceAdded.CardIssuanceId;
                    }
                }
                #endregion

                #region Nth Time Operation
                //Nth Time Issuance..........................
                else if (associatedCardIssuance.Count() > 0)
                {
                    using (var db = _uoWork.BeginTransaction())
                    {
                        #region Card Issuance Operation
                        var newCardIssuance = new CardIssuance
                        {
                            CardRequisitionId = regObj.CardRequisitionId,
                            CardRequisitionItemId = regObj.CardRequisitionItemId,
                            CardTypeId = regObj.CardTypeId,
                            BeneficiaryId = regObj.BeneficiaryId,
                            CardItemId = regObj.CardItemId,
                            BatchId = ValidBatch.BatchId,
                            StartBatchNumber = ValidBatchStartNumber,
                            StopBatchNumber = ValidBatchStopNumber,
                            QuantityIssued = regObj.QuantityIssued,
                            IssuedBy = regObj.AdminUserId,
                            TimeStampIssued = nowDateTime,
                        };

                        var issuanceAdded = _repository.Add(newCardIssuance);
                        _uoWork.SaveChanges();
                        if (issuanceAdded.CardIssuanceId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Associated Card Item Operation

                        associatedCardItem.IssuedQuantity += issuanceAdded.QuantityIssued;
                        associatedCardItem.AvailableQuantity -= issuanceAdded.QuantityIssued;
                        associatedCardItem.TimeStampLastIssued = nowDateTime;
                        associatedCardItem.Status = CardStatus.Issued;

                        var cardItemAdded = _cardItemRepository.Update(associatedCardItem);
                        _uoWork.SaveChanges();
                        if (cardItemAdded.CardItemId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Associated Card Operation

                        associatedCard.Status = CardStatus.Issued;

                        var cardAdded = _cardRepository.Update(associatedCard);
                        _uoWork.SaveChanges();
                        if (cardAdded.CardId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        #endregion

                        #region Associated Card Requistion Item
                        if (associatedRequisitionItem.QuantityApproved != associatedCardIssuance.Sum(q => q.QuantityIssued) + issuanceAdded.QuantityIssued)
                        {
                            associatedRequisitionItem.Status = CardRequisitionStatus.Issued;

                            var reqItemAdded = _requisitionItemRepository.Update(associatedRequisitionItem);
                            _uoWork.SaveChanges();
                            if (reqItemAdded.CardRequisitionItemId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        else if (associatedRequisitionItem.QuantityApproved == associatedCardIssuance.Sum(q => q.QuantityIssued) + issuanceAdded.QuantityIssued)
                        {
                            associatedRequisitionItem.Status = CardRequisitionStatus.Closed;

                            var reqItemAdded = _requisitionItemRepository.Update(associatedRequisitionItem);
                            _uoWork.SaveChanges();
                            if (reqItemAdded.CardRequisitionItemId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        #endregion

                        db.Commit();

                        #region Associated Card Requisition Operation
                        if (associatedRequisitionItems.FindAll(cardItem => cardItem.Status == CardRequisitionStatus.Closed).Count() == associatedRequisitionItems.Count())
                        {
                            associatedRequisition.Status = CardRequisitionStatus.Closed;

                            var requisitionAdded = _requisitionRepository.Update(associatedRequisition);
                            _uoWork.SaveChanges();
                            if (requisitionAdded.CardRequisitionId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        else
                        {
                            associatedRequisition.Status = CardRequisitionStatus.Issued;

                            var requisitionAdded = _requisitionRepository.Update(associatedRequisition);
                            _uoWork.SaveChanges();
                            if (requisitionAdded.CardRequisitionId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                                response.Status.Message.TechnicalMessage = "Unable to save to database";
                                return response;
                            }
                        }
                        #endregion

                        db.Commit();

                        response.Status.IsSuccessful = true;
                        response.CardIssuanceId = issuanceAdded.CardIssuanceId;
                    }

                }

                #endregion
                #endregion
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

        public CardIssuanceRespObj LoadCardIssuanceByDate(LoadCardIssuanceByDateObj regObj)
        {
            var response = new CardIssuanceRespObj
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


                var thisCardIssuance = getCardIssuance(regObj.CardRequisitionId);

                if (!thisCardIssuance.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Issuance  Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Issuance  Information found!";
                    return response;
                }

                var CardIssuanceByDate = new List<CardIssuanceObj>();

                foreach (var m in thisCardIssuance)
                {
                    if (!string.IsNullOrWhiteSpace(regObj.BeginDate) && !string.IsNullOrWhiteSpace(regObj.EndDate))
                    {
                        var dateCreated = m.TimeStampIssued;
                        var value = dateCreated.IndexOf(' ');
                        if (value > 0) { dateCreated = dateCreated.Substring(0, value); }
                        var realDate = DateTime.Parse(dateCreated);
                        if (realDate >= DateTime.Parse(regObj.BeginDate) && realDate <= DateTime.Parse(regObj.EndDate))
                        {

                            CardIssuanceByDate.Add(new CardIssuanceObj
                            {
                                CardIssuanceId = m.CardIssuanceId,
                                CardRequisitionId = m.CardRequisitionId,
                                CardRequisitionLabel = new CardRequisitionRepository().getCardRequisitionInfo(m.CardRequisitionId).RequisitionTitle,
                                BatchId = m.BatchId,
                                CardItemId = m.CardItemId,
                                CardTypeId = m.CardTypeId,
                                BeneficiaryId = m.BeneficiaryId,
                                BeneficiaryLabel = new BeneficiaryRepository().GetBeneficiary(m.BeneficiaryId).Fullname,
                                StartBatchNumber = m.StartBatchNumber,
                                StopBatchNumber = m.StopBatchNumber,
                                CardRequisitionItemId = m.CardRequisitionItemId,
                                QuantityIssued = m.QuantityIssued,
                                IssuedBy = m.IssuedBy,
                                TimeStampIssued = m.TimeStampIssued
                            });
                        }

                    }
                    else
                    {
                        CardIssuanceByDate.Add(new CardIssuanceObj
                        {
                            CardIssuanceId = m.CardIssuanceId,
                            CardRequisitionId = m.CardRequisitionId,
                            CardRequisitionLabel = new CardRequisitionRepository().getCardRequisitionInfo(m.CardRequisitionId).RequisitionTitle,
                            BatchId = m.BatchId,
                            CardItemId = m.CardItemId,
                            CardTypeId = m.CardTypeId,
                            BeneficiaryId = m.BeneficiaryId,
                            BeneficiaryLabel = new BeneficiaryRepository().GetBeneficiary(m.BeneficiaryId).Fullname,
                            StartBatchNumber = m.StartBatchNumber,
                            StopBatchNumber = m.StopBatchNumber,
                            CardRequisitionItemId = m.CardRequisitionItemId,
                            QuantityIssued = m.QuantityIssued,
                            IssuedBy = m.IssuedBy,
                            TimeStampIssued = m.TimeStampIssued
                        });
                    }
                }


                response.Status.IsSuccessful = true;
                response.CardIssuance = CardIssuanceByDate;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardIssuanceRespObj();
            }

        }

        private List<CardIssuance> getCardIssuance(int cardRequisitionId)
        {
            try
            {
                if (cardRequisitionId > 0)
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardIssuance\" WHERE \"CardRequisitionId\" = {cardRequisitionId} ORDER BY \"CardIssuanceId\"");

                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardIssuance>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<CardIssuance>() : agentInfos;
                }
                else
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardIssuance\" ORDER BY \"CardIssuanceId\"");


                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardIssuance>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<CardIssuance>() : agentInfos;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardIssuance>();
            }
        }

        private List<CardIssuance> GetIssuedListAssociatedWithRequisitionItem(long cardRequisitionItemId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardIssuance\" WHERE \"CardRequisitionItemId\" = {cardRequisitionItemId} ORDER BY \"CardIssuanceId\"");

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardIssuance>(sql.ToString()).ToList();

                return !agentInfos.Any() ? new List<CardIssuance>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardIssuance>();
            }
        }
    }
}
