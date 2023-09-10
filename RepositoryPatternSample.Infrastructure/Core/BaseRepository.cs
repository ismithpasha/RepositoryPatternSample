
using RepositoryPatternSample.Entities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RepositoryPatternSample.Infrastructure.Core
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _entitiySet;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entitiySet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T? entity, CancellationToken cancellationToken = default)
        {
            await _entitiySet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return _entitiySet.AsNoTracking().AnyAsync(filter);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return await _entitiySet.AsNoTracking().CountAsync(filter);
        }

        public async Task<long> CountAsync(List<Expression<Func<T, bool>>> filters)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            return await query.AsNoTracking().CountAsync();
        }



        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip < 0)
            {
                return await query.ToListAsync();
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(List<Expression<Func<T, bool>>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }

            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip < 0)
            {
                return await query.ToListAsync();
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.Where(expression).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _entitiySet.ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().Where(expression);
            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.FirstOrDefaultAsync(expression);
        }

        public async Task<T> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.FindAsync(id);
        }

        public async Task<T> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.FindAsync(id);
        }

        public async Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.FindAsync(id);
        }

        public async Task<T> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip < 0)
            {
                return await query.ToListAsync();
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }


        public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }

        public async Task Remove(long id, CancellationToken cancellationToken = default)
        {
            var entity = await _entitiySet.FindAsync(id, cancellationToken);

            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }

        public async Task Remove(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _entitiySet.FindAsync(id, cancellationToken);

            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }

        public async Task Remove(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _entitiySet.FindAsync(id, cancellationToken);

            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }

        public async Task Remove(string id, CancellationToken cancellationToken = default)
        {
            var entity = await _entitiySet.FindAsync(id, cancellationToken);

            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }

        public async Task Remove(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = _entitiySet.Where(predicate).ToList();
            foreach (var entity in entities)
            {
                await Task.Run(() =>
                {
                    _dbContext.Remove(entity);
                });
            }
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _entitiySet.RemoveRange(entities);
            });
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            //_dbContext.Entry(entity).State = EntityState.Modified;
            //var createdAt = entity.GetType().GetProperty("CreatedAt");
            //if (createdAt != null)
            //{
            //    _dbContext.Entry(entity).Property(createdAt.Name).IsModified = false;
            //}
            //var createdBy = entity.GetType().GetProperty("CreatedBy");
            //if (createdBy != null)
            //{
            //    _dbContext.Entry(entity).Property(createdBy.Name).IsModified = false;
            //}

            await Task.Run(() =>
            {
                _entitiySet.Update(entity);
                //_dbContext.Update(entity);
                //_dbContext.SaveChanges();
            });
        }


        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _entitiySet.UpdateRange(entities);
            });
        }

        public async Task ExecuteStoredProcedure(string procedureName, object[] parammValues)
        {
            StringBuilder sbProcedure = new StringBuilder();
            sbProcedure.Append("CALL " + procedureName);

            if (parammValues.Length > 0)
            {
                sbProcedure.Append("(");
            }
            for (int i = 0; i < parammValues.Length; i++)
            {
                sbProcedure.Append("@p" + i);
                if (i != parammValues.Length - 1)
                {
                    sbProcedure.Append(",");
                }
                else
                {
                    sbProcedure.Append(")");
                }
            }
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync(sbProcedure.ToString(), parameters: parammValues);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }



        public void Attach(T temp, T entity)
        {
            _dbContext.Entry(temp).CurrentValues.SetValues(entity);
        }

        public async Task<IEnumerable<T>> GetAllPagingAsync(int skip, int take)
        {
            IQueryable<T> query = _dbContext.Set<T>();


            query = query.Skip(skip);

            query = query.Take(take);


            return await query.ToListAsync();
        }
    }
}
