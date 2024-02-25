using System.Linq.Expressions;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName)
    {
        return ApplyOrder<T>(source, propertyName, "OrderBy");
    }

    public static IQueryable<T> OrderByPropertyDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return ApplyOrder<T>(source, propertyName, "OrderByDescending");
    }

    private static IQueryable<T> ApplyOrder<T>(IQueryable<T> source, string propertyName, string methodName)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        
        if (string.IsNullOrEmpty(propertyName))
            return source;

        var type = typeof(T);
        var parameter = Expression.Parameter(type, "x");
        var property = type.GetProperty(propertyName);

        if (property == null)
            throw new ArgumentException($"Property '{propertyName}' not found on type '{type}'.");

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        var typeArgs = new Type[] { type, property.PropertyType };
        var methodNameArg = methodName == "OrderBy" ? "OrderBy" : "OrderByDescending";
        var method = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodNameArg && m.IsGenericMethodDefinition)
            .Where(m => {
                var parameters = m.GetParameters().ToList();
                return parameters.Count == 2
                    && parameters[0].ParameterType.IsGenericType
                    && parameters[1].ParameterType.IsGenericType;
            })
            .Single()
            .MakeGenericMethod(typeArgs);

        return (IQueryable<T>)method.Invoke(null, new object[] { source, orderByExp });
    }
}