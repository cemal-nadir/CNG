using CNG.Core.Exceptions;
using CNG.EntityFrameworkCore.Enums;
using CNG.EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CNG.EntityFrameworkCore.Extensions
{
    public static class MyDbContextDesignTimeFactoryExtensions
    {
        public static TContext CreateDbContext<TContext>(string[] args,DatabaseOption option)
            where TContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            var connectionString = option.GetConnectionString();
            var migrationAssemblyName = typeof(TContext).Assembly.GetName().Name;
            var migrationNamespace = typeof(TContext).Namespace + ".Migrations";
			switch (option.DatabaseType)
            {
                case DatabaseType.MsSql:
                    builder.UseSqlServer(connectionString, options =>
                    {
	                    options.MigrationsAssembly(migrationAssemblyName);
	                    options.MigrationsHistoryTable("__EFMigrationsHistory", migrationNamespace);
                    });
                    break;
                case DatabaseType.PostgreSql:
                    builder.UseNpgsql(connectionString, options =>
                    {
	                    options.MigrationsAssembly(migrationAssemblyName);
	                    options.MigrationsHistoryTable("__EFMigrationsHistory", migrationNamespace);
                    });
                    break;
                default:
                    throw new NotFoundException("DatabaseType not found");
            }
        
            var response = (TContext?)Activator.CreateInstance(
                typeof(TContext),
                builder.Options);
            return response ?? throw new Exception("Context is null");
        }

       
    }
}
