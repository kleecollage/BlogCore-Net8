using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Areas.Admin.Controllers;
[Authorize(Roles = "Administrador")]
[Area("Admin")]
public class CategoriasController : Controller
{
    private readonly IContenedorTrabajo _contenedorTrabajo;
    private readonly ApplicationDbContext _context;

    public CategoriasController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
    {
        _contenedorTrabajo = contenedorTrabajo;
        _context = context;
    }
    
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            // Logica para guardar en BD
            _contenedorTrabajo.Categoria.Add(categoria);
            _contenedorTrabajo.Save();
            return RedirectToAction(nameof(Index));
        }
        
        return View(categoria);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        Categoria categoria = new Categoria();
        categoria = _contenedorTrabajo.Categoria.Get(id);
        if (categoria == null)
        {
            return NotFound();
        }
        
        return View(categoria);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            // Logica para actualizar en BD
            _contenedorTrabajo.Categoria.Update(categoria);
            _contenedorTrabajo.Save();
            return RedirectToAction(nameof(Index));
        }
        
        return View(categoria);
    }
    
    #region Llamadas a la API

    [HttpGet]
    public IActionResult GetAll()
    {
        var categorias = _context.Categoria.FromSqlRaw<Categoria>("spGetCategorias").ToList();
        return Json(new { data = categorias });
        //return Json(new { data = _contenedorTrabajo.Categoria.GetAll() });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var objFromDb = _contenedorTrabajo.Categoria.Get(id);
        if (objFromDb == null )
        {
            return Json(new {success = false, message = "Error al borrar la categoria" });
        }
        _contenedorTrabajo.Categoria.Remove(objFromDb);
        _contenedorTrabajo.Save();
        
        return Json(new {success = true, message = "Categoria borrada correctamente" });
    }
    #endregion
}