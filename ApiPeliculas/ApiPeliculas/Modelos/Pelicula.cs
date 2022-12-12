using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public String RutaImagen { get; set; }
        public String Descripcion { get; set; }
        public int Duracion { get; set;}
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }    //ESTO VA A OCUPAR EL TIPO DE CLASIFICACION ANTERIOR OSEA, PUEDE SER SIETE, TRECE, DIECISEIS, DIECIOCHO, de la funcion anterior-
        public DateTime FechaCreacion { get; set; }
        [ForeignKey("categoriaId")]
        public int categoriaId { get; set; }
        //Tenemos que instanciar el modelo categoria igual.
        public Categoria Categoria { get; set; }


    }
}
