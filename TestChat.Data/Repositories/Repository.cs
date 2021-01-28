using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestChat.Core.Repositories;
using TestChat.Data.Contexts;

namespace TestChat.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        #region Declaration & Construstion & Dispose
        private DbSet<TEntity> _entities;
        private bool _isDisposed;
        public ChatDbContext Context { get; set; }

        public Repository(ChatDbContext context)
        {
            _isDisposed = false;
            Context = context;
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }

        public virtual DbSet<TEntity> Entities
        {
            get { return _entities ?? (_entities = Context.Set<TEntity>()); }
        }
        #endregion Declaration & Construstion & Dispose

        #region Add
        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity is empty!");
            }
            Entities.Add(entity);
            return entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity is empty!");
            }
            var returnResult = await Entities.AddAsync(entity);
            return entity;
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
            {
                throw new ArgumentNullException("Entity is empty!");
            }
            Entities.AddRange(entityList);
            return entityList;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
            {
                throw new ArgumentNullException("Entity is empty!");
            }
            await Entities.AddRangeAsync(entityList);
            return entityList;
        }
        #endregion Add


        #region Update
        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity is empty!");
            }
            Entities.Update(entity);
            return entity;
        }

        public virtual IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
            {
                throw new ArgumentNullException("Entity is empty!");
            }
            Entities.UpdateRange(entityList);
            return entityList;
        }
        #endregion Update


        #region Remove
        public virtual TEntity Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity is empty!");
            Entities.Remove(entity);
            return entity;
        }

        public virtual IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException("entity list is empty!");
            Entities.RemoveRange(entityList);
            return entityList;
        }
        #endregion Remove

        #region SingleOrDefault
        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).SingleOrDefault();
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.Where(predicate).SingleOrDefaultAsync();
        }
        #endregion SingleOrDefault


        #region GetAll
        public virtual IEnumerable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            return await query.ToListAsync();
        }
        #endregion GetAll

        #region Find
        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate);

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate);
            
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Context.Set<TEntity>().Where(predicate);
            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }
        #endregion Find
    }
}
