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
using VPlusSalesManager.BusinessObject.Production;
using VPlusSalesManager.Common;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManager.Business.Repository.Production
{
    internal class CardDeliveryRepository
    {
        private readonly IVPlusSalesManagerRepository<CardDelivery> _repository;
        private readonly IVPlusSalesManagerRepository<CardItem> _cardItemRepository;
        private readonly IVPlusSalesManagerRepository<Card> _cardRepository;
        private readonly VPlusSalesManagerUoWork _uoWork;

        public CardDeliveryRepository()
        {
            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<CardDelivery>(_uoWork);
            _cardItemRepository = new VPlusSalesManagerRepository<CardItem>(_uoWork);
            _cardRepository = new VPlusSalesManagerRepository<Card>(_uoWork);

        }

        public CardDeliveryRegRespObj AddCardDelivery(RegCardDeliveryObj regObj)
        {
            var response = new CardDeliveryRegRespObj
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

                var associatedCard = getCardInfo(regObj.CardId);
                if (associatedCard == null)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! No Card  Information Found";
                    response.Status.Message.TechnicalMessage = "Error Occurred! No Card  Information Found";
                    return response;
                }

                var associatedCardItem = getCardItemInfo(regObj.CardItemId);
                if (associatedCardItem == null)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! No Card Item Information Found";
                    response.Status.Message.TechnicalMessage = "Error Occurred! No Card Item Information Found";
                    return response;
                }

                if (associatedCardItem.Status != CardStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! This Card Item Is Not Available For Delivery";
                    response.Status.Message.TechnicalMessage = "Error Occurred! This Card Item Is Not Available For Delivery";
                    return response;
                }

                //Check validity Of Start/Stop Batch Number
                if ((int.Parse(regObj.StopBatchNumber) - int.Parse(regObj.StartBatchNumber)) + 1 != associatedCard.QuantityPerBatch)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Incorrect StopBatchNumber/StartBatchNumber Data";
                    response.Status.Message.TechnicalMessage = "Error Occurred! Incorrect StopBatchNumber/StartBatchNumber Data";
                    return response;
                }

                if (regObj.BatchId != associatedCardItem.BatchId)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Incorrect BatchId";
                    response.Status.Message.TechnicalMessage = "Error Occurred! Incorrect BatchId";
                    return response;
                }
                if (regObj.QuantityDelivered < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! You Cannot Register An empty delivery ";
                    response.Status.Message.TechnicalMessage = "Error Occurred! Incorrect quantity Delivered Data";
                    return response;
                }
                if (regObj.QuantityDelivered + associatedCardItem.DeliveredQuantity > associatedCardItem.BatchQuantity)
                {
                    if (associatedCardItem.BatchQuantity - (associatedCardItem.DeliveredQuantity) > 0)
                    {
                        response.Status.Message.FriendlyMessage = $"Incorrect Quantity Delivered,{associatedCardItem.BatchQuantity - (associatedCardItem.DeliveredQuantity)} is only available for delivery";
                    }
                    else if (associatedCardItem.BatchQuantity - (associatedCardItem.DeliveredQuantity) == 0)
                    {
                        response.Status.Message.FriendlyMessage = $"This Delivery is Complete";
                    }

                    response.Status.Message.TechnicalMessage = "Error Occurred! Incorrect Quantity Delivered";
                    return response;
                }

                if (DateTime.Parse(regObj.DeliveryDate) > DateTime.Now)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! You Cannot Register A delivery before DeliveryDate";
                    response.Status.Message.TechnicalMessage = "Error Occurred! Incorrect Delivery Date Data";
                    return response;
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    var newCardDelivery = new CardDelivery
                    {
                        CardItemId = regObj.CardItemId,
                        CardId = regObj.CardId,
                        CardTypeId = regObj.CardTypeId,
                        BatchId = associatedCardItem.BatchId,
                        Status = CardStatus.Registered,
                        ApprovedBy = 0,
                        ApproverComment = "",
                        DefectiveQuantity = regObj.DefectiveQuantity,
                        DeliveryDate = regObj.DeliveryDate,
                        MissingQuantity = regObj.MissingQuantity,
                        QuantityDelivered = regObj.QuantityDelivered,
                        StartBatchNumber = associatedCardItem.BatchId + "" + "000",
                        StopBatchNumber = associatedCardItem.BatchId + "" + "999",
                        RecievedBy = regObj.AdminUserId,
                        TimeStampRegistered = DateMap.CurrentTimeStamp(),
                        TimeStampApproved = "",
                    };

                    var deliveryAdded = _repository.Add(newCardDelivery);
                    _uoWork.SaveChanges();
                    if (deliveryAdded.CardDeliveryId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    associatedCardItem.MissingQuantity += regObj.MissingQuantity;
                    associatedCardItem.DefectiveQuantity += regObj.DefectiveQuantity;
                    associatedCardItem.DeliveredQuantity += regObj.QuantityDelivered;
                    associatedCardItem.AvailableQuantity += regObj.QuantityDelivered - (regObj.MissingQuantity + regObj.DefectiveQuantity);
                    associatedCardItem.TimeStampDelivered = deliveryAdded.TimeStampRegistered;
                    associatedCardItem.DefectiveBatchNumbers = regObj.DefectiveBatchNumbers;
                    associatedCardItem.Status = CardStatus.Registered;

                    var added = _cardItemRepository.Update(associatedCardItem);
                    _uoWork.SaveChanges();
                    if (added.CardItemId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    associatedCard.Status = CardStatus.Registered;

                    var cardAdded = _cardRepository.Update(associatedCard);
                    _uoWork.SaveChanges();
                    if (added.CardId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    db.Commit();

                    response.Status.IsSuccessful = true;
                    response.CardDeliveryId = deliveryAdded.CardDeliveryId;
                    response.Status.Message.FriendlyMessage = "Card Delivery Added Successfully!";
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

        public CardDeliveryRegRespObj UpdateCardDelivery(EditCardDeliveryObj regObj)
        {
            var response = new CardDeliveryRegRespObj
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

                var thisCardDelivery = getCardDeliveryInfo(regObj.CardDeliveryId);
                if (thisCardDelivery == null)
                {
                    response.Status.Message.FriendlyMessage = "No CardDelivery Information found for the specified CardDelivery Id";
                    response.Status.Message.TechnicalMessage = "No CardDelivery Information found!";
                    return response;
                }

                if (thisCardDelivery.Status != CardStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Card Delivery Is not valid for update";
                    response.Status.Message.TechnicalMessage = "Card Delivery Is not valid for update!";
                    return response;
                }

                var associatedCard = getCardInfo(regObj.CardId);
                if (associatedCard == null)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! No Card  Information Found";
                    response.Status.Message.TechnicalMessage = "Error Occurred! No Card  Information Found";
                    return response;
                }

                var associatedCardItem = getCardItemInfo(regObj.CardItemId);
                if (associatedCardItem == null)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! No Card Item Information Found";
                    response.Status.Message.TechnicalMessage = "Error Occurred! No Card Item Information Found";
                    return response;
                }

                if (associatedCardItem.Status != CardStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! This Card Item Is Not Available For Delivery";
                    response.Status.Message.TechnicalMessage = "Error Occurred! This Card Item Is Not Available For Delivery";
                    return response;
                }

                if (DateTime.Parse(regObj.DeliveryDate) > DateTime.Now)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! You Cannot Register A delivery before DeliveryDate";
                    response.Status.Message.TechnicalMessage = "Error Occurred! Incorrect Delivery Date Data";
                    return response;
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    thisCardDelivery.DefectiveQuantity = regObj.DefectiveQuantity > 0 ? regObj.DefectiveQuantity : thisCardDelivery.DefectiveQuantity;
                    thisCardDelivery.MissingQuantity = regObj.MissingQuantity > 0 ? regObj.MissingQuantity : thisCardDelivery.MissingQuantity;
                    thisCardDelivery.QuantityDelivered = regObj.QuantityDelivered > 0 ? regObj.QuantityDelivered : thisCardDelivery.QuantityDelivered;
                    thisCardDelivery.DeliveryDate = !string.IsNullOrWhiteSpace(regObj.DeliveryDate) ? regObj.DeliveryDate : thisCardDelivery.DeliveryDate;

                    var deliveryAdded = _repository.Update(thisCardDelivery);
                    _uoWork.SaveChanges();
                    if (deliveryAdded.CardDeliveryId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    associatedCardItem.MissingQuantity = regObj.MissingQuantity;
                    associatedCardItem.DefectiveQuantity = regObj.DefectiveQuantity;
                    associatedCardItem.DeliveredQuantity = regObj.QuantityDelivered;
                    associatedCardItem.AvailableQuantity = regObj.QuantityDelivered - (regObj.MissingQuantity + regObj.DefectiveQuantity);
                    associatedCardItem.DefectiveBatchNumbers = regObj.DefectiveBatchNumbers;

                    var added = _cardItemRepository.Update(associatedCardItem);
                    _uoWork.SaveChanges();
                    if (added.CardItemId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    db.Commit();

                    response.Status.IsSuccessful = true;
                    response.CardDeliveryId = deliveryAdded.CardDeliveryId;
                    response.Status.Message.FriendlyMessage = "Card Delivery Updated Successfully!";
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

        public CardDeliveryRegRespObj ApproveCardDelivery(ApproveCardDeliveryObj regObj)
        {
            var response = new CardDeliveryRegRespObj
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

                var thisCardDelivery = getCardDeliveryInfo(regObj.CardDeliveryId);
                if (thisCardDelivery == null)
                {
                    response.Status.Message.FriendlyMessage = "No CardDelivery Information found!";
                    response.Status.Message.TechnicalMessage = "No CardDelivery  Information found!";
                    return response;
                }

                var associatedCard = getCardInfo(thisCardDelivery.CardId);
                if (associatedCard == null)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! No Card  Information Found";
                    response.Status.Message.TechnicalMessage = "Error Occurred! No Card  Information Found";
                    return response;
                }

                var associatedCardItem = getCardItemInfo(thisCardDelivery.CardItemId);
                if (associatedCardItem == null)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! No Card Item Information Found";
                    response.Status.Message.TechnicalMessage = "Error Occurred! No Card Item Information Found";
                    return response;
                }

                if (regObj.IsApproved && regObj.IsDefective || regObj.IsApproved && regObj.IsDefective)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This CardDelivery Cannot be  Approved/Defected! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " IsApproved and IsDefected is both true/false";
                    return response;
                }
                if (thisCardDelivery.Status != CardStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This CardDelivery Is Not Valid For Approval! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " CardDelivery Status is already Available!";
                    return response;
                }


                if (regObj.IsApproved)
                {
                    using (var db = _uoWork.BeginTransaction())
                    {
                        thisCardDelivery.ApprovedBy = regObj.AdminUserId;
                        thisCardDelivery.ApproverComment = regObj.ApproverComment;
                        thisCardDelivery.TimeStampApproved = DateMap.CurrentTimeStamp();
                        thisCardDelivery.Status = CardStatus.Available;

                        var added = _repository.Update(thisCardDelivery);
                        _uoWork.SaveChanges();
                        if (added.CardDeliveryId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }

                        associatedCardItem.Status = CardStatus.Available;

                        var cardItemAdded = _cardItemRepository.Update(associatedCardItem);
                        _uoWork.SaveChanges();
                        if (added.CardItemId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }

                        associatedCard.Status = CardStatus.Available;

                        var cardAdded = _cardRepository.Update(associatedCard);
                        _uoWork.SaveChanges();
                        if (added.CardId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }

                        db.Commit();

                        response.Status.IsSuccessful = true;
                        response.CardDeliveryId = added.CardDeliveryId;
                        response.Status.Message.FriendlyMessage = "Approval Succesful! ";
                    }

                }
                else if (regObj.IsDefective)
                {
                    using (var db = _uoWork.BeginTransaction())
                    {
                        thisCardDelivery.ApprovedBy = regObj.AdminUserId;
                        thisCardDelivery.ApproverComment = regObj.ApproverComment;
                        thisCardDelivery.TimeStampApproved = DateMap.CurrentTimeStamp();
                        thisCardDelivery.Status = CardStatus.Defective;
                        thisCardDelivery.DefectiveQuantity = thisCardDelivery.QuantityDelivered;

                        var added = _repository.Update(thisCardDelivery);
                        _uoWork.SaveChanges();
                        if (added.CardDeliveryId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }

                        associatedCardItem.Status = CardStatus.Defective;
                        associatedCardItem.DefectiveQuantity = added.DefectiveQuantity;
                        associatedCardItem.AvailableQuantity = 0;

                        var cardItemAdded = _cardItemRepository.Update(associatedCardItem);
                        _uoWork.SaveChanges();
                        if (added.CardItemId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                            response.Status.Message.TechnicalMessage = "Unable to save to database";
                            return response;
                        }
                        db.Commit();
                        response.Status.IsSuccessful = true;
                        response.CardDeliveryId = added.CardDeliveryId;
                        response.Status.Message.FriendlyMessage = "Defected Succesfully! ";
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

        public CardDeliveryRespObj LoadCardDeliveryByDate(LoadCardDeliveryByDateObj regObj)
        {
            var response = new CardDeliveryRespObj
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


                var thisCardDelivery = getCardDeliveries(regObj.CardId);

                if (!thisCardDelivery.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Delivery  Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Delivery  Information found!";
                    return response;
                }

                var CardDeliveryByDate = new List<CardDeliveryObj>();

                foreach (var m in thisCardDelivery)
                {
                    if (!string.IsNullOrWhiteSpace(regObj.BeginDate) && !string.IsNullOrWhiteSpace(regObj.EndDate))
                    {
                        var dateCreated = m.TimeStampRegistered;
                        var value = dateCreated.IndexOf(' ');
                        if (value > 0) { dateCreated = dateCreated.Substring(0, value); }
                        var realDate = DateTime.Parse(dateCreated);
                        if (realDate >= DateTime.Parse(regObj.BeginDate) && realDate <= DateTime.Parse(regObj.EndDate))
                        {

                            CardDeliveryByDate.Add(new CardDeliveryObj
                            {
                                CardDeliveryId = m.CardDeliveryId,
                                CardId = m.CardId,
                                CardIdLabel = new CardRepository().getCardInfo(m.CardId).CardTitle,
                                BatchId = m.BatchId,
                                CardItemId = m.CardItemId,
                                CardTypeId = m.CardTypeId,
                                DeliveryDate = m.DeliveryDate,
                                StartBatchNumber = m.StartBatchNumber,
                                StopBatchNumber = m.StopBatchNumber,
                                DefectiveQuantity = m.DefectiveQuantity,
                                MissingQuantity = m.MissingQuantity,
                                QuantityDelivered = m.QuantityDelivered,
                                RecievedBy = m.RecievedBy,
                                ApprovedBy = m.ApprovedBy,
                                ApproverComment = m.ApproverComment,
                                TimeStampRegistered = m.TimeStampRegistered,
                                Status = (int)m.Status,
                                StatusLabel = m.Status.ToString().Replace("_", " "),
                                TimeStampApproved = m.TimeStampApproved
                            });
                        }

                    }
                    else
                    {
                        CardDeliveryByDate.Add(new CardDeliveryObj
                        {
                            CardDeliveryId = m.CardDeliveryId,
                            CardId = m.CardId,
                            CardIdLabel = new CardRepository().getCardInfo(m.CardId).CardTitle,
                            BatchId = m.BatchId,
                            CardItemId = m.CardItemId,
                            CardTypeId = m.CardTypeId,
                            DeliveryDate = m.DeliveryDate,
                            StartBatchNumber = m.StartBatchNumber,
                            StopBatchNumber = m.StopBatchNumber,
                            DefectiveQuantity = m.DefectiveQuantity,
                            MissingQuantity = m.MissingQuantity,
                            QuantityDelivered = m.QuantityDelivered,
                            RecievedBy = m.RecievedBy,
                            ApprovedBy = m.ApprovedBy,
                            ApproverComment = m.ApproverComment,
                            TimeStampRegistered = m.TimeStampRegistered,
                            Status = (int)m.Status,
                            StatusLabel = m.Status.ToString().Replace("_", " "),
                        });
                    }
                }


                response.Status.IsSuccessful = true;
                response.CardDeliveries = CardDeliveryByDate;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardDeliveryRespObj();
            }

        }

        private Card getCardInfo(int cardId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"Card\" WHERE  \"CardId\" = {cardId};";

                var agentInfos = _cardRepository.RepositoryContext().Database.SqlQuery<Card>(sql1).ToList();
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

        private CardItem getCardItemInfo(long cardItemId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardItem\" WHERE  \"CardItemId\" = {cardItemId};";

                var agentInfos = _cardItemRepository.RepositoryContext().Database.SqlQuery<CardItem>(sql1).ToList();
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

        private CardDelivery getCardDeliveryInfo(long cardDeliveryId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardDelivery\" WHERE  \"CardDeliveryId\" = {cardDeliveryId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardDelivery>(sql1).ToList();
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

        private List<CardDelivery> getCardDeliveries(int cardId)
        {
            try
            {
                if (cardId > 0)
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardDelivery\" WHERE \"CardId\" = {cardId}");

                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardDelivery>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<CardDelivery>() : agentInfos;
                }
                else
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"CardDelivery\" ");


                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardDelivery>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<CardDelivery>() : agentInfos;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardDelivery>();
            }
        }

        private bool IsCardDeliveryDuplicate(string batchId, int callType, ref CardDeliveryRegRespObj response)
        {
            try
            {
                #region Check BatchId
                var sql1 =
                          $"Select * FROM  \"VPlusSales\".\"CardDelivery\"  WHERE \"BatchId\" ~ '{batchId}'";

                var check = _repository.RepositoryContext().Database.SqlQuery<CardDelivery>(sql1).ToList();

                if (check.Count() > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error! Batch Id already exist";
                        response.Status.Message.TechnicalMessage = "Duplicate Error! Batch Id already exist";
                        return true;
                    }

                }
                #endregion

                return false;
            }
            catch (Exception ex)
            {
                response.Status.Message.FriendlyMessage =
                         "Unable to complete your request due to validation error. Please try again later";
                response.Status.Message.TechnicalMessage = "Duplicate Check Error: " + ex.Message;
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }

    }
}
