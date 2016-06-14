using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public class BacklogManager : CRUDManager
    {
        #region C'tor

        private static BacklogManager _Instance = new BacklogManager();

        public static BacklogManager Instance
        {
            get
            {
                return BacklogManager._Instance;
            }
        }

        private BacklogManager()
        {

        }

        #endregion

        /// <summary>
        /// 逻辑删除待办事项
        /// </summary>
        /// <param name="sod"></param>
        /// <returns></returns>
        public bool RemoveBacklog(StrObjectDict sod)
        {
            var id = sod.GetString("ID");
            //var batchStates = new DBState[] 
            //{
            //    new DBState(){
            //        Name =  "DELETE_Backlog",
            //        Param = new {ID =id},
            //        Type = ESqlType.DELETE
            //    },
            //    new DBState(){
            //        Name =  "DELETE_BacklogDetails",
            //        Param = new {BacklogID =id},
            //        Type = ESqlType.DELETE
            //    }
            //};

            //return DB.ExecuteWithTransaction(batchStates) > 0;

            return DB.Execute(new DBState
            {
                Name = "UPDATE_Backlog",
                Param = sod,
                Type = ESqlType.UPDATE
            }) > 0;
        }

        public PageData<Backlog> GetList(StrObjectDict sod, StrObjectDict pageInfo)
        {
            var pageNumber = pageInfo.GetInt("PageNum").Value;
            var pageSize = pageInfo.GetInt("PageSize").Value;
            int listcount = DB.Count<Backlog>(sod);
            IList<Backlog> listData = DB.List<Backlog>(sod, pageNumber, pageSize);
            return new PageData<Backlog>(listData, pageNumber, pageSize, listcount);
        }

        public bool SaveBacklogDetails(StrObjectDict sod)
        {

            var backlogIds = sod.GetObject("BacklogIds") as object[];
            if (backlogIds == null)
                return false;

            var batchStates = backlogIds.Select(id =>
            {
                var dic = id as Dictionary<string, object>;
                if (dic == null)
                    return null;
                return new DBState()
                {
                    Name = "INSERT_BacklogDetails",
                    Param = new
                    {
                        ID = Utils.getGUID(),
                        FormID = sod.GetString("FormID"),
                        ObjectID = sod.GetString("ObjectID"),
                        BacklogID = dic["eventId"],
                        TargetID = dic["id"],
                        Hander = sod.GetString("Hander")
                    }.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
            }).ToList();

            return batchStates.Count != 0 && DB.Execute(batchStates) > 0;
        }

        public List<StrObjectDict> GetBacklogs(StrObjectDict sod)
        {
            //PatientID
            var lstByDetails = DB.ListSod("GetBacklogsByDetails", sod);

            var marks = DB.ListSod("GetTriggerResult", sod);
            var backlogIds = new List<string>();
            foreach (var str in marks.Select(mark => mark.GetString("BacklogIds")))
            {
                if (str.Contains(','))
                {
                    var ids = str.Split(',');
                    backlogIds.AddRange(ids);
                }
                else
                {
                    backlogIds.Add(str);
                }
            }

            var lstByMark = DB.ListSod("", sod);
            return null;
        }
    }
}
