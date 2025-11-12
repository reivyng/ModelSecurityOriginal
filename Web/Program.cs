using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Data.Interfaces;
using Business.Interfaces;
using Business.Implements;
using Web.ServiceExtension;
using FluentValidation;
using Data.Implements;
using Data.Implements.BaseData;
using Entity.Context;
using Utilities.Mappers.Profiles;
using Data.Interface;
using Web.Service;
using Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entity.Domain.Config;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddSingleton<IValidatorFactory>(sp =>
    new ServiceProviderValidatorFactory(sp));

// Make HttpContext available for per-request DB provider resolution
builder.Services.AddHttpContextAccessor();

// Swagger
builder.Services.AddSwaggerDocumentation();

// DbContext - register multi-provider service (per-request provider resolution)
builder.Services.AddDatabase(builder.Configuration);

// Register generic repositories and business logic
builder.Services.AddScoped(typeof(IBaseData<>), typeof(BaseModelData<>));
builder.Services.AddScoped(typeof(IBaseBusiness<,>), typeof(BaseBusiness<,>));

// Registrar servicios para entidades nuevas
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
// Refresh token data and token service
builder.Services.AddScoped<IRefreshTokenData, RefreshTokenData>();
builder.Services.AddScoped<Business.Interfaces.ITokenBusiness, Business.Implements.TokenService>();

// Auth service (registration logic)
builder.Services.AddScoped<Business.Interfaces.IAuthBusiness, Business.Implements.AuthService>();

// Jwt settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();
builder.Services.AddScoped<IPersonData, PersonData>();

builder.Services.AddScoped<IRolData, RolData>();
builder.Services.AddScoped<IRolBusiness, RolBusiness>();

builder.Services.AddScoped<IPermissionData, PermissionData>();
builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();

builder.Services.AddScoped<IFormData, FormData>();
builder.Services.AddScoped<IFormBusiness, FormBusiness>();

builder.Services.AddScoped<IFormModuleData, FormModuleData>();
builder.Services.AddScoped<IFormModuleBusiness, FormModuleBusiness>();

builder.Services.AddScoped<IModuleData, ModuleData>();
builder.Services.AddScoped<IModuleBusiness, ModuleBusiness>();

builder.Services.AddScoped<IRolFormPermissionData, RolFormPermissionData>();
builder.Services.AddScoped<IRolFormPermissionBusiness, RolFormPermissionBusiness>();


// Registrar perfiles de AutoMapper
builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<PersonProfile>();
    cfg.AddProfile<RolProfile>();
    cfg.AddProfile<PermissionProfile>();
    cfg.AddProfile<FormProfile>();
    cfg.AddProfile<FormModuleProfile>();
    cfg.AddProfile<ModuleProfile>();
    cfg.AddProfile<RolFormPermissionProfile>();
});

var app = builder.Build();

// Middleware that sets the request DB provider from header X-DB-Provider
app.UseMiddleware<DbContextMiddleware>();

// Swagger (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Gesti�n v1");
        c.RoutePrefix = string.Empty;
    });
}

// Usa la pol�tica de CORS registrada en ApplicationServiceExtensions
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

// Autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicializar base de datos y aplicar migraciones
await InitializeDatabaseAsync(app.Services);

app.Run();

/// <summary>
/// Inicializa la base de datos de manera segura, eliminando y recreando si hay conflictos
/// </summary>
static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        // Verificar si existen migraciones pendientes
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

        logger.LogInformation($"Migraciones aplicadas: {appliedMigrations.Count()}");
        logger.LogInformation($"Migraciones pendientes: {pendingMigrations.Count()}");

        if (pendingMigrations.Any())
        {
            logger.LogInformation("Hay migraciones pendientes. Verificando estado de la base de datos...");

            try
            {
                // Intentar aplicar migraciones normalmente
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migraciones aplicadas exitosamente.");
            }
            catch (Exception migrationEx)
            {
                logger.LogWarning($"Error al aplicar migraciones: {migrationEx.Message}");
                logger.LogInformation("Eliminando y recreando la base de datos...");

                // Si hay error, eliminar y recrear la base de datos
                await dbContext.Database.EnsureDeletedAsync();
                logger.LogInformation("Base de datos eliminada.");

                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Base de datos recreada con �xito.");
            }
        }
        else
        {
            logger.LogInformation("No hay migraciones pendientes. Base de datos actualizada.");
        }

        // Verificar conectividad
        if (await dbContext.Database.CanConnectAsync())
        {
            logger.LogInformation("Conexi�n a la base de datos verificada exitosamente.");
        }
        else
        {
            logger.LogError("No se pudo conectar a la base de datos.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error cr�tico durante la inicializaci�n de la base de datos.");
        throw; // Re-lanzar para detener la aplicaci�n si no se puede inicializar la BD
    }
}
