using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository;

public interface ICategoriaRepository: IRepository<Categoria>
{
    void Update(Categoria categoria);
    
    IEnumerable<SelectListItem> GetListaCategorias();
}