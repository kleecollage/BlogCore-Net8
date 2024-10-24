using System.Linq.Expressions;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.AccesoDatos.Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext Context;
    internal DbSet<T> dbSet;

    public Repository(DbContext context)
    {
        Context = context;
        this.dbSet = context.Set<T>();
    }
    
    public T Get(int id)
    {
        return dbSet.Find(id);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
    {
        // Se crea una consulta IQueryable a partir del DbSet del contexto
        IQueryable<T> query = dbSet;
        // Se aplica el filtro si es que se proporciona
        if (filter != null)
        {
            query = query.Where(filter);
        }
        // Se incluyen propiedades de navegacion si es que se proporcionan
        if (includeProperties != null)
        {
            // Se divide la cadena de propiedades por coma y se itera sobre ellas
            foreach (var incudeProperty in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(incudeProperty);
            }
        }
        // Se aplica el ordenmiento s se proporciona
        if (orderBy != null)
        {
            // Se ejecuta la funcion de ordenamiento y se convierte la consulta en una lista
            return orderBy(query).ToList();
        }
        // Si no se proporciona nada de lo anterior la consulta se convierte en una lista
        return query.ToList();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        // Se crea una consulta IQueryable a partir del DbSet del contexto
        IQueryable<T> query = dbSet;
        // Se aplica el filtro si es que se proporciona
        if (filter != null)
        {
            query = query.Where(filter);
        }
        // Se incluyen propiedades de navegacion si es que se proporcionan
        if (includeProperties != null)
        {
            // Se divide la cadena de propiedades por coma y se itera sobre ellas
            foreach (var incudeProperty in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(incudeProperty);
            }
        }
        
        return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Remove(int id)
    {
        T entityToRemove = dbSet.Find(id);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }
}