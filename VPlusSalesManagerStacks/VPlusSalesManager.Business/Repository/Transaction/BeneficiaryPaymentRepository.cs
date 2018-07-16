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
    internal class BeneficiaryPaymentRepository
    {

        private readonly IVPlusSalesManagerRepository<BeneficiaryPayment> _repository;
        private readonly IVPlusSalesManagerRepository<Beneficiary> _beneficiaryRepository;
        private readonly IVPlusSalesManagerRepository<BeneficiaryAccount> _accountRepository;
        private readonly IVPlusSalesManagerRepository<BeneficiaryAccountTransaction> _transactRepository;
        private readonly VPlusSalesManagerUoWork _uoWork;
        public BeneficiaryPaymentRepository()
        {

            _uoWork = new VPlusSalesManagerUoWork();
            _repository = new VPlusSalesManagerRepository<BeneficiaryPayment>(_uoWork);
            _accountRepository = new VPlusSalesManagerRepository<BeneficiaryAccount>(_uoWork);
            _beneficiaryRepository = new VPlusSalesManagerRepository<Beneficiary>(_uoWork);
            _transactRepository = new VPlusSalesManagerRepository<BeneficiaryAccountTransaction>(_uoWork);

        }
        public BeneficiaryPaymentRegRespObj AddBeneficiaryPayment(RegBeneficiaryPaymentObj regObj)
        {
            var response = new BeneficiaryPaymentRegRespObj
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

                if (!DataCheck.IsNumeric(regObj.PaymentReference))
                {

                    response.Status.Message.FriendlyMessage = "Error Occurred! Payment reference must be Numbers Only";
                    response.Status.Message.TechnicalMessage = "Payment Reference  is invalid";
                    return response;
                }

                var associatedBeneficiary = getBeneficiaryInfo(regObj.BeneficiaryId);
                if (associatedBeneficiary == null)
                {
                    response.Status.Message.FriendlyMessage = "Ooops ! Beneficiary Does not Exist!";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Information Found";
                    return response;
                }

                var associatedBeneficiaryAccount = GetBeneficiaryAccountInfo(associatedBeneficiary.BeneficiaryAccountId);
                if (associatedBeneficiaryAccount == null)
                {
                    response.Status.Message.FriendlyMessage = "Sorry! Beneficiary Does not Have An Account With Us! Contact Administrator";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Account Information Found";
                    return response;
                }

                if (associatedBeneficiary.Status != Status.Active) {
                    response.Status.Message.FriendlyMessage = "Sorry!This Beneficiary Cannot Perform Any Transaction Yet! Please Contact Administrator";
                    response.Status.Message.TechnicalMessage = "Beneficiary Isn't Approved yet";
                    return response;
                }

                //store date for Concurrency...
                var nowDateTime = DateMap.CurrentTimeStamp();
                var nowDate = nowDateTime.Substring(0, nowDateTime.IndexOf(' '));
                var nowTime = nowDateTime.Substring(nowDateTime.IndexOf('-') + 1);


                using (var db = _uoWork.BeginTransaction())
                {
                    #region Beneficiary Account Transaction Operation

                    var newBeneficiaryTransaction = new BeneficiaryAccountTransaction
                    {
                        BeneficiaryAccountId = associatedBeneficiaryAccount.BeneficiaryAccountId,
                        BeneficiaryId = regObj.BeneficiaryId,
                        PreviousBalance = associatedBeneficiaryAccount.AvailableBalance,
                        Amount = regObj.AmountPaid,
                        NewBalance = (associatedBeneficiaryAccount.AvailableBalance + regObj.AmountPaid),
                        TransactionType = TransactionType.Credit,
                        TransactionSource = (TransactionSourceType)regObj.TransactionSourceId,
                        Status = Status.Active,
                        RegisteredBy = regObj.AdminUserId,
                        TimeStampRegistered = nowDateTime,

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

                    #region Beneficiary Payment Operation
                    var newBeneficiaryPayment = new BeneficiaryPayment
                    {
                        BeneficiaryId = regObj.BeneficiaryId,
                        BeneficiaryAccountId = associatedBeneficiaryAccount.BeneficiaryAccountId,
                        PaymentDate = nowDate,
                        PaymentReference = regObj.PaymentReference,
                        PaymentSource = (PaySource)regObj.PaymentSource,
                        PaymentSourceName = ((PaySource)regObj.PaymentSource).ToString().Replace("_", " "),
                        RegisteredBy = regObj.AdminUserId,
                        TimeStampRegistered = nowDateTime,
                        Status = Status.Active,
                        AmountPaid = regObj.AmountPaid,
                        BeneficiaryAccountTransactionId = transactionAdded.BeneficiaryAccountTransactionId,

                    };

                    var paymentAdded = _repository.Add(newBeneficiaryPayment);
                    _uoWork.SaveChanges();
                    if (paymentAdded.BeneficiaryPaymentId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }
                    #endregion

                    #region Beneficiary Account Update

                    associatedBeneficiaryAccount.AvailableBalance = transactionAdded.NewBalance;
                    associatedBeneficiaryAccount.CreditLimit = 0;
                    associatedBeneficiaryAccount.LastTransactionAmount = transactionAdded.Amount;
                    associatedBeneficiaryAccount.LastTransactionType = transactionAdded.TransactionType;
                    associatedBeneficiaryAccount.Status = Status.Active;
                    associatedBeneficiaryAccount.LastTransactionId = transactionAdded.BeneficiaryAccountTransactionId;
                    associatedBeneficiaryAccount.LastTransactionTimeStamp = DateMap.CurrentTimeStamp();




                    var acctAdded = _accountRepository.Update(associatedBeneficiaryAccount);
                    _uoWork.SaveChanges();
                    if (acctAdded.BeneficiaryAccountId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                        response.Status.Message.TechnicalMessage = "Unable to save to database";
                        return response;
                    }
                    #endregion

                    db.Commit();

                    response.Status.IsSuccessful = true;
                    response.PaymentId = paymentAdded.BeneficiaryPaymentId;
                    response.Status.Message.FriendlyMessage = "Payment Successful";
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

        public BeneficiaryPaymentRespObj LoadBeneficiaryPayments(SettingSearchObj searchObj)
        {
            var response = new BeneficiaryPaymentRespObj
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



                var thisBeneficiaryPayments = GetBeneficiaryPayments(searchObj);
                if (!thisBeneficiaryPayments.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Payment Information found!";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Payment Information found!";
                    return response;
                }

                var beneficiaryPayments = new List<BeneficiaryPaymentObj>();

                foreach (var m in thisBeneficiaryPayments)
                {

                    beneficiaryPayments.Add(new BeneficiaryPaymentObj
                    {
                        BeneficiaryId = m.BeneficiaryId,
                        BeneficiaryAccountTransactionId = m.BeneficiaryAccountTransactionId,
                        BeneficiaryAccountId = m.BeneficiaryAccountId,
                        AmountPaid = m.AmountPaid,
                        BeneficiaryPaymentId = m.BeneficiaryPaymentId,
                        PaymentDate = m.PaymentDate,
                        PaymentReference = m.PaymentReference,
                        Status = (int)m.Status,
                        StatusLabel = m.Status.ToString().Replace("_", " "),
                        PaymentSource = (int)m.PaymentSource,
                        PaymentSourceName = m.PaymentSource.ToString().Replace("_", " "),
                        RegisteredBy = m.RegisteredBy,
                        TimeStampRegistered = m.TimeStampRegistered
                    });
                }

                response.Status.IsSuccessful = true;
                response.BeneficiaryPayments = beneficiaryPayments;
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

        public BeneficiaryPaymentRespObj LoadBeneficiaryPaymentsByDate(LoadBeneficiaryPaymentsByDateObj regObj)
        {
            var response = new BeneficiaryPaymentRespObj
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

                var thisBeneficiaryPayments = GetBeneficiaryPayments(regObj.BeneficiaryId);

                if (!thisBeneficiaryPayments.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Payments  Information found!";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Payments  Information found!";
                    return response;
                }

                var BeneficiaryPaymentsByDate = new List<BeneficiaryPaymentObj>();

                foreach (var m in thisBeneficiaryPayments)
                {
                    var dateCreated = m.TimeStampRegistered;
                    var value = dateCreated.IndexOf(' ');
                    if (value > 0) { dateCreated = dateCreated.Substring(0, value); }
                    var realDate = DateTime.Parse(dateCreated);
                    if (realDate >= DateTime.Parse(regObj.BeginDate) && realDate <= DateTime.Parse(regObj.EndDate))
                    {

                        BeneficiaryPaymentsByDate.Add(new BeneficiaryPaymentObj
                        {
                            BeneficiaryId = m.BeneficiaryId,
                            BeneficiaryAccountTransactionId = m.BeneficiaryAccountTransactionId,
                            BeneficiaryAccountId = m.BeneficiaryAccountId,
                            AmountPaid = m.AmountPaid,
                            BeneficiaryPaymentId = m.BeneficiaryPaymentId,
                            PaymentDate = m.PaymentDate,
                            PaymentReference = m.PaymentReference,
                            Status = (int)m.Status,
                            StatusLabel = m.Status.ToString().Replace("_", " "),
                            PaymentSource = (int)m.PaymentSource,
                            PaymentSourceName = m.PaymentSource.ToString().Replace("_", " "),
                            RegisteredBy = m.RegisteredBy,
                            TimeStampRegistered = m.TimeStampRegistered
                        });
                    }
                }


                response.Status.IsSuccessful = true;
                response.BeneficiaryPayments = BeneficiaryPaymentsByDate;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BeneficiaryPaymentRespObj();
            }

        }

        public BeneficiaryAccountTransactionRespObj LoadBeneficiaryAccountTransactionsByDate(LoadBeneficiaryAccountTransactionsByDateObj regObj)
        {
            var response = new BeneficiaryAccountTransactionRespObj
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

                var thisBeneficiaryAccountTransactions = GetBeneficiaryAccountTransactions(regObj.BeneficiaryId);

                if (!thisBeneficiaryAccountTransactions.Any())
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Payments  Information found!";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Payments  Information found!";
                    return response;
                }

                var BeneficiaryAccountTransactionsByDate = new List<BeneficiaryAccountTransactionObj>();

                foreach (var m in thisBeneficiaryAccountTransactions)
                {
                    if (!string.IsNullOrWhiteSpace(regObj.BeginDate) && !string.IsNullOrWhiteSpace(regObj.EndDate))
                    {
                        var dateCreated = m.TimeStampRegistered;
                        var value = dateCreated.IndexOf(' ');
                        if (value > 0) { dateCreated = dateCreated.Substring(0, value); }
                        var realDate = DateTime.Parse(dateCreated);
                        if (realDate >= DateTime.Parse(regObj.BeginDate) && realDate <= DateTime.Parse(regObj.EndDate))
                        {

                            BeneficiaryAccountTransactionsByDate.Add(new BeneficiaryAccountTransactionObj
                            {
                                BeneficiaryId = m.BeneficiaryId,
                                Amount = m.Amount,
                                BeneficiaryAccountId = m.BeneficiaryAccountId,
                                NewBalance = m.NewBalance,
                                PreviousBalance = m.PreviousBalance,
                                TransactionSource = (int)m.TransactionSource,
                                TransactionSourceLabel = m.TransactionSource.ToString().Replace("_", " "),
                                TransactionType = (int)m.TransactionType,
                                TransactionTypeLabel = m.TransactionType.ToString().Replace("_", " "),
                                Status = (int)m.Status,
                                StatusLabel = m.Status.ToString().Replace("_", " "),
                                RegisteredBy = m.RegisteredBy,
                                TimeStampRegistered = m.TimeStampRegistered
                            });
                        }
                    }
                    else
                    {
                        BeneficiaryAccountTransactionsByDate.Add(new BeneficiaryAccountTransactionObj
                        {
                            BeneficiaryId = m.BeneficiaryId,
                            Amount = m.Amount,
                            BeneficiaryAccountId = m.BeneficiaryAccountId,
                            NewBalance = m.NewBalance,
                            PreviousBalance = m.PreviousBalance,
                            TransactionSource = (int)m.TransactionSource,
                            TransactionSourceLabel = m.TransactionSource.ToString().Replace("_", " "),
                            TransactionType = (int)m.TransactionType,
                            TransactionTypeLabel = m.TransactionType.ToString().Replace("_", " "),
                            Status = (int)m.Status,
                            StatusLabel = m.Status.ToString().Replace("_", " "),
                            RegisteredBy = m.RegisteredBy,
                            TimeStampRegistered = m.TimeStampRegistered
                        });
                    }
                }

                response.Status.IsSuccessful = true;
                response.BeneficiaryAccountTransactions = BeneficiaryAccountTransactionsByDate;
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BeneficiaryAccountTransactionRespObj();
            }

        }

        public BeneficiaryPaymentRegRespObj DeleteBeneficiaryPayment(DeleteBeneficiaryPaymentObj regObj)
        {
            var response = new BeneficiaryPaymentRegRespObj
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

                var thisBeneficiaryPayment = GetBeneficiaryPaymentInfo(regObj.BeneficiaryPaymentId);
                if (thisBeneficiaryPayment == null)
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Payment Information found for the specified BeneficiaryPayment Id";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Payment Information found!";
                    return response;
                }

                thisBeneficiaryPayment.Status = Status.Deleted;

                var added = _repository.Update(thisBeneficiaryPayment);
                _uoWork.SaveChanges();
                if (added.BeneficiaryPaymentId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }


                response.Status.IsSuccessful = true;
                response.PaymentId = added.BeneficiaryPaymentId;

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

        public BeneficiaryAccountTransactionRegRespObj DeleteBeneficiaryAccountTransaction(DeleteBeneficiaryAccountTransactionObj regObj)
        {
            var response = new BeneficiaryAccountTransactionRegRespObj
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

                var thisBeneficiaryAccountTransaction = GetBeneficiaryAccountTransactionInfo(regObj.BeneficiaryAccountTransactionId);
                if (thisBeneficiaryAccountTransaction == null)
                {
                    response.Status.Message.FriendlyMessage = "No Beneficiary Payment Information found for the specified BeneficiaryAccountTransaction Id";
                    response.Status.Message.TechnicalMessage = "No Beneficiary Payment Information found!";
                    return response;
                }

                thisBeneficiaryAccountTransaction.Status = Status.Deleted;

                var added = _transactRepository.Update(thisBeneficiaryAccountTransaction);
                _uoWork.SaveChanges();
                if (added.BeneficiaryAccountTransactionId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to complete your request. Please try again later";
                    response.Status.Message.TechnicalMessage = "Unable to save to database";
                    return response;
                }


                response.Status.IsSuccessful = true;
                response.TransactionId = added.BeneficiaryAccountTransactionId;

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

        private List<BeneficiaryAccountTransaction> GetBeneficiaryAccountTransactions(int beneficiaryId)
        {
            try
            {
                if (beneficiaryId > 0)
                {

                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryAccountTransaction\" WHERE \"BeneficiaryId\" = {beneficiaryId}");


                    var agentInfos = _transactRepository.RepositoryContext().Database.SqlQuery<BeneficiaryAccountTransaction>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<BeneficiaryAccountTransaction>() : agentInfos;
                }
                else
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryAccountTransaction\" ORDER BY \"BeneficiaryAccountTransactionId\" ");

                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<BeneficiaryAccountTransaction>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<BeneficiaryAccountTransaction>() : agentInfos;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BeneficiaryAccountTransaction>();
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

        private BeneficiaryAccount GetBeneficiaryAccountInfo(int beneficiaryAccountId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryAccount\" WHERE  \"BeneficiaryAccountId\" = {beneficiaryAccountId};";

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

        private List<BeneficiaryPayment> GetBeneficiaryPayments(SettingSearchObj searchObj)
        {
            try
            {
                if (searchObj.Status == -2)
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryPayment\"");


                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<BeneficiaryPayment>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<BeneficiaryPayment>() : agentInfos;
                }
                else
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryPayment\" WHERE \"Status\" = {searchObj.Status}");


                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<BeneficiaryPayment>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<BeneficiaryPayment>() : agentInfos;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BeneficiaryPayment>();
            }
        }

        private List<BeneficiaryPayment> GetBeneficiaryPayments(int beneficiaryId)
        {
            try
            {

                if (beneficiaryId > 0)
                {

                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryPayment\" WHERE \"BeneficiaryId\" = {beneficiaryId} ");


                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<BeneficiaryPayment>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<BeneficiaryPayment>() : agentInfos;
                }
                else
                {
                    var sql = new StringBuilder();
                    sql.Append($"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryPayment\" ORDER BY \"BeneficiaryPaymentId\" ");


                    var agentInfos = _repository.RepositoryContext().Database.SqlQuery<BeneficiaryPayment>(sql.ToString()).ToList();

                    return !agentInfos.Any() ? new List<BeneficiaryPayment>() : agentInfos;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BeneficiaryPayment>();
            }
        }

        private BeneficiaryPayment GetBeneficiaryPaymentInfo(long beneficiaryPaymentId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryPayment\" WHERE  \"BeneficiaryPaymentId\" = {beneficiaryPaymentId};";

                var agentInfos = _repository.RepositoryContext().Database.SqlQuery<BeneficiaryPayment>(sql1).ToList();
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

        private BeneficiaryAccountTransaction GetBeneficiaryAccountTransactionInfo(long beneficiaryAccountTransactionId)
        {
            try
            {

                var sql1 = $"SELECT *  FROM  \"VPlusSales\".\"BeneficiaryAccountTransaction\" WHERE  \"BeneficiaryAccountTransactionId\" = {beneficiaryAccountTransactionId};";

                var agentInfos = _transactRepository.RepositoryContext().Database.SqlQuery<BeneficiaryAccountTransaction>(sql1).ToList();
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
