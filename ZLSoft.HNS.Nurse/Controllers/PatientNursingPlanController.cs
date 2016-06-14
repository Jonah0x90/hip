using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Pub;

namespace ZLSoft.HNS.Nurse.Controllers
{
    /// <summary>
    /// 病人护理计划
    /// </summary>
    public class PatientNursingPlanController : MVCController
    {
        PatientNursingPlanManager _dal;
        public PatientNursingPlanController()
        {
            _dal = PatientNursingPlanManager.Instance;
        }

        /// <summary>
        /// 获取病人护理计划
        /// </summary>
        /// <param name="PatientID">病人ID</param>
        /// <returns></returns>
        public ActionResult GetPlans()
        {
            var sod = GetParams();
            var result = _dal.ListSod("GetPlans", sod);
            return this.MyJson(result);
        }

        /// <summary>
        /// 获取方案明细
        /// </summary>
        /// <params name="SchemeID">方案ID</params>
        /// <returns></returns>
        public ActionResult GetSchemeDetails()
        {
            var sod = GetParams();
            return this.MyJson(_dal.GetSchemeDetails(sod));
        }

        /// <summary>
        /// 保存病人护理计划
        /// </summary>
        /// <params name=""></params>
        /// <returns></returns>
        public ActionResult SaveNewPlan()
        {
            var sod = GetParams();
            var isOk = _dal.SaveNewPlan(sod, LoginSession.Current.NAME);
            //var isOk = _dal.SaveNewPlan(sod, "1583", "张三");
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult RemoveScheme()
        {
            var sod = GetParams();
            var isOk = _dal.RemoveScheme(sod);
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "删除失败");
        }

        public ActionResult UpdateSchemeDetails()
        {
            var sod = GetParams();
            var user = LoginSession.Current.NAME;
            var isOk = _dal.UpdateSchemeDetails(sod, user);
            return isOk ? this.MyJson(1, "修改成功") : this.MyJson(0, "修改失败");
        }

        public ActionResult GetQuestionByPatient()
        {
            var sod = GetParams();
            var result = _dal.ListSod("GetQuestionByPatient", sod);
            return this.MyJson(result);
        }

        public ActionResult CreateSign()
        {
            var sod = GetParams();
            var isOk = _dal.CreateSign(sod, LoginSession.Current.NAME);
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult RemoveSign()
        {
            var sod = GetParams();
            var isOk = _dal.RemogeSign(sod);
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult CreateEvaluat()
        {
            var sod = GetParams();
            var isOk = _dal.CreateEvaluat(sod, LoginSession.Current.NAME);
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult RemoveEvaluat()
        {
            var sod = GetParams();
            var isOk = _dal.RemoveEvaluat(sod);
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "操作失败");
        }

        /// <summary>
        /// 获取评估列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEvaluatLst()
        {
            var sod = GetParams();
            var result = _dal.GetEvaluatLst(sod);
            return this.MyJson(result);
        }
    }
}
