using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]                               //El nombre es requerido, y enviamos un mensaje por si no lo ingresaron.
        [MaxLength(60, ErrorMessage = "El número maximo de caracteres es de 60!")]          //limitamos el numero maximo de catacteres y mandamos un mensaje si se pasan de esa cantidad.
        public string Nombre { get; set; }

    }
}
