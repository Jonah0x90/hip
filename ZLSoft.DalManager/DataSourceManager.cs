using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.DalManager
{
    public class DataSourceManager:CRUDManager
    {
                #region
        private static DataSourceManager _Instance = new DataSourceManager();
        public static DataSourceManager Instance
        {
            get
            {
                return DataSourceManager._Instance;
            }
        }
        private DataSourceManager()
        {
        }

        #endregion


    }
}
