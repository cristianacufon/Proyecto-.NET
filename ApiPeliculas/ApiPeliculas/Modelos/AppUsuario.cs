using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Modelos
{
    public class AppUsuario : IdentityUser //DEBE HEREDAR DE IdentityUser
    {
        //Añadir campos personalizados.
        public string Nombre { get; set; }   //Se va a añadir el Nombre por que Identity, no tiene el campo Nombre, tiene password, role, username, pero Nombre no lo tiene. Todos estos datos se van a crear en la BD como ASPNetUser
                 
    }
}
