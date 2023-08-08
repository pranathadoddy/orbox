using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Identity;
using Orderbox.Core.Resources.Account;
using Orderbox.Dto.Authentication;
using Orderbox.ServiceContract.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using Orderbox.ServiceContract.Email;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orderbox.Service.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUserDto> _userManager;
        private readonly IResetPasswordEmailManager _resetPasswordEmailManager;

        public UserService(UserManager<ApplicationUserDto> userManager,
            IResetPasswordEmailManager resetPasswordEmailManager)
        {
            _userManager = userManager;
            this._resetPasswordEmailManager = resetPasswordEmailManager;
        }

        public async Task<GenericResponse<ApplicationUserDto>> InsertAsync(UserRequest request)
        {
            var response = new GenericResponse<ApplicationUserDto>();

            var result = await _userManager.CreateAsync(request.User, request.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.Description);
                }
                return response;
            }

            result = await _userManager.AddToRoleAsync(request.User, request.Role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.Description);
                }
                return response;
            }

            response.Data = request.User;
            response.Data.Id = request.User.Id;
            response.AddInfoMessage(GeneralResource.Info_Saved);

            return response;
        }

        public async Task<GenericResponse<ApplicationUserDto>> ReadByUserNameAsync(GenericRequest<string> request)
        {
            var response = new GenericResponse<ApplicationUserDto>();

            response.Data = await _userManager.FindByNameAsync(request.Data);

            if (response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }

        public async Task<GenericResponse<ApplicationUserDto>> UpdateAsync(UserRequest request)
        {
            var response = new GenericResponse<ApplicationUserDto>();

            var user = await _userManager.FindByIdAsync(request.User.Id);
            user.IsActive = request.User.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.Description);
                }
                return response;
            }

            if (!string.IsNullOrEmpty(request.Role) && !await _userManager.IsInRoleAsync(user, request.Role))
            {
                var roles = await _userManager.GetRolesAsync(user);
                result = await _userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        response.AddErrorMessage(error.Description);
                    }
                    return response;
                }

                result = await _userManager.AddToRoleAsync(user, request.Role);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        response.AddErrorMessage(error.Description);
                    }
                    return response;
                }
            }

            response.Data = request.User;
            response.AddInfoMessage(GeneralResource.Info_Saved);

            return response;
        }

        public async Task<BasicResponse> DeleteAsync(GenericRequest<string> request)
        {
            var response = new BasicResponse();

            var user = await _userManager.FindByIdAsync(request.Data);
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.Description);
                }
                return response;
            }

            response.AddInfoMessage(GeneralResource.Info_Deleted);

            return response;
        }

        public async Task<GenericResponse<ApplicationUserDto>> ReadByUserIdAsync(GenericRequest<string> request)
        {
            var response = new GenericResponse<ApplicationUserDto>();

            response.Data = await _userManager.FindByIdAsync(request.Data);

            if (response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }

        public async Task<BasicResponse> SendResetPasswordEmailAsync(GenericWithEmailRequest<string> request)
        {
            var replacementTemplateData = JsonSerializer.Serialize(new Dictionary<string, string>
            {
                { "EMAIL_RESET_PASSWORD_URL", request.Url }
            });

            this._resetPasswordEmailManager.Recipients.Add(new Dictionary<string, string> {
                { "ToAddress", request.Data },
                { "ReplacementTemplateData", replacementTemplateData }
            });

            await this._resetPasswordEmailManager.SendAsync();

            return new BasicResponse();
        }

        public async Task<BasicResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new BasicResponse();

            var user = await _userManager.FindByNameAsync(request.EmailAddress);

            if (user == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            var result = await this._userManager.ResetPasswordAsync(user,
                request.PasswordResetToken,
                request.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage($"{error.Description}");
                }
            }

            return response;
        }

        public async Task<BasicResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = new BasicResponse();

            var result = await this._userManager.ChangePasswordAsync(request.User, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.Description);
                }

                return response;
            }

            response.AddInfoMessage(ChangePasswordResource.SuccessInfo);

            return response;
        }
    }
}
