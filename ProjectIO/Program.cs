using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ProjectIO.model;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//to do test�w z SQLServer - preferowane raczej
builder.Services.AddDbContext<SportCenterContext>(options =>
{

    // Computer name (default local database name)
    String machineName = Environment.MachineName;
    options.UseSqlServer($"Server={machineName};Database=SportCenterDB;Trusted_Connection=True;TrustServerCertificate=True;");
});

builder.Services.AddDistributedMemoryCache(); // Wymagane dla sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas życia sesji
    options.Cookie.HttpOnly = true; // Sesja dostępna tylko przez HTTP
    options.Cookie.IsEssential = true; // Sesja jest kluczowa dla działania aplikacji
});


// Automatyczne zastosowanie migracji przy uruchomieniu aplikacji
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SportCenterContext>();
    //context.Database.Migrate();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession(); // Włącz obsługę sesji

app.Run();
