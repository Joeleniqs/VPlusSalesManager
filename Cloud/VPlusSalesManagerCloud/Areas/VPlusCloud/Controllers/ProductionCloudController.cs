using System;
using System.Web.Http;
using VPlusSalesManager.APIObjects.Common;
using VPlusSalesManager.APIObjects.Settings;
using VPlusSalesManager.Business.Service;
using PlugPortal.Cloud.Controllers;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManagerCloud.Areas.VPlusCloud.Controllers
{
    public class ProductionCloudController : FrameworkAuthenticator
    {
        #region Card Production Service
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesProduction/AddCardProduction")]
        public CardRegRespObj AddCard(RegCardObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRegRespObj
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
                var response = APIServiceManager.AddCard(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRegRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/UpdateCardProduction")]
        public CardRegRespObj UpdateCard(EditCardObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRegRespObj
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
                var response = APIServiceManager.UpdateCard(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRegRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/LoadCardProductionByDate")]
        public CardRespObj LoadCardByDate(LoadCardByDateObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRespObj
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
                var response = APIServiceManager.LoadCardByDate(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/DeleteCardProduction")]
        public SettingRegRespObj DeleteCard(DeleteCardObj appRegObj)
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
                var response = APIServiceManager.DeleteCard(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesProduction/LoadCardProductions")]
        public CardRespObj LoadCards(SettingSearchObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardRespObj
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
                var response = APIServiceManager.LoadCards(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/AddCardDelivery")]
        public CardDeliveryRegRespObj AddCardDelivery(RegCardDeliveryObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardDeliveryRegRespObj
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
                var response = APIServiceManager.AddCardDelivery(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardDeliveryRegRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/UpdateCardDelivery")]
        public CardDeliveryRegRespObj UpdateCardDelivery(EditCardDeliveryObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardDeliveryRegRespObj
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
                var response = APIServiceManager.UpdateCardDelivery(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardDeliveryRegRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/LoadCardDeliveryByDate")]
        public CardDeliveryRespObj LoadCardDeliveriesByDate(LoadCardDeliveryByDateObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardDeliveryRespObj
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
                var response = APIServiceManager.LoadCardDeliveriesByDate(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardDeliveryRespObj
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
        [Route("VPlusSalesManager/VPlusSalesProduction/ApproveCardDelivery")]
        public CardDeliveryRegRespObj ApproveCardDelivery(ApproveCardDeliveryObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardDeliveryRegRespObj
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
                var response = APIServiceManager.ApproveCardDelivery(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardDeliveryRegRespObj
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
