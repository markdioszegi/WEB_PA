using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using PA.Services;

namespace PA
{
    public sealed class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private static void RemoveReturnUrlFromRedirectUri(RedirectContext<CookieAuthenticationOptions> context)
        {
            var ub = new UriBuilder(context.RedirectUri);
            var query = QueryHelpers.ParseQuery(ub.Query);
            ub.Query = null;
            query.Remove("ReturnUrl");
            context.RedirectUri = ub.Uri.ToString();
            foreach (var key in query.Keys)
            {
                context.RedirectUri = QueryHelpers.AddQueryString(context.RedirectUri, key, query[key]);
            }
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            RemoveReturnUrlFromRedirectUri(context);
            return base.RedirectToAccessDenied(context);
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            RemoveReturnUrlFromRedirectUri(context);
            return base.RedirectToLogin(context);
        }
    }

    public class Startup
    {
        readonly string ConnectionString;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = "Host=localhost;Username=postgres;Password=admin;Database=getset";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(typeof(IUserService))
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.EventsType = typeof(CustomCookieAuthenticationEvents));
            services.AddScoped<CustomCookieAuthenticationEvents>();
            services.AddScoped<IDbConnection>(_ =>
            {
                var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                //System.Console.WriteLine("Connection made!");
                return connection;
            });
            services.AddScoped<IDBService, PostgreSqlDBService>();
            services.AddScoped<IProductsService, SqlProductsService>();
            services.AddScoped<IUsersService, SqlUsersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            /* app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" }
            }); */

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


}
