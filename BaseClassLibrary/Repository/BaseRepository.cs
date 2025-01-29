using BaseClassLibrary.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BaseClassLibrary.Repository
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private IDbContextTransaction _transaction;

        public BaseRepository(TContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            // Apply includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)  
            {
                query = query.Where(predicate).AsQueryable();
            }

            return await query.ToListAsync();
        }
        
        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            // Ensure the entity is reloaded to get any database generated values
            _context.Entry(entity).State = EntityState.Detached; // Detach first to refresh
            _context.Entry(entity).State = EntityState.Unchanged; // Reattach as unchanged to prevent re-adding

            var primaryKey = _context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties[0];
            var primaryKeyValue = entity.GetType()?.GetProperty(primaryKey?.Name)?.GetValue(entity);

            return await _context.Set<TEntity>().FindAsync(primaryKeyValue);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TEntity>> ExecuteStoredProcedureAsync(string storedProcedureName, params object[] parameters)
        {
            var parameterNames = Enumerable.Range(0, parameters.Length / 2).Select(i => (string)parameters[i * 2]).ToArray();
            var parameterValues = Enumerable.Range(0, parameters.Length / 2).Select(i => parameters[i * 2 + 1]).ToArray();

            var paramList = new List<SqlParameter>();
            for (int i = 0; i < parameterNames.Length; i++)
            {
                paramList.Add(new SqlParameter(parameterNames[i], parameterValues[i]));
            }

            var query = $"EXEC {storedProcedureName} {string.Join(", ", parameterNames)}";

            return await _context.Set<TEntity>().FromSqlRaw(query, paramList.ToArray()).ToListAsync();
        }

        public async Task<DataSet> ExecuteStoredProcedureDSAsync(string storedProcedureName, params object[] parameters)
        {
            // Extract parameter names and values from the input array
            string[] parameterNames = (from i in Enumerable.Range(0, parameters.Length / 2)
                                       select (string)parameters[i * 2]).ToArray();
            object[] parameterValues = (from i in Enumerable.Range(0, parameters.Length / 2)
                                        select parameters[i * 2 + 1]).ToArray();
        
            // Create a DataSet to hold the result
            DataSet resultDataSet = new DataSet();
        
            // Create a new SQL connection using the connection string from your DbContext
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    // Specify that this command is for a stored procedure
                    command.CommandType = CommandType.StoredProcedure;
        
                    // Add the parameters to the command
                    for (int i = 0; i < parameterNames.Length; i++)
                    {
                        command.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                    }
        
                    // Use a SqlDataAdapter to execute the command and fill the DataSet
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        await connection.OpenAsync();
                        adapter.Fill(resultDataSet);
                    }
                }
            }
        
            return resultDataSet;
        }

        public async Task<List<TEntity>> QueryAsync(            
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, object>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (predicate != null)
            { 
                query = query.Where(predicate).AsQueryable();
            }
            
            query = orderBy != null ? query.OrderBy(orderBy) : query;            

            return await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                _transaction.Dispose();
            }
        }
    }
}
