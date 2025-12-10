using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var combined = new ReplaceParameterVisitor();
            combined.Add(expr1.Parameters[0], parameter);
            combined.Add(expr2.Parameters[0], parameter);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    combined.Visit(expr1.Body),
                    combined.Visit(expr2.Body)),
                parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map = new();

            public void Add(ParameterExpression from, ParameterExpression to)
                => _map[from] = to;

            protected override Expression VisitParameter(ParameterExpression node) =>
                _map.TryGetValue(node, out var replacement) ? replacement : node;
        }

    }

}
