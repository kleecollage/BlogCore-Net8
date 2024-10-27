namespace BlogCore.Models.ViewModels;

public class HomeVM
{
    public IEnumerable<Slider> Sliders { get; set; }
    public IEnumerable<Articulo> ListaArticulos { get; set; }
    // Paginacion
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
}