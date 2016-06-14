using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.System;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;

namespace ZLSoft.DalManager
{
    public class ParmsManager : CRUDManager
    {
        private static ParmsManager _Instance = new ParmsManager();
        public static ParmsManager Instance
        {
            get
            {
                return ParmsManager._Instance;
            }
        }
        private ParmsManager()
        {
        }
        /// <summary>
        /// 通过ID获取系统参数
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        public string getParmValueByID(string ID)
        {
            SysParameters sysParm = DB.Load<SysParameters, PK_SysParameters>(new PK_SysParameters
            {
                ID = ID
            });
            return sysParm.DefaultValue;
        }

        /// <summary>
        /// 通过名称获取系统参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string getParmByName(string Name)
        {
            string result = null;
            IList<SysParameters> list = DB.List<SysParameters>(new
            {
                Name = Name
            }.toStrObjDict());
            if(list.Count > 0){
                result = list[0].DefaultValue;
            }
            return result;
        }

        public IList<StrObjectDict> list(IDictionary<string, object> obj)
        {
            return DB.ListSod("LISTfor_SysParameters", obj);
        }

        public int save(IDictionary<string, object> obj)
        {
            return 0;
        }

        public int del(IDictionary<string, object> obj)
        {
            return 0;
        }

    }
}
