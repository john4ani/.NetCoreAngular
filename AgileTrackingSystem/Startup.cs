using AgileTrackingSystem.Data;
using AgileTrackingSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using AgileTrackingSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AgileTrackingSystem
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>  
            {
                cfg.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DBContext>();

            services.AddAuthentication()
                    .AddCookie()
                    .AddJwtBearer(cfg => cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = _configuration["Tokens:Issuer"],
                        ValidAudience = _configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
                    });

            services.AddDbContext<DBContext>(cfg => {
                cfg.UseSqlServer(_configuration.GetConnectionString("SqlServerConnectionString"));
            });

            services.AddAutoMapper();

            services.AddTransient<DBSeeder>();
            services.AddScoped<IDBRepository, DBRepository>();
            services.AddTransient<IMailService, NullMailService>();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt=>opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseNodeModules(env);

            app.UseMvc(cfg => {
                cfg.MapRoute("Default","/{controller}/{action}/{id?}", new { controller = "App", Action = "Index"});
            });
        }
    }
}
