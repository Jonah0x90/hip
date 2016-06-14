using DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.ThirdInterface.Proxy.Default
{
    public class ThirdServiceWrapper : IInvokeWrapper
    {
        private IThirdService _target;
        public ThirdServiceWrapper(IThirdService target)
        {
            this._target = target;
        }
        public void BeforeInvoke(InvocationInfo info)
        {
            Console.WriteLine(">>Intercepting " + info.TargetMethod.Name);
        }
        public object DoInvoke(InvocationInfo info)
        {
            return info.TargetMethod.Invoke(this._target, info.Arguments);
        }
        public void AfterInvoke(InvocationInfo info, object returnValue)
        {
            Console.WriteLine("<<Intercepted " + info.TargetMethod.Name + ", result: " + (returnValue == null ? "null" : returnValue.ToString()));
        }
    }

}
