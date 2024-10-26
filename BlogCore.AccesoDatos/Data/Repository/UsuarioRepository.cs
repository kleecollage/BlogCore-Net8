using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.AccesoDatos.Data.Repository;

public class UsuarioRepository: Repository<ApplicationUser>, IUsuarioRepository
{
    private readonly ApplicationDbContext _context;
    
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void BloquearUsuario(string IdUsuario)
    {
        var usuarioDesdeBd = _context.ApplicationUser.FirstOrDefault(u => u.Id == IdUsuario);
        usuarioDesdeBd.LockoutEnd = DateTime.Now.AddYears(1000);
        _context.SaveChanges();
    }

    public void DesbloquearUsuario(string IdUsuario)
    {
        var usuarioDesdeBd = _context.ApplicationUser.FirstOrDefault(u => u.Id == IdUsuario);
        usuarioDesdeBd.LockoutEnd = DateTime.Now;
        _context.SaveChanges();
    }
}
