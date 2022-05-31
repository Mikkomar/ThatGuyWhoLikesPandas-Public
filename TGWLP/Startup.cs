using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.BLL;
using TGWLP.BLL.Interfaces;
using TGWLP.BLL.Clients;
using TGWLP.BLL.Services;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;

namespace TGWLP
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            // AutoMapper - Should be removed?
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            // AntiForgery
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            // Controllers
            services.AddControllers();

            // Database connection
            services.AddTransient<IDbConnection>((sp) =>
                new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Logging
            services.AddLogging();

            // DbContext
            if (!string.IsNullOrEmpty(this.Configuration.GetConnectionString("DefaultConnection"))){
                // If a ConnectionString is provided
                services.AddDbContext<IAppContext, DAL.AppContext>(options =>
                    options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies());
            }
            else
            {
                // Use in-memory database if no ConnectionString is provided
                services.AddDbContext<IAppContext, DAL.AppContext>(options =>
                    options.UseInMemoryDatabase("TGWLPDatabase"));
            }

            // Appsettings file
            services.AddSingleton(_ => Configuration);

            // Localizer
            services.AddSingleton<CultureLocalizer>();
            services.AddLocalization();

            // API Clients and services
            services.AddTransient<IGoogleAPIClient, GoogleAPIClient>();
            services.AddTransient<IBookService, BookService>();
            services.AddHttpClient("GoogleBooks", client => {
                client.BaseAddress = new Uri(this.Configuration["API:GoogleAPI:Books:BaseAddress"]);
            });

            // Identity
            services.AddIdentity<User, Role>(config => {
                config.Password.RequiredLength = Int32.Parse(this.Configuration.GetSection("PasswordSettings")["MinLength"]);
                config.Password.RequireDigit = Boolean.Parse(this.Configuration.GetSection("PasswordSettings")["RequireDigits"]);
                config.Password.RequireNonAlphanumeric = Boolean.Parse(this.Configuration.GetSection("PasswordSettings")["RequireNonAlphanumeric"]);
                config.Password.RequireUppercase = Boolean.Parse(this.Configuration.GetSection("PasswordSettings")["RequireUppercase"]);
                config.Password.RequireLowercase = Boolean.Parse(this.Configuration.GetSection("PasswordSettings")["RequireLowercase"]);
            })
                .AddEntityFrameworkStores<DAL.AppContext>()
                .AddRoleManager<RoleManager<Role>>()
                .AddDefaultTokenProviders();

            // Authorization cookie
            services.ConfigureApplicationCookie(configuration => {
                configuration.Cookie.Name = "TGWLP.Cookie";
                configuration.LoginPath = "/Login";
            });

            // Authorization
            services.AddAuthorization(options => {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                options.DefaultPolicy = defaultAuthBuilder
                .RequireAuthenticatedUser()
                .Build();
            });

            // Turn Appsettings into class
            services.Configure<AppSettings>(Configuration);
            services.AddOptions();

            // Set localization languages - not in use currently!
            services.Configure<RequestLocalizationOptions>(
                options => {
                    var supportedCultures = new List<CultureInfo> {
                        new CultureInfo("en-GB"),
                        //new CultureInfo("en-US"),
                        new CultureInfo("en")
                        //new CultureInfo("fi")
                    };
                    options.DefaultRequestCulture = new RequestCulture("en");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
                }
            );

            // Cookie consent
            services.Configure<CookiePolicyOptions>(options => {
                options.ConsentCookie.Name = "TGWLP.ConsentCookie";
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            services.AddRazorPages()
                .AddMvcOptions(options => { })
                .AddMicrosoftIdentityUI()
                .AddRazorRuntimeCompilation();

            services.AddMvc(options => {
                var F = services.BuildServiceProvider().GetService<CultureLocalizer>();
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => string.Format(F["Validation_InvalidField"], x));
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) => string.Format(F["Validation_InvalidField"], x));
            })
                    .AddViewLocalization(
                        LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (t, f) => f.Create(typeof(SharedResource))); // Needed for localizing properties by attribute
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}"
                    );
            });
        }
    }
}
