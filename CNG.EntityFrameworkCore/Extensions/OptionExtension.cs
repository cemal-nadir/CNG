using System.Runtime.CompilerServices;
using CNG.Core.Exceptions;
using CNG.EntityFrameworkCore.Enums;
using CNG.EntityFrameworkCore.Models;

namespace CNG.EntityFrameworkCore.Extensions
{
    static class OptionExtension
    {
        internal static string GetConnectionString(this DatabaseOption option)
        {
            if (!string.IsNullOrEmpty(option.ConnectionString)) return option.ConnectionString;
            switch (option.DatabaseType)
            {
                case DatabaseType.MsSql:
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
                    return interpolatedStringHandler1.ToStringAndClear();
                case DatabaseType.PostgreSql:
                    var interpolatedStringHandler3 = new DefaultInterpolatedStringHandler(37, 5);
                    interpolatedStringHandler3.AppendLiteral("Server=");
                    interpolatedStringHandler3.AppendFormatted(option.Host);
                    interpolatedStringHandler3.AppendLiteral(";Database=");
                    interpolatedStringHandler3.AppendFormatted(option.DbName);
                    interpolatedStringHandler3.AppendLiteral(";User ID=");
                    interpolatedStringHandler3.AppendFormatted(option.UserName);
                    interpolatedStringHandler3.AppendLiteral(";Password=");
                    interpolatedStringHandler3.AppendFormatted(option.Password);
                    interpolatedStringHandler3.AppendLiteral(";Port=");
                    interpolatedStringHandler3.AppendFormatted(option.Port);
                    interpolatedStringHandler3.AppendLiteral(";Include Error Detail=true;");
                    return interpolatedStringHandler3.ToStringAndClear();
                default:
                    throw new NotFoundException("DatabaseType not found");
            }
        }
    }
}
