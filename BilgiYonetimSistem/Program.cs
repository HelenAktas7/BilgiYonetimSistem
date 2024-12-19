using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilgiYonetimSistem.Models;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // MVC ve API'yi ayn? anda ekliyoruz
        builder.Services.AddControllersWithViews();  // MVC i�in
        builder.Services.AddControllers(); // API i�in

        // Swagger'? sadece API i�in etkinle?tirin
        builder.Services.AddEndpointsApiExplorer();
 
        builder.Services.AddHttpClient();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum s�resi (30 dakika)
            options.Cookie.HttpOnly = true; // �erezlere sadece sunucu taraf?ndan eri?ilebilmesi i�in
            options.Cookie.IsEssential = true; // �erezlerin GDPR uyumlu olmas? i�in gerekli
        });

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Swagger'? sadece API u� noktalar? i�in etkinle?tirme
        if (app.Environment.IsDevelopment())
        {
            
        }
        app.UseSession();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();


        // MVC route ekleyin (login sayfas?na y�nlendirme)
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=LoginUser}/{id?}"); // Login sayfas?na y�nlendirme

        // API route
        app.MapControllers(); // API u� noktalar? i�in

        app.Run();
    }
}