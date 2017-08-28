using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IOC.FW.Expressions
{
    public class ExpressionAppender<TModel>
    {
        private readonly List<Expression<Func<TModel, object>>> _expressions;
        private object _addLock = new object();
        private object _evaluateLock = new object();

        public ExpressionAppender()
        {
            _expressions = new List<Expression<Func<TModel, object>>>();
        }

        public ExpressionAppender<TModel> Add(Expression<Func<TModel, object>> exp)
        {
            lock (_addLock)
                _expressions.Add(exp);

            return this;
        }

        public Expression<Func<TModel, object>>[] Evaluate()
        {
            var output = default(Expression<Func<TModel, object>>[]);

            lock (_evaluateLock)
            {
                output = _expressions.ToArray();
                _expressions.Clear();
            }

            return output;
        }
    }
}
