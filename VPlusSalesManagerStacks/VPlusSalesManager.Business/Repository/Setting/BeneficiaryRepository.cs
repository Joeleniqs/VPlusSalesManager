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

namespace VPlusSalesManager.Business.Repository
{
    internal class BeneficiaryRepository
    {

        private readonly IVPlusSalesManagerRepository<Beneficiary> _repository;
        private readonly IVPlusSalesManagerRepository<BeneficiaryAccount> _accountRepository;
        private readonly VPlusSalesManagerUoWork _uoWork;

        public BeneficiaryRepository()
        {
            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<Beneficiary>(_uoWork);
            _accountRepository = new VPlusSalesManagerRepository<BeneficiaryAccount>(_uoWork);
        }

        public Beneficiary GetBeneficiary(int beneficiaryId)
        {
            try
            {
                return GetBeneficiaries().Find(k => k.BeneficiaryId == beneficiaryId) ?? new Beneficiary();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new Beneficiary();
            }
        }

        public List<Beneficiary> GetBeneficiaries()
        {
            try
            {
                if (!(CacheManager.GetCache("ccBeneficiaryList") is List<Beneficiary> settings) || settings.IsNullOrEmpty())
                {
                    var myItemList = _repository.GetAll().OrderBy(m => m.BeneficiaryId);
                    if (!myItemList.Any()) return new List<Beneficiary>();
                    settings = myItemList.ToList();
                    if (settings.IsNullOrEmpty()) { return new List<Beneficiary>(); }
                    CacheManager.SetCache("ccBeneficiaryList", settings, DateTime.Now.AddYears(1));
                }
                return settings;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }

        internal void resetCache()
        {
            try
            {
                HelperMethods.clearCache("ccBeneficiaryList");
                GetBeneficiaries();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public SettingRegRespObj AddBeneficiary(RegBeneficiaryObj regObj)
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

                //Check for email, mobile number and fullname separately
                if (IsBeneficiaryDuplicate(regObj.Email, regObj.MobileNumber, regObj.FullName, 1, ref response)) { return response; }

                using (var db = _uoWork.BeginTransaction())
                {
                    var BeneficiaryAccount = new BeneficiaryAccount
                    {
                        AvailableBalance = 0,
                        CreditLimit = 0,
                        LastTransactionAmount = 0,
                        LastTransactionType = TransactionType.Unknown,
                        Status = Status.Inactive,
                        LastTransactionId = 0,
                        LastTransactionTimeStamp = DateMap.CurrentTimeStamp(),

                    };

                    var acctAdded = _accountRepository.Add(BeneficiaryAccount);
                    _uoWork.SaveChanges();
                    if (acctAdded.BeneficiaryAccountId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    var item = new Beneficiary
                    {
                        BeneficiaryAccountId = acctAdded.BeneficiaryAccountId,
                        BeneficiaryType = (BeneficiaryType)regObj.BeneficiaryType,
                        Fullname = regObj.FullName,
                        Status = Status.Inactive,
                        MobileNumber = regObj.MobileNumber,
                        Email = regObj.Email,
                        Address = regObj.Address,
                        ApprovedBy = 0,
                        ApproverComment = "",
                        TimeStampApproved = "",
                        TimeStampRegistered = DateMap.CurrentTimeStamp(),

                    };

                    var added = _repository.Add(item);
                    _uoWork.SaveChanges();
                    if (added.BeneficiaryId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }

                    db.Commit();

                    resetCache();
                    response.Status.IsSuccessful = true;
                    response.SettingId = added.BeneficiaryId;
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

        public SettingRegRespObj UpdateBeneficiary(EditBeneficiaryObj regObj)
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

                if (IsBeneficiaryDuplicate(regObj.Email, regObj.MobileNumber, regObj.FullName, 2, ref response)) { return response; }

                var thisBeneficiary = getBeneficiaryInfo(regObj.BeneficiaryId);
                if (thisBeneficiary == null)
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Information found for the specified Beneficiary Id";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Information found!";
                    return response;
                }

                thisBeneficiary.Fullname = !string.IsNullOrWhiteSpace(regObj.FullName) ? regObj.FullName : thisBeneficiary.Fullname;
                thisBeneficiary.BeneficiaryType = regObj.BeneficiaryType > 0 ? (BeneficiaryType)regObj.BeneficiaryType : thisBeneficiary.BeneficiaryType;
                thisBeneficiary.Address = !string.IsNullOrWhiteSpace(regObj.Address) ? regObj.Address : thisBeneficiary.Address;
                thisBeneficiary.Email = !string.IsNullOrWhiteSpace(regObj.Email) ? regObj.Email : thisBeneficiary.Email;
                thisBeneficiary.MobileNumber = !string.IsNullOrWhiteSpace(regObj.MobileNumber) ? regObj.MobileNumber : thisBeneficiary.MobileNumber;

                var added = _repository.Update(thisBeneficiary);
                _uoWork.SaveChanges();
                if (added.BeneficiaryId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }
                resetCache();
                response.Status.IsSuccessful = true;
                response.SettingId = added.BeneficiaryId;

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

        public SettingRegRespObj DeleteBeneficiary(DeleteBeneficiaryObj regObj)
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

                var thisBeneficiary = getBeneficiaryInfo(regObj.BeneficiaryId);
                if (thisBeneficiary == null)
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Information found for the specified Beneficiary Id";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Information found!";
                    return response;
                }

                thisBeneficiary.Fullname =
                    thisBeneficiary.Fullname + "_Deleted_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                thisBeneficiary.Status = Status.Deleted;

                var added = _repository.Update(thisBeneficiary);
                _uoWork.SaveChanges();
                if (added.BeneficiaryId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }

                resetCache();
                response.Status.IsSuccessful = true;
                response.SettingId = added.BeneficiaryId;

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

        public BeneficiaryRespObj LoadBeneficiaries(SettingSearchObj searchObj)
        {
            var response = new BeneficiaryRespObj
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



                var thisBeneficiaries = GetBeneficiaries();
                if (!thisBeneficiaries.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Information found!";
                    response.Status.Message.TechnicalMessage = "No Beneficiary  Information found!";
                    return response;
                }

                if (searchObj.Status > -1)
                {
                    thisBeneficiaries = thisBeneficiaries.FindAll(p => p.Status == (Status)searchObj.Status);
                }

                var beneficiaryItems = new List<BeneficiaryItemObj>();

                foreach (var m in thisBeneficiaries)
                {
                    var associatedAccount = GetBeneficiaryAccountInfo(m.BeneficiaryAccountId);

                    if (associatedAccount == null)
                    {
                        response.Status.Message.FriendlyMessage = "One or More Beneficiary Account Not Found!";
                        response.Status.Message.TechnicalMessage = "One or More Beneficiary Account Not Found!!";
                        return response;
                    }

                    var benAccount = new BeneficiaryAccountObj
                    {
                        BeneficiaryAccountId = associatedAccount.BeneficiaryAccountId,
                        //BeneficiaryId = associatedAccount.BeneficiaryId,
                        AvailableBalance = associatedAccount.AvailableBalance,
                        CreditLimit = associatedAccount.CreditLimit,
                        LastTransactionAmount = associatedAccount.LastTransactionAmount,
                        LastTransactionType = (int)associatedAccount.LastTransactionType,
                        LastTransactionTypeLabel = associatedAccount.LastTransactionType.ToString().Replace("_", " ")
                    };

                    beneficiaryItems.Add(new BeneficiaryItemObj
                    {
                        BeneficiaryId = m.BeneficiaryId,
                        FullName = m.Fullname,
                        MobileNumber = m.MobileNumber,
                        EmailAddress = m.Email,
                        Address = m.Address,
                        BeneficiaryType = (int)m.BeneficiaryType,
                        BeneficiaryTypeLabel = m.BeneficiaryType.ToString().Replace("_", " "),
                        Status = (int)m.Status,
                        StatusLabel = m.Status.ToString().Replace("_", ""),
                        ApprovedBy = m.ApprovedBy,
                        ApproverComment = m.ApproverComment,
                        TimeStampApproved = m.TimeStampApproved,
                        TimeStampRegistered = m.TimeStampRegistered,
                        BeneficiaryAccount = benAccount

                    });
                }


                response.Status.IsSuccessful = true;
                response.Beneficiaries = beneficiaryItems;
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

        public SettingRegRespObj ApproveBeneficiary(ApproveBeneficiaryObj regObj)
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

                if (!HelperMethods.IsUserValid(regObj.AdminUserId, regObj.SysPathCode, HelperMethods.getMgtExecutiveRoles(), ref response.Status.Message))
                {
                    return response;
                }

                var thisBeneficiary = getBeneficiaryInfo(regObj.BeneficiaryId);
                if (thisBeneficiary == null)
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Information found!";
                    response.Status.Message.TechnicalMessage = "No Beneficiary  Information found!";
                    return response;
                }

                if (thisBeneficiary.Status != Status.Inactive)
                {
                    response.Status.Message.FriendlyMessage = "Sorry This Beneficiary Is Not Valid For Approval! Please Try Again Later";
                    response.Status.Message.TechnicalMessage = " Beneficiary Status is either already Active!";
                    return response;
                }



                thisBeneficiary.ApprovedBy = regObj.AdminUserId;
                thisBeneficiary.ApproverComment = regObj.ApproverComment;
                thisBeneficiary.TimeStampApproved = DateMap.CurrentTimeStamp();
                thisBeneficiary.Status = Status.Active;


                var added = _repository.Update(thisBeneficiary);
                _uoWork.SaveChanges();
                if (added.BeneficiaryId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }
                resetCache();
                response.Status.IsSuccessful = true;
                response.SettingId = added.BeneficiaryId;
                response.Status.Message.FriendlyMessage = "Approval Succesful! ";


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

        private bool IsBeneficiaryDuplicate(string email, string mobile, string fullName, int callType, ref SettingRegRespObj response)
        {
            try
            {


                #region Check Email
                var sql1 =
                          $"Select * FROM  \"VPlusSales\".\"Beneficiary\"  WHERE \"Email\" ~ '{email}'";


                var check = _repository.RepositoryContext().Database.SqlQuery<Beneficiary>(sql1).ToList();

                if (check.Count() > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error! Beneficiary email already exist";
                        response.Status.Message.TechnicalMessage = "Duplicate Error! Beneficiary email already exist";
                        return true;
                    }

                }
                #endregion

                #region Check Mobile Number
                var sql =
                         $"Select * FROM  \"VPlusSales\".\"Beneficiary\"  WHERE \"MobileNumber\" ~ '{mobile}'";


                var check1 = _repository.RepositoryContext().Database.SqlQuery<Beneficiary>(sql).ToList();

                if (check1.Count() > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error! Beneficiary mobile already exist";
                        response.Status.Message.TechnicalMessage = "Duplicate Error! Beneficiary mobile already exist";
                        return true;
                    }

                }
                #endregion

                #region Full Name

                var sql2 =
                            $"Select * FROM  \"VPlusSales\".\"Beneficiary\"  WHERE \"Fullname\" ~ '{fullName}'";


                var check2 = _repository.RepositoryContext().Database.SqlQuery<Beneficiary>(sql2).ToList();

                if (check2.Count() > 0)
                {
                    if (callType != 2)
                    {
                        response.Status.Message.FriendlyMessage = "Duplicate Error! Beneficiary full name already exist";
                        response.Status.Message.TechnicalMessage = "Duplicate Error! Beneficiary full name already exist";
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

        private Beneficiary getBeneficiaryInfo(long beneficiaryId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"Beneficiary\" WHERE  \"BeneficiaryId\" = {beneficiaryId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<Beneficiary>(sql1).ToList();
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

        private BeneficiaryAccount GetBeneficiaryAccountInfo(int beneficiaryId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryAccount\" WHERE  \"BeneficiaryAccountId\" = {beneficiaryId};";

                var agentInfos = _accountRepository.RepositoryContext().Database.SqlQuery<BeneficiaryAccount>(sql1).ToList();
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
