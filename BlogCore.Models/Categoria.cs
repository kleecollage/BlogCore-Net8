using System.ComponentModel.DataAnnotations;

namespace BlogCore.Models;

public class Categoria
{
    [Key] /* Tener la clave Id permite obviar la anotacion 'Key' */
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ingrese un nombre para la categoria")]
    [Display(Name = "Nombre de Categoria")]
    public string Nombre { get; set; }
    
    [Display(Name = "Orden de Visualizacion")]
    [Range(1, 100, ErrorMessage = "El valor debe estar entre 1 y 100")]
    public int? Orden { get; set; }
}