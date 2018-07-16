using System;
using System.Linq;
using System.Web.Http;
using VPlusSalesManager.APIObjects.Common;
using VPlusSalesManager.APIObjects.Settings;
using VPlusSalesManager.Business.Service;
using PlugPortal.Cloud.Controllers;
using PlugPortal.Cloud.PortalCore;
using XPLUG.WEBTOOLS;

namespace VPlusSalesManagerCloud.Areas.TestEngineCloud.Controllers
{
    public class CandidateCloudController : FrameworkAuthenticator
    {
        #region Candidate Management
            [HttpPost]
            [Route("JetTest/CandidateManagement/AddCandidate")]
            public CandidateRegRespObj AddCandidate(RegCandidateObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new CandidateRegRespObj
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
                    var response = APIServiceManager.AddCandidate(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new CandidateRegRespObj
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
            [Route("JetTest/CandidateManagement/AddBulkCandidateInfo")]
            public BulkRegRespObj AddBulkCandidateInfo(BulkCandidateRegObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new BulkRegRespObj
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
                    var response = APIServiceManager.AddBulkCandidateInfo(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new BulkRegRespObj
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
            [Route("JetTest/CandidateManagement/UpdateCandidatenfo")]
            public CandidateRegRespObj UpdateCandidatenfo(EditCandidateObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new CandidateRegRespObj
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
                    var response = APIServiceManager.UpdateCandidate(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new CandidateRegRespObj
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
            [Route("JetTest/CandidateManagement/DeleteCandidate")]
            public CandidateRegRespObj DeleteCandidate(DeleteCandidateObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new CandidateRegRespObj
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
                    var response = APIServiceManager.DeleteCandidate(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new CandidateRegRespObj
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
            [Route("JetTest/CandidateManagement/LoadCandidates")]
            public CandidateRespObj LoadCandidates(CandidateSearchObj appRegObj)
            {
                try
                {
                    var secData = GetAuthenticationData(Request, appRegObj.SysPathCode);
                    if (string.IsNullOrEmpty(secData?.AuthToken))
                    {
                        return new CandidateRespObj
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
                    var response = APIServiceManager.LoadCandidates(appRegObj);
                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new CandidateRespObj
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

        
            [AllowAnonymous]
            [HttpPost]
            [Route("JetTest/CandidateManagement/LoginCandidate")]
            public CandidateLoginResp LoginCandidate(CandidateLoginObj appRegObj)
            {
                try
                {
                    var ipAddress = NetworkHelper.GetClientIp(Request) ?? "";
                    appRegObj.LoginSourceIP = ipAddress;
                var response = APIServiceManager.LoginCandidate(appRegObj);
                    if (!response.Status.IsSuccessful)
                    {
                        return response;

                    }

                    var mySec = new SecObj
                    {
                        AuthToken = response.AuthToken,
                        RoleIds = "6",
                        RoleNames = "Candidate",
                        Username = appRegObj.Username,
                        UserId = response.UserId,
                    };
                    var secToken = PortalSecurity.GenerateToken(mySec);
                    if (string.IsNullOrEmpty(secToken))
                    {
                        return new CandidateLoginResp
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Post Authentication Failed -1",
                                    TechnicalMessage = "Error: Unable to process Authentication"
                                },
                            },
                        };
                    }

                    var secTokenData = secToken.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!secTokenData.Any() || secTokenData.Length != 3)
                    {
                        return new CandidateLoginResp
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Post Authentication Failed -2",
                                    TechnicalMessage = "Error: Unable to process Authentication"
                                },
                            },
                        };
                    }


                    response.UserId = 0;
                    response.Username = appRegObj.Username;
                    response.AuthToken = secTokenData[2];
                    response.CustomSetting = secTokenData[0] + "." + secTokenData[1];

                    return response;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.Source, ex.StackTrace, ex.Message);
                    return new CandidateLoginResp
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
