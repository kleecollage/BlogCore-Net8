using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.AccesoDatos.Data.Repository;

public class SliderRepository: Repository<Slider>, ISliderRepository
{
    private readonly ApplicationDbContext _context;
    
    public SliderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Slider slider)
    {
        var objDesdeDb = _context.Slider.FirstOrDefault(s => s.Id == slider.Id);
        objDesdeDb.Nombre = slider.Nombre;
        objDesdeDb.Estado = slider.Estado;
        objDesdeDb.UrlImagen = slider.UrlImagen;
    }
}