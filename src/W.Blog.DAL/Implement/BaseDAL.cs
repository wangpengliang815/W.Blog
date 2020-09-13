namespace W.Blog.DAL.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EFCore.BulkExtensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using W.Blog.Entity.Entitys;

    public interface IBaseDAL<TEntity, TContext>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        Task<TEntity> FindAsync(int id);

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<List<TEntity>> GetListAsync();

        Task<List<TEntity>> BatchInsertAsync(List<TEntity> entityList);

        Task<List<TEntity>> BatchUpdateAsync(List<TEntity> entityList);

        Task<List<TEntity>> BulkInsertOrUpdateAsync(List<TEntity> entityList);
    }

    public abstract class BaseDAL<TEntity, TContext> : IBaseDAL<TEntity, TContext>
          where TEntity : BaseEntity
          where TContext : DbContext
    {
        protected TContext DbContext { get; private set; }

        protected BaseDAL(TContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<TEntity> FindAsync(int id)
        {
            return await DbContext
                .FindAsync<TEntity>(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbContext.Set<TEntity>()
                .Where(p => p.Id == id)
                .AsNoTracking()
                .FirstAsync();
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            return await DbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<TEntity> entry = DbContext.Entry(entity);
            entry.State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> BatchInsertAsync(List<TEntity> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            if (entityList.Count == 0)
                return entityList;

            await DbContext.BulkInsertAsync(entityList);
            return entityList;
        }

        public async Task<List<TEntity>> BatchUpdateAsync(List<TEntity> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            if (entityList.Count == 0)
                return entityList;

            await DbContext.BulkUpdateAsync(entityList);
            return entityList;
        }

        public async Task<List<TEntity>> BulkInsertOrUpdateAsync(List<TEntity> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            if (entityList.Count == 0)
                return entityList;

            await DbContext.BulkInsertOrUpdateAsync(entityList);
            return entityList;
        }
    }
}
