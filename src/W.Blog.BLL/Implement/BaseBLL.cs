namespace W.Blog.BLL.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using W.Blog.DAL.Implement;
    using W.Blog.Entity.Entitys;

    public interface IBaseBLL<TEntity, TDAL, TContext>
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

    public abstract class BaseBLL { }

    public class BaseBLL<TEntity, TDAL, TContext> : BaseBLL,
        IBaseBLL<TEntity, TDAL, TContext>
          where TEntity : BaseEntity
          where TDAL : IBaseDAL<TEntity, TContext>
          where TContext : DbContext
    {
        protected TDAL Dal { get; set; }

        public BaseBLL(TDAL dal)
        {
            Dal = dal;
        }

        public virtual async Task<TEntity> FindAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));
            return await Dal.FindAsync(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));
            return await Dal.GetByIdAsync(id);
        }

        public virtual async Task<List<TEntity>> GetListAsync()
        {
            return await Dal.GetListAsync();
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await Dal.InsertAsync(entity);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Dal.UpdateAsync(entity);
        }

        public virtual async Task<List<TEntity>> BatchInsertAsync(List<TEntity> entityList)
        {
            return await Dal.BatchInsertAsync(entityList);
        }

        public virtual async Task<List<TEntity>> BatchUpdateAsync(List<TEntity> entityList)
        {
            return await Dal.BatchUpdateAsync(entityList);
        }

        public virtual async Task<List<TEntity>> BulkInsertOrUpdateAsync(List<TEntity> entityList)
        {
            return await Dal.BulkInsertOrUpdateAsync(entityList);
        }
    }
}