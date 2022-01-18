using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Options;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Infrastructure.Identity.TokenProvider;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            
            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<User>>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider<EmailConfirmationTokenProvider>("EmailConfirmationTokenProvider");
            
            services.Configure<IdentityOptions>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    // Identity : Default password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                });

            // services required using Identity
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUploadService,UploadService>();

            services.AddOptions();
            services.Configure<GmailOptions>(options => configuration.GetSection("GmailOptions").Bind(options));
            services.Configure<CloudinaryOptions>(options => configuration.GetSection("CloudinaryOptions").Bind(options));

            return services;
        }
    }
}