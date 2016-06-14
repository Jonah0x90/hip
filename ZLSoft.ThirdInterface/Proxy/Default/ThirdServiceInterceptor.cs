using DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLSoft.Cache;
using ZLSoft.Pub;

namespace ZLSoft.ThirdInterface.Proxy.Default
{
    public class ThirdServiceInterceptor : IInterceptor
    {
        private IThirdService _target;
        public ThirdServiceInterceptor(IThirdService target)
        {
            this._target = target;
        }
        public object Intercept(InvocationInfo info)
        {
            //Console.WriteLine(">>Intercepting " + info.TargetMethod.Name);
            PropertyInfo p = this._target.GetType().GetProperty("ID");
            string s = p.GetValue(this._target, null).ToString();

            string key = s + "(";
            if (info.Arguments != null && info.Arguments.Length > 0)
            {
                StrObjectDict sod = null;
                for (int i = 0; i < info.Arguments.Length; i++)
                {
                    if (i != 0) key += ", ";
                    sod =  info.Arguments[i] as StrObjectDict;

                    if(sod == null || sod.Count() == 0){
                        key += "null";
                    }
                    else
                    {
                        foreach (var item in sod.Keys)
                        {
                            key += sod[item] + ",";
                        }
                    }
                }
            }
            key += ")";
            object obj = CacheIO.Get(key);

            return obj == null ? Fetch(key, info) : obj;
        }

        private object Fetch(string key,InvocationInfo info)
        {
            object obj = info.TargetMethod.Invoke(this._target, info.Arguments);
            return obj;
        }
    }
}
