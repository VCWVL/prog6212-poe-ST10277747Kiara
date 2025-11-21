using CMCSP3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;   
using System;
using System.IO;

namespace CMCSP3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

        
        QuestPDF.Settings.License = LicenseType.Community;
            
            builder.Services.AddControllersWithViews();

            
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            
            builder.Services.AddHttpContextAccessor();

            
            builder.Services.AddDbContext<CMCSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

           
            app.UseSession();

            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

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
//Anglia Ruskin University (2025) Harvard System of Referencing Guide. Available at: https://library.aru.ac.uk/referencing/harvard.htm (Accessed: 17 September 2025).