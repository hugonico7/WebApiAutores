using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs;

public class AutorCreacionDTO
{
    [Required(ErrorMessage = "El {0} es requerido")]
    [StringLength(maximumLength: 255, ErrorMessage = "El {0} debe tener una longitud de {1} caracteres")]
    [PrimeraLetraMayuscula]
    public string Nombre { get; set; }
}