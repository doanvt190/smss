using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.Interfaces;
using WebSIMS.Repositories;
using WebSIMS.Services;

namespace WebSIMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure database with EntityFramework
            builder.Services.AddDbContext<SIMSDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("default")));
            // dang ky Interface, Repository, Service
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<ITimetableRepository, TimetableRepository>();
            builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
            builder.Services.AddScoped<UserService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // cau hinh Authentication
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
                options.AddPolicy("FacultyOnly", policy => policy.RequireRole("Faculty"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
