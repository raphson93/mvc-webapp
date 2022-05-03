using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using mvc_webapp.Models;
using System;
using System.IO;
using System.Text;

namespace mvc_webapp
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
            //services.AddControllersWithViews();
            //services.AddMvc();

            //04/04/2022 - Configure Data Proctection
            //services.AddDataProtection();
            
            //03/05/2022 - Add key storage UNC path *Local*
            //services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\Vincent\AppData\Local\ASP.NET\DataProtection-Keys\"));

            //03/05/2022 - Add key storage UNC path *Server*
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/home/webapp/.aspnet/DataProtection-Keys/"));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                //9/1/2021 - set the application cookies over secure connection.
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
                //9/1/2021 - make the session cookie essential.
                options.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews();
            
            var SecretKey = Encoding.ASCII.GetBytes
         ("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                    ValidateIssuer = true,
                    //ValidIssuer = "http://localhost:5001/",
                    ValidIssuer = "https://rihk-clsv2.ap-southeast-1.elasticbeanstalk.com/",
                    ValidateAudience = true,
                    //ValidAudience = "http://localhost:5001/",
                    ValidAudience = "https://rihk-clsv2.ap-southeast-1.elasticbeanstalk.com/",
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            /*services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EntityDatabaseConnection")));*/
            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EntityDatabaseConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null);
                    })
            );

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
                app.UseExceptionHandler("/Reports/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Account}/{action=Login}");
                    pattern: "{controller=Reports}/{action=Index}/{id?}");
            });
        }
           
    }
}
