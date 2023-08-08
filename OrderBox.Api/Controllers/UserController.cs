using Framework.Application.Controllers;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Api.Infrastructure.ServerUtility.Identity;

namespace Orderbox.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Google")]
    public class UserController : ApiBaseController
    {
        public UserController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment
        ) : base(configuration, hostEnvironment)
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            var firstName = this.User.Identity.GetFirstName();
            var lastName = this.User.Identity.GetLastName();
            var email = this.User.Identity.GetEmail();
            var picture = this.User.Identity.GetPicture();

            return this.GetSuccessJson(new BasicResponse(), new
            {
                firstName,
                lastName,
                email,
                picture
            });
        }
    }
}
