using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models;

public class Autor 
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El {0} es requerido")]
    [StringLength(maximumLength: 255, ErrorMessage = "El {0} debe tener una longitud de {1} caracteres")]
    [PrimeraLetraMayuscula]
    public string Nombre { get; set; }
    
    public List<AutorLibro> AutoresLibros { get; set; }

}