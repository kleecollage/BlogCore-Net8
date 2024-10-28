using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Data.Inicializador;

public class InicializadorBd: IInicializadorBd
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public void Inicializar(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        context = _context;
        userManager = _userManager;
        roleManager = _roleManager;    
    }

    public void Inicializar()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate(); 
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (_context.Roles.Any(ro => ro.Name == CNT.Administrador)) return;
        
        
        /* CREACION DE ROLES */
        _roleManager.CreateAsync(new IdentityRole(CNT.Administrador)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(CNT.Registrado)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(CNT.Cliente)).GetAwaiter().GetResult();
        
        /* CREACION DEL USUARIO INICIAL */
        _userManager.CreateAsync(new ApplicationUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            Nombre = "admin"
        }, "Password123$").GetAwaiter().GetResult();
        ApplicationUser usuario = _context.ApplicationUser.Where(us => us.Email == "admin@admin.com").FirstOrDefault();
        _userManager.AddToRoleAsync(usuario, CNT.Administrador).GetAwaiter().GetResult();
    }
}