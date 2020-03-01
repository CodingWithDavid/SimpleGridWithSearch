/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InternalLib
{
    public static class RecordSorterHelper
    {
        public static IOrderedQueryable<T> PageResult<T>(IQueryable<T> source, int page, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = 25;
            }
            int skipNumber = pageSize * (page - 1);
            var result = source.Skip(skipNumber).Take(pageSize);
            return (IOrderedQueryable<T>)result;
        }

        public static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi == null)
                {
                    throw new ArgumentException("Property Not Found: " + prop + " on " + type.FullName);
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        private static IOrderedQueryable<T> OrderBy<T>(IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        private static IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        private static IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        private static IOrderedQueryable<T> ThenByDescending<T>(IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }
    }
}
