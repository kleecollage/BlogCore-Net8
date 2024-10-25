using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogCore.Models;

public class Articulo
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre del Articulo")]
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "La descripcion es obligatoria")]
    public string Descripcion { get; set; }
    
    [Display(Name = "Fecha de Creacion")]
    public string FechaCreacion { get; set; }

    [DataType(DataType.ImageUrl)]
    [Display(Name = "Imagen")]
    public string UrlImagen { get; set; }

    [Required(ErrorMessage = "La categoria es obligatoria")]
    public int CategoriaId { get; set; }
    
    [ForeignKey("CategoriaId")]
    public Categoria Categoria { get; set; }
}