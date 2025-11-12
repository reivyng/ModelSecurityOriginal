using System;
using Entity.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Entity.Context;

namespace Web.Infrastructure
{
    public class DbContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DbContextFactory(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext()
        {
            var provider = _httpContextAccessor.HttpContext?.Items["DbProvider"] as DatabaseType?
                           ?? DatabaseType.SqlServer;

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            switch (provider)
            {
                case DatabaseType.postgres:
                    optionsBuilder.UseNpgsql(RequireConnectionString("Postgres"));
                    break;

                case DatabaseType.MySql:
                    var mysql = RequireConnectionString("MySql");
                    optionsBuilder.UseMySql(mysql, ServerVersion.AutoDetect(mysql));
                    break;

                case DatabaseType.SqlServer:
                default:
                    optionsBuilder.UseSqlServer(RequireConnectionString("SqlServer"));
                    break;
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private string RequireConnectionString(string name)
        {
            var value = _configuration.GetConnectionString(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Connection string '{name}' is not configured.");
            }

            return value;
        }
    }
}
