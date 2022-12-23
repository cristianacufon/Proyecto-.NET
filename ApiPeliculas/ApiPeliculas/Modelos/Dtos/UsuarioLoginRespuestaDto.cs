namespace ApiPeliculas.Modelos.Dtos
{
    public class UsuarioLoginRespuestaDto
    {
        //Accedo al modelo Usuario. Para que me devuelva sus datos.
        public UsuarioDatosDto Usuario { get; set; }

    //Y devuelvo el token tambien.
        public string Token { get; set; }
    }
}
