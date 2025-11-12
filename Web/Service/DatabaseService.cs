using System;
using Entity.Domain.Enums;
using Entity.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Web.Service
{
    public static class DatabaseService
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var config = serviceProvider.GetRequiredService<IConfiguration>();

                var provider = ResolveProvider(httpContextAccessor, config);

                switch (provider)
                {
                    case DatabaseType.postgres:
                        ConfigurePostgres(options, RequireConnectionString(config, "Postgres"));
                        break;

                    case DatabaseType.MySql:
                        ConfigureMySql(options, RequireConnectionString(config, "MySql"));
                        options.EnableDetailedErrors();
                        options.EnableSensitiveDataLogging();
                        break;

                    case DatabaseType.SqlServer:
                    default:
                        ConfigureSqlServer(options, RequireConnectionString(config, "SqlServer"));
                        break;
                }
            });

            var pg = configuration.GetConnectionString("Postgres");
            if (!string.IsNullOrWhiteSpace(pg))
            {
                // Optional explicit Postgres context registration
                services.AddDbContext<PostgresDbContext>(opt => ConfigurePostgres(opt, pg));
            }

            var my = configuration.GetConnectionString("MySql");
            if (!string.IsNullOrWhiteSpace(my))
            {
                services.AddDbContext<MySqlApplicationDbContext>(opt =>
                {
                    ConfigureMySql(opt, my);
                    opt.EnableDetailedErrors();
                    opt.EnableSensitiveDataLogging();
                });
            }

            return services;
        }

        private static DatabaseType ResolveProvider(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            if (accessor.HttpContext?.Items["DbProvider"] is DatabaseType providerFromHeader)
            {
                return providerFromHeader;
            }

            var configuredProvider = configuration.GetValue<string>("MigrationProvider");
            if (!string.IsNullOrWhiteSpace(configuredProvider) &&
                Enum.TryParse(configuredProvider, true, out DatabaseType parsed))
            {
                return parsed;
            }

            return DatabaseType.SqlServer;
        }

        private static void ConfigureSqlServer(DbContextOptionsBuilder builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, s =>
            {
                s.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                s.EnableRetryOnFailure();
                s.CommandTimeout(60);
            });
        }

        private static void ConfigurePostgres(DbContextOptionsBuilder builder, string connectionString)
        {
            builder.UseNpgsql(connectionString, n =>
            {
                n.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName);
                n.EnableRetryOnFailure();
                n.CommandTimeout(60);
            });
        }

        private static void ConfigureMySql(DbContextOptionsBuilder builder, string connectionString)
        {
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), m =>
            {
                m.MigrationsAssembly(typeof(MySqlApplicationDbContext).Assembly.FullName);
                m.EnableStringComparisonTranslations();
            });
        }

        private static string RequireConnectionString(IConfiguration configuration, string name)
        {
            var value = configuration.GetConnectionString(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Connection string '{name}' is not configured.");
            }

            return value;
        }
    }
}
