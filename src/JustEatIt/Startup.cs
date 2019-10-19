using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using JustEatIt.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustEatIt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(opts => opts.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Verify which keys is available and add authentication for them
            if ((Configuration["Authentication:Facebook:AppId"] != null) && (Configuration["Authentication:Facebook:AppSecret"] != null))
            {
                services.AddAuthentication().AddFacebook(opt =>
                {
                    opt.AppId = Configuration["Authentication:Facebook:AppId"];
                    opt.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                });
            }
            if ((Configuration["Authentication:Twitter:ConsumerAPIKey"] != null) && (Configuration["Authentication:Twitter:ConsumerSecret"] != null))
            {
                services.AddAuthentication().AddTwitter(opt =>
                {
                    opt.ConsumerKey = Configuration["Authentication:Twitter:ConsumerAPIKey"];
                    opt.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                });
            }
            if ((Configuration["Authentication:Google:ClientId"] != null) && (Configuration["Authentication:Google:ClientSecret"] != null))
            {
                services.AddAuthentication().AddGoogle(opt =>
                {
                    opt.ClientId = Configuration["Authentication:Google:ClientId"];
                    opt.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });
            }
            if ((Configuration["Authentication:Microsoft:ClientId"] != null) && (Configuration["Authentication:Microsoft:ClientSecret"] != null))
            {
                services.AddAuthentication().AddMicrosoftAccount(opt =>
                {
                    opt.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    opt.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                });
            }

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
