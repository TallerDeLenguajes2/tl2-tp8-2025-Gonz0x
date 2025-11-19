using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.Services;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
//     SESIÓN
// -------------------------
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// -------------------------
//     INYECCIÓN DE DEPENDENCIAS (DI)
// -------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Repositorios CON cadena de conexión
builder.Services.AddScoped<IProductoRepository>(sp =>
    new ProductosRepository(connectionString));

builder.Services.AddScoped<IPresupuestoRepository>(sp =>
    new PresupuestosRepository(connectionString));

builder.Services.AddScoped<IUserRepository>(sp =>
    new UsuarioRepository(connectionString));

// Servicios
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// MVC
builder.Services.AddControllersWithViews();

// -------------------------
//     PIPELINE
// -------------------------
var app = builder.Build();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Run();
