using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.Public.Controllers
{
    public class ChangesTationLogController : MVCController
    {
        public override ActionResult InsertOrUpdate()
        {
            var sod = GetParams();
            sod["RecordPerson"] = LoginSession.Current.USERNAME;
            if (string.IsNullOrEmpty(LoginSession.Current.USERNAME))
                return this.MyJson(0, "获取用户名有误,请刷新页面");
            sod["RecordTime"] = sod["TimeOffset"];
            sod["IsInvalid"] = 0;
            IList<DBState> dblist;
            if (sod.ContainsKey("ID") && string.IsNullOrEmpty(sod["ID"].ToString())) //增加
            {
                sod["ID"] = Utils.getGUID();
                sod.Remove("TimeOffset");
                dblist = new List<DBState>
                {
                    new DBState
                    {
                        Name = "INSERT_ChangesTationLogRec",
                        Param = sod,
                        Type = ESqlType.INSERT
                    },
                    new DBState
                    {
                        Name = "INSERT_ChangesTationLog",
                        Param = new
                        {
                            ChangesTationLogID = sod["ID"],
                            SubmitTime = DateTime.Now,
                            SubmitPerson = sod["RecordPerson"]
                        },
                        Type = ESqlType.INSERT
                    }
                };
            }
            else//修改
            {
                dblist = new List<DBState>
                {
                    new DBState
                    {
                        Name = "UPDATE_ChangesTationLogRec",
                        Param = sod,
                        Type = ESqlType.INSERT
                    },
                    new DBState
                    {
                        Name = "INSERT_ChangesTationLog",
                        Param = new
                        {
                            ChangesTationLogID = sod["ID"],
                            SubmitTime = DateTime.Now,
                            SubmitPerson = sod["RecordPerson"]
                        },
                        Type = ESqlType.INSERT
                    }
                };
            }
            return DB.Execute(dblist) > 0 ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public override ActionResult List()
        {
            var sod = GetParams();
            //sod["LesionID"] = LoginSession.Current.DEPTID;
            //sod["RecordTime"] = DateTime.Now;
            var lst = crudManager.ListSod("LIST_ChangesTationLogRec", sod);

            var group = lst.GroupBy(a => ClazzExtend.GetDate(a, "RecordTime"));
            var result = new List<dynamic>();
            foreach (var item in group)
            {
                var data = item.First();
                result.Add(new
                {
                    TimeOffset = item.Key,
                    Content = item.First()["Content"],
                    Data = new { ID = data["ID"], RecordPerson = data["RecordPerson"] }
                });
            }

            //var morning = new List<StrObjectDict>();
            //var middle = new List<StrObjectDict>();
            //var night = new List<StrObjectDict>();
            //foreach (var item in lst)
            //{
            //    var date = item.GetDate("RecordTime");
            //    if (date != null)
            //    {
            //        var hour = date.Value.Hour;
            //        if (hour >= 0 && hour < 8)
            //        {
            //            morning.Add(item);
            //        }
            //        else if (hour >= 8 && hour < 17)
            //        {
            //            middle.Add(item);
            //        }
            //        else
            //        {
            //            night.Add(item);
            //        }
            //    }
            //}
            //var result = new { morning, middle, night };
            return this.MyJson(result);
        }
    }
}
