using System.Linq.Expressions;

namespace WebApi.Extensions;

public static class PredicateExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters[0]);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> AndIfNotDefault<T, K>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, K target)
        where K : class
    {
        return AndIf(expr1, expr2, () => target != null);
    }

    public static Expression<Func<T, bool>> AndIfNotDefault<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, Guid? target)
    {
        return AndIf(expr1, expr2, () => target.HasValue && target.Value != default);
    }

    public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, bool target)
    {
        return AndIf(expr1, expr2, () => target);
    }

    public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, Func<bool> condition)
    {
        var invoked = condition.Invoke(); 
        return invoked ?
            And(expr1, expr2)
            : expr1;
    }
}
