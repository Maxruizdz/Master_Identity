using Curso_Identity.Datos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options => {
    var cadena_conexion = builder.Configuration.GetConnectionString("ConexionSql");
    options.UseSqlServer(cadena_conexion);


});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.ConfigureApplicationCookie(options => {


    options.LoginPath = new PathString("/Cuentas/Acceso");
    options.AccessDeniedPath = new PathString("/Cuentas/Bloqueado");
});

//Configurando Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;




});
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
