using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ProjectIO.DBModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//to do testów z SQLServer - preferowane raczej
builder.Services.AddDbContext<SportCenterContext>(options =>
{
    options.UseSqlServer("Server=LAPTOKMICHAL;Database=SportCenterDB;Trusted_Connection=True;TrustServerCertificate=True;");
});



void AddUser(SportCenterContext context, string userName, string userPassword)
{
    var user = new User()
    {
        userName = userName,
        userPassword = userPassword
    };

    context.Users.Add(user);
    context.SaveChanges();

}

AddUser(builder.Services.BuildServiceProvider().GetService<SportCenterContext>(), "paduch", "avr");
AddUser(builder.Services.BuildServiceProvider().GetService<SportCenterContext>(), "drabol", "pic");
AddUser(builder.Services.BuildServiceProvider().GetService<SportCenterContext>(), "gilu", "zilog");

// Automatyczne zastosowanie migracji przy uruchomieniu aplikacji
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SportCenterContext>();
    context.Database.Migrate();
}

//niech jest zakomentowane bo problemy s¹
//to do testów z InMemoryDatabase
//using (var context = new SportCenterContext())
//{
//    var gabrys = new User()
//    {
//        userID = 1,
//        userName = "drabol",
//        userPassword = "gabriel"
//    };

//    var paduszek = new User()
//    {
//        userID = 2,
//        userName = "paduszek",
//        userPassword = "kocham esp"
//    };

//    context.Users.Add(gabrys);
//    context.Users.Add(paduszek);
//    context.SaveChanges();
//    var allUsers = context.Users.ToList();
//    context.Users.Remove(gabrys);
//    context.SaveChanges();
//    allUsers = context.Users.ToList();
//}
//uwaga - wszystko z baz¹ musi siê dziaæ przed t¹ linijk¹ var app = builder.Build();
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

app.Run();
