using System;
using BlazorProyectoPostres.Data;
using BlazorProyectoPostres.Models.DatabaseModels;
using BlazorProyectoPostres.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BlazorProyectoPostres
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
       
            services.AddRazorPages();
            services.AddServerSideBlazor()
                .AddCircuitOptions(opts => opts.DetailedErrors = true);


            
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddScoped<PostresService>();
            services.AddScoped<DialogService>();



            services.AddDbContext<IdentityModels>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ContextoTablasPostres>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityModels>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            services.AddScoped<SignInManager<ApplicationUser>, CustomSignInManager>();
            services.AddSingleton<ITicketStore, InMemoryTicketStore>();
            services.ConfigureApplicationCookie(options =>
            {
                options.SessionStore = new InMemoryTicketStore();
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.LoginPath = "/Identity/Account/Login";

            
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";

   
                options.LogoutPath = "/Identity/Account/Logout";
            });

 
            services.Configure<PasswordHasherOptions>(opts =>
            {
                opts.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            });

            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();
            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //});

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ? Debe estar activo
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/test-anonymous", async context =>
                {
                    await context.Response.WriteAsync("Esta página ES anónima.");
                }).AllowAnonymous(); // Endpoint explícitamente anónimo

                endpoints.MapGet("/test-fallback", async context =>
                {
                    await context.Response.WriteAsync("Esta página debería requerir login (fallback).");
                }); // Sin metadata de autorización, debería usar FallbackPolicy

                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                //endpoints.MapFallbackToPage("/_Host").RequireAuthorization();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
