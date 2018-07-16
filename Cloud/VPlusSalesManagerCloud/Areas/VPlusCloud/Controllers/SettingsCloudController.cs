using System;
using System.Web.Http;
using VPlusSalesManager.APIObjects.Common;
using VPlusSalesManager.APIObjects.Settings;
using VPlusSalesManager.Business.Service;
using PlugPortal.Cloud.Controllers;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManagerCloud.Areas.VPlusCloud.Controllers
{
    public class SettingsCloudController : FrameworkAuthenticator
    {
        #region Card Type Setting
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesSetting/AddCardType")]
        public SettingRegRespObj AddCardType(RegCardTypeObj appRegObj)
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
                var response = APIServiceManager.AddCardType(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/UpdateCardType")]
        public SettingRegRespObj UpdateCardType(EditCardTypeObj appRegObj)
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
                var response = APIServiceManager.UpdateCardType(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/DeleteCardType")]
        public SettingRegRespObj DeleteCardType(DeleteCardTypeObj appRegObj)
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
                var response = APIServiceManager.DeleteCardType(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/LoadCardTypes")]
        public CardTypeRespObj LoadCardTypes(SettingSearchObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardTypeRespObj
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
                var response = APIServiceManager.LoadCardTypes(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardTypeRespObj
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

        #region Card Commission Setting
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesSetting/AddCardCommission")]
        public SettingRegRespObj AddCardCommission(RegCardCommissionObj appRegObj)
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
                var response = APIServiceManager.AddCardCommission(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/UpdateCardCommission")]
        public SettingRegRespObj UpdateCardCommission(EditCardCommissionObj appRegObj)
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
                var response = APIServiceManager.UpdateCardCommission(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/DeleteCardCommission")]
        public SettingRegRespObj DeleteCardCommission(DeleteCardCommissionObj appRegObj)
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
                var response = APIServiceManager.DeleteCardCommission(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/LoadCardCommissions")]
        public CardCommissionRespObj LoadCardCommissions(SettingSearchObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new CardCommissionRespObj
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
                var response = APIServiceManager.LoadCardCommissions(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new CardCommissionRespObj
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

        #region Beneficiary Setting
        [HttpPost]
        [Route("VPlusSalesManager/VPlusSalesSetting/AddBeneficiary")]
        public SettingRegRespObj AddBeneficiary(RegBeneficiaryObj appRegObj)
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
                var response = APIServiceManager.AddBeneficiary(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/UpdateBeneficiary")]
        public SettingRegRespObj UpdateBeneficiary(EditBeneficiaryObj appRegObj)
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
                var response = APIServiceManager.UpdateBeneficiary(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/DeleteBeneficiary")]
        public SettingRegRespObj DeleteBeneficiary(DeleteBeneficiaryObj appRegObj)
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
                var response = APIServiceManager.DeleteBeneficiary(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/ApproveBeneficiary")]
        public SettingRegRespObj ApproveBeneficiary(ApproveBeneficiaryObj appRegObj)
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
                var response = APIServiceManager.ApproveBeneficiary(appRegObj);
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
        [Route("VPlusSalesManager/VPlusSalesSetting/LoadBeneficiaries")]
        public BeneficiaryRespObj LoadBeneficiaries(SettingSearchObj appRegObj)
        {
            try
            {
                var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                if (string.IsNullOrEmpty(secData?.AuthToken))
                {
                    return new BeneficiaryRespObj
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
                var response = APIServiceManager.LoadBeneficiaries(appRegObj);
                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                return new BeneficiaryRespObj
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

        //#region Sales Requisition

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/AddSalesRequisition")]
        //public SalesRequisitionRegRespObj AddSalesRequisition(RegSalesRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.AddSalesRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}
        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/UpdateSalesRequisition")]
        //public SalesRequisitionRegRespObj UpdateSalesRequisition(EditSalesRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.UpdateSalesRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/ApproveSalesRequisition")]
        //public SalesRequisitionRegRespObj ApproveSalesRequisition(ApproveSalesRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.ApproveSalesRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/IssueSalesRequisition")]
        //public SalesRequisitionRegRespObj IssueSalesRequisition(IssueSalesRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.IssueSalesRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/DeleteSalesRequisition")]
        //public SettingRegRespObj DeleteSalesRequisition(DeleteSalesRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SettingRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.DeleteSalesRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SettingRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}
        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadSalesRequisitions")]
        //public SalesRequisitionRespObj LoadSalesRequisitions(SettingSearchObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRequisitionRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadSalesRequisitions(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRequisitionRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadSalesRequisitionByDate")]
        //public SalesRequisitionRespObj LoadSalesRequisitionByDate(LoadSalesRequisitionByDateObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRequisitionRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadSalesRequisitionByDate(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRequisitionRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //#endregion

        //#region Sales Retirement
        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/AddSalesRetirement")]
        //public SalesRetirementRegRespObj AddSalesRetirement(RegSalesRetirementObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRetirementRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.AddSalesRetirement(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRetirementRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/UpdateSalesRetirement")]
        //public SalesRetirementRegRespObj UpdateSalesRetirement(EditSalesRetirementObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRetirementRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.UpdateSalesRetirement(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRetirementRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/ApproveSalesRetirement")]
        //public SalesRetirementRegRespObj ApproveSalesRetirement(ApproveSalesRetirementObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRetirementRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.ApproveSalesRetirement(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRetirementRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadSalesRetirements")]
        //public SalesRetirementRespObj LoadSalesRetirements(SettingSearchObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRetirementRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadSalesRetirements(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRetirementRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/DeleteSalesRetirement")]
        //public SettingRegRespObj DeleteSalesRetirement(DeleteSalesRetirementObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SettingRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.DeleteSalesRetirement(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SettingRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadSalesRetirementByDate")]
        //public SalesRetirementRespObj LoadSalesRetirementByDate(LoadSalesRetirementByDateObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRetirementRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadSalesRetirementByDate(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRetirementRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}
        //#endregion

        //#region Supply Requisition
        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/AddSupplyRequisition")]
        //public SupplyRequisitionRegRespObj AddSupplyRequisition(RegSupplyRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SupplyRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.AddSupplyRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SupplyRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/UpdateSupplyRequisition")]
        //public SupplyRequisitionRegRespObj UpdateSupplyRequisition(EditSupplyRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SupplyRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.UpdateSupplyRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SupplyRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/ApproveSupplyRequisition")]
        //public SupplyRequisitionRegRespObj ApproveSupplyRequisition(ApproveSupplyRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SupplyRequisitionRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.ApproveSupplyRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SupplyRequisitionRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/DeleteSupplyRequisition")]
        //public SettingRegRespObj DeleteSupplyRequisition(DeleteSupplyRequisitionObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SettingRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.DeleteSupplyRequisition(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SettingRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadSupplyRequisitions")]
        //public SupplyRequisitionRespObj LoadSupplyRequisitions(SettingSearchObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SupplyRequisitionRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadSupplyRequisitions(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SupplyRequisitionRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadSupplyRequisitionByDate")]
        //public SupplyRequisitionRespObj LoadSupplyRequisitionByDate(LoadSupplyRequisitionByDateObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SupplyRequisitionRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadSupplyRequisitionByDate(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SupplyRequisitionRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}
        //#endregion

        //#region Card Delivery
        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/AddCardDelivery")]
        //public CardDeliveryRegRespObj AddCardDelivery(RegCardDeliveryObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new CardDeliveryRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.AddCardDelivery(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new CardDeliveryRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        ////[HttpPost]
        ////[Route("LRTradePlus/LRTradePlusTransactions/UpdateCardDelivery")]
        ////public CardDeliveryRegRespObj UpdateCardDelivery(EditCardDeliveryObj appRegObj)
        ////{
        ////    try
        ////    {
        ////        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        ////        if (string.IsNullOrEmpty(secData?.AuthToken))
        ////        {
        ////            return new CardDeliveryRegRespObj
        ////            {
        ////                Status = new APIResponseStatus
        ////                {
        ////                    IsSuccessful = false,
        ////                    Message = new APIResponseMessage
        ////                    {
        ////                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        ////                        TechnicalMessage = "Authentication Error"
        ////                    },
        ////                }
        ////            };
        ////        }

        ////        appRegObj.SysPathCode = secData.AuthToken;
        ////        appRegObj.AdminUserId = secData.UserId;
        ////        var response = APIServiceManager.UpdateCardDelivery(appRegObj);
        ////        return response;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        ////        return new CardDeliveryRegRespObj
        ////        {
        ////            Status = new APIResponseStatus
        ////            {
        ////                IsSuccessful = false,
        ////                Message = new APIResponseMessage
        ////                {
        ////                    FriendlyMessage = "Unable to complete your request! Please try again later",
        ////                    TechnicalMessage = "Error: " + ex.Message
        ////                },
        ////            }
        ////        };

        ////    }
        ////}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/ApproveCardDelivery")]
        //public CardDeliveryRegRespObj ApproveCardDelivery(ApproveCardDeliveryObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new CardDeliveryRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.ApproveCardDelivery(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new CardDeliveryRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/DeleteCardDelivery")]
        //public SettingRegRespObj DeleteCardDelivery(DeleteCardDeliveryObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SettingRegRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.DeleteCardDelivery(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SettingRegRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadCardDeliveries")]
        //public CardDeliveryRespObj LoadCardDeliveries(SettingSearchObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new CardDeliveryRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadCardDeliveries(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new CardDeliveryRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}

        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/LoadCardDeliveryByDate")]
        //public CardDeliveryRespObj LoadCardDeliveryByDate(LoadCardDeliveryByDateObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new CardDeliveryRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.LoadCardDeliveryByDate(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new CardDeliveryRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}
        //#endregion

        //#region Retirement Report By Date
        //[HttpPost]
        //[Route("LRTradePlus/LRTradePlusTransactions/GetSalesRetirementReportByDate")]
        //public SalesRetirementsReportRespObj GetSalesRetirementReportByDate(GetSalesRetirementsReportByDateObj appRegObj)
        //{
        //    try
        //    {
        //        var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
        //        if (string.IsNullOrEmpty(secData?.AuthToken))
        //        {
        //            return new SalesRetirementsReportRespObj
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Authentication Failed! Invalid Request Source",
        //                        TechnicalMessage = "Authentication Error"
        //                    },
        //                }
        //            };
        //        }

        //        appRegObj.SysPathCode = secData.AuthToken;
        //        appRegObj.AdminUserId = secData.UserId;
        //        var response = APIServiceManager.GetSalesRetirementReportByDate(appRegObj);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
        //        return new SalesRetirementsReportRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Unable to complete your request! Please try again later",
        //                    TechnicalMessage = "Error: " + ex.Message
        //                },
        //            }
        //        };

        //    }
        //}
        //#endregion
    }
}
