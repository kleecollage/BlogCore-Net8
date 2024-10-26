using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers;

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
        return View(_contenedorTrabajo.Usuario.GetAll());
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