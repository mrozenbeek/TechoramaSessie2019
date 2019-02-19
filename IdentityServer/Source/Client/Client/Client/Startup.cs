using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.ClaimActions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Serialization;

namespace Client
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

            //IdentityModelEventSource.ShowPII = true;

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


            services.AddAuthentication(options =>
           {
               options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
           })
           .AddCookie()
           .AddOpenIdConnect(options =>
           {
               options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

               options.Authority = "http://localhost:3254/";
               options.RequireHttpsMetadata = false;
               options.ClientId = "mvc";
               options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
               options.ClaimActions.Add(new RoleClaimAction());
               options.ResponseType = "code id_token";
               options.GetClaimsFromUserInfoEndpoint = true;

               options.Scope.Clear();

               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("role");
               options.Scope.Add("offline_access");
               options.Scope.Add("api1");

               options.SaveTokens = true;

               options.TokenValidationParameters.NameClaimType = "name";
               options.TokenValidationParameters.RoleClaimType = "role";

               options.Events = new OpenIdConnectEvents()
               {

                   OnRemoteFailure = context =>
                   {
                       context.Response.Redirect("/Home/Error");
                       context.HandleResponse();
                       return Task.FromResult(0);
                   },
                   OnUserInformationReceived = context =>
                   {
                       return Task.FromResult(context);
                   }
               };

               
           }
           );



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
