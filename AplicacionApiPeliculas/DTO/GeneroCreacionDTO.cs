using AplicacionApiPeliculas.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace AplicacionApiPeliculas.DTO
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requierido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
