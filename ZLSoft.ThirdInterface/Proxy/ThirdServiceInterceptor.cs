
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Cache;

namespace ZLSoft.ThirdInterface.Proxy
{
    public class ThirdServiceInterceptor  // : IInterceptor
    {
        //private int _indent = 0;
        //private object HitObject(IInvocation invocation)
        //{
        //    //if (this._indent > 0)
        //    //    Console.Write(" ".PadRight(this._indent * 4, ' '));
        //    //this._indent++;
        //    string key = invocation.Method.Name + "(";
        //    if (invocation.Arguments != null && invocation.Arguments.Length > 0)
        //        for (int i = 0; i < invocation.Arguments.Length; i++)
        //        {
        //            if (i != 0) Console.Write(", ");
        //            key+=invocation.Arguments[i] == null
        //                ? "null"
        //                : invocation.Arguments[i].GetType() == typeof(string)
        //                   ? "\"" + invocation.Arguments[i].ToString() + "\""
        //                   : invocation.Arguments[i].ToString();
        //        }
        //    key+=")";
        //    return CacheIO.Get(key);
        //}
        //private void PostProceed(IInvocation invocation)
        //{
        //    //this._indent--;
        //}
        //public void Intercept(IInvocation invocation)
        //{
        //    object obj = this.HitObject(invocation);
        //    if(obj != null){
        //        invocation.ReturnValue = obj;
        //    }
        //    else
        //    {
        //        invocation.Proceed();
        //    }
        //    this.PostProceed(invocation);
        //}
    }
}
