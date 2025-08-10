using Project_Equinox.Models.Util;

namespace Project_Equinox.Models.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T? Get(int id);
        T? Get(QueryOptions<T> options);
        IEnumerable<T> List(QueryOptions<T> options);
        GridData<T> List(QueryOptions<T> options, bool returnGridData);
        int Count();
        int Count(QueryOptions<T> options);
        
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        
        bool Exists(int id);
        void Save();
    }
}
