using System.Net;

namespace ApiPeliculas.Modelos
{
    public class RespuestaAPI
    {
        //Aqui vamos a capturar los errores del servidor.
        public RespuestaAPI()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }  //Aqui vamos a capturar el codigo del Status.
        public bool IsSuccess { get; set; } = true; //Para saber si fue correcta, verdadera o falsa la peticion.
        public List<string> ErrorMessages { get; set;}  //Una lista con los posibles errores de respuesta del servidor.
        public object Result { get; set; }
    }
}
