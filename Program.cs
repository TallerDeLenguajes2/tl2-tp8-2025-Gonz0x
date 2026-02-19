using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.Repositorios;
using tl2_tp8_2025_Gonz0x.Services;

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
//     MVC
// -------------------------
builder.Services.AddControllersWithViews();

// -------------------------
//     CADENA DE CONEXIÓN
// -------------------------
var CadenaDeConexion = builder.Configuration
    .GetConnectionString("SqliteConexion")!
    .ToString();

// Registrar la cadena como Singleton
builder.Services.AddSingleton<string>(CadenaDeConexion);

// -------------------------
//     INYECCIÓN DE DEPENDENCIAS
// -------------------------
builder.Services.AddScoped<IProductoRepository, ProductosRepository>();
builder.Services.AddScoped<IPresupuestoRepository, PresupuestosRepository>();
builder.Services.AddScoped<IUserRepository, UsuarioRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

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
