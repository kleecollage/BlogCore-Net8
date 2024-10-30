using System.Diagnostics;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using BlogCore.Models;
using BlogCore.Models.ViewModels;

namespace BlogCore.Areas.Cliente.Controllers;
[Area("Cliente")]
public class HomeController : Controller
{
    private readonly IContenedorTrabajo _contenedorTrabajo;
    public HomeController(IContenedorTrabajo contenedorTrabajo)
    {
        _contenedorTrabajo = contenedorTrabajo;
    }

    // Primera version pagina de inicio sin paginacion
    /*[HttpGet]
    public IActionResult Index()
    {
        HomeVM homeVM = new HomeVM()
        {
            Sliders = _contenedorTrabajo.Slider.GetAll(),
            ListaArticulos = _contenedorTrabajo.Articulo.GetAll()
        };
        /* Con esta linea sabemos si nos encontramos o no en Home #1#
        ViewBag.IsHome = true;
        
        return View(homeVM);
    }
    */

    // Segunda version, ppagina de inicio con paginacon
    [HttpGet]
    public IActionResult Index(int page = 1, int pageSize = 9)
    {
        var articulos = _contenedorTrabajo.Articulo.AsQueryable();
        // Paginar los resultados
        var paginatedEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);
        HomeVM homeVM = new HomeVM()
        {
            Sliders = _contenedorTrabajo.Slider.GetAll(),
            ListaArticulos = paginatedEntries.ToList(),
            PageIndex = page,
            TotalPages = (int)Math.Ceiling(articulos.Count() / (double)pageSize)
        };
        /* Con esta linea sabemos si nos encontramos o no en Home */
        ViewBag.IsHome = true;
        
        return View(homeVM);
    }
    
    [HttpGet]
    public IActionResult Detalle(int id)
    {
        var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(id);
        return View(articuloDesdeBd);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    // Para el buscador
    [HttpGet]
    public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSize = 3)
    {
        var articulos = _contenedorTrabajo.Articulo.AsQueryable();
        // Filtrar por titulo si hay un termino de busqueda
        if (!string.IsNullOrEmpty(searchString))
        {
            articulos = articulos.Where(e => e.Nombre.Contains(searchString));
        }
        // Paginar los resultados
        var paginatedEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);
        // Crear el modelo de la vista
        var model = new ListaPaginada<Articulo>(paginatedEntries.ToList(), articulos.Count(), page, pageSize,
            searchString);
        
        return View(model);
    }
}
