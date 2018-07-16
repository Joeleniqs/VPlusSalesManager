using System;
using System.Web.Http;
using VPlusSalesManager.APIObjects.Common;
using VPlusSalesManager.APIObjects.Settings;
using VPlusSalesManager.Business.Service;
using PlugPortal.Cloud.Controllers;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManagerCloud.Areas.TestEngineCloud.Controllers
{
    public class TransactionCloudController : FrameworkAuthenticator
    {
        #region Transaction Service
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/AddCardRequisition")]
        public CardRequisitionRegRespObj AddCardRequisition(RegCardRequisitionObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRequisitionRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.AddCardRequisition(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRequisitionRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/UpdateCardRequisition")]
        public CardRequisitionRegRespObj UpdateCardRequisition(EditCardRequisitionObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRequisitionRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.UpdateCardRequisition(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRequisitionRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/DeleteCardRequisition")]
        public SettingRegRespObj DeleteCardRequisition(DeleteCardRequisitionObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new SettingRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.DeleteCardRequisition(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new SettingRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/ApproveCardRequisition")]
        public CardRequisitionRegRespObj ApproveCardRequisition(ApproveCardRequisitionObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRequisitionRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.ApproveCardRequisition(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRequisitionRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/LoadCardRequisitions")]
        public CardRequisitionRespObj LoadCardRequisitions(SettingSearchObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRequisitionRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.LoadCardRequisitions(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRequisitionRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/LoadCardRequisitionsByDate")]
        public CardRequisitionRespObj LoadCardRequisitionByDate(LoadCardRequisitionByDateObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRequisitionRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.LoadCardRequisitionByDate(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRequisitionRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }

        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/AddBeneficiaryPayment")]
        public BeneficiaryPaymentRegRespObj AddBeneficiaryPayment(RegBeneficiaryPaymentObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new BeneficiaryPaymentRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.AddBeneficiaryPayment(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new BeneficiaryPaymentRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }

        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/LoadBeneficiaryPayments")]
        public BeneficiaryPaymentRespObj LoadBeneficiaryPayments(SettingSearchObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new BeneficiaryPaymentRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.LoadBeneficiaryPayments(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new BeneficiaryPaymentRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }

        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/LoadBeneficiaryPaymentsByDate")]
        public BeneficiaryPaymentRespObj LoadBeneficiaryPaymentsByDate(LoadBeneficiaryPaymentsByDateObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new BeneficiaryPaymentRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.LoadBeneficiaryPaymentsByDate(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new BeneficiaryPaymentRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }

        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesTransaction/LoadBeneficiaryAccountTransactionsByDate")]
        public BeneficiaryAccountTransactionRespObj LoadBeneficiaryAccountTransactionsByDate(LoadBeneficiaryAccountTransactionsByDateObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new BeneficiaryAccountTransactionRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.LoadBeneficiaryAccountTransactionsByDate(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new BeneficiaryAccountTransactionRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }

        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesProduction/AddCardIssuance")]
        public CardIssuanceRegRespObj AddCardIssuance(RegCardIssuanceObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardIssuanceRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.AddCardIssuance(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardIssuanceRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }

        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesProduction/LoadCardIssuanceByDate")]
        public CardIssuanceRespObj LoadCardIssuanceByDate(LoadCardIssuanceByDateObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardIssuanceRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Authentication Failed! Invalid Request Source",
                                TechnicalMessage = "Authentication Error"
                            },
                        }
                    };
                }

                appRegObj.SysPathCode = secData.AuthToken;
                appRegObj.AdminUserId = secData.UserId;
                var response = APIServiceManager.LoadCardIssuanceByDate(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardIssuanceRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to complete your request! Please try again later",
                            TechnicalMessage = "Error: " + ex.Message
                        },
                    }
                };

            }
        }
 
        #endregion
    }
}
