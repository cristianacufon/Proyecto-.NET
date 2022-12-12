using ApiPeliculas.Data;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.InterfaceRepositorio;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
//using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Configuramos la conexion a sql server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//Agregamos los repositorios.   ICategoriaRepositorio la interfaz del repositorio y CategoriaRepositorio el repositorio como tal.
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

//AGregar el AUTOMAPPER.  y llamamos al mapper de PeliculasMapper.
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
