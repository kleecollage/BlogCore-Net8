using System.Globalization;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers;

[Area("Admin")]
public class ArticulosController: Controller
{
    private readonly IContenedorTrabajo _contenedorTrabajo;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
    {
        _contenedorTrabajo = contenedorTrabajo;
        _webHostEnvironment = webHostEnvironment;
    }
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        /* Debemos crear un view model para traer informacion de dos tablas */
        ArticuloVM artiVM = new ArticuloVM()
        {
            Articulo = new Articulo(),
            ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
        };
        
        return View(artiVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ArticuloVM artiVM)
    {
        Console.WriteLine("Entrando a la acción Create");
        if (ModelState.IsValid)
        {
            Console.WriteLine("El ModelState es válido");
            string rutaPrincipal = _webHostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            Console.WriteLine("Cantidad de archivos recibidos: " + archivos.Count);
            if (artiVM.Articulo.Id == 0 && archivos.Count > 0)
            {
                Console.WriteLine("Subiendo un nuevo artículo");
                // Nuevo articulo
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, "imagenes", "articulos");
                var extension = Path.GetExtension(archivos[0].FileName);

                Console.WriteLine("Ruta de subida: " + subidas);
                Console.WriteLine("Nombre del archivo: " + nombreArchivo + extension);

                using (var fileStreams =
                       new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                artiVM.Articulo.UrlImagen = Path.Combine("imagenes", "articulos", nombreArchivo + extension).Replace(Path.DirectorySeparatorChar, '/');
                artiVM.Articulo.FechaCreacion = DateTime.Now.ToString(CultureInfo.CurrentCulture);

                _contenedorTrabajo.Articulo.Add(artiVM.Articulo);
                _contenedorTrabajo.Save();

                Console.WriteLine("Artículo guardado exitosamente");
                return RedirectToAction(nameof(Index)); 
            } 
            else
            {
                Console.WriteLine("No se seleccionó una imagen");
                /* Error personalizado */
                ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
            }
        }
        Console.WriteLine("El ModelState no es válido");
        artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
        return View(artiVM);
    }

    public IActionResult Edit(int? id)
    {
        ArticuloVM artiVM = new ArticuloVM()
        {
            Articulo = new Articulo(),
            ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
        };
        if (id != null)
        {
            artiVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
        }
        
        return View(artiVM); 
    }
    
    
    #region Llamadas a la API
    [HttpGet]
    public IActionResult GetAll()
    {
        return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
    }

    #endregion
}