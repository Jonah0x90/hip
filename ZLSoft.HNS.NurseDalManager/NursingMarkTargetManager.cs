using System;
using System.Collections.Generic;
using System.Linq;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HNS.NurseDalManager
{
    /// <summary>
    /// 评分项目
    /// </summary>
    public class NursingMarkTargetManager : CRUDManager
    {
        #region 构造

        private static NursingMarkTargetManager _instance = new NursingMarkTargetManager();
        public static NursingMarkTargetManager Instance
        {
            get
            {
                return NursingMarkTargetManager._instance;
            }
        }
        private NursingMarkTargetManager()
        {
        }

        #endregion
    }
}
