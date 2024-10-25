using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

public interface IArticuloRepository: IRepository<Articulo>
{
    void Update(Articulo articulo);
}