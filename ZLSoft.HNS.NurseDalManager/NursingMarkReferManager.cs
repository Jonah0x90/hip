using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HNS.NurseDalManager
{
    /// <summary>
    /// 评分参考
    /// </summary>
    public class NursingMarkReferManager : CRUDManager
    {
        #region 构造

        private static NursingMarkReferManager _instance = new NursingMarkReferManager();
        public static NursingMarkReferManager Instance
        {
            get
            {
                return NursingMarkReferManager._instance;
            }
        }
        private NursingMarkReferManager()
        {
        }

        #endregion
    }
}
