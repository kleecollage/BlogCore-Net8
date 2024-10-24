using System.Linq.Expressions;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

/* T es un marcador generico para mis entidades */
public interface IRepository<T> where T : class
{
    T Get(int id);
    
    IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null
    );

    T GetFirstOrDefault(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null  
    );
    
    void Add(T entity);
    void Remove(int id);
    void Remove(T entity);
}