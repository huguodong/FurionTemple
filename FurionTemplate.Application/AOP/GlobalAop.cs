using Furion;
using Furion.Reflection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FurionTemplate.Application.AOP
{
    public class GlobalAop : AspectDispatchProxy, IGlobalDispatchProxy
    {
        public object Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IServiceProvider Services { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
