using System;
using System.Text;
using KingsWalletAPI.Data.Implementations;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Model.Enums;
using KingsWalletAPI.Service.Implementations;
using KingsWalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace KingsWalletAPI.Extensions
{
    public static class MiddlewareExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<KingsWalletContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("KingsWalletConnection"),
                    b => b.MigrationsAssembly("KingsWalletAPI"));
            });


            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.MaxFailedAccessAttempts = 4;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            }).AddEntityFrameworkStores<KingsWalletContext>().AddDefaultTokenProviders();

            services.AddAuthorization(option => option.AddPolicy("AdminRolePolicy", p => p.RequireRole(Roles.Admin.ToString())));
            services.AddAuthorization(option => option.AddPolicy("UserRolePolicy", p => p.RequireRole(Roles.User.ToString())));
           
            services.AddTransient<IServiceFactory, ServiceFactory>();
            services.AddTransient<DbContext, KingsWalletContext>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWalletService, WalletService>();           
            services.AddTransient<IUnitOfWork, UnitofWork<KingsWalletContext>>();
            services.AddScoped<IAuthentication, AuthenticationManager>();                      
            services.AddScoped<IAuthentication, AuthenticationManager>();                      
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = "srstsdtdgudutd" /*configuration["SecretKey"]*/;

            services.AddAuthentication(opt => { opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(5);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });
        }
    }
}
