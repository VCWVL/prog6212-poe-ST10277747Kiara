using CMCSP3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;   // <-- Correct import
using System;
using System.IO;

namespace CMCSP3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

        // ---------------------------------------------------------
        // QUESTPDF LICENSE SETUP (must run before app.Build())
        // ---------------------------------------------------------
        QuestPDF.Settings.License = LicenseType.Community;
            // ---------------------------------------------------------

            // MVC
            builder.Services.AddControllersWithViews();

            // ---------------------------------------------------------
            // ENABLE SESSION (Fixes throwback after login)
            // ---------------------------------------------------------
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Allows controllers to access HttpContext.Session
            builder.Services.AddHttpContextAccessor();

            // ---------------------------------------------------------
            // DATABASE
            // ---------------------------------------------------------
            builder.Services.AddDbContext<CMCSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // ---------------------------------------------------------
            // ERROR HANDLING
            // ---------------------------------------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // IMPORTANT – must come BEFORE Authorization
            app.UseSession();

            app.UseAuthorization();

            // ---------------------------------------------------------
            // DEFAULT ROUTE → Login Page
            // ---------------------------------------------------------
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            // ---------------------------------------------------------
            // ENSURE DOCUMENT FOLDERS EXIST
            // ---------------------------------------------------------
            string documentsPath = Path.Combine(Directory.GetCurrentDirectory(), "Documents");
            if (!Directory.Exists(documentsPath))
                Directory.CreateDirectory(documentsPath);

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            app.Run();
        }
    }


}
