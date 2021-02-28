using Furion;
using Furion.Reflection;
using FurionTemplate.Common.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace FurionTemplate.Application.AOP
{
    public class RedisAop : AspectDispatchProxy, IDispatchProxy
    {
        public object Target { get; set; }
        public IServiceProvider Services { get; set; }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Invoke(MethodInfo method, object[] args)
        {
            if (method.ReturnType == typeof(void))
            {
                return method.Invoke(Target, args);
            }
            else
            {
                var result = method.Invoke(Target, args);
                RecordRedis(method, result, args);
                return result;
            }
        }

        /// <summary>
        /// 异步无返回值
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            var task = method.Invoke(Target, args) as Task;
            RecordRedis(method, task, args);
            await task;
        }

        /// <summary>
        /// 异步带返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            var taskT = method.Invoke(Target, args) as Task<T>;
            var result = await taskT;
            RecordRedis(method, result, args);
            return result;
        }


        /// <summary>
        /// 添加到redis
        /// </summary>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        /// <param name="args"></param>
        private void RecordRedis(MethodInfo method, object returnValue, object[] args)
        {
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) is CachingAttribute qCachingAttribute)
            {
                var redis = App.GetService<IRedisCacheManager>();
                //获取自定义缓存键
                var cacheKey = qCachingAttribute.CustomKeyValue ?? CustomCacheKey(method, args);
                //注意是 string 类型，方法GetValue
                var cacheValue = redis.Get<string>(cacheKey);
            }
        }
        protected string CustomCacheKey(MethodInfo method, object[] args)
        {
            var typeName = Target.GetType().Name;
            var methodName = method.Name;
            var a = Target.GetType().GetGenericArguments();
            var methodArguments = args.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}:";
            }

            return key.TrimEnd(':');
        }

        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected static string GetArgumentValue(object arg)
        {
            if (arg is DateTime || arg is DateTime?)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is string || arg is ValueType || arg is Nullable)
                return arg.ToString();

            if (arg != null)
            {
                if (arg is Expression)
                {
                    var obj = arg as Expression;
                    var result = Resolve(obj);
                    return Common.Helper.MD5Helper.MD5Encrypt16(result);
                }
                else if (arg.GetType().IsClass)
                {
                    return Common.Helper.MD5Helper.MD5Encrypt16(Newtonsoft.Json.JsonConvert.SerializeObject(arg));
                }
            }
            return string.Empty;
        }

        private static string Resolve(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                LambdaExpression lambda = expression as LambdaExpression;
                expression = lambda.Body;
                return Resolve(expression);
            }
            if (expression is BinaryExpression)
            {
                BinaryExpression binary = expression as BinaryExpression;
                if (binary.Left is MemberExpression && binary.Right is ConstantExpression)//解析x=>x.Name=="123" x.Age==123这类
                    return ResolveFunc(binary.Left, binary.Right, binary.NodeType);
                if (binary.Left is MethodCallExpression && binary.Right is ConstantExpression)//解析x=>x.Name.Contains("xxx")==false这类的
                {
                    object value = (binary.Right as ConstantExpression).Value;
                    return ResolveLinqToObject(binary.Left, value, binary.NodeType);
                }
                if ((binary.Left is MemberExpression && binary.Right is MemberExpression)
                    || (binary.Left is MemberExpression && binary.Right is UnaryExpression))//解析x=>x.Date==DateTime.Now这种
                {
                    LambdaExpression lambda = Expression.Lambda(binary.Right);
                    Delegate fn = lambda.Compile();
                    ConstantExpression value = Expression.Constant(fn.DynamicInvoke(null), binary.Right.Type);
                    return ResolveFunc(binary.Left, value, binary.NodeType);
                }
            }
            if (expression is UnaryExpression)
            {
                UnaryExpression unary = expression as UnaryExpression;
                if (unary.Operand is MethodCallExpression)//解析!x=>x.Name.Contains("xxx")或!array.Contains(x.Name)这类
                    return ResolveLinqToObject(unary.Operand, false);
                if (unary.Operand is MemberExpression && unary.NodeType == ExpressionType.Not)//解析x=>!x.isDeletion这样的 
                {
                    ConstantExpression constant = Expression.Constant(false);
                    return ResolveFunc(unary.Operand, constant, ExpressionType.Equal);
                }
            }
            if (expression is MemberExpression && expression.NodeType == ExpressionType.MemberAccess)//解析x=>x.isDeletion这样的 
            {
                MemberExpression member = expression as MemberExpression;
                ConstantExpression constant = Expression.Constant(true);
                return ResolveFunc(member, constant, ExpressionType.Equal);
            }
            if (expression is MethodCallExpression)//x=>x.Name.Contains("xxx")或array.Contains(x.Name)这类
            {
                MethodCallExpression methodcall = expression as MethodCallExpression;
                return ResolveLinqToObject(methodcall, true);
            }
            //已经修改过代码body应该不会是null值了
            if (!(expression is BinaryExpression body))
                return string.Empty;
            var Operator = GetOperator(body.NodeType);
            var Left = Resolve(body.Left);
            var Right = Resolve(body.Right);
            string Result = string.Format("({0} {1} {2})", Left, Operator, Right);
            return Result;
        }

        private static string ResolveFunc(Expression left, Expression right, ExpressionType expressiontype)
        {
            var Name = (left as MemberExpression).Member.Name;
            var Value = (right as ConstantExpression).Value;
            var Operator = GetOperator(expressiontype);
            return Name + Operator + Value ?? "null";
        }

        private static string ResolveLinqToObject(Expression expression, object value, ExpressionType? expressiontype = null)
        {
            var MethodCall = expression as MethodCallExpression;
            var MethodName = MethodCall.Method.Name;
            switch (MethodName)
            {
                case "Contains":
                    if (MethodCall.Object != null)
                        return Like(MethodCall);
                    return In(MethodCall, value);
                case "Count":
                    return Len(MethodCall, value, expressiontype.Value);
                case "LongCount":
                    return Len(MethodCall, value, expressiontype.Value);
                default:
                    throw new Exception(string.Format("不支持{0}方法的查找！", MethodName));
            }
        }

        private static string GetOperator(ExpressionType expressiontype)
        {
            return expressiontype switch
            {
                ExpressionType.And => "and",
                ExpressionType.AndAlso => "and",
                ExpressionType.Or => "or",
                ExpressionType.OrElse => "or",
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                _ => throw new Exception(string.Format("不支持{0}此种运算符查找！" + expressiontype)),
            };
        }

        private static string In(MethodCallExpression expression, object isTrue)
        {
            var Argument1 = (expression.Arguments[0] as MemberExpression).Expression as ConstantExpression;
            var Argument2 = expression.Arguments[1] as MemberExpression;
            var Field_Array = Argument1.Value.GetType().GetFields().First();
            object[] Array = Field_Array.GetValue(Argument1.Value) as object[];
            List<string> SetInPara = new List<string>();
            for (int i = 0; i < Array.Length; i++)
            {

                string Value = Array[i].ToString();
                SetInPara.Add(Value);
            }
            string Name = Argument2.Member.Name;
            string Operator = Convert.ToBoolean(isTrue) ? "in" : " not in";
            string CompName = string.Join(",", SetInPara);
            string Result = string.Format("{0} {1} ({2})", Name, Operator, CompName);
            return Result;
        }
        private static string Like(MethodCallExpression expression)
        {

            var Temp = expression.Arguments[0];
            LambdaExpression lambda = Expression.Lambda(Temp);
            Delegate fn = lambda.Compile();
            var tempValue = Expression.Constant(fn.DynamicInvoke(null), Temp.Type);
            string Value = string.Format("%{0}%", tempValue);
            string Name = (expression.Object as MemberExpression).Member.Name;
            string Result = string.Format("{0} like {1}", Name, Value);
            return Result;
        }


        private static string Len(MethodCallExpression expression, object value, ExpressionType expressiontype)
        {
            object Name = (expression.Arguments[0] as MemberExpression).Member.Name;
            string Operator = GetOperator(expressiontype);
            string Result = string.Format("len({0}){1}{2}", Name, Operator, value.ToString());
            return Result;
        }
    }
    /// <summary>
    /// 这个Attribute就是使用时候的验证，把它添加到要缓存数据的方法中，即可完成缓存的操作。
    /// </summary>
    //[AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 过期时间单位:分钟
        /// </summary>
        public int AbsoluteExpiration { get; set; }
        /// <summary>
        /// 自定义KEY
        /// </summary>
        public string CustomKeyValue { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; } = false;
    }

}
