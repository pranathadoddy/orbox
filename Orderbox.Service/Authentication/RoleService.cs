using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orderbox.Dto.Authentication;
using Orderbox.ServiceContract.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Service.Authentication
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRoleDto> _roleManager;

        public RoleService(RoleManager<ApplicationRoleDto> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<GenericResponse<ApplicationRoleDto>> InsertAsync(GenericRequest<string> request)
        {
            var response = new GenericResponse<ApplicationRoleDto>();

            var role = new ApplicationRoleDto
            {
                Name = request.Data
            };

            var result = await this._roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.Description);
                }
                return response;
            }

            response.Data = role;
            response.AddInfoMessage(GeneralResource.Info_Saved);

            return response;
        }

        public async Task<GenericResponse<ApplicationRoleDto>> ReadAsync(GenericRequest<string> request)
        {
            var response = new GenericResponse<ApplicationRoleDto>();

            response.Data = await this._roleManager.FindByNameAsync(request.Data);

            if (response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }

        public async Task<BasicResponse> DeleteAsync(GenericRequest<string> request)
        {
            var response = new BasicResponse();

            var role = await this._roleManager.FindByIdAsync(request.Data);
            var result = await this._roleManager.DeleteAsync(role);

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

        public async Task<GenericResponse<List<ApplicationRoleDto>>> AllRoleAsync()
        {
            var response = new GenericResponse<List<ApplicationRoleDto>>();

            response.Data = await this._roleManager.Roles.ToListAsync();

            if (!response.Data.Any())
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }
    }
}
