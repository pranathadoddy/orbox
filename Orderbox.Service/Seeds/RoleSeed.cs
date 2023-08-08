using Framework.ServiceContract.Request;
using Orderbox.ServiceContract.Seeds;
using Orderbox.ServiceContract.Authentication;
using System.Threading.Tasks;

namespace Orderbox.Service.Seeds
{
    public class RoleSeed : IRoleSeed
    {
        private readonly IRoleService _roleService;

        public RoleSeed(IRoleService roleService)
        {
            this._roleService = roleService;
        }

        public async Task<string> SeedAdministratorRoleAsync()
        {
            return await this.Seed("Administrator");
        }

        public async Task<string> SeedUserRoleAsync()
        {
            return await this.Seed("User");
        }

        public async Task<string> SeedAgentRoleAsync()
        {
            return await this.Seed("Agent");
        }

        private async Task<string> Seed(string roleName)
        {
            var readResponse = await this._roleService.ReadAsync(new GenericRequest<string>
            {
                Data = roleName
            });

            if (readResponse.Data != null)
            {
                return $"Role with name {roleName} is already exist.";
            }

            var insertResponse = await this._roleService.InsertAsync(new GenericRequest<string>
            {
                Data = roleName
            });

            if (insertResponse.IsError())
            {
                return $"Failed to insert role with name {roleName}.";
            }

            return $"Role with name {roleName} has been successfully inserted.";
        }
    }
}
