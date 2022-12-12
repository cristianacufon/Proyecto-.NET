using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearCategoriaDto
    {
        //QUITAMOS EL ID POR QUE ES CLAVE PRIMARIA Y AUTOINCREMENTAL. NO ES NECESARIO PARA CREAR UNA NUEVA CATEGORIA.


        //Esta validacion es importante para el nombre, sino se crea vacio el nombre de categoria.
        [Required(ErrorMessage = "El nombre es obligatorio")]                               //El nombre es requerido, y enviamos un mensaje por si no lo ingresaron.
        [MaxLength(60, ErrorMessage = "El número maximo de caracteres es de 60!")]          //limitamos el numero maximo de catacteres y mandamos un mensaje si se pasan de esa cantidad.
        public string Nombre { get; set; }

    }
}
