using System.Linq.Expressions;

namespace ReviewApp.Repository.IRepository
{
    public interface IRepository<T> where T : class 
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        public bool ObjectExist(Expression<Func<T, bool>> filter);
        void Create(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);


    }
}
