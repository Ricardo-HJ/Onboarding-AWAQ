using Microsoft.AspNetCore.Identity; // Add this for the IPasswordHasher<> and ApplicationUser types
using Onboarding_AWAQ.Pages; // Add this for the PasswordHasher class

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add the CORS middleware
app.UseCors("AllowAllOrigins");

app.UseAuthorization();
app.UseSession();

// Endpoint to get the user session value
app.MapGet("/GetUserSession", (HttpContext httpContext) =>
{
    var session = httpContext.Session;
    var user = session.GetString("usuario");
    return user;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
