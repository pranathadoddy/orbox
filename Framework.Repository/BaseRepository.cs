using AutoMapper;
using Framework.Core;
using Framework.Dto;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Repository
{
    public abstract class BaseRepository<TContext, TEntity, TDto, TDtoType> : IBaseRepository<TDto>
        where TContext : DbContext, new()
        where TEntity : class, new()
        where TDto : BaseDto<TDtoType>, new()
    {
        protected BaseRepository(TContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected IMapper Mapper { get; }

        protected TContext Context { get; }

        public virtual async Task<TDto> InsertAsync(TDto dto)
        {
            var entity = new TEntity();
            DtoToEntity(dto, entity);

            var dbSet = this.Context.Set<TEntity>();
            dbSet.Add(entity);
            var numObj = await this.Context.SaveChangesAsync();
            if (numObj > 0)
            {
                var type = entity.GetType();
                var prop = type.GetProperty("Id");
                dto.Id = (TDtoType)Convert.ChangeType(prop.GetValue(entity).ToString(), typeof(TDtoType));
            }

            return dto;
        }

        public virtual async Task<TDto> ReadAsync(object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();
            var entity = await dbSet.FindAsync(primaryKey);
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.FindAsync(dto.Id);
            if (entity == null) return null;

            DtoToEntity(dto, entity);
            dbSet.Update(entity);

            await this.Context.SaveChangesAsync();

            return dto;
        }

        public virtual async Task<TDto> DeleteAsync(object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.FindAsync(primaryKey);
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);

            dbSet.Remove(entity);
            await this.Context.SaveChangesAsync();

            return dto;
        }

        public virtual async Task<PagedSearchResult<TDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = this.Context.Set<TEntity>();

            var queryable =
                string.IsNullOrEmpty(parameter.Filters)
                    ? dbSet.AsQueryable()
                    : dbSet.Where(parameter.Filters);

            queryable =
                string.IsNullOrEmpty(parameter.Keyword)
                    ? queryable
                    : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }


        public virtual async Task<ICollection<TDto>> BulkInsert(ICollection<TDto> dtoCollection)
        {

            var dbSet = this.Context.Set<TEntity>();

            foreach (var dto in dtoCollection)
            {
                var entity = new TEntity();
                DtoToEntity(dto, entity);

                dbSet.Add(entity);
            }

            await this.Context.SaveChangesAsync();

            return dtoCollection;
        }


        protected virtual void DtoToEntity(TDto dto, TEntity entity)
        {
            Mapper.Map(dto, entity);
        }

        protected virtual void EntityToDto(TEntity entity, TDto dto)
        {
            Mapper.Map(entity, dto);
        }

        protected virtual async Task<PagedSearchResult<TDto>> GetPagedSearchEnumerableAsync(
            PagedSearchParameter parameter,
            IQueryable<TEntity> queryable)
        {
            var result = new PagedSearchResult<TDto>();

            queryable = string.IsNullOrEmpty(parameter.OrderByFieldName)
                ? GetOrderedQueryableEntity(queryable, "Id", FrameworkCoreConstant.SortOrder.Ascending)
                : GetOrderedQueryableEntity(queryable, parameter.OrderByFieldName, parameter.SortOrder);

            result.Count = await queryable.CountAsync();

            var entityList = parameter.PageSize == -1
                ? await queryable.ToListAsync()
                : await queryable.Skip(parameter.PageIndex * parameter.PageSize).Take(parameter.PageSize).ToListAsync();

            foreach (var entity in entityList)
            {
                var dto = new TDto();
                EntityToDto(entity, dto);
                result.Result.Add(dto);
            }

            return result;
        }

        protected virtual IOrderedQueryable<TEntity> GetOrderedQueryableEntity(IQueryable<TEntity> queryable,
            string orderByFieldName,
            string sortOrder)
        {
            orderByFieldName = FirstCharToUpper(orderByFieldName);

            var orderByMethodName = "OrderBy";
            if (sortOrder.Equals(FrameworkCoreConstant.SortOrder.Descending))
                orderByMethodName = "OrderByDescending";

            var typeParams = new[] { Expression.Parameter(typeof(TEntity), "") };

            Type[] type;
            LambdaExpression lambda;

            if (orderByFieldName.Contains('.'))
            {
                var nestedProps = orderByFieldName.Split('.');
                Expression body = typeParams[0];
                var listOfType = new List<Type> { typeof(TEntity) };
                var baseType = typeof(TEntity);
                foreach (var nestedProp in nestedProps)
                {
                    body = Expression.PropertyOrField(body, nestedProp);
                    baseType = baseType.GetProperty(nestedProp).PropertyType;
                    listOfType.Add(baseType);
                }

                type = new[] { listOfType.First(), listOfType.Last() };
                lambda = Expression.Lambda(body, typeParams);
            }
            else
            {
                var pi = typeof(TEntity).GetProperty(orderByFieldName);
                type = new[] { typeof(TEntity), pi.PropertyType };
                lambda = Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams);
            }

            return (IOrderedQueryable<TEntity>)queryable.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    orderByMethodName,
                    type,
                    queryable.Expression,
                    lambda)
            );
        }

        protected string FirstCharToUpper(string orderByFieldName)
        {
            orderByFieldName = orderByFieldName.First().ToString().ToUpper() + orderByFieldName.Substring(1);
            return orderByFieldName;
        }

        protected virtual IQueryable<TEntity> GetKeywordPagedSearchQueryable(IQueryable<TEntity> entities,
            string keyword)
        {
            return entities.AsQueryable();
        }
    }
}
