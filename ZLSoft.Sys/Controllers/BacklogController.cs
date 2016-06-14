using System;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;

namespace ZLSoft.Sys.Controllers
{
    public class BacklogController : MVCController
    {
        private readonly BacklogManager _dal;
        public BacklogController()
        {
            _dal = BacklogManager.Instance;
        }

        public override ActionResult List()
        {
            var pageInfo = GetPageInfo();
            var sod = GetParams();
            sod["IsInvalid"] = 0;
            //入参不包含分页
            if (pageInfo.Count == 0)
            {
                var listData = _dal.List<Backlog>(sod);
                return this.MyJson(listData);
            }
            var result = _dal.GetList(sod, pageInfo);
            return this.MyJson(1, result.DataList, result.PageInfo);
        }

        public ActionResult BacklogSave()
        {
            var sod = GetParams();
            sod["UpdateUser"] = LoginSession.Current.NAME;
            sod["ModifyTime"] = DateTime.Now;
            sod["IsInvalid"] = 0;
            var isOk = _dal.InsertOrUpdate<Backlog>(sod) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult RemoveBacklog()
        {
            var sod = GetParams();
            var isOk = _dal.RemoveBacklog(sod);
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult SaveBacklogDetails()
        {
            var sod = GetParams();
            sod["Hander"] = LoginSession.Current.NAME;
            var isOk = _dal.SaveBacklogDetails(sod);
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult GetBacklogs()
        {
            var sod = GetParams();
            var result = _dal.GetBacklogs(sod);

            return this.MyJson(result);
        }
    }
}
