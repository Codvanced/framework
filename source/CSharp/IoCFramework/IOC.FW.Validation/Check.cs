using System;
using System.Collections;
using System.Linq.Expressions;

namespace IOC.FW.Validation
{
    public class Check
    {
        private static string GetName(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        private static string GetName<TComparison>(Expression<Func<TComparison>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static bool Comparison<TComparison>(
            Expression<Func<TComparison>> exp,
            Func<TComparison, bool> predicate,
            bool throwIfFail = true,
            Exception exceptionType = null
        )
        {
            var fail = true;
            var value = default(TComparison);

            var compiled = exp != null
                ? exp.Compile()
                : null;

            if (compiled != null)
            {
                value = compiled();
                fail = predicate(value);
            }

            if (throwIfFail && fail)
            {
                string argumentName = GetName(exp);
                if (exceptionType == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
                else
                {
                    exceptionType.Data.Add(argumentName, value);
                    throw exceptionType;
                }
            }

            return fail;
        }

        public static bool Comparison(
            Expression<Func<object>> exp,
            Func<object, bool> predicate,
            bool throwIfFail = true,
            Exception exceptionType = null
        )
        {
            return Comparison<object>(
                exp,
                predicate,
                throwIfFail,
                exceptionType
            );
        }

        public static bool IfNull(Expression<Func<object>> exp, bool throwIfFail = true)
        {
            return Comparison(
                exp,
                p => p == null,
                throwIfFail
            );
        }

        public static bool IfNull(Expression<Func<object>>[] exp, bool throwIfFail = true)
        {
            var fail = false;

            for (var i = 0; i < exp.Length && !fail; i++)
            {
                var expression = exp[i];
                fail = IfNull(expression, throwIfFail);
            }

            return fail;
        }

        public static bool IfNullOrEmpty(Expression<Func<string>> exp, bool throwIfFail = true)
        {
            return Comparison(
                exp,
                p => string.IsNullOrEmpty(p),
                throwIfFail
            );
        }

        public static bool IfNullOrEmpty(Expression<Func<IEnumerable>> exp, bool throwIfFail = true)
        {
            return Comparison(
                exp,
                p => p == null || !p.GetEnumerator().MoveNext(),
                throwIfFail
            );
        }

        public static bool IfNullOrEmpty(Expression<Func<string>>[] exp, bool throwIfFail = true)
        {
            var fail = false;

            for (var i = 0; i < exp.Length && !fail; i++)
            {
                var expression = exp[i];
                fail = IfNullOrEmpty(expression, throwIfFail);
            }

            return fail;
        }

        public static bool IfNullOrEmpty(Expression<Func<IEnumerable>>[] exp, bool throwIfFail = true)
        {
            var fail = false;

            for (var i = 0; i < exp.Length && !fail; i++)
            {
                var expression = exp[i];
                fail = IfNullOrEmpty(expression, throwIfFail);
            }

            return fail;
        }

        public static bool If<TException>(
            Expression<Func<object>> exp,
            Func<object, bool> testCase,
            bool throwIfFail = true
        )
        where TException :
            Exception, new()
        {
            return Comparison(
                exp,
                p => testCase != null
                    && testCase(p),
                throwIfFail,
                new TException()
            );
        }

        public static bool If<TException, TComparison>(
            Expression<Func<TComparison>> exp,
            Func<TComparison, bool> testCase,
            bool throwIfFail = true
        )
        where TException : Exception, new()
        {
            return Comparison(
                exp,
                p => testCase != null
                    && testCase(p),
                throwIfFail,
                new TException()
            );
        }
    }
}
