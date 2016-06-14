using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLSoft.DalManager;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Enum;

namespace ZLSoft.ThirdInterface
{
    public class ThirdServiceContext
    {
        private static DeployMode _mode = DeployMode.MODE_NORMAL;
        private static IDictionary<string, StrObjectDict> service = new Dictionary<string, StrObjectDict>();
        public static void Initializer(DeployMode mode)
        {
            _mode = mode;
            if (mode == DeployMode.MODE_NORMAL)
            {
                if (service == null)
                {
                    service = new Dictionary<string, StrObjectDict>();
                }

                IList<StrObjectDict> list = DataServiceManager.Instance.ListSod<DataService>(null);
                foreach (var item in list)
                {
                    //是否重复添加  tanjian - 2015/10/27
                    if (!service.ContainsKey(item["ID"].ToString()))
                    {
                        service.Add(item["ID"].ToString(), item);
                    }
                }
            }
        }

        public static IThirdService FindService(string id)
        {
            if(_mode == DeployMode.MODE_NORMAL){//本地代理服务对象
                if (service.ContainsKey(id))
                {
                    return ServiceFactory.CreateService(id, service[id]);
                }
            }else if(_mode == DeployMode.MODE_SOA){
                //获取服务总线代理服务对象
                return ServiceFactory.CreateSoaService(id,service[id]);
            }
            

            return null;
        }
    }
}
