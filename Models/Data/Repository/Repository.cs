using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models.Util;
using Project_Equinox.Models.Infrastructure;

namespace Project_Equinox.Models.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IEnumerable<T> ListAll()
        {
            return _dbSet.ToList();
        }
        protected readonly EquinoxContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(EquinoxContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual T? Get(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T? Get(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);
            return query.FirstOrDefault();
        }

        public virtual IEnumerable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);
            return query.ToList();
        }

        public virtual GridData<T> List(QueryOptions<T> options, bool returnGridData)
        {
            if (!returnGridData)
            {
                var resultItems = List(options);
                return new GridData<T> { Items = resultItems };
            }

            IQueryable<T> query = _dbSet;

            // Apply includes
            if (!string.IsNullOrEmpty(options.Includes))
            {
                foreach (string include in options.Includes.Split(','))
                {
                    query = query.Include(include.Trim());
                }
            }

            // Apply where clause
            if (options.HasWhere)
            {
                query = query.Where(options.Where!);
            }

            // Get total count before pagination
            int totalItems = query.Count();

            // Apply ordering
            if (options.HasOrderBy)
            {
                if (options.OrderByDirection)
                    query = query.OrderBy(options.OrderBy!);
                else
                    query = query.OrderByDescending(options.OrderBy!);

                if (options.HasThenOrderBy)
                {
                    var orderedQuery = query as IOrderedQueryable<T>;
                    if (options.OrderByDirection)
                        query = orderedQuery!.ThenBy(options.ThenOrderBy!);
                    else
                        query = orderedQuery!.ThenByDescending(options.ThenOrderBy!);
                }
            }

            // Apply pagination
            if (options.HasPaging)
            {
                query = query.Skip((options.PageNumber - 1) * options.PageSize)
                           .Take(options.PageSize);
            }

            var items = query.ToList();
            return new GridData<T>(items, totalItems, options.PageNumber, options.PageSize);
        }

        public virtual int Count()
        {
            return _dbSet.Count();
        }

        public virtual int Count(QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            
            if (options.HasWhere)
            {
                query = query.Where(options.Where!);
            }
            
            return query.Count();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual bool Exists(int id)
        {
            var entity = Get(id);
            return entity != null;
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        protected virtual IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;

            // Apply includes
            if (!string.IsNullOrEmpty(options.Includes))
            {
                foreach (string include in options.Includes.Split(','))
                {
                    query = query.Include(include.Trim());
                }
            }

            // Apply where clause
            if (options.HasWhere)
            {
                query = query.Where(options.Where!);
            }

            // Apply ordering
            if (options.HasOrderBy)
            {
                if (options.OrderByDirection)
                    query = query.OrderBy(options.OrderBy!);
                else
                    query = query.OrderByDescending(options.OrderBy!);

                if (options.HasThenOrderBy)
                {
                    var orderedQuery = query as IOrderedQueryable<T>;
                    if (options.OrderByDirection)
                        query = orderedQuery!.ThenBy(options.ThenOrderBy!);
                    else
                        query = orderedQuery!.ThenByDescending(options.ThenOrderBy!);
                }
            }

            // Apply pagination
            if (options.HasPaging)
            {
                query = query.Skip((options.PageNumber - 1) * options.PageSize)
                           .Take(options.PageSize);
            }

            return query;
        }
    }
}
