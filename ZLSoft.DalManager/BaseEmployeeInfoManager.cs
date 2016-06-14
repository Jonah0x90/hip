using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.HR;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    public class BaseEmployeeInfoManager:CRUDManager
    {
        private static BaseEmployeeInfoManager _Instance = new BaseEmployeeInfoManager();

        public static BaseEmployeeInfoManager Instance
        {
            get
            {
                return BaseEmployeeInfoManager._Instance;
            }
        }

        private BaseEmployeeInfoManager()
        {

        }


    }
}
