using System.Net;
using BlogCore.AccesoDatos.Data.Inicializador;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogCore.Data;
using BlogCore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConnectionSql") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
    
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(DataProtectorTokenProvider<ApplicationUser>)));
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

// Agregar contenedor de trabajo al contenedor IoC de inyeccion de dependencias
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

// SIEMBRA DE DATOS (paso 1)
builder.Services.AddScoped<IInicializadorBd, InicializadorBd>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// METODO QUE EJECUUTA LA SIEMBRA DE DATOS
SiembraDatos();

app.UseRouting();

app.UseAuthorization(); // Importante para bloqueo de rutas

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// FUNCIONALIDAD DEL METODO
void SiembraDatos()
{
    using (var scope = app.Services.CreateScope())
    {
        var inicializadorBd = scope.ServiceProvider.GetRequiredService<IInicializadorBd>();
        inicializadorBd.Inicializar();
    }
}
