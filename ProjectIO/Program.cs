using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ProjectIO.model;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


//to do test�w z SQLServer - preferowane raczej
builder.Services.AddDbContext<SportCenterContext>(options =>
{

    // Computer Name (default local database Name)
    String machineName = Environment.MachineName;
    var your_password = "Laptop12345!@#xd";
    options.UseSqlServer($"Server=tcp:ioproject.database.windows.net,1433;Initial Catalog=ioproject;Persist Security Info=False;User ID=CloudSAe1559317;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
