using Framework.Application.Controllers;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.Seeds;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Controllers
{
    public class SeedController : BaseController
    {
        private readonly IRoleSeed _roleSeed;
        private readonly IUserSeed _userSeed;

        public SeedController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IRoleSeed roleSeed,
            IUserSeed userSeed) :
            base(configuration, webHostEnvironment)
        {
            this._roleSeed = roleSeed;
            this._userSeed = userSeed;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var messages = new List<string>
            {
                //Seed Role
                await this._roleSeed.SeedAdministratorRoleAsync(),
                await this._roleSeed.SeedUserRoleAsync(),
                await this._roleSeed.SeedAgentRoleAsync(),

                //Seed User
                await this._userSeed.SeedAdministratorUserAsync(),
                await this._userSeed.SeedAdministrator2UserAsync(),
                await this._userSeed.SeedAdministrator3UserAsync(),
                await this._userSeed.SeedAdministrator4UserAsync()
            };

            return this.GetSuccessJson(new BasicResponse(), messages);
        }
    }
}