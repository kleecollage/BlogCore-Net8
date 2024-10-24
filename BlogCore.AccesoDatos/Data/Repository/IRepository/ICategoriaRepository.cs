using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

public interface ICategoriaRepository: IRepository<Categoria>
{
    void Update(Categoria categoria);
}