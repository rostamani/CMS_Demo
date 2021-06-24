using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCMS.DataLayer.Context;
using MyCMS.Services.Repository;
using MyCMS.Services.Repository.IRepository;
using MyCMS.Utilities.Senders;
using MyCMS.Utilities.Convertors;
using MyCMS.Web.Mappers;

namespace MyCMS.Web
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();



            services.AddDbContext<MyCMSDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyCMSConnectionString"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<MyCMSDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(options=> {
                options.ClientId = "927102086055-iedonpuisspa1ic58dsio0i9cr6kfcoo.apps.googleusercontent.com";
                options.ClientSecret = "V5B2e2xbTkoqDfeXkthp6iB-";
            });

            #region Dependency Injection

            services.AddScoped<IPageGroupRepository, PageGroupRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
           
            services.AddAutoMapper(typeof(Mapping));
            services.AddScoped<IMessageSender, MessageSender>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddAuthorization();
            #endregion

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                //endpoints.MapAreaControllerRoute(
                //      name: "areas",
                //      areaName: "Admin",
                //      pattern: "{area:exists}/{controller=pagegroups}/{action=Index}/{id?}"
                //  );
                endpoints.MapControllerRoute(
                                        name: "default",
                                        pattern: "{controller=Home}/{action=Index}/{id?}"
                                    );
                endpoints.MapControllerRoute(
                                        name: "default",
                                        pattern: "{area=admin}/{controller=Home}/{action=Index}/{id?}"
                                    );
                endpoints.MapAreaControllerRoute(
                      name: "areas",
                      areaName: "Admin",
                      pattern: "{area:exists}/{controller=pagegroups}/{action=Index}/{id?}"
                  );




            });
        }
    }
}
