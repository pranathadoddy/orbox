using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Transaction;
using Orderbox.RepositoryContract.Transaction;
using System;
using System.Collections.Generic;

namespace Orderbox.Repository.Transaction
{
    public class OrderItemRepository : BaseTenantRepository<OrderboxContext, TrxOrderItem, OrderItemDto, ulong>, IOrderItemRepository
    {
        #region constructor

        public OrderItemRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion

        #region Public Methods

        public async void BulkInsertAsync(ulong orderId, ICollection<OrderItemDto> orderItemDtos)
        {
            using (var transactionScope = await this.Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var dbSet = this.Context.Set<TrxOrderItem>();
                    foreach (var dto in orderItemDtos)
                    {
                        dto.Id = orderId;
                        var entity = new TrxOrderItem();
                        this.DtoToEntity(dto, entity);

                        dbSet.Add(entity);
                    }

                    await this.Context.SaveChangesAsync();
                    await transactionScope.CommitAsync();
                }
                catch (Exception)
                {
                    transactionScope.Rollback();
                    throw;
                }
            }
        }

        #endregion
    }
}
