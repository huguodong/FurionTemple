using Furion;
using Furion.Reflection;
using System.Reflection;
using System.Threading.Tasks;

namespace FurionTemplate.Application.AOP
{
    public class GlobalAop : AspectDispatchProxy, IGlobalDispatchProxy
    {
        public override object Invoke(MethodInfo method, object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override Task InvokeAsync(MethodInfo method, object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}
