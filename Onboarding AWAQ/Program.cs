using Onboarding_AWAQ.Pages; // Add this for the PasswordHasher class
using Microsoft.AspNetCore.Identity; // Add this for the IPasswordHasher<> and ApplicationUser types

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add services to the container.

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Endpoint para obtener el valor de la sesión de usuario
app.MapGet("/GetUserSession", (HttpContext httpContext) =>
{
    // Accede a la sesión de usuario
    var session = httpContext.Session;

    // Obtiene el valor de la sesión de usuario
    var user = session.GetString("usuario");

    // Devuelve el valor de la sesión
    return user;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();