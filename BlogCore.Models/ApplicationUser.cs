using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlogCore.Models;

public class ApplicationUser: IdentityUser
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "La direcccion es obligatoria")]
    public string Direccion { get; set; }
    
    [Required(ErrorMessage = "La ciudad es obligatoria")]
    public string Ciudad { get; set; }
    
    [Required(ErrorMessage = "El pais es obligatorio")]
    public string Pais { get; set; }
}