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
        if (ModelState.IsValid)
        {
            string rutaPrincipal = _webHostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            if (artiVM.Articulo.Id == 0 && archivos.Count > 0)
            {
                // Nuevo articulo
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, "imagenes", "articulos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams =
                       new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                artiVM.Articulo.UrlImagen = "/imagenes/articulos/" + nombreArchivo + extension;
                artiVM.Articulo.FechaCreacion = DateTime.Now.ToString(CultureInfo.CurrentCulture);

                _contenedorTrabajo.Articulo.Add(artiVM.Articulo);
                _contenedorTrabajo.Save();
                
                return RedirectToAction(nameof(Index)); 
            } 
            else
            {
                /* Error personalizado */
                ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
            }
        }
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ArticuloVM artiVM)
    {
        if (ModelState.IsValid)
        {
            string rutaPrincipal = _webHostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);
            if (archivos.Count > 0)
            {
                // Nueva imagen
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, "imagenes", "articulos");
                var extension = Path.GetExtension(archivos[0].FileName);
                var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeBd.UrlImagen.TrimStart('/'));

                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen); // Eliminamos el archivo anterior
                }
                // Volvemos a subir el archivo
                using (var fileStreams =
                       new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                artiVM.Articulo.UrlImagen = "/imagenes/articulos/" + nombreArchivo + extension;
                artiVM.Articulo.FechaCreacion = DateTime.Now.ToString(CultureInfo.CurrentCulture);

                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();
                
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                // Se conserva la imagen
                artiVM.Articulo.UrlImagen = articuloDesdeBd.UrlImagen;
            }
            _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
            _contenedorTrabajo.Save();
                
            return RedirectToAction(nameof(Index)); 
        }
        artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
        return View(artiVM);
    }
    
    
    #region Llamadas a la API
    [HttpGet]
    public IActionResult GetAll()
    {
        return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(id);
        string rutaDirectorioPrincipal = _webHostEnvironment.WebRootPath;
        var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeBd.UrlImagen.TrimStart('/'));
        
        if (System.IO.File.Exists(rutaImagen))
        {
            System.IO.File.Delete(rutaImagen); // Eliminamos la imagen
        }
        
        if (articuloDesdeBd == null )
        {
            return Json(new {success = false, message = "Error al borrar el articulo" });
        }
        
        _contenedorTrabajo.Articulo.Remove(articuloDesdeBd);
        _contenedorTrabajo.Save();
        
        return Json(new {success = true, message = "Articulo eliminado correctamente" });
    }
    #endregion
}