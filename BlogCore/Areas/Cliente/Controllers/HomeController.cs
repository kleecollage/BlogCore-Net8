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

    [HttpGet]
    public IActionResult Index()
    {
        HomeVM homeVM = new HomeVM()
        {
            Sliders = _contenedorTrabajo.Slider.GetAll(),
            ListaArticulos = _contenedorTrabajo.Articulo.GetAll()
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
}
