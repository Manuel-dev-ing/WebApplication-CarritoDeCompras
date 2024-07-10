using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositoriLibros, RepositorioLibros>();
builder.Services.AddTransient<IRepositorioAutores, RepositorioAutores>();
builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddTransient<IRepositorioEditorial, RepositorioEditorial>();
builder.Services.AddTransient<IRepositorioCarrito, RepositorioCarrito>();
builder.Services.AddTransient<IRepositorioVentas, RepositorioVentas>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();

builder.Services.AddTransient<IUserStore<Usuarios>, UsuarioStore>();
builder.Services.AddTransient<IRoleStore<Rol>, RoleStore>();

builder.Services.AddTransient<IRepositorioRoles, RepositorioRoles>();


builder.Services.AddTransient<SignInManager<Usuarios>>();

builder.Services.AddIdentityCore<Usuarios>(opciones =>
{
    opciones.Password.RequireDigit = false;
    opciones.Password.RequireLowercase = false;
    opciones.Password.RequireUppercase = false;
    opciones.Password.RequireNonAlphanumeric = false;
    opciones.Password.RequiredLength = 4;
})
.AddRoles<Rol>()
.AddSignInManager<SignInManager<Usuarios>>()
.AddUserStore<UsuarioStore>()
.AddRoleStore<RoleStore>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    //var admin = ClaimTypes.Role == "Administrador" ? "Administrador" : "Administrador";
    //var user = ClaimTypes.Role == "Usuario" ? "Usuario" : "Usuario";


    options.AddPolicy("CustomAdministrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("CustomUsuario", policy => policy.RequireRole("Usuario"));


});

builder.Services.AddAuthentication(opciones =>
{
    opciones.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    opciones.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
    opciones.DefaultChallengeScheme= IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>//Configurando Claims en la Cookie de Autenticacion
{
    options.LoginPath = "/Usuario/Login";
    options.AccessDeniedPath = "/Usuario/Login";

    options.Events.OnSigningIn = async context =>
    {
        var usePrincipal = context.Principal;
        if (usePrincipal != null)
        {
            var userId = usePrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var userManger = context.HttpContext.RequestServices.GetRequiredService<UserManager<Usuarios>>();
                var user = await userManger.FindByIdAsync(userId);
                if (user != null)
                {
                    var claimIdentity = (ClaimsIdentity)usePrincipal.Identity;
                    claimIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.nombre));
                    claimIdentity.AddClaim(new Claim(ClaimTypes.Surname, user.apellidos));
                }
            }

        }
    };

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
    pattern: "{controller=Carrito}/{action=Index}/{id?}");

app.Run();
