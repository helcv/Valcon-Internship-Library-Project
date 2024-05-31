using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ValconLibrary.Authentication;
using ValconLibrary.Data;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;
using ValconLibrary.Repository;
using ValconLibrary.Services;

namespace ValconLibrary.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddDbContext<LibraryIdentityDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DbConnection"));
            });

            services.AddDbContext<LibraryDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DbConnection"));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationTokenHandler, AuthenticationTokenHandler>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IRentRepository, RentRepository>();
            services.AddScoped<IRentService, RentService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddIdentityCore<UserIdentity>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 7;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<LibraryIdentityDbContext>()
                .AddApiEndpoints();

            return services;
        }
    }
}
