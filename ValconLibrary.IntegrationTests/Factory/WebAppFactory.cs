using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data.Common;
using ValconLibrary.Data;
using ValconLibrary.IntegrationTests.IntegrationTests;

namespace ValconLibrary.IntegrationTests.Factory
{
    public class WebAppFactory : WebApplicationFactory<Program>
    {
        public string DefaultUserId { get; set; } = "1";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            });

            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LibraryDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                if (dbConnectionDescriptor != null)
                {
                    services.Remove(dbConnectionDescriptor);
                }

                var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                string connectionString = configuration.GetConnectionString("DbConnection");

                services.AddDbContext<LibraryDbContext>(opt =>
                    opt.UseInMemoryDatabase(connectionString));

                services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

                services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

               var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

                    db.Database.EnsureCreated();
                }
            });
            builder.UseEnvironment("Development");
        }
    }
}
