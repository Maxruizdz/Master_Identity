using Curso_Identity.Datos;
using Curso_Identity.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options => {
    var cadena_conexion = builder.Configuration.GetConnectionString("ConexionSql");
    options.UseSqlServer(cadena_conexion);


});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();



builder.Services.ConfigureApplicationCookie(options => {

     
    options.LoginPath = new PathString("/Cuentas/Acceso");
    options.AccessDeniedPath = new PathString("/Cuentas/Denegado");
});

//Configurando Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;




});

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId =builder.Configuration.GetSection( "Facebook:IdApp").Value;
    options.AppSecret = builder.Configuration.GetSection("Facebook:SecretApp").Value;
  
});
//Soporte para autorización basada en directivas/Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
options.AddPolicy("Registrado", policy => policy.RequireRole("Registrado"));
options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
options.AddPolicy("UsuarioYAdministrador", policy => policy.RequireRole("Administrador").RequireRole("Usuario"));

//Uso de claims
options.AddPolicy("AdministradorCrear", policy => policy.RequireRole("Administrador").RequireClaim("Crear", "True"));
options.AddPolicy("AdministradorEditarBorrar", policy => policy.RequireRole("Administrador").RequireClaim("Editar", "True").RequireClaim("Borrar", "True"));
options.AddPolicy("AdministradorCrearEditarBorrar", policy => policy.RequireRole("Administrador").RequireClaim("Crear", "True")
.RequireClaim("Editar", "True").RequireClaim("Borrar", "True"));
});


builder.Services.AddScoped<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
