#nullable enable
using System.Runtime.CompilerServices;
using CNG.Core.Exceptions;
using CNG.EntityFrameworkCore.Enums;
using CNG.EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

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
                default:
                    services.AddTransient<DbContext, TContext>();
                    break;
            }
            services.AddDbContext<TContext>((Action<DbContextOptionsBuilder>)(o =>
            {
                if (string.IsNullOrEmpty(option.ConnectionString))
                {
                    switch (option.DatabaseType)
                    {
                        case DatabaseType.MsSql:
                            var optionsBuilder1 = o;
                            var interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(34, 5);
                            interpolatedStringHandler1.AppendLiteral("Server=");
                            interpolatedStringHandler1.AppendFormatted(option.Host);
                            ref var local1 = ref interpolatedStringHandler1;
                            var port1 = option.Port;
                            string str1;
                            if (!port1.HasValue || port1.GetValueOrDefault() <= 0)
                            {
                                str1 = "";
                            }
                            else
                            {
                                var interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 1);
                                interpolatedStringHandler2.AppendLiteral(",");
                                interpolatedStringHandler2.AppendFormatted(option.Port);
                                str1 = interpolatedStringHandler2.ToStringAndClear();
                            }
                            local1.AppendFormatted(str1);
                            interpolatedStringHandler1.AppendLiteral(";Database=");
                            interpolatedStringHandler1.AppendFormatted(option.DbName);
                            interpolatedStringHandler1.AppendLiteral(";User=");
                            interpolatedStringHandler1.AppendFormatted(option.UserName);
                            interpolatedStringHandler1.AppendLiteral(";Password=");
                            interpolatedStringHandler1.AppendFormatted(option.Password);
                            interpolatedStringHandler1.AppendLiteral(";");
                            var stringAndClear1 = interpolatedStringHandler1.ToStringAndClear();
                            optionsBuilder1.UseSqlServer(stringAndClear1, (Action<SqlServerDbContextOptionsBuilder>)(x => x.CommandTimeout(int.MaxValue)));
                            break;
                        case DatabaseType.PostgreSql:
                            var optionsBuilder2 = o;
                            var interpolatedStringHandler3 = new DefaultInterpolatedStringHandler(37, 5);
                            interpolatedStringHandler3.AppendLiteral("Server=");
                            interpolatedStringHandler3.AppendFormatted(option.Host);
                            ref var local2 = ref interpolatedStringHandler3;
                            var port2 = option.Port;
                            string str2;
                            if (!port2.HasValue || port2.GetValueOrDefault() <= 0)
                            {
                                str2 = "";
                            }
                            else
                            {
                                var interpolatedStringHandler4 = new DefaultInterpolatedStringHandler(1, 1);
                                interpolatedStringHandler4.AppendLiteral(",");
                                interpolatedStringHandler4.AppendFormatted(option.Port);
                                str2 = interpolatedStringHandler4.ToStringAndClear();
                            }
                            local2.AppendFormatted(str2);
                            interpolatedStringHandler3.AppendLiteral(";Database=");
                            interpolatedStringHandler3.AppendFormatted(option.DbName);
                            interpolatedStringHandler3.AppendLiteral(";User ID=");
                            interpolatedStringHandler3.AppendFormatted(option.UserName);
                            interpolatedStringHandler3.AppendLiteral(";Password=");
                            interpolatedStringHandler3.AppendFormatted(option.Password);
                            interpolatedStringHandler3.AppendLiteral(";");
                            var stringAndClear2 = interpolatedStringHandler3.ToStringAndClear();
                            optionsBuilder2.UseNpgsql(stringAndClear2, (Action<NpgsqlDbContextOptionsBuilder>)(x => x.CommandTimeout(int.MaxValue)));
                            break;
                        default:
                            throw new NotFoundException("DatabaseType not found");
                    }
                }
                else
                {
                    switch (option.DatabaseType)
                    {
                        case DatabaseType.MsSql:
                            o.UseSqlServer(option.ConnectionString, (Action<SqlServerDbContextOptionsBuilder>)(x => x.CommandTimeout(int.MaxValue)));
                            break;
                        case DatabaseType.PostgreSql:
                            o.UseNpgsql(option.ConnectionString, (Action<NpgsqlDbContextOptionsBuilder>)(x => x.CommandTimeout(int.MaxValue)));
                            break;
                        default:
                            throw new NotFoundException("DatabaseType not found");
                    }
                }
            }), lifetime);
            Console.WriteLine(option.DatabaseType.ToString() + " Database Service is installed");
        }

        public static void DbMigrate<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            var service = app.ApplicationServices.GetService<TContext>();

            service?.Database.Migrate();

            Console.WriteLine("Connection String :" + service?.Database.GetConnectionString());
            Console.WriteLine("Database Migrated!!");
        }
    }
}
