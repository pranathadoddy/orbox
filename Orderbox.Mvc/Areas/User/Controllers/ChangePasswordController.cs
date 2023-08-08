using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Mvc.Areas.User.Models.ChangePassword;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class ChangePasswordController : BaseController
    {
        private readonly IUserService _userService;

        public ChangePasswordController(IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            IUserService userService) : base(configuration, hostEnvironment)
        {
            this._userService = userService;
        }

        public IActionResult Index()
        {
            var model = new IndexModel();

            return PartialView("_Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(IndexModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var userId = this.User.Identity.GetUserId();
            var userResponse = await this._userService.ReadByUserIdAsync(new GenericRequest<string> { Data = userId });
            var user = userResponse.Data;

            var response = await this._userService.ChangePasswordAsync(new ChangePasswordRequest
            {
                User = user,
                NewPassword = model.NewPassword,
                OldPassword = model.OldPassword
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return GetSuccessJson(response, null);
        }
    }
}
