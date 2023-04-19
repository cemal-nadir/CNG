#nullable enable
using CNG.Core.Exceptions;
using CNG.EntityFrameworkCore.Enums;
using CNG.EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CNG.EntityFrameworkCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseService<TContext>(
            this IServiceCollection services,
            DatabaseOption option,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TContext : DbContext
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<DbContext, TContext>();
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<DbContext, TContext>();
                    break;
                case ServiceLifetime.Transient:
                default:
                    services.AddTransient<DbContext, TContext>();
                    break;

            }
            services.AddDbContext<TContext>((Action<DbContextOptionsBuilder>)(o =>
            {
                var connectionString = option.GetConnectionString();
                switch (option.DatabaseType)
                {
                    case DatabaseType.MsSql:
                        o.UseSqlServer(connectionString, (x => x.CommandTimeout(600))).EnableDetailedErrors();

                        break;
                    case DatabaseType.PostgreSql:
                        o.UseNpgsql(connectionString, (x => x.CommandTimeout(600))).EnableDetailedErrors();

                        break;
                    default:
                        throw new NotFoundException("DatabaseType not found");
                }
            }), lifetime);
            Console.WriteLine(option.DatabaseType.ToString() + " Database Service is installed");
        }

        public static void DbMigrate<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            var service = app.ApplicationServices.GetService<TContext>();

            //service?.Database.Migrate();

            Console.WriteLine("Connection String :" + service?.Database.GetConnectionString());
            Console.WriteLine("Database Migrated!!");
        }
    }
}
