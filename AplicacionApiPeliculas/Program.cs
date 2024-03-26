
using AplicacionApiPeliculas;
using AplicacionApiPeliculas.ApiBehavior;
using AplicacionApiPeliculas.Filtros;
using AplicacionApiPeliculas.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


// Add services to the container.

//builder.Services.AddSingleton<IRepositorio, RepositorioEnMemoria>();
builder.Services.AddAutoMapper(typeof(Program));
//almacenar en Azure
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorAzureStorage>();
//almacenar en local
//builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddResponseCaching();

builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add(typeof(FiltroDeExcepcion));
        options.Filters.Add(typeof(ParsearBadRequests));
    }
    ).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    var frontend = builder.Configuration.GetValue<string>("frontend_URL");
    options.AddDefaultPolicy(builder =>
    { builder.WithOrigins(frontend).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
