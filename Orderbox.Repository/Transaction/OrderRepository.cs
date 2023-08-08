using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Transaction;
using Orderbox.RepositoryContract.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Orderbox.Repository.Transaction
{
    public class OrderRepository : BaseTenantRepository<OrderboxContext, TrxOrder, OrderDto, ulong>, IOrderRepository
    {
        private readonly IConfiguration _configuration;

        public OrderRepository(
            OrderboxContext context,
            IMapper mapper,
            IConfiguration configuration
        ) : base(context, mapper)
        {
            this._configuration = configuration;
        }

        #region Public Methods

        public override async Task<OrderDto> InsertAsync(OrderDto dto)
        {
            var entity = new TrxOrder();
            DtoToEntity(dto, entity);

            entity.TrxOrderItems = new List<TrxOrderItem>();
            foreach (var orderItemDto in dto.OrderItems)
            {
                var orderItemEntity = new TrxOrderItem();
                this.Mapper.Map(orderItemDto, orderItemEntity);
                entity.TrxOrderItems.Add(orderItemEntity);
            }

            var dbSet = this.Context.Set<TrxOrder>();
            dbSet.Add(entity);
            var numObj = await this.Context.SaveChangesAsync();
            if (numObj > 0)
            {
                var type = entity.GetType();
                var prop = type.GetProperty("Id");
                dto.Id = (ulong)Convert.ChangeType(prop.GetValue(entity).ToString(), typeof(ulong));
            }

            return dto;
        }

        public override async Task<OrderDto> TenantReadAsync(ulong tenantId, object primaryKey)
        {
            var dbSet = this.Context.Set<TrxOrder>();

            var entity = await dbSet
                .Include(o => o.TrxOrderItems)
                .Include(o => o.TrxOrderAdditionalCharges)
                .FirstOrDefaultAsync(o => o.TenantId == tenantId && o.Id == (ulong)primaryKey);

            if (entity == null)
            {
                return null;
            }

            var dto = new OrderDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public override async Task<PagedSearchResult<OrderDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = this.Context.Set<TrxOrder>();

            var queryable =
                string.IsNullOrEmpty(parameter.Filters) ?
                    dbSet
                        .Include(item => item.Tenant)
                        .Include(item => item.TrxOrderItems)
                        .Include(item => item.TrxOrderAdditionalCharges)
                        .AsQueryable() :
                    dbSet
                        .Include(item => item.Tenant)
                        .Include(item => item.TrxOrderItems)
                        .Include(item => item.TrxOrderAdditionalCharges)
                        .Where(parameter.Filters);

            queryable =
                string.IsNullOrEmpty(parameter.Keyword)
                    ? queryable
                    : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }

        public async Task<OrderDto> GetLatestOrderOfTheCurrentTenantAsync(ulong tenantId)
        {
            var dbSet = this.Context.Set<TrxOrder>();
            
            var entity = await
                dbSet
                    .Where(item => item.TenantId == tenantId)
                    .OrderByDescending(item => item.CreatedDateTime)
                    .FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }

            var dto = new OrderDto();
            this.EntityToDto(entity, dto);

            return dto;
        }

        public async Task<OrderDto> ReadByIdAsync(ulong CustomerId, ulong Id)
        {
            var dbSet = this.Context.Set<TrxOrder>();

            var entity = await
                dbSet
                    .Where(item => item.CustomerId == CustomerId && item.Id == Id)
                    .OrderByDescending(item => item.CreatedDateTime)
                    .FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }

            var dto = new OrderDto();
            this.EntityToDto(entity, dto);

            return dto;
        }

        #endregion

        protected override IQueryable<TrxOrder> GetKeywordPagedSearchQueryable(IQueryable<TrxOrder> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();
            return entities.Where(item =>
               item.BuyerName.ToLower().Contains(loweredKeyword) ||
               item.BuyerPhoneNumber.ToLower().Contains(loweredKeyword) ||
               item.OrderNumber.ToLower().Contains(loweredKeyword)
            );
        }

        protected override void EntityToDto(TrxOrder entity, OrderDto dto)
        {
            base.EntityToDto(entity, dto);

            if (entity.TrxOrderItems != null) {
                foreach(var orderItemEntity in entity.TrxOrderItems)
                {
                    var orderItemDto = new OrderItemDto();
                    this.Mapper.Map(orderItemEntity, orderItemDto);
                    dto.OrderItems.Add(orderItemDto);
                }
            }

            if (entity.TrxOrderAdditionalCharges != null)
            {
                foreach (var additionalChargeEntity in entity.TrxOrderAdditionalCharges)
                {
                    var additionalChargeDto = new OrderAdditionalChargeDto();
                    this.Mapper.Map(additionalChargeEntity, additionalChargeDto);
                    dto.OrderAdditionalCharge.Add(additionalChargeDto);
                }
            }
        }
    }
}
