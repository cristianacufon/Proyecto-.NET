using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dtos
{
    public class PeliculaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        //public String RutaImagen { get; set; }
        public byte[] RutaImagen { get; set; }
        [Required(ErrorMessage = "La Descripcion es obligatoria")]
        public String Descripcion { get; set; }
        [Required(ErrorMessage = "La Duracion es obligatoria")]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }    //ESTO VA A OCUPAR EL TIPO DE CLASIFICACION ANTERIOR OSEA, PUEDE SER SIETE, TRECE, DIECISEIS, DIECIOCHO, de la funcion anterior-
        public DateTime FechaCreacion { get; set; }
        
        public int categoriaId { get; set; }
        

    }
}
