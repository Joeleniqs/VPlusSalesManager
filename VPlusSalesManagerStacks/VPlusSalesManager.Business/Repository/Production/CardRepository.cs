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
    internal class CardRepository
    {

        private readonly IVPlusSalesManagerRepository<Card> _repository;
        private readonly IVPlusSalesManagerRepository<CardItem> _itemRepository;
        private readonly IVPlusSalesManagerRepository<CardDelivery> _deliveryRepository;
        private readonly VPlusSalesManagerUoWork _uoWork;

        public CardRepository()
        {
            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<Card>(_uoWork);
            _itemRepository = new VPlusSalesManagerRepository<CardItem>(_uoWork);
            _deliveryRepository = new VPlusSalesManagerRepository<CardDelivery>(_uoWork);
        }

        public Card GetCard(long CardId)
        {
            try
            {
                return getCardInfo(CardId) ?? new Card();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new Card();
            }
        }

        public List<Card> GetCards()
        {
            try
            {
                var myItemList = _repository.GetAll().OrderBy(m => m.CardId);
                if (!myItemList.Any()) return new List<Card>();
                var settings = myItemList.ToList();
                if (settings.IsNullOrEmpty()) { return new List<Card>(); }
                return settings;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Card>();
            }
        }

        public CardRegRespObj AddCard(RegCardObj regObj)
        {
            var response = new CardRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage()
                }
            };

            try
            {

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
                if (!DataCheck.IsNumeric(regObj.BatchKey))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Batch Key Invalid";
                    response.Status.Message.TechnicalMessage = "Batch Prefix Number Must be greater than 0";
                    return response;
                }
                if (!DataCheck.IsNumeric(regObj.StartBatchId))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Invalid Start Batch Id";
                    response.Status.Message.TechnicalMessage = "Start Batch Id Is not numeric";
                    return response;
                }
                if (!DataCheck.IsNumeric(regObj.StopBatchId))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Invalid Stop Batch Id";
                    response.Status.Message.TechnicalMessage = "Stop Batch Id Is not numeric";
                    return response;
                }

                if ((int.Parse(regObj.StopBatchId) - int.Parse(regObj.StartBatchId)) + 1 != regObj.NumberOfBatches)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Incorrect StopBatchId/StartBatchId/NumberOfBatches";
                    response.Status.Message.TechnicalMessage = "Incorrect StopBatchId/StartBatchId/NumberOfBatches";
                    return response;
                }
                if (regObj.QuantityPerBatch < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Quantity Per Batch Is Required";
                    response.Status.Message.TechnicalMessage = "Error Occurred! Quantity Per Batch must be greater than zero!";
                    return response;
                }


                //store date for Concurrency...
                var nowDateTime = DateMap.CurrentTimeStamp();
                var nowDate = nowDateTime.Substring(0, nowDateTime.IndexOf(' '));
                var nowTime = nowDateTime.Substring(nowDateTime.IndexOf('-') + 1);

                var cardItemList = new List<CardItem>();

                for (int i = int.Parse(regObj.StartBatchId); i < (int.Parse(regObj.StopBatchId) + 1); i++)
                {
                    cardItemList.Add(new CardItem
                    {
                        CardTypeId = regObj.CardTypeId,
                        BatchId = i.ToString(),
                        BatchStartNumber = i.ToString() + "" + "000", //77001 000
                        BatchStopNumber = i.ToString() + "" + "999", //77001999
                        DefectiveBatchNumbers = "",
                        AvailableQuantity = 0,
                        BatchQuantity = 1000,
                        DeliveredQuantity = 0,
                        MissingQuantity = 0,
                        DefectiveQuantity = 0,
                        IssuedQuantity = 0,
                        RegisteredBy = regObj.AdminUserId,
                        TimeStampRegistered = nowDateTime,
                        TimeStampDelivered = "",
                        TimeStampLastIssued = "",
                        Status = CardStatus.Registered,
                    });
                }

                #region Former Work Commented

                //for (int batch = 0; batch < regObj.NumberOfBatches; batch++)
                //{
                //    var batchkey = regObj.BatchKey;
                //    var batchId = (Int32.Parse(batchkey + "000") + batch).ToString();
                //    var startBatchNumber = (batchId + "000");
                //    var stopBatchNumber = (Int32.Parse(startBatchNumber) + 999).ToString();

                //    cardItemList.Add(new CardItem
                //    {
                //        CardTypeId = regObj.CardTypeId,
                //        BatchId = batchId,
                //        BatchStartNumber = startBatchNumber,
                //        BatchStopNumber = stopBatchNumber,
                //        DefectiveBatchNumbers = " ",
                //        AvailableQuantity = 0,
                //        BatchQuantity = 0,
                //        DeliveredQuantity = 0,
                //        MissingQuantity = 0,
                //        DefectiveQuantity = 0,
                //        IssuedQuantity = 0,
                //        RegisteredBy = regObj.AdminUserId,
                //        TimeStampRegistered = nowDateTime,
                //        TimeStampDelivered = " ",
                //        TimeStampIssued = " ",
                //        Status = CardStatus.Unknown,
                //    });
                //}

                #endregion

                var Card = new Card
                {
                    CardTitle = $"Card Production On {nowDate} At {nowTime}",
                    CardTypeId = regObj.CardTypeId,
                    BatchKey = regObj.BatchKey,
                    StartBatchId = regObj.BatchKey + "000",
                    StopBatchId = (Int32.Parse(regObj.BatchKey + "000") + (regObj.NumberOfBatches - 1)).ToString(),
                    NumberOfBatches = regObj.NumberOfBatches,
                    QuantityPerBatch = 1000,
                    TotalQuantity = regObj.NumberOfBatches * regObj.QuantityPerBatch,
                    Status = CardStatus.Registered,
                    TimeStampRegistered = nowDateTime,
                    CardItems = cardItemList
                };

                var added = _repository.Add(Card);

                _uoWork.SaveChanges();
                if (added.CardId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }

                response.Status.IsSuccessful = true;
                response.CardId = added.CardId;
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

        public CardRegRespObj UpdateCard(EditCardObj regObj)
        {
            var response = new CardRegRespObj
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

                var thisCardItem = getCardItemInfo(regObj.CardItemId);
                if (thisCardItem == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Item Information found for the specified Card Item Id";
                    response.Status.Message.TechnicalMessage = "No Card Item Information found!";
                    return response;
                }

                var thisCardDelivery = getCardDeliveryInfo(regObj.CardItemId);
                if (thisCardDelivery == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Delivery Information found for the specified Card Item Id";
                    response.Status.Message.TechnicalMessage = "No Card Delivery Information found!";
                    return response;
                }

                if (regObj.MissingQuantityFound > thisCardItem.MissingQuantity)
                {
                    response.Status.Message.FriendlyMessage = "Quantity Found Cannot be more than Missing quantity";
                    response.Status.Message.TechnicalMessage = "Quantity Found Cannot be more than Missing quantity!";
                    return response;
                }

                if (regObj.DefectiveQuantityRectified > thisCardItem.DefectiveQuantity)
                {
                    response.Status.Message.FriendlyMessage = "defective Quantity Rectified Cannot be more than defective quantity";
                    response.Status.Message.TechnicalMessage = "defective Quantity Found Cannot be more than Missing quantity!";
                    return response;
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    //updating card item
                    thisCardItem.AvailableQuantity = regObj.MissingQuantityFound > 0 || regObj.DefectiveQuantityRectified > 0 ? thisCardItem.AvailableQuantity + regObj.MissingQuantityFound + regObj.DefectiveQuantityRectified : thisCardItem.AvailableQuantity;
                    thisCardItem.MissingQuantity = regObj.MissingQuantityFound > 0 ? thisCardItem.MissingQuantity - regObj.MissingQuantityFound : thisCardItem.MissingQuantity;
                    thisCardItem.DefectiveQuantity = regObj.DefectiveQuantityRectified > 0 ? thisCardItem.DefectiveQuantity - regObj.DefectiveQuantityRectified : thisCardItem.DefectiveQuantity;

                    var added = _itemRepository.Update(thisCardItem);
                    _uoWork.SaveChanges();
                    if (added.CardItemId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }


                    //updating card delivery
                    thisCardDelivery.MissingQuantity = regObj.MissingQuantityFound > 0 ? thisCardDelivery.MissingQuantity - regObj.MissingQuantityFound : thisCardDelivery.MissingQuantity;
                    thisCardDelivery.DefectiveQuantity = regObj.DefectiveQuantityRectified > 0 ? thisCardDelivery.DefectiveQuantity - regObj.DefectiveQuantityRectified : thisCardDelivery.DefectiveQuantity;

                    var deliveryAdded = _deliveryRepository.Update(thisCardDelivery);
                    _uoWork.SaveChanges();
                    if (deliveryAdded.CardDeliveryId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }
                    db.Commit();

                    response.Status.IsSuccessful = true;
                    response.CardId = deliveryAdded.CardId;
                    response.Status.Message.FriendlyMessage = "Card Item Updated Successfully!";
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

        public CardRespObj LoadCardByDate(LoadCardByDateObj regObj)
        {
            var response = new CardRespObj
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

                var thisCard = getCards();

                if (!thisCard.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Production Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Production Information found!";
                    return response;
                }

                var CardByDate = new List<CardObj>();
                var CardItemByDate = new List<CardItemObj>();

                foreach (var m in thisCard)
                {
                    var dateCreated = m.TimeStampRegistered;
                    var value = dateCreated.IndexOf(' ');
                    if (value > 0) { dateCreated = dateCreated.Substring(0, value); }
                    var realDate = DateTime.Parse(dateCreated);
                    if (realDate >= DateTime.Parse(regObj.BeginDate) && realDate <= DateTime.Parse(regObj.EndDate))
                    {
                        var cardItemList = GetCardItems(m.CardId);
                        if (!cardItemList.Any())
                        {
                            response.Status.Message.FriendlyMessage = "No Card Production Item Information found!";
                            response.Status.Message.TechnicalMessage = "No Card Production Item Information found!";
                            return response;
                        }

                        foreach (var item in cardItemList)
                        {
                            CardItemByDate.Add(new CardItemObj
                            {
                                CardItemId = item.CardItemId,
                                CardId = item.CardId,
                                CardTypeId = item.CardTypeId,
                                BatchId = item.BatchId,
                                BatchStartNumber = item.BatchStartNumber,
                                BatchStopNumber = item.BatchStopNumber,
                                AvailableQuantity = item.AvailableQuantity,
                                BatchQuantity = item.BatchQuantity,
                                DefectiveBatchNumbers = item.DefectiveBatchNumbers,
                                DefectiveQuantity = item.DefectiveQuantity,
                                DeliveredQuantity = item.DeliveredQuantity,
                                IssuedQuantity = item.IssuedQuantity,
                                MissingQuantity = item.MissingQuantity,
                                RegisteredBy = item.RegisteredBy,
                                Status = (int)item.Status,
                                StatusLabel = item.Status.ToString().Replace("_", " "),
                                TimeStampDelivered = item.TimeStampDelivered,
                                TimeStampIssued = item.TimeStampLastIssued,
                                TimeStampRegistered = item.TimeStampRegistered
                            });
                        }

                        CardByDate.Add(new CardObj
                        {
                            CardId = m.CardId,
                            CardTypeId = m.CardTypeId,
                            CardTitle = m.CardTitle,
                            BatchKey = m.BatchKey,
                            StartBatchId = m.StartBatchId,
                            StopBatchId = m.StopBatchId,
                            NoOfBatches = m.NumberOfBatches,
                            QuantityPerBatch = m.QuantityPerBatch,
                            TotalQuantity = m.TotalQuantity,
                            TimeStampRegistered = m.TimeStampRegistered,
                            Status = (int)m.Status,
                            StatusLabel = m.Status.ToString().Replace("_", " "),
                            CardItems = CardItemByDate
                        });
                    }
                }


                response.Status.IsSuccessful = true;
                response.Cards = CardByDate;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardRespObj();
            }

        }

        public SettingRegRespObj DeleteCard(DeleteCardObj regObj)
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

                var thisCard = getCardInfo(regObj.CardId);
                if (thisCard == null)
                {
                    response.Status.Message.FriendlyMessage = "No Card Production Information found for the specified Card Id";
                    response.Status.Message.TechnicalMessage = "No Card Production Information found!";
                    return response;
                }

                if (thisCard.Status != CardStatus.Registered)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This Card Production Is Not Valid For Delete! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " Supply Requisition Status is either already Active/Issued/Retired!";
                    return response;
                }

                thisCard.CardTitle =
                    thisCard.CardTitle + "_Deleted_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                thisCard.Status = CardStatus.Defective;

                var added = _repository.Update(thisCard);
                _uoWork.SaveChanges();
                if (added.CardId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }


                response.Status.IsSuccessful = true;
                response.SettingId = added.CardId;

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

        public CardRespObj LoadCards(SettingSearchObj searchObj)
        {
            var response = new CardRespObj
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

                var thisCards = getCards(searchObj);
                if (!thisCards.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Production Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Production  Information found!";
                    return response;
                }
                var Cards = new List<CardObj>();
                var CardItemByDate = new List<CardItemObj>();

                foreach (var m in thisCards)
                {

                    var cardItemList = GetCardItems(m.CardId);
                    if (!cardItemList.Any())
                    {
                        response.Status.Message.FriendlyMessage = "No Card Production Item Information found!";
                        response.Status.Message.TechnicalMessage = "No Card Production Item Information found!";
                        return response;
                    }

                    foreach (var item in cardItemList)
                    {
                        CardItemByDate.Add(new CardItemObj
                        {
                            CardItemId = item.CardItemId,
                            CardId = item.CardId,
                            CardTypeId = item.CardTypeId,
                            BatchId = item.BatchId,
                            BatchStartNumber = item.BatchStartNumber,
                            BatchStopNumber = item.BatchStopNumber,
                            AvailableQuantity = item.AvailableQuantity,
                            BatchQuantity = item.BatchQuantity,
                            DefectiveBatchNumbers = item.DefectiveBatchNumbers,
                            DefectiveQuantity = item.DefectiveQuantity,
                            DeliveredQuantity = item.DeliveredQuantity,
                            IssuedQuantity = item.IssuedQuantity,
                            MissingQuantity = item.MissingQuantity,
                            RegisteredBy = item.RegisteredBy,
                            Status = (int)item.Status,
                            StatusLabel = item.Status.ToString().Replace("_", " "),
                            TimeStampDelivered = item.TimeStampDelivered,
                            TimeStampIssued = item.TimeStampLastIssued,
                            TimeStampRegistered = item.TimeStampRegistered
                        });
                    }

                    Cards.Add(new CardObj
                    {
                        CardId = m.CardId,
                        CardTypeId = m.CardTypeId,
                        CardTitle = m.CardTitle,
                        BatchKey = m.BatchKey,
                        StartBatchId = m.StartBatchId,
                        StopBatchId = m.StopBatchId,
                        NoOfBatches = m.NumberOfBatches,
                        QuantityPerBatch = m.QuantityPerBatch,
                        TotalQuantity = m.TotalQuantity,
                        TimeStampRegistered = m.TimeStampRegistered,
                        Status = (int)m.Status,
                        StatusLabel = m.Status.ToString().Replace("_", " "),
                        CardItems = CardItemByDate
                    });

                }

                response.Status.IsSuccessful = true;
                response.Cards = Cards;
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

        public Card getCardInfo(long cardId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"Card\" WHERE  \"CardId\" = {cardId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<Card>(sql1).ToList();
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

        private List<Card> getCards(SettingSearchObj searchObj)
        {
            try
            {
                var sql = new StringBuilder();

                if (searchObj.Status == -2)
                {
                    sql.Append($"SELECT * FROM  \"VPlusSales\".\"Card\" WHERE  \"Status\" != {2} AND \"Status\" != {-100} ");
                }
                else
                {
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"Card\" WHERE  \"Status\" = {searchObj.Status}");
                }

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<Card>(sql.ToString()).ToList();

                return !agentInfos.Any() ? new List<Card>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Card>();
            }
        }

        public List<Card> getCards()
        {
            try
            {
                var sql = new StringBuilder();

                sql.Append($"SELECT * FROM  \"VPlusSales\".\"Card\" WHERE  \"Status\" != {(int)CardStatus.Defective} AND \"Status\" != {(int)CardStatus.Deleted} ");

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<Card>(sql.ToString()).ToList();

                return !agentInfos.Any() ? new List<Card>() : agentInfos;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Card>();
            }
        }

        public List<CardItem> GetCardItems(int cardId)
        {
            try
            {
                var sql1 = $"SELECT * FROM  \"VPlusSales\".\"CardItem\" WHERE  \"CardId\" = {cardId}";

                var agentInfos = _itemRepository.RepositoryContext().Database.SqlQuery<CardItem>(sql1).ToList();
                if (!agentInfos.Any())
                {
                    return null;
                }
                return agentInfos;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public CardItem getCardItemInfo(long cardItemId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardItem\" WHERE  \"CardItemId\" = {cardItemId};";

                var agentInfos = _itemRepository.RepositoryContext().Database.SqlQuery<CardItem>(sql1).ToList();
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

        private CardDelivery getCardDeliveryInfo(long cardItemId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardDelivery\" WHERE  \"CardItemId\" = {cardItemId};";

                var agentInfos = _deliveryRepository.RepositoryContext().Database.SqlQuery<CardDelivery>(sql1).ToList();
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

    }
}
