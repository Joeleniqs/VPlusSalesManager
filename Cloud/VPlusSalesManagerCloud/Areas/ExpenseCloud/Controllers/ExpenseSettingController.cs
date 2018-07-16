using System;
using System.Web.Http;
using ExpenseManager.APIObjects.Common;
using ExpenseManager.APIObjects.Settings;
using ExpenseManager.Business.Service;
using PlugPortal.Cloud.Controllers;
using XPLUG.WEBTOOLS;

namespace JetTestEngineCloud.Areas.ExpenseCloud.Controllers
{
    public class ExpenseSettingController : FrameworkAuthenticator
    {
        #region Account Head Setting
            [HttpPost]
            [Route("ExpenseMgt/ExpenseSetting/AddAccountHead")]
            public SettingRegRespObj AddAccountHead(RegAccountHeadObj appRegObj)
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
                    var response = APIServiceManager.AddAccountHead(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/UpdateAccountHead")]
            public SettingRegRespObj UpdateAccountHead(EditAccountHeadObj appRegObj)
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
                    var response = APIServiceManager.UpdateAccountHead(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/DeleteAccountHead")]
            public SettingRegRespObj DeleteAccountHead(DeleteAccountHeadObj appRegObj)
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
                    var response = APIServiceManager.DeleteAccountHead(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/LoadAccountHeads")]
            public AccountHeadRespObj LoadAccountHeads(SettingSearchObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new AccountHeadRespObj
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
                    var response = APIServiceManager.LoadAccountHeads(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new AccountHeadRespObj
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
        #region Expense Category
            [HttpPost]
            [Route("ExpenseMgt/ExpenseSetting/AddExpenseCategory")]
            public SettingRegRespObj AddExpenseCategory(RegExpenseCategoryObj appRegObj)
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
                    var response = APIServiceManager.AddExpenseCategory(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/UpdateExpenseCategory")]
            public SettingRegRespObj UpdateExpenseCategory(EditExpenseCategoryObj appRegObj)
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
                    var response = APIServiceManager.UpdateExpenseCategory(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/DeleteExpenseCategory")]
            public SettingRegRespObj DeleteExpenseCategory(DeleteExpenseCategoryObj appRegObj)
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
                    var response = APIServiceManager.DeleteExpenseCategory(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/LoadExpenseCategories")]
            public ExpenseCategoryRespObj LoadExpenseCategories(SettingSearchObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new ExpenseCategoryRespObj
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
                    var response = APIServiceManager.LoadExpenseCategories(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new ExpenseCategoryRespObj
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
        #region Expense Item
            [HttpPost]
            [Route("ExpenseMgt/ExpenseSetting/AddExpenseItem")]
            public SettingRegRespObj AddExpenseItem(RegExpenseItemObj appRegObj)
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
                    var response = APIServiceManager.AddExpenseItem(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/UpdateExpenseItem")]
            public SettingRegRespObj UpdateExpenseItem(EditExpenseItemObj appRegObj)
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
                    var response = APIServiceManager.UpdateExpenseItem(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/ApprovedExpenseItem")]
            public SettingRegRespObj ApprovedExpenseItem(ApproveExpenseItemObj appRegObj)
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
                    var response = APIServiceManager.ApprovedExpenseItem(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/DeleteExpenseItem")]
            public SettingRegRespObj DeleteExpenseItem(DeleteExpenseItemObj appRegObj)
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
                    var response = APIServiceManager.DeleteExpenseItem(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/LoadExpenseItems")]
            public ExpenseItemRespObj LoadExpenseItems(SettingSearchObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new ExpenseItemRespObj
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
                    var response = APIServiceManager.LoadExpenseItems(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new ExpenseItemRespObj
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
        #region Expense Type
            [HttpPost]
            [Route("ExpenseMgt/ExpenseSetting/AddExpenseType")]
            public SettingRegRespObj AddExpenseType(RegExpenseTypeObj appRegObj)
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
                    var response = APIServiceManager.AddExpenseType(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/UpdateExpenseType")]
            public SettingRegRespObj UpdateExpenseType(EditExpenseTypeObj appRegObj)
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
                    var response = APIServiceManager.UpdateExpenseType(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/DeleteExpenseType")]
            public SettingRegRespObj DeleteExpenseType(DeleteExpenseTypeObj appRegObj)
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
                    var response = APIServiceManager.DeleteExpenseType(appRegObj);
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
            [Route("ExpenseMgt/ExpenseSetting/LoadExpenseCategories")]
            public ExpenseTypeRespObj LoadExpenseTypes(SettingSearchObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new ExpenseTypeRespObj
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
                    var response = APIServiceManager.LoadExpenseTypes(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new ExpenseTypeRespObj
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
