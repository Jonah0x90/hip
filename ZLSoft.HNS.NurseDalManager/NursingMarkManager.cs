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
    /// 护理评分
    /// </summary>
    public class NursingMarkManager : CRUDManager
    {
        #region 字段
        private const string KEY = "NursingMarkManager";
        #endregion

        #region 构造

        private static NursingMarkManager _instance = new NursingMarkManager();
        public static NursingMarkManager Instance
        {
            get
            {
                return NursingMarkManager._instance;
            }
        }

        private NursingMarkManager()
        {
        }



        #endregion

        /// <summary>
        /// 查询评分表中是否包含项目、参考
        /// </summary>
        /// <param name="markTabID"></param>
        /// <returns></returns>
        public bool IsSubset(string markTabID)
        {
            var rCount = DB.Count<NursingMarkRefer>(new { MarkTabID = markTabID });
            var tCount = DB.Count<NursingMarkTarget>(new { MarkTabID = markTabID });
            return rCount > 0 || tCount > 0;
        }

        public bool MarkDelete(string id)
        {
            var batchStates = new DBState[] 
            {
                new DBState(){
                    Name =  "DeleteTargetByMark",
                    Param = new {MarkTabID =id},
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name =  "DeleteReferByMark",
                    Param = new {MarkTabID =id},
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name =  new NursingMark().MAP_DELETE,
                    Param = new {ID =id},
                    Type = ESqlType.DELETE
                }
            };
            return DB.ExecuteWithTransaction(batchStates) > 0;
        }

        public int GetMaxCount(StrObjectDict sod)
        {
            var num = DB.Scalar("GetMarkTargetMaxNum", sod);
            return num == DBNull.Value ? 1 : Convert.ToInt32(num);
        }

        public IList<NursingMark> GetMarks()
        {
            var cache = CacheIO.Get(KEY);
            if (cache == null)
            {
                var data = this.List<NursingMark>(null);
                CacheIO.Insert(KEY, data);
                return data;
            }

            return cache as IList<NursingMark>;
        }

        public void ClearCache()
        {
            CacheIO.Remove(KEY);
        }
    }
}
