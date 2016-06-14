using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;

namespace ZLSoft.HNS.Nurse.Controllers
{
    public class NursingPlanTargetController : MVCController
    {
        private NursingPlanTargetManager _dal;

        public NursingPlanTargetController()
        {
            _dal = NursingPlanTargetManager.Instance;
        }

        /// <summary>
        /// 计划选项查询 (Param:Code)/  护理问题关联查询(Param:SuperID)
        /// </summary>
        /// <returns></returns>
        public ActionResult TargetLoad()
        {
            var sod = GetParams();
            var data = _dal.List<NursingPlanTarget>(sod);
            return data.Count > 0 ? this.MyJson(1, data) : this.MyJson(0, "无数据信息");
        }

        /// <summary>
        /// Code:选项分类
        /// InputCode:检索码
        /// </summary>
        /// <returns></returns>
        public ActionResult TargetSearch()
        {
            var sod = GetParams();
            var code = sod.GetString("Code");
            var strInputCode = sod.GetString("InputCode");
            var data = _dal.GetPlanTargetByCode(code);

            if (sod.ContainsKey(strInputCode))
            {
                var value = sod.GetString(strInputCode);
                if (string.IsNullOrEmpty(value))
                    return this.MyJson(1, data);

                return this.MyJson(1, data.Where(d => d.Name.Contains(value) || d.InputCode.Contains(value)));
            }
            return this.MyJson(1, data);
        }

        public ActionResult TargetSave()
        {
            var sod = GetParams();

            var isOk = _dal.InsertOrUpdate<NursingPlanTarget>(sod) > 0;
            if (isOk)
            {
                var code = sod.GetString("Code");
                _dal.RemoveCache(code);
                return this.MyJson(1, "保存成功");
            }
            return this.MyJson(0, "未知错误");
        }

        public ActionResult TargetDelete()
        {
            var sod = GetParams();
            var isOk = _dal.TargetDelete(sod);
            if (isOk)
            {
                var code = sod.GetString("Code");
                _dal.RemoveCache(code);
                return this.MyJson(1, "删除成功");
            }
            return this.MyJson(0, "未知错误");
        }

        public ActionResult ExistReference()
        {
            var sod = GetParams();
            var resutl = _dal.ExistReference(sod);
            return resutl ? this.MyJson(1, "") : this.MyJson(0, "该项被引用");
        }
    }
}
