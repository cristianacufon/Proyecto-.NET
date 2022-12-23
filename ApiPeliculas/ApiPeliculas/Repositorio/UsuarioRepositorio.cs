using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.InterfaceRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    //Este va a implementar a la clase IUsuarioRepositorio
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        //Implementamos esto para dar acceso a la BASE De DATOS
        private readonly ApplicationDbContext _bd;                       //Instancio una variable privada de ApplicationDbContext y lo instancio en una variable que se llamará _db;
        private string claveSecreta; //Para instanciar appsetings, de donde viene la palabra secreta para los token
        private readonly UserManager<AppUsuario> _userManager;   //UserManager normalmente trabajaria con el IdentityUser, pero en nuestro caso es el AppUsuario, POR QUE AppUsuario???? -- POR QUE EL MODELO ESTA IMPLEMENTANDO IdentityUser  
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config, UserManager<AppUsuario> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper)             //Crear el constructor.   E IConfiguration config es para acceder al appSetings y acceder a la palabra secreta.
        {
            _bd = bd;                                                     //le damos acceso a la DB.A través de está variable
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuario.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.AppUsuario.OrderBy(u => u.UserName).ToList();

        }

        public bool IsUniqueUser(string usuario)
        {
            var usuariobd = _bd.AppUsuario.FirstOrDefault(u =>u.UserName == usuario);
            if (usuariobd == null)
            {
                return true;
            }
            
            return false;
        }

        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            //Mandamos la password a encriptarla.
            //var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);
            //Instanciamos el usuario, para registrar el usuarioRegistroDto que esta llegando como parametro con toda su info.
            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario,
                Nombre = usuarioRegistroDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);    //El usuarioRegistroDto le manda el password. Por la variable usuario vienen todos los demas campos que estan arriba, estos se guardan en esa variable.
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())  //Si no existe el rol admin... entonces lo creamos.   **Este metodo se utilizara solo una vez, que sería la primera vez cuando no existe nada en la BD.
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));     //Creamos el Rol admin
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));   //Creamos el Rol registrado.
                }

                await _userManager.AddToRoleAsync(usuario, "registrado");
                //await _userManager.AddToRoleAsync(usuario, "admin"); ESTE CODIGO SE EJECUTA SOLO UNA VEZ, AL PRINCIPIO PARA CREAR EL ADMIN. //Agregar el usuario al Rol.El usuario, se va a agregar el Rol admin.
                var usuarioRetornado = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);  //Para que se muestre el usuario registrado en la API, llamamos a la BD.
                //Opcion 1 
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre
                //};
                
                //Opcion 2
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);

            }
            //Accedemos a la BD, al modelo Usuario y guardamos el usuario(en azul.).
            //_bd.Usuario.Add(usuario);
            //Guardamos los datos de forma asincrono.
            //await _bd.SaveChangesAsync();
            //retornamos el passwordEncriptado en la respuesta del servidor para que nos muestre allí... para que no se exponga la contraseña.
            //usuario.Password = passwordEncriptado;
            //return usuario;

            return new UsuarioDatosDto();
        }

        public async Task<UsuarioLoginRespuestaDto>Login(UsuarioLoginDto usuarioLoginDto)
        {
            //Encriptamos tambien la contraseña.   ***Con Identity ya no necesita encriptación.
            //var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            //Accedemos al modelo usuario por la BD... con un condicional donde el u.nombreUsuario sea igual a usuarioLoginDto.NombreUsuario y donde el password sea igual a password encriptado.
            var usuario = _bd.AppUsuario.FirstOrDefault(
                u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                //&& u.Password == passwordEncriptado
                ) ;

            bool isValida = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);   //Aqui se puede valir si el password es correcto. Con Identity
            //Validamos si el usuario no existe con la combinacion de usuario y contraseña correcta.z
            if (usuario == null || isValida == false)
            {
                //En esta caso como no hubo un usuario correcto en la autenticacion, el token que retorna es vacio y el usuario es igual a null.
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //AQUI SI EXISTE EL USUARIO. existe el usuario entonces podemos procesar el login
            var roles = await _userManager.GetRolesAsync(usuario);     //Con esto estamos obteniendo los roles del usuario.

            var manejadorToken = new JwtSecurityTokenHandler();
            //Palabra clave para validar los token 
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //autenticacion basada en politicas. CLAIMS
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                //cuando se haga la autenticación durará 7 dias.
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)
            };

            return usuarioLoginRespuestaDto;


        }


        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro.
        //public static string obtenermd5(string valor)
        //{
        //    MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //    data = x.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //        resp += data[i].ToString("x2").ToLower();
        //    return resp;
        //}
    }
}
