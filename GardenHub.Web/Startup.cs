using System;
using GardenHub.Domain;
using GardenHub.Domain.Account;
using GardenHub.Services.Account;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using GardenHub.Repository.Account;
using GardenHub.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using GardenHub.Domain.Account.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using GardenHub.Services.Post;
using GardenHub.Domain.Post.Repository;
using GardenHub.Repository.Post;
using GardenHub.Services.Comment;
using GardenHub.Domain.Comment.Repository;
using GardenHub.Repository.Comment;
using GardenHub.Domain.Post;
using GardenHub.Domain.Comment;
using GardenHub.CrossCutting.Storage;

namespace GardenHub.Web
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
            services.AddTransient<IAccountService, AccountServices>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            services.AddTransient<IUserStore<Account>, AccountRepository>();
            services.AddTransient<IAccountIdentityManager, AccountIdentityManager>();
            services.AddTransient<IRoleStore<Role>, RoleRepository>();
            
            services.AddTransient<IPostServices, PostServices>();
            services.AddTransient<IPostRepository, PostRepository>();

            services.AddTransient<ICommentServices, CommentServices>();
            services.AddTransient<ICommentRepository, CommentRepository>();

            //Azure Blob Storage Configuration
            services.AddTransient<AzureStorage>();
            services.Configure<AzureStorageOptions>(Configuration.GetSection("Microsoft.Storage"));

            services.AddDbContext<GardenHubContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("GardenHubConnection"));
            });

            services.AddIdentity<Account, Role>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.AddControllersWithViews();
            services.AddSession();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Post}/{action=Home}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
