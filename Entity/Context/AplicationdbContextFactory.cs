﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Entity.Context.DesignTime
{
    // Factory para crear AplicationdbContext en tiempo de diseño (migraciones).
    // Lee appsettings.json y permite pasar el proveedor como argumento.
    public class AplicationdbContextFactory : IDesignTimeDbContextFactory<AplicationdbContext>
    {
        public AplicationdbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // provider puede venir en args (dotnet ef ... -- MySql) o de appsettings: DatabaseProvider
            var provider = args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0])
                ? args[0]
                : config["DatabaseProvider"] ?? "MySql";

            string connKey = provider switch
            {
                "SqlServer" => "ConnectionStrings:SqlServer",
                "Postgres" => "ConnectionStrings:Postgres",
                _ => "ConnectionStrings:MySql",
            };

            var connectionString = config[connKey] ?? throw new InvalidOperationException($"No se encontró la cadena: {connKey}");

            var optionsBuilder = new DbContextOptionsBuilder<AplicationdbContext>();

            switch (provider)
            {
                case "SqlServer":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case "Postgres":
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                default: // MySql (Pomelo)
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    break;
            }

            return new AplicationdbContext(optionsBuilder.Options);
        }
    }
}