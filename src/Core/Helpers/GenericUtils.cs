using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TeaBagBot.Helpers
{
    public static class GenericUtils
    {
        public delegate object ConstructorDelegate(params object[] args);

        public static ConstructorDelegate CreateConstructor<T>(params Type[] parameters)
        {
            var constructorInfo = typeof(T).GetConstructor(parameters);

            var paramExpr = Expression.Parameter(typeof(object[]));

            var constructorParameters = parameters.Select((paramType, index) =>
                Expression.Convert(
                    Expression.ArrayAccess(
                        paramExpr,
                        Expression.Constant(index)),
                    paramType)).ToArray();

            var body = Expression.New(constructorInfo, constructorParameters);

            var constructor = Expression.Lambda<ConstructorDelegate>(body, paramExpr);
            return constructor.Compile();
        }
    }
}
