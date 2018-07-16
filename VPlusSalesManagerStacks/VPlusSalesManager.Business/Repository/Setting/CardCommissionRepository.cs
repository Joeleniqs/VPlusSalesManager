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
using VPlusSalesManager.Common;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManager.Business.Repository
{
    internal class CardCommissionRepository
    {
        private readonly IVPlusSalesManagerRepository<CardCommission> _repository;
        private readonly VPlusSalesManagerUoWork _uoWork;

        public CardCommissionRepository()
        {
            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<CardCommission>(_uoWork);
        }

        public CardCommission GetCardCommission(int CardCommissionId)
        {
            try
            {
                return GetCardCommissions().Find(k => k.CardCommissionId == CardCommissionId) ?? new CardCommission();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new CardCommission();
            }
        }

        public List<CardCommission> GetCardCommissions()
        {
            try
            {
                if (!(CacheManager.GetCache("ccCardCommissionList") is List<CardCommission> settings) || settings.IsNullOrEmpty())
                {
                    var myItemList = _repository.GetAll().OrderBy(m => m.CardCommissionId);
                    if (!myItemList.Any()) return new List<CardCommission>();
                    settings = myItemList.ToList();
                    if (settings.IsNullOrEmpty()) { return new List<CardCommission>(); }
                    CacheManager.SetCache("ccCardCommissionList", settings, DateTime.Now.AddYears(1));
                }
                return settings;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<CardCommission>();
            }
        }

        internal void resetCache()
        {
            try
            {
                HelperMethods.clearCache("ccCardCommissionList");
                GetCardCommissions();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public SettingRegRespObj AddCardCommission(RegCardCommissionObj regObj)
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

                if (regObj.LowerAmount == 0 || regObj.UpperAmount == 0)
                {
                    response.Status.Message.FriendlyMessage = "Validation Error Occurred! Lower/Upper Amount Cannot be Zero";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                if (regObj.LowerAmount > regObj.UpperAmount || regObj.LowerAmount < getCardTypeInfo(regObj.CardTypeId).FaceValue)
                {

                    response.Status.Message.FriendlyMessage = "Invalid Lower/Upper Amount Data";
                    response.Status.Message.TechnicalMessage = "Invalid Lower/Upper Amount Data";
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

                if (IsCardCommissionDuplicate(regObj.LowerAmount, regObj.UpperAmount, regObj.CommissionRate, regObj.CardTypeId, 1, ref response)) { return response; }

                var cardCommission = new CardCommission
                {
                    CardTypeId = regObj.CardTypeId,
                    LowerAmount = regObj.LowerAmount,
                    UpperAmount = regObj.UpperAmount,
                    CommissionRate = regObj.CommissionRate,
                    Status = (Status)regObj.Status
                };

                var added = _repository.Add(cardCommission);
                _uoWork.SaveChanges();
                if (added.CardCommissionId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }

                resetCache();
                response.Status.IsSuccessful = true;
                response.SettingId = added.CardCommissionId;

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

        public SettingRegRespObj UpdateCardCommission(EditCardCommissionObj regObj)
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

                var thisCardCommission = getCardCommissionInfo(regObj.CardCommissionId);

                if (thisCardCommission == null)
                {
                    response.Status.Message.FriendlyMessage = "No CardCommission Information found for the specified CardCommission Id";
                    response.Status.Message.TechnicalMessage = "No CardCommission Information found!";
                    return response;
                }

                if (IsCardCommissionDuplicate(regObj.LowerAmount, regObj.UpperAmount, regObj.CommissionRate, regObj.CardTypeId, 2, ref response)) { return response; }


                thisCardCommission.CardTypeId = regObj.CardTypeId;
                thisCardCommission.LowerAmount = regObj.LowerAmount;
                thisCardCommission.UpperAmount = regObj.UpperAmount;
                thisCardCommission.CommissionRate = regObj.CommissionRate;
                thisCardCommission.Status = (Status)regObj.Status;

                var added = _repository.Update(thisCardCommission);
                _uoWork.SaveChanges();
                if (added.CardCommissionId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }
                resetCache();
                response.Status.IsSuccessful = true;
                response.SettingId = added.CardCommissionId;

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

        public SettingRegRespObj DeleteCardCommission(DeleteCardCommissionObj regObj)
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

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, new[] { "PortalAdmin", "CRMAdmin", "CRMManager" }, ref response.Status.Message))
                {
                    return response;
                }

                var thisCardCommission = getCardCommissionInfo(regObj.CardCommissionId);
                if (thisCardCommission == null)
                {
                    response.Status.Message.FriendlyMessage = "No CardCommission Information found for the specified CardCommission Id";
                    response.Status.Message.TechnicalMessage = "No CardCommission Information found!";
                    return response;
                }

                thisCardCommission.Status = Status.Deleted;

                var added = _repository.Update(thisCardCommission);
                _uoWork.SaveChanges();
                if (added.CardCommissionId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }

                resetCache();
                response.Status.IsSuccessful = true;
                response.SettingId = added.CardCommissionId;

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

        public CardCommissionRespObj LoadCardCommissions(SettingSearchObj searchObj)
        {
            var response = new CardCommissionRespObj
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


                var thisCardCommissions = GetCardCommissions();

                if (!thisCardCommissions.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Card Type Information found!";
                    response.Status.Message.TechnicalMessage = "No Card Type  Information found!";
                    return response;
                }

                if (searchObj.Status > -1)
                {
                    thisCardCommissions = thisCardCommissions.FindAll(p => p.Status == (Status)searchObj.Status);
                }

                var CardCommissionItems = new List<CardCommissionItemObj>();

                thisCardCommissions.ForEachx(m =>
                {
                    CardCommissionItems.Add(new CardCommissionItemObj
                    {
                        CardCommissionId = m.CardCommissionId,
                        CardTypeId = m.CardTypeId,
                        CardTypeName = new CardTypeRepository().GetCardType(m.CardTypeId).Name,
                        LowerAmount = m.LowerAmount,
                        UpperAmount = m.UpperAmount,
                        CommissionRate = m.CommissionRate,
                        Status = (int)m.Status,
                        StatusLabel = m.Status.ToString().Replace("_", " ")
                    });
                });


                response.Status.IsSuccessful = true;
                response.CardCommissions = CardCommissionItems;
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

        private bool IsCardCommissionDuplicate(decimal lowerAmount, decimal upperAmount, decimal commissionRate, int cardTypeId, int callType, ref SettingRegRespObj response)
        {
            try
            {
                var sql1 =
                    $"Select * FROM  \"VPlusSales\".\"CardCommission\"  WHERE \"CardTypeId\" = {cardTypeId} AND \"Status\" != {-100}";

                var check = _repository.RepositoryContext().Database.SqlQuery<CardCommission>(sql1).ToList();

                if (check.Count == 0)
                {
                    return false;
                }

                if (check.FindAll(amt => amt.LowerAmount <= lowerAmount && amt.UpperAmount >= lowerAmount).Count > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error Check Lower Amount! The selected range is already covered Range already exist";
                        response.Status.Message.TechnicalMessage = "Duplicate Error Check Lower Amount!The selected range is already covered Range already exist";
                        return true;
                    }

                }

                if (check.FindAll(amt => amt.LowerAmount <= upperAmount && amt.UpperAmount >= upperAmount).Count > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error Check Upper Amount!The selected range is already covered";
                        response.Status.Message.TechnicalMessage = "Duplicate Error Check Upper Amount!The selected range is already covered";
                        return true;
                    }

                }

                if (check.FindAll(rate => rate.CommissionRate == commissionRate).Count > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error! Commission Rate already exist";
                        response.Status.Message.TechnicalMessage = "Duplicate Error! Commission Rate already exist";
                        return true;
                    }

                }

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

        private CardCommission getCardCommissionInfo(int cardCommissionId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardCommission\" WHERE  \"CardCommissionId\" = {cardCommissionId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardCommission>(sql1).ToList();
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

        private CardType getCardTypeInfo(int cardTypeId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"CardType\" WHERE  \"CardTypeId\" = {cardTypeId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<CardType>(sql1).ToList();
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
