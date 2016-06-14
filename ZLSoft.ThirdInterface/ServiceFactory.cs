
using DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLSoft.Pub;
using ZLSoft.ThirdInterface.Proxy.Default;
using ZLSoft.ThirdInterface.Services;

namespace ZLSoft.ThirdInterface
{
    public class ServiceFactory
    {
        public static IThirdService CreateService(string id,StrObjectDict obj)
        {
            string type = obj["Type"].ToString();

            //1：ws，2：socket，3：存储过程d
            if(type == "1"){
                return Create<WSSvrs>(id, obj);
            }else if(type == "2"){
                return Create<SocketSvrs>(id, obj);
            }
            else if(type == "3")
            {
                return Create<ProcSvrs>(id, obj);
            }else if(type == "4"){
                return Create<SqlSvrs>(id, obj);
            }
            return null;
        }

        public static IThirdService CreateSoaService(string id, StrObjectDict obj)
        {
            string type = obj["Type"].ToString();

            //1：ws，2：socket，3：存储过程.4:sql数据源
            if (type == "1")
            {
                return CreateProxy<SOAWS>(id, obj);
            }
            else if (type == "2")
            {
                return CreateProxy<SOASocket>(id, obj);
            }
            else if(type == "3")
            {
                return CreateProxy<SOAProc>(id, obj);
            }else if(type == "4"){
                return null;
                //return CreateProxy<SOASql>(id, obj);
            }
            else
            {
                return null;
            }
        }

        //private static IThirdService CreateCastleProxy<T>() where T : IThirdService, new()
        //{
        //    ProxyGenerator generator = new ProxyGenerator();
        //    ThirdServiceInterceptor interceptor = new ThirdServiceInterceptor();
        //    T t = System.Activator.CreateInstance<T>();
        //    //IThirdService iservice = generator.CreateInterfaceProxyWithTargetInterface<IThirdService>(t, interceptor);
        //    IThirdService iservice = generator.CreateInterfaceProxyWithTarget<IThirdService>(t,interceptor);
        //    return iservice;
        //}

        private static IThirdService CreateProxy<T>(string id, StrObjectDict otherProperties) where T : IThirdService, new()
        {
            ProxyFactory factory = new ProxyFactory();
            T t = System.Activator.CreateInstance<T>();
            PropertyInfo p = t.GetType().GetProperty("OtherProperties");
            p.SetValue(t, otherProperties, null);
            p = t.GetType().GetProperty("ID");
            p.SetValue(t, id, null);
            ThirdServiceInterceptor interceptor = new ThirdServiceInterceptor(t);
            IThirdService proxy1 = factory.CreateProxy<IThirdService>(interceptor);
            return proxy1;
        }

        private static IThirdService Create<T>(string id, StrObjectDict otherProperties) where T : IThirdService, new()
        {
            T t = System.Activator.CreateInstance<T>();
            PropertyInfo p = t.GetType().GetProperty("OtherProperties");
            p.SetValue(t, otherProperties, null);
            p = t.GetType().GetProperty("ID");
            p.SetValue(t, id, null);
            return t;
        }
    }
}
