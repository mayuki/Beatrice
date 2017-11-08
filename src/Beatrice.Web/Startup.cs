using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utf8Json.AspNetCoreMvcFormatter;
using Utf8Json.Resolvers;
using Utf8Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Server;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Beatrice.Configuration;
using Beatrice.Formatters;
using Beatrice.Web.Models.Configuration;
using Beatrice.Web.Infrastracture;

namespace Beatrice.Web
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
            services.AddOptions();

            var beatriceSecurityConfiguration = Configuration.GetSection("Beatrice:Security").Get<BeatriceSecurityConfiguration>();
            services.AddBeatrice(Configuration.GetSection("Beatrice:DeviceConfiguration"));
            services.AddTransient<Models.UseCase.Resync>();
            services.AddTransient<Models.UseCase.ValidateUser>();
            services.AddScoped<BeatriceOpenIdConnectServerProvider>();
            services.Configure<BeatriceSecurityConfiguration>(Configuration.GetSection("Beatrice:Security"));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Account/SignIn";
                    options.LogoutPath = "/Account/SignOut";
                })
                .AddOAuthValidation()
                .AddOpenIdConnectServer(options =>
                {
                    options.AllowInsecureHttp = beatriceSecurityConfiguration.OAuth.AllowInsecureHttp;
                    options.ApplicationCanDisplayErrors = true;
                    options.UseSlidingExpiration = true;

                    options.ProviderType = typeof(BeatriceOpenIdConnectServerProvider);
                    options.AuthorizationEndpointPath = "/connect/authorize";
                    options.TokenEndpointPath = "/connect/token";
                });

            services.AddMvc()
                .AddMvcOptions(option =>
                {
                    option.OutputFormatters.Clear();
                    option.InputFormatters.Clear();
                    option.OutputFormatters.Add(new JsonOutputFormatter(BeatriceCompositeResolver.Instance));
                    option.InputFormatters.Add(new JsonInputFormatter(BeatriceCompositeResolver.Instance));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
