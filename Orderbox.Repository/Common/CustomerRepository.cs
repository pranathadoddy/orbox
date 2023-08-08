using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Orderbox.Repository.Common
{
    public class CustomerRepository : BaseRepository<OrderboxContext, ComCustomer, CustomerDto, ulong>, ICustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(
            OrderboxContext context,
            IMapper mapper,
            IConfiguration configuration
        ) : base(context, mapper)
        {
            this._configuration = configuration;
        }

        #region Public Methods

        public async Task<CustomerDto> ReadByCustomerIdAsync(string Id)
        {
            var dbSet = this.Context.Set<ComCustomer>();

            var entity = await
                dbSet
                    .Where(item => item.ExternalId == Id)
                    .OrderByDescending(item => item.CreatedDateTime)
                    .FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }

            var dto = new CustomerDto();
            this.EntityToDto(entity, dto);

            return dto;
        }

       


        #endregion

        #region Private Methods


        #endregion
    }
}
