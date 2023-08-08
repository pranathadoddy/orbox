using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Core.Resources.Account;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Email;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orderbox.Service.Common
{
    public class RegistrationService : BaseService<RegistrationDto, ulong, IRegistrationRepository>, IRegistrationService
    {
        private readonly IVerificationEmailManager _verificationEmailManager;

        public RegistrationService(
            IRegistrationRepository repository,
            IVerificationEmailManager verificationEmailManager) : base(repository)
        {
            this._verificationEmailManager = verificationEmailManager;
        }

        public async Task<GenericResponse<RegistrationDto>> RegisterAsync(
            GenericWithEmailRequest<RegistrationDto> request)
        {
            var response = await InsertAsync(request);

            if (!response.IsError())
            {
                var replacementTemplateData = JsonSerializer.Serialize(new Dictionary<string, string>
                {
                    { "EMAIL_VERIFICATION_CODE", request.Data.VerificationCode }
                });

                _verificationEmailManager.Recipients.Add(new Dictionary<string, string> {
                    { "ToAddress", request.Data.Email },
                    { "ReplacementTemplateData",  replacementTemplateData }
                });

                await _verificationEmailManager.SendAsync();
            }

            return response;
        }

        public async Task<GenericResponse<RegistrationDto>> ReadByEmailAndVerificationCodeAsync(
            GenericRequest<RegistrationDto> request)
        {
            var response = new GenericResponse<RegistrationDto>();

            response.Data =
                await _repository.ReadByEmailAndVerificationCodeAsync(
                    request.Data.Email,
                    request.Data.VerificationCode
                );

            if (response.Data == null) response.AddErrorMessage(RegistrationResource.EmailNotRegistered);

            return response;
        }
    }
}
