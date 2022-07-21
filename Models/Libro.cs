using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models;

public class Libro
{
    
    public int Id { get; set; }
    [Required]
    [PrimeraLetraMayuscula]
    [StringLength(maximumLength: 250)]
    public string Titulo { get; set; }

    public List<Comentario> Comentarios { get; set; }
    
    public List<AutorLibro> AutoresLibros { get; set; }

}