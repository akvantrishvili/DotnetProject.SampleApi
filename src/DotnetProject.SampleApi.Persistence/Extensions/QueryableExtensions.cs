using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotnetProject.SampleApi.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace DotnetProject.SampleApi.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly ConcurrentDictionary<(Type Type, string Property), (Type PropertyType, LambdaExpression Lambda)> s_orderByExpressions = new();
        private static readonly ConcurrentDictionary<(Type Type, Type PropertyType, string MethodName), MethodInfo> s_orderByMethods = new();
        public static IQueryable<TEntity> ApplyIncludes<TEntity>(this IQueryable<TEntity> source, string[]? relatedProperties)
            where TEntity : class
        {
            if (relatedProperties is not { Length: > 0 })
                return source;

            var finalList = FlattenRelatedProperties(relatedProperties);
            foreach (var prop in finalList)
            {
                source = source.Include(prop);
            }

            return source;
        }
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            SortingDetails sortingDetails)
        {
            if (sortingDetails?.SortItems == null || sortingDetails.SortItems.Count == 0)
                return source.OrderBy(x => 1);

            IOrderedQueryable<T> result = null;
            var sorted = false;
            foreach (var sortBy in sortingDetails.SortItems)
            {
                if (sortBy == null)
                    continue;
                if (!string.IsNullOrWhiteSpace(sortBy.SortBy))
                {
                    if (sorted)
                    {
                        result = sortBy.SortDirection == SortDirection.Asc
                            ? result.ThenBy(sortBy.SortBy)
                            : result.ThenByDescending(sortBy.SortBy);
                    }
                    else
                    {
                        result = sortBy.SortDirection == SortDirection.Asc
                            ? source.OrderBy(sortBy.SortBy)
                            : source.OrderByDescending(sortBy.SortBy);
                        sorted = true;
                    }
                }
            }

            return result ?? (source.OrderBy(x => 1));
        }
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                return source.OrderBy(x => 1);

            return ApplyOrder(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(
            this IQueryable<T> source,
            string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                return source.OrderBy(x => 1);

            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IEnumerable<string> FlattenRelatedProperties(IEnumerable<string> props)
        {
            var finalList = new List<string>();
            foreach (var prop in props.OrderByDescending(x => x))
            {
                if (!finalList.Exists(x => x.StartsWith($"{prop}.", StringComparison.OrdinalIgnoreCase)))
                    finalList.Add(prop);
            }
            return finalList;
        }
        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                return source;

            return ApplyOrder(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                return source;

            return ApplyOrder(source, property, "ThenByDescending");
        }
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var type = typeof(T);

            property = property.Trim();

            var (propertyType, lambda) = GetOrderByExpression(type, property);

            var orderByMethod = GetOrderByMethod(type, propertyType, methodName);

            var result = orderByMethod.Invoke(null, [source, lambda]);

            return result as IOrderedQueryable<T> ?? throw new InvalidOperationException();
        }
        private static (Type PropertyType, LambdaExpression Lambda) GetOrderByExpression(Type type, string property)
        {
            var propertyType = type;
            if (!s_orderByExpressions.TryGetValue((type, property), out var propertyTypeAndLambda))
            {
                var props = property.Split('.');
                var arg = Expression.Parameter(type, "x");
                Expression expr = arg;
                foreach (var prop in props)
                {
                    var propertyInfo = propertyType.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (propertyInfo == null)
                        return (null, null)!;

                    expr = Expression.Property(expr, propertyInfo);
                    propertyType = propertyInfo.PropertyType;
                }
                var delegateType = typeof(Func<,>).MakeGenericType(type, propertyType);
                propertyTypeAndLambda = (propertyType, Expression.Lambda(delegateType, expr, arg));

                s_orderByExpressions.AddOrUpdate((type, property), propertyTypeAndLambda, (tuple, lambdaExpression) => propertyTypeAndLambda);
            }

            return propertyTypeAndLambda;
        }
        private static MethodInfo GetOrderByMethod(Type type, Type propertyType, string methodName)
        {
            if (!s_orderByMethods.TryGetValue((type, propertyType, methodName), out var methodInfo))
            {
                methodInfo = typeof(Queryable).GetMethods().Single(
                        method => string.Equals(method.Name, methodName, StringComparison.Ordinal)
                                  && method.IsGenericMethodDefinition
                                  && method.GetGenericArguments().Length == 2
                                  && method.GetParameters().Length == 2)
                    .MakeGenericMethod(type, propertyType);

                s_orderByMethods.AddOrUpdate((type, propertyType, methodName), methodInfo, (tuple, info) => methodInfo);
            }
            return methodInfo;
        }
    }
}
