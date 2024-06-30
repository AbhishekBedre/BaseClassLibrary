using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassLibrary.Interface
{
    public interface IBaseRepository<TEntity, TContext> 
        where TEntity : class
        where TContext: DbContext
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetByIdAsync(long id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(long id);
        Task<List<TEntity>> ExecuteStoredProcedureAsync(string storedProcedureName, params object[] parameters);
        Task<List<TEntity>> QueryAsync(                    
                    Expression<Func<TEntity, bool>> predicate = null,
                    Expression<Func<TEntity, object>> orderBy = null,
                    int pageIndex = 0,
                    int pageSize = 10);

        // Transaction methods
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}
