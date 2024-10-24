using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;

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
        
        _context.SaveChanges();
    }
}