using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ValconLibrary.Data;
using ValconLibrary.Entities;
using ValconLibrary.Extensions;
using ValconLibrary.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( opt =>
{
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<UserIdentity>();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var contextIdentity = services.GetRequiredService<LibraryIdentityDbContext>();
    var context = services.GetRequiredService<LibraryDbContext>();
    var userManager = services.GetService<UserManager<UserIdentity>>();
    var roleManager = services.GetService<RoleManager<Role>>();
    await contextIdentity.Database.MigrateAsync();
    await context.Database.MigrateAsync();
    await Seed.SeedRolesAndAdmin(userManager, roleManager, builder.Configuration, context);
}

catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();

public partial class Program { }
