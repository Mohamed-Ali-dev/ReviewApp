using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Repository.IRepository;
using System.Linq.Expressions;

namespace ReviewApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeprop in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(includeprop);
                }
            }
            return query.ToList();

        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbset;
            }
            else
            {
                query = dbset.AsNoTracking();
            }
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeprop in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.FirstOrDefault();

        }

        public bool ObjectExist(Expression<Func<T, bool>> filter)
        {
            return dbset.Any(filter);
        }

        public void Create(T entity)
        {
            _db.Add(entity);
        }

        public void Delete(T entity)
        {
           _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }
    }
}
