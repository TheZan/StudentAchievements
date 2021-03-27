using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Razor;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Infrastructure;
using StudentAchievements.Models;
using StudentAchievements.Areas.Message.Infrastructure;

namespace StudentAchievements
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StudentAchievementsDbContext>(options =>
                options.UseNpgsql(Configuration["Data:StudentAchievementsIdentity:ConnectionString"]));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<StudentAchievementsDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = true;
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDataRepository, DataRepository>();
            services.AddTransient<Messenger>();
            services.AddSingleton<ConnectionProvider>();

            /* services.AddMvc(options => options.EnableEndpointRouting = false); */

            services.AddMvc();

            services.AddMemoryCache();
            services.AddSession();
            services.AddSignalR();
            services.Configure<RazorViewEngineOptions>(options => options.ViewLocationExpanders.Add(new RazorViewLocationExpander()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints => 
            {
                 endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "AreaAdmin",
                    areaName: "Admin",
                    pattern: "{controller=Admin}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "AreaTeacher",
                    areaName: "Teacher",
                    pattern: "{controller=Teacher}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "AreaEmployer",
                    areaName: "Employer",
                    pattern: "{controller=Employer}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "AreaStudent",
                    areaName: "Student",
                    pattern: "{controller=Student}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "AreaAuthorization",
                    areaName: "Authorization",
                    pattern: "{controller=Account}/{action=Login}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "AreaMessage",
                    areaName: "Message",
                    pattern: "{controller=Message}/{action=Index}/{id?}"
                );

                endpoints.MapHub<ChatHub>("/chat");
            });

            /* app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    name: null,
                    areaName: "Admin",
                    template: "Admin/{controller=Admin}/{action=Index}");

                routes.MapAreaRoute(
                    name: null,
                    areaName: "Teacher",
                    template: "Teacher/{controller=Teacher}/{action=Index}");

                routes.MapAreaRoute(
                    name: null,
                    areaName: "Employer",
                    template: "Employer/{controller=Employer}/{action=Index}");

                routes.MapAreaRoute(
                    name: null,
                    areaName: "Student",
                    template: "Student/{controller=Student}/{action=Index}");

                routes.MapAreaRoute(
                    name: null,
                    areaName: "Authorization",
                    template: "Identity/{controller=Account}/{action=Login}");

                routes.MapAreaRoute(
                    name: null,
                    areaName: "Message",
                    template: "Message/{controller=Message}/{action=Index}");

                routes.MapRoute(
                    name: null,
                    template: "{controller=Home}/{action=Index}/{id?}");
            }); */
        }
    }
}
