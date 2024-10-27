using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

public interface IArticuloRepository: IRepository<Articulo>
{
    void Update(Articulo articulo);
    // Metodo para el buscador
    IQueryable<Articulo> AsQueryable();
}