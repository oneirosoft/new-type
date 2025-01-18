using System.Linq.Expressions;
using System.Reflection;

namespace Oneiro;

internal static class ActivatorUtil {
    public static Func<T, TNewType> CreateInstanceFactory<T, TNewType>()
        where TNewType : NewType<T, TNewType>
        where T : struct 
    {
        var constructor = typeof(TNewType)
            .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, [typeof(T)],
                null);
        if (constructor is null) {
            throw new InvalidOperationException(
                $"Type {typeof(TNewType)} does not have a constructor with a single parameter of type {typeof(T)}.");
        }

        var parameter = Expression.Parameter(typeof(T), "value");
        var newExpression = Expression.New(constructor, parameter);
        var lambda = Expression.Lambda<Func<T, TNewType>>(newExpression, parameter);
        return lambda.Compile();
    }
}