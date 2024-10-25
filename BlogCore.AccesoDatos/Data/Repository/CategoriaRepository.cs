using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.AccesoDatos.Data.Repository;

public class CategoriaRepository: Repository<Categoria>, ICategoriaRepository
{
    private readonly ApplicationDbContext _context;
    
    public CategoriaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Categoria categoria)
    {
        var objDesdeDb = _context.Categoria.FirstOrDefault(s => s.Id == categoria.Id);
        objDesdeDb.Nombre = categoria.Nombre;
        objDesdeDb.Orden = categoria.Orden;
        
        // _context.SaveChanges();
    }

    public IEnumerable<SelectListItem> GetListaCategorias()
    {
        return _context.Categoria.Select(i => new SelectListItem()
        {
            Text = i.Nombre,
            Value = i.Id.ToString()
        });
    }
}