using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestChat.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Add
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entityList);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entityList);
        #endregion Add


        #region Update
        TEntity Update(TEntity entity);
        //Task<TEntity> UpdateAsync(TEntity entity); // not allowed
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entityList);
        //Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entityList);
        #endregion Update


        #region Remove
        TEntity Remove(TEntity entity);
        //Task<TEntity> RemoveAsync(TEntity entity);
        IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entityList);
        //Task<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entityList);
        #endregion Remove


        #region SingleOrDefault
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion SingleOrDefault


        #region GetAll
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        #endregion GetAll


        #region Find
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        #endregion Find
    }
}
