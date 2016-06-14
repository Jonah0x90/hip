using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;

namespace ZLSoft.HNS.Nurse.Controllers
{
    public class NursingPlanController : MVCController
    {
        private NursingPlanManager _dal;
        public NursingPlanController()
        {
            _dal = NursingPlanManager.Instance;
        }

        public override ActionResult Page()
        {
            //var pageInfo = GetPageInfo();
            //var sod = GetParams();
            //var result = _dal.List(sod, pageInfo);
            //return this.MyJson(1, result.DataList, result.PageInfo);


            var pageInfo = GetPageInfo();
            var sod = GetParams();
            var result = _dal.List1(sod, pageInfo);
            return this.MyJson(1, result.DataList, result.PageInfo);
        }

        public  ActionResult Page1()
        {
            var pageInfo = GetPageInfo();
            var sod = GetParams();
            var result = _dal.List1(sod, pageInfo);
            return this.MyJson(1, result.DataList, result.PageInfo);
        }

        public override ActionResult Index()
        {
            var sod = GetParams();
            var data = _dal.List<NursingPlan>(sod);
            return data.Count > 0 ? this.MyJson(0, data) : this.MyJson(0, "无数据信息");
        }

        public ActionResult PlanSave()
        {
            var sod = GetParams();
            var isSys = sod["IsSys"].ToString();
            if (isSys == "1")
                return this.MyJson(0, "不允许编辑系统级的标准护理计划！");
            sod.Add("ModifyTime", DateTime.Now);
            sod.Add("ModifyUser", LoginSession.Current.NAME);
            var isOk = _dal.InsertOrUpdate<NursingPlan>(sod) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        /// <summary>
        /// 设置适用科室
        /// </summary>
        /// <returns></returns>
        public ActionResult SetStandardCompliant()
        {
            var sod = GetParams();
            var isSys = sod.GetInt("IsSys");
            if (isSys == 1)
            {
                sod["IsSys"] = 0;
                sod.Add("ModifyUser", LoginSession.Current.NAME);
                sod.Add("ModifyTime", DateTime.Now);
            }
            var isOk = _dal.InsertOrUpdate<NursingPlan>(sod) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult PlanDelete()
        {
            var sod = GetParams();
            if (sod.GetInt("IsSys") == 1)
                return this.MyJson(0, "不允许删除系统级的标准护理计划！");
            var isOk = _dal.Delete(sod);
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "未知错误");
        }

        public ActionResult GetStandardPlan()
        {
            var sod = GetParams();
            var data = _dal.LoadByID<NursingPlan>(sod.GetString("ID"));
            return this.MyJson(1, data);
        }

        /// <summary>
        /// PlanID
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanDetails()
        {
            var sod = GetParams();
            var data = _dal.GetPlanDetails(sod);
            return this.MyJson(1, data);
        }

        public ActionResult PlanDetailSave()
        {
            var sod = GetParams();
            var pData = sod["planData"] as Dictionary<string, object>;
            var uid = LoginSession.Current.USERID;
            var o2u = OrganizationManager.Instance.GetOrgListByUser(new { UserID = uid, DepartType = 2 }.toStrObjDict());
            var deptRange = pData.GetString("DeptRange").Split(',');
            if (o2u.Count == 0)
                return this.MyJson(0, "只有护理病区的操作员才能创建护理计划！");
            if (!o2u.Any(o => deptRange.Contains(o.GetString("ID"))))
                return this.MyJson(0, "你不能修改其他病区的护理计划！");

            if (_dal.SvaeRelationship(sod))
                return this.MyJson(1, "保存成功");
            return this.MyJson(0, "未知错误");
        }
    }
}
