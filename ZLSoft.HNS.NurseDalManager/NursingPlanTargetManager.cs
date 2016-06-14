using System;
using System.Collections.Generic;
using System.Linq;
using ZLSoft.Cache;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HNS.NurseDalManager
{
    /// <summary>
    /// 护理计划选项
    /// </summary>
    public class NursingPlanTargetManager : CRUDManager
    {
        public const string PREFIX = "Target";

        #region 构造

        private static NursingPlanTargetManager _instance = new NursingPlanTargetManager();
        public static NursingPlanTargetManager Instance
        {
            get
            {
                return NursingPlanTargetManager._instance;
            }
        }

        private NursingPlanTargetManager() { }

        #endregion

        #region 方法


        /// <summary>
        /// 根据Code查找
        /// </summary>
        /// <param name="code">因素/ys、目标/mb、措施/cs、评估/pg</param>
        /// <returns></returns>
        public IList<NursingPlanTarget> GetPlanTargetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            var key = string.Format("{0}_{1}", PREFIX, code);

            var cache = CacheIO.Get(key);
            if (cache == null)
            {
                var data = List<NursingPlanTarget>(new { Code = code }.toStrObjDict());
                CacheIO.Insert(key, data);
                return data;
            }
            return cache as IList<NursingPlanTarget>;
        }

        public void RemoveCache(string code)
        {
            var key = string.Format("{0}_{1}", PREFIX, code);
            CacheIO.Remove(key);
        }

        /// <summary>
        /// 查询该<问题方法>是否被引用
        /// </summary>
        /// <param name="sod"></param>
        /// <returns></returns>
        public bool ExistReference(StrObjectDict sod)
        {
            var id = sod.GetString("ID");
            var qCount = DB.Count<QuestionRelation>(new { FunID = id });
            var nCount = DB.Count<NursingPlanDetails>(new { PlanTargetID = id });
            return qCount > 0 || nCount > 0;
        }

        public bool TargetDelete(StrObjectDict obj)
        {
            var id = obj.GetString("ID");

            var batchStates = new DBState[] 
            { 
                new DBState(){
                    Name =  new NursingPlanTarget().MAP_DELETE,
                    Param = new{ ID =id},
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name =  new NursingPlanDetails().MAP_DELETE,
                    Param = new{ PlanTargetID = id},
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name = new QuestionRelation().MAP_DELETE,
                    Param = new{ FunID =id},
                    Type = ESqlType.DELETE
                }
            };
            return DB.ExecuteWithTransaction(batchStates) > 0;
        }

        #endregion
    }
}
