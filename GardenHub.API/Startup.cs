using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenHub.CrossCutting.Storage;
using GardenHub.Domain;
using GardenHub.Domain.Account;
using GardenHub.Domain.Account.Repository;
using GardenHub.Domain.Comment.Repository;
using GardenHub.Domain.Post.Repository;
using GardenHub.Repository.Account;
using GardenHub.Repository.Comment;
using GardenHub.Repository.Context;
using GardenHub.Repository.Post;
using GardenHub.Services.Account;
using GardenHub.Services.Authenticate;
using GardenHub.Services.Comment;
using GardenHub.Services.Post;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GardenHub.API
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
            //services.AddScoped<IAccountService, AccountServices>();
            //services.AddScoped<IAccountRepository, AccountRepository>();

            //services.AddScoped<IUserStore<Account>, AccountRepository>();
            //services.AddScoped<IAccountIdentityManager, AccountIdentityManager>();
            //services.AddScoped<IRoleStore<Role>, RoleRepository>();

            //services.AddScoped<IPostServices, PostServices>();
            //services.AddScoped<IPostRepository, PostRepository>();

            //services.AddScoped<ICommentServices, CommentServices>();
            //services.AddScoped<ICommentRepository, CommentRepository>();

            //services.AddScoped<AccountRepository>();

            services.AddTransient<IAccountService, AccountServices>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            //services.AddTransient<IUserStore<Account>, AccountRepository>();
            //services.AddTransient<IAccountIdentityManager, AccountIdentityManager>();
            //services.AddTransient<IRoleStore<Role>, RoleRepository>();

            services.AddTransient<IPostServices, PostServices>();
            services.AddTransient<IPostRepository, PostRepository>();

            services.AddTransient<ICommentServices, CommentServices>();
            services.AddTransient<ICommentRepository, CommentRepository>();

            services.AddTransient<AccountRepository>();

            services.AddDbContext<GardenHubContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("GardenHubConnection"));
            });

            // JWT
            services.AddScoped<AuthenticateService>();

            var key = Encoding.UTF8.GetBytes(this.Configuration["Token:Secret"]);

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Bearer";
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters.ValidIssuer = "LOGIN-API";
                o.TokenValidationParameters.ValidAudience = "LOGIN-API";
                o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
            });

            // Azure
            services.Configure<AzureStorageOptions>(Configuration.GetSection("Microsoft.Storage"));
            services.AddScoped<AzureStorage>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // JWT
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
