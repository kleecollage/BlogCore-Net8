using System.ComponentModel.DataAnnotations;

namespace BlogCore.Models;

public class Slider
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ingrese un nombre para el slider")]
    [Display(Name = "Nombre del Slider")]
    public string Nombre { get; set; }
    
    [Required]
    public bool Estado { get; set; }
    
    [DataType(DataType.ImageUrl)]
    [Display(Name = "Imagen")]
    public string UrlImagen { get; set; }
    
    
    
    
}