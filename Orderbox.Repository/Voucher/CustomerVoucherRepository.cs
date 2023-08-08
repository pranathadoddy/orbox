using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Voucher;
using Orderbox.RepositoryContract.Voucher;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Orderbox.Repository.Voucher
{
    public class CustomerVoucherRepository : BaseRepository<OrderboxContext, VchCustomerVoucher, CustomerVoucherDto, ulong>, ICustomerVoucherRepository
    {
        public CustomerVoucherRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public override async Task<PagedSearchResult<CustomerVoucherDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = this.Context.Set<VchCustomerVoucher>();

            var queryable =
                string.IsNullOrEmpty(parameter.Filters)
                    ? dbSet
                        .Include(item => item.Customer)
                        .Include(item => item.Voucher)
                        .AsQueryable()
                    : dbSet
                        .Include(item => item.Customer)
                        .Include(item => item.Voucher)
                        .Where(parameter.Filters);

            queryable =
                string.IsNullOrEmpty(parameter.Keyword)
                    ? queryable
                    : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }
    }
}
