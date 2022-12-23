using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
//****EL NOMBRE DE ESTOS REPOSITORIOS SIEMPRE VAN EN PROGRAM.CS IPELICULAREPOSITORIO Y PELICULAREPOSITORIO****

namespace ApiPeliculas.Repositorio.InterfaceRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario>GetUsuarios(); //ICollection de Modelo categoria, obtener categorias... ESTE ES EL METODO QUE NOS TRAERA LAS CATEGORIAS.
        //LOS VERDES SON LOS MODELOS.
        AppUsuario GetUsuario(string usuarioId); //Instanciamos el metodo categoria y va a recibir el Id de la categoria 
        bool IsUniqueUser(string usuario);   //Para saber si existe o no el nombre de usuario. Para que no se registren mas con el mismo nombre.
        
        //La Tarea va a estar realizada en el UsuarioLoginRespuestaDto y lo vamos a utilizar en el Login y va a llamar el modelo UsuarioLoginDto.
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);   //Esto lo vamos a usar en el LOGIN.  

        //Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);


    }
}
