using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.DalManager
{
    public class BPMManager:CRUDManager
    {
             #region
        private static BPMManager _Instance = new BPMManager();
        public static BPMManager Instance
        {
            get
            {
                return BPMManager._Instance;
            }
        }
        private BPMManager()
        {
        }

        #endregion

        
    }
}
