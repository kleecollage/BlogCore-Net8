using System.Security.Claims;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers;

[Authorize(Roles = "Administrador")]
[Area("Admin")]
public class UsuariosController : Controller
{
    private readonly IContenedorTrabajo _contenedorTrabajo;

    public UsuariosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
    {
        _contenedorTrabajo = contenedorTrabajo;
    }
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        // Opcion 1: Obtener todos los usuarios
        // return View(_contenedorTrabajo.Usuario.GetAll());
        // Opcion 2: Obtener todos los usuarios menos el que esta loggeado
        var claimsIdentity = (ClaimsIdentity)this.User.Identity;
        var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));
    }
    
    [HttpGet]
    public IActionResult Bloquear(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        _contenedorTrabajo.Usuario.BloquearUsuario(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Desbloquear(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
        return RedirectToAction(nameof(Index));
    }
}