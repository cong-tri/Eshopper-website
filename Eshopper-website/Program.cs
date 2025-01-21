using Eshopper_website.Areas.Admin.Repository;
using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Eshopper_website.Services.VNPay;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Eshopper_website.Services.GHN;
using Eshopper_website.Models.GHN;
using Eshopper_website.Models.Momo;
using Eshopper_website.Services.Momo;
using Eshopper_website.Models.Recaptcha;
using Eshopper_website.Services.Recaptcha;

using Eshopper_website.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Facebook;
using Eshopper_website.Areas.Admin.Models.SendEmail;

namespace Eshopper_website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //connect MOMO API
            builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
            builder.Services.AddScoped<IMomoService, MomoService>();

            // Add GHN Services
            builder.Services.Configure<GHN_Setting>(builder.Configuration.GetSection("GHNSettings"));
            builder.Services.AddScoped<IGHNService, GHNService>();

            // connect Recaptcha Google
            //builder.Services.Configure<Recaptcha_Setting>(builder.Configuration.GetSection("ReCaptchaSetting"));
            //builder.Services.AddHttpClient<IRecaptchaService, RecaptchaService>();

            //connect VnPay API
            builder.Services.AddScoped<IVnPayService, VnPayService>();
          
            // Add services to the container.
            builder.Services.AddDbContext<EShopperContext>(opt =>
			    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.Configure<Appsettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddCookie()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
					IConfigurationSection googleConfig = builder.Configuration.GetSection("Authentication:Google");

                    //options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
                    //options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
                    options.ClientId = googleConfig["ClientId"];
                    options.ClientSecret = googleConfig["ClientSecret"];
                })
                .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
                {
                    IConfigurationSection facebookConfig = builder.Configuration.GetSection("Authentication:Facebook");
                    options.AppId = facebookConfig["AppId"];
                    options.AppSecret = facebookConfig["AppSecret"];
                    options.CallbackPath = "/dang-nhap-tu-facebook";
                });
				//.AddJwtBearer(options =>
				//{
				//	var jwtSettings = builder.Configuration.GetSection("JwtSettings");
				//	options.TokenValidationParameters = new TokenValidationParameters
				//	{
				//		ValidateIssuer = true,
				//		ValidateAudience = true,
				//		ValidateLifetime = true,
				//		ValidateIssuerSigningKey = true,
				//		ValidIssuer = jwtSettings["Issuer"],
				//		ValidAudience = jwtSettings["Audience"],
				//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
				//	};
				//});

            builder.Services.AddAuthorization();

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = null;
                    //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
                //.AddJsonOptions(options =>
                //{
                //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //    //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //});

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Path = "/";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            //builder.Services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.Lax;
            //});

            builder.Services.Configure<EmailConfiguration>(
                builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            // Force HTTPS in production
            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = 7068;
                });
            }

            var app = builder.Build();

            app.UseCookiePolicy();

            //app.UseStatusCodePagesWithRedirects("/Home/Error404?statuscode={0}");

            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Add CSP headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add(
                    "Content-Security-Policy",
                    "default-src 'self';" +
                    "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://code.jquery.com https://cdn.jsdelivr.net;" +
                    "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com;" +
                    "img-src 'self' data: https:;" +
                    "font-src 'self' https://fonts.gstatic.com;" +
                    "frame-src 'self' https://sandbox.vnpayment.vn/;" +
                    "connect-src 'self' https://sandbox.vnpayment.vn/;"
                );
                
                await next();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            //app.UseMiddleware();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "blog",
                pattern: "blog/{Slug?}",
                defaults: new { controller = "Blog", action = "Details" });

            //app.MapControllerRoute(
            //    name: "product",
            //    pattern: "product/{Slug?}",
            //    defaults: new { controller = "Product", action = "Details" });

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
