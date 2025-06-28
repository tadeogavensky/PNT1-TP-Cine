using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer("Data Source=localhost;Initial Catalog=PNT1_TP1_Cine;Integrated Security=true;TrustServerCertificate=true;Encrypt=true"));
builder.Services.AddSession(); // ✅ ESTA LÍNEA NUEVA

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // ✅ ESTA LÍNEA NUEVA

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
