using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.DalManager
{
    public class WorkItemManager:CRUDManager
    {
          #region
        private static WorkItemManager _Instance = new WorkItemManager();
        public static WorkItemManager Instance
        {
            get
            {
                return WorkItemManager._Instance;
            }
        }
        private WorkItemManager()
        {
        }

        #endregion
    }
}
