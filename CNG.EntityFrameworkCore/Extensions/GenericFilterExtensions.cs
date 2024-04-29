using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using CNG.Abstractions.Models;

namespace CNG.EntityFrameworkCore.Extensions;

public static class GenericFilterExtensions<T>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!;
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!;
    public static Expression<Func<T, bool>>? Create(SearchFilter searchFilter)
    {
        Expression? filter = null;
        var efObject = Expression.Parameter(typeof(T), "t");
        foreach (var searchValue in searchFilter.SearchValues)
        {
            var index = 0;
            foreach (var value in searchValue.FieldValues)
            {

                Expression? binaryExpression = null;


                var memberExpression = searchValue.FieldName.Contains(".")
                        ? Expression.PropertyOrField(
                            Expression.PropertyOrField(efObject, searchValue.FieldName.Split('.')[0]),
                            searchValue.FieldName.Split('.')[1])
                        : Expression.PropertyOrField(efObject, searchValue.FieldName);

                var propertyType = ((PropertyInfo)memberExpression.Member).PropertyType;
                var converter = TypeDescriptor.GetConverter(propertyType);

                var propertyValue = converter.ConvertFromInvariantString(value);
                var constant = Expression.Constant(propertyValue);
                var constantExpression = Expression.Convert(constant, propertyType);

                binaryExpression = searchValue.Operation switch
                {
                    FilterOperations.Equals => Expression.Equal(memberExpression, constantExpression),
                    FilterOperations.GreaterThan => Expression.GreaterThan(memberExpression, constantExpression),
                    FilterOperations.GreaterThanOrEqual => Expression.GreaterThanOrEqual(memberExpression,
                        constantExpression),
                    FilterOperations.LessThan => Expression.LessThan(memberExpression, constantExpression),
                    FilterOperations.LessThanOrEqual => Expression.LessThanOrEqual(memberExpression,
                        constantExpression),
                    FilterOperations.Contains => Expression.Call(memberExpression, containsMethod,
                        constantExpression),
                    FilterOperations.StartsWith => Expression.Call(memberExpression, startsWithMethod,
                        constantExpression),
                    FilterOperations.EndsWith => Expression.Call(memberExpression, endsWithMethod,
                        constantExpression),
                    _ => binaryExpression
                };
                if ((searchValue.FieldValues.Count > 1) && (index > 0))
                {
                    if (binaryExpression != null)
                        filter = (filter != null) ? Expression.Or(filter, binaryExpression) : binaryExpression;
                }
                else
                {
                    if (binaryExpression != null)
                        filter = (filter != null) ? Expression.And(filter, binaryExpression) : binaryExpression;
                }
                index += 1;
            }

        }

        return filter == null ? null : Expression.Lambda<Func<T, bool>>(filter, efObject);
    }
#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
    public static IQueryable<T> OrderByDynamicEX<T>(IQueryable<T> source, string orderByProperty, bool isDescending)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
    {
        var type = typeof(T);
        var parameter = Expression.Parameter(type, "x");
        MemberExpression? propertyAccess;
        MethodCallExpression? orderByCall;
        if (orderByProperty.Contains("."))
        {
            var baseObject = type.GetProperty(orderByProperty.Split('.')[0]);
            propertyAccess = Expression.PropertyOrField(
                Expression.PropertyOrField(parameter, orderByProperty.Split('.')[0]),
                orderByProperty.Split('.')[1]);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";
            orderByCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { type, baseObject?.PropertyType??throw new InvalidOperationException()},
                source.Expression,
                Expression.Quote(orderByExp));


            return source.Provider.CreateQuery<T>(orderByCall);
        }
        else
        {
            var property = type.GetProperty(orderByProperty);
            propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException());
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";
            orderByCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { type, property.PropertyType },
                source.Expression,
                Expression.Quote(orderByExp));
        }


        return source.Provider.CreateQuery<T>(orderByCall);
    }
#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
    public static IQueryable<T> OrderByDynamic<T>(IQueryable<T> source, string orderByProperty, bool isDescending)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
    {
        var type = typeof(T);
        PropertyInfo? property;
        MemberExpression? propertyAccess;
        var parameter = Expression.Parameter(type, "x");
        if (orderByProperty.Contains("."))
        {
            var baseObject = type.GetProperty(orderByProperty.Split('.')[0]);
            property = baseObject?.PropertyType.GetProperty(orderByProperty.Split('.')[1]);
            propertyAccess = Expression.PropertyOrField(
                Expression.PropertyOrField(parameter, orderByProperty.Split('.')[0]),
                orderByProperty.Split('.')[1]);
        }
        else
        {
            property = type.GetProperty(orderByProperty);
            propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException());
        }

        if (property == null)
            throw new ArgumentException($"Property {orderByProperty} doesn't exist in type {type.Name}");

        var orderByExp = Expression.Lambda(propertyAccess, parameter);
        var methodName = isDescending ? "OrderByDescending" : "OrderBy";
        var orderByCall = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { type, property.PropertyType },
            source.Expression,
            Expression.Quote(orderByExp));

        return source.Provider.CreateQuery<T>(orderByCall);
    }
}