using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipify.Data;
using Recipify.Data.Seeding;
namespace Recipify.Web
{
    using Data;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Identity.Client;
    using Recipify.Data.Seeding;
    using Recipify.Services.Core;
    using Recipify.Services.Core.Contracts;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {


            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<ICuisinesService, CuisinesService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IDifficultyLevelService, DifficultyLevelService>();
            builder.Services.AddScoped<ICommentService, CommentService>();

            WebApplication? app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error/500");
               app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //using (var scope = app.Services.CreateScope())
            //{
            //   var services = scope.ServiceProvider;
            //    try
            //    {

            //       await  IdentitySeeder.SeedRolesAsync(services);
            //        await IdentitySeeder.SeedAdminAsync(services);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred seeding the DB.");
            //    }
            //}
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAsync(services);
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

           // app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminPanel}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();

            async Task SeedRolesAsync(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Administrator", "Editor", "User" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
    }

}
