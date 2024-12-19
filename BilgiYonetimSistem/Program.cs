using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilgiYonetimSistem.Models;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // MVC ve API'yi ayn? anda ekliyoruz
        builder.Services.AddControllersWithViews();  // MVC için
        builder.Services.AddControllers(); // API için

        // Swagger'? sadece API için etkinle?tirin
        builder.Services.AddEndpointsApiExplorer();
 
        builder.Services.AddHttpClient();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum süresi (30 dakika)
            options.Cookie.HttpOnly = true; // Çerezlere sadece sunucu taraf?ndan eri?ilebilmesi için
            options.Cookie.IsEssential = true; // Çerezlerin GDPR uyumlu olmas? için gerekli
        });

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Swagger'? sadece API uç noktalar? için etkinle?tirme
        if (app.Environment.IsDevelopment())
        {
            
        }
        app.UseSession();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();


        // MVC route ekleyin (login sayfas?na yönlendirme)
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=LoginUser}/{id?}"); // Login sayfas?na yönlendirme

        // API route
        app.MapControllers(); // API uç noktalar? için

        app.Run();
    }
}