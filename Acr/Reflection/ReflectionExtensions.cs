using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Acr.Reflection
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyInfo<TSender, TRet>(this TSender sender, Expression<Func<TSender, TRet>> expression)
        {
            var lambda = expression as LambdaExpression;
            var member = lambda.Body as MemberExpression;
            var property = sender.GetType().GetRuntimeProperty(member.Member.Name);
            return property;
        }


        public static Type UnwrapType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return type;
        }
    }
}
