using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.AccesoDatos.Data.Repository;

public class ArticuloRepository: Repository<Articulo>, IArticuloRepository
{
    private readonly ApplicationDbContext _context;
    
    public ArticuloRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Articulo articulo)
    {
        var objDesdeDb = _context.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
        objDesdeDb.Nombre = articulo.Nombre;
        objDesdeDb.Descripcion = articulo.Descripcion;
        objDesdeDb.UrlImagen = articulo.UrlImagen;
        objDesdeDb.CategoriaId = articulo.CategoriaId;
        
        // _context.SaveChanges();
    }

    // Metodo para el buscador
    public IQueryable<Articulo> AsQueryable()
    {
        return _context.Set<Articulo>().AsQueryable();
    }
}