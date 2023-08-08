using AutoMapper;
using Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Threading.Tasks;

namespace Orderbox.Repository.Common
{
    public class RegistrationRepository : BaseRepository<OrderboxContext, ComRegistration, RegistrationDto, ulong>, IRegistrationRepository
    {
        public RegistrationRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<RegistrationDto> ReadByEmailAndVerificationCodeAsync(string email, string code)
        {
            var dbSet = this.Context.Set<ComRegistration>();

            var entity = await dbSet.FirstOrDefaultAsync(item =>
                item.Email == email && item.VerificationCode == code);
            if (entity == null) return null;

            var dto = new RegistrationDto();
            EntityToDto(entity, dto);
            return dto;
        }
    }
}
