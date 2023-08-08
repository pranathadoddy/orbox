using Framework.Application.Controllers;
using Framework.Core;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Core.Resources.Account;
using Orderbox.Dto.Authentication;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Infrastructure.Helper;
using Orderbox.Mvc.Infrastructure.ServerUtility.Tool;
using Orderbox.Mvc.Models.Account;
using Orderbox.ServiceContract.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Common.Request;
using Orderbox.ServiceContract.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Orderbox.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUserDto> _signInManager;
        private readonly UserManager<ApplicationUserDto> _userManager;

        private readonly IAgentService _agentService;
        private readonly IRegistrationService _registrationService;
        private readonly IUserService _userService;
        private readonly ITenantService _tenantService;
        private readonly IRecaptchaValidator _recaptchaValidator;

        public AccountController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            SignInManager<ApplicationUserDto> signInManager,
            UserManager<ApplicationUserDto> userManager,
            IAgentService agentService,
            IRegistrationService registrationService,
            IUserService userService,
            ITenantService tenantService,
            IRecaptchaValidator recaptchaValidator) :
            base(configuration, webHostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _agentService = agentService;
            _registrationService = registrationService;
            _userService = userService;
            _tenantService = tenantService;
            _recaptchaValidator = recaptchaValidator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var role = User.Claims.First(c => c.Type == ClaimTypes.Role);
                return Redirect($"/{role.Value}/Home/Index");
            }

            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return GetErrorJsonFromModelState();

            var userName = model.EmailAddress;
            var result =
                await _signInManager.PasswordSignInAsync(
                    userName,
                    model.Password,
                    true,
                    false);

            if (!result.Succeeded) return GetErrorJson(LoginResource.InvalidLoginInfo);

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl =
                    string.IsNullOrEmpty(model.ReturnUrl) ?
                        "/Account/Login" : model.ReturnUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Account/Login");
        }

        [HttpGet]
        public IActionResult Registration(string aid)
        {
            var model = new RegistrationModel {
                AgencyId = aid
            };

            return View(model);
        }

        public Microsoft.AspNetCore.Http.HttpRequest GetRequest()
        {
            return Request;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) return GetErrorJsonFromModelState();

            var isValidCaptcha = await this._recaptchaValidator.IsValidResponseAsync(model.CaptchaToken);
            if (!isValidCaptcha)
            {
                return this.GetErrorJson(new string[] {
                    RegistrationResource.InvalidRegistration
                });
            }

            var user = await this._userManager.FindByNameAsync(model.EmailAddress);
            if (user != null)
            {
                return GetErrorJson(RegistrationResource.EmailAddressHasBeenUsed);
            }

            var currentUtcTime = DateTime.UtcNow;

            var response = await _registrationService.RegisterAsync(
                new GenericWithEmailRequest<RegistrationDto>
                {
                    Data = new RegistrationDto
                    {
                        Email = model.EmailAddress,
                        VerificationCode = RandomCodeGenerator.Generate(),
                        CreatedBy = model.EmailAddress,
                        CreatedDateTime = currentUtcTime,
                        LastModifiedBy = model.EmailAddress,
                        LastModifiedDateTime = currentUtcTime
                    }
                });

            if (response.IsError()) return GetErrorJson(response);

            var token = Cryptographer.Encrypt($"{model.EmailAddress}|{model.AgencyId}");

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Account/Verification?token={HttpUtility.UrlEncode(token)}"
            });
        }

        [HttpGet]
        public IActionResult Verification(string token)
        {
            var model = new VerificationModel
            {
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Verification(VerificationModel model)
        {
            if (!ModelState.IsValid) return GetErrorJsonFromModelState();

            var emailAndAgencyId = Cryptographer.Decrypt(model.Token);
            var splitedEmailAndAgencyId = emailAndAgencyId.Split("|");
            var response = await _registrationService.ReadByEmailAndVerificationCodeAsync(
                new GenericRequest<RegistrationDto>
                {
                    Data = new RegistrationDto
                    {
                        Email = splitedEmailAndAgencyId[0].ToString(),
                        VerificationCode = model.Code
                    }
                });

            if (response.IsError())
                return GetErrorJson(response);

            var token = Cryptographer.Encrypt($"{response.Data.Id.ToString()}|{splitedEmailAndAgencyId[1].ToString()}");

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Account/Activation?token={HttpUtility.UrlEncode(token)}"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Activation(string token)
        {
            var registrationIdAndAgencyIdString = Cryptographer.Decrypt(token.Trim());
            var splitedRegistrationIdAndAgencyIdString = registrationIdAndAgencyIdString.Split("|");
            if (!ulong.TryParse(splitedRegistrationIdAndAgencyIdString[0], out var registrationId))
                return NotFound();

            var response = await _registrationService.ReadAsync(new GenericRequest<ulong>
            {
                Data = registrationId
            });
            if (response.IsError()) return NotFound();

            var domainPostfix = this.Configuration.GetValue<string>("Application:DomainPostfix");
            var model = new ActivationModel
            {
                Token = token,
                DomainPostfix = domainPostfix,
                Countries = CountryHelper.GenerateCountrySelecList(string.Empty)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Activation(ActivationModel model)
        {
            if (!ModelState.IsValid) return GetErrorJsonFromModelState();

            var registrationIdAndAgencyIdString = Cryptographer.Decrypt(model.Token);
            var splitedRegistrationIdAndAgencyIdString = registrationIdAndAgencyIdString.Split("|");
            if (!ulong.TryParse(splitedRegistrationIdAndAgencyIdString[0].ToString(), out var registrationId))
                return GetErrorJson(RegistrationResource.InvalidToken);

            var domainPostfix = this.Configuration.GetValue<string>("Application:DomainPostfix");
            var tenantDomain = model.TenantDomain.ToLower();

            var isSubdomainExistResponse = await this._tenantService.IsDomainExistAsync(new IsDomainExistRequest
            {
                Id = 0,
                DomainPostfix = string.Format($"{tenantDomain}{domainPostfix}")
            });

            if (isSubdomainExistResponse.Data)
            {
                return this.GetErrorJson(isSubdomainExistResponse);
            }

            var response = await _registrationService.ReadAsync(new GenericRequest<ulong>
            {
                Data = registrationId
            });
            if (response.IsError()) return NotFound();

            var insertResponse = await this._userService.InsertAsync(new UserRequest
            {
                User = new ApplicationUserDto
                {
                    UserName = response.Data.Email,
                    Email = response.Data.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = true,
                    TimeZone = "Singapore Standard Time"
                },
                Password = model.Password,
                Role = CoreConstant.Role.User
            });

            if (insertResponse.IsError())
            {
                return this.GetErrorJson(insertResponse);
            }

            var regionInfo = new RegionInfo(model.CountryId);
            ulong? agencyId = null;
            if(!string.IsNullOrEmpty(splitedRegistrationIdAndAgencyIdString[1]))
            {
                var stringAgencyId = Cryptographer.Base64OTPDecrypt(splitedRegistrationIdAndAgencyIdString[1]);
                agencyId = ulong.Parse(stringAgencyId);
            }
            var tenantDto = new TenantDto
            {
                Name = model.BusinessName,
                UserId = insertResponse.Data.Id,
                ShortName = tenantDomain,
                OrderboxDomain = string.Format($"{tenantDomain}{domainPostfix}"),
                IsActive = true,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                CountryCode = model.CountryId,
                PhoneAreaCode = model.AreaCode,
                Phone = model.Phone,
                Currency = regionInfo.ISOCurrencySymbol,
                AgencyId = agencyId,
                EnableShop = !agencyId.HasValue,
                CreatedBy = "Registration"
            };
            this.PopulateAuditFieldsOnCreate(tenantDto);

            var tenantInsertResponse = await this._tenantService.InsertAsync(new GenericRequest<TenantDto>
            {
                Data = tenantDto
            });

            if (tenantInsertResponse.IsError())
            {
                return this.GetErrorJson(tenantInsertResponse);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Account/ActivationSuccess"
            });
        }

        [HttpGet]
        public IActionResult ActivationSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AgentActivation(string token)
        {
            try
            {
                var dictionary = Cryptographer.JsonWebTokenDecode(token);

                var model = new AgentActivationModel
                {
                    Token = token,
                };

                return View(model);
            }
            catch (Exception)
            {
                return View("AgentActivationInvalid");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgentActivation(AgentActivationModel model)
        {
            try
            {
                var dictionary = Cryptographer.JsonWebTokenDecode(model.Token);
                var agentId = ulong.Parse(dictionary["AgentId"].ToString());
                var emailAddress = dictionary["EmailAddress"].ToString();

                var insertResponse = await this._userService.InsertAsync(new UserRequest
                {
                    User = new ApplicationUserDto
                    {
                        UserName = emailAddress,
                        Email = emailAddress,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsActive = true,
                        TimeZone = "Singapore Standard Time"
                    },
                    Password = model.Password,
                    Role = CoreConstant.Role.Agent
                });

                if (insertResponse.IsError())
                {
                    return this.GetErrorJson(insertResponse);
                }

                var agentResponse =
                    await this._agentService.ReadAsync(new GenericRequest<ulong>
                    {
                        Data = agentId
                    });

                if (agentResponse.IsError())
                {
                    return this.GetErrorJson(agentResponse);
                }

                var agentDto = agentResponse.Data;
                agentDto.UserId = insertResponse.Data.Id;
                agentDto.Status = CoreConstant.AgentStatus.Active;
                this.PopulateAuditFieldsOnUpdate(agentDto);

                var updateAgentResponse =
                    await this._agentService.UpdateAsync(new GenericRequest<AgentDto>
                    {
                        Data = agentDto
                    });

                if (updateAgentResponse.IsError())
                {
                    return this.GetErrorJson(updateAgentResponse);
                }

                return Json(new
                {
                    IsSuccess = true,
                    RedirectUrl = $"/Account/ActivationSuccess"
                });
            }
            catch (Exception)
            {
                return this.GetErrorJson(RegistrationResource.AgentActivationInvalidTitle);
            }
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            var model = new ForgetPasswordModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel model)
        {
            if (!ModelState.IsValid) return GetErrorJsonFromModelState();

            var user = await this._userManager.FindByNameAsync(model.EmailAddress);
            if (user == null)
            {
                return GetErrorJson(ForgetPasswordResource.EmailNotFound);
            }

            var expirationInDay = Configuration.GetValue<int>("ResetPasswordExpirationInDays");
            var secondsSinceEpoch = DateTimeManager.GetSecondSinceEpoch(expirationInDay);
            var passwordResetToken = await this._userManager.GeneratePasswordResetTokenAsync(user);

            var token = Cryptographer.JsonWebTokenEncode(new Dictionary<string, object>
            {
                {"exp", secondsSinceEpoch},
                {"EmailAddress", model.EmailAddress},
                {"PasswordResetToken", passwordResetToken }
            });

            await this._userService.SendResetPasswordEmailAsync(new GenericWithEmailRequest<string>
            {
                Data = model.EmailAddress,
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Account/ResetPassword?token={token}"
            });

            return this.Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Account/ForgetPasswordConfirmation"
            });
        }

        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var model = new ResetPasswordModel
            {
                Token = token
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid) return GetErrorJsonFromModelState();

            var decodedToken = Cryptographer.JsonWebTokenDecode(model.Token);

            var response = await this._userService.ResetPasswordAsync(new ResetPasswordRequest
            {
                EmailAddress = (string)decodedToken["EmailAddress"],
                PasswordResetToken = (string)decodedToken["PasswordResetToken"],
                NewPassword = model.NewPassword
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return this.Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Account/ResetPasswordConfirmation"
            });
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }
    }
}