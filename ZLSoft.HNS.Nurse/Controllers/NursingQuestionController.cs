using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Pub;
using ZLSoft.Model.HNS;

namespace ZLSoft.HNS.Nurse.Controllers
{
    public class NursingQuestionController : MVCController
    {
        private NurseQuestionManager _dal;

        public NursingQuestionController()
        {
            _dal = NurseQuestionManager.Instance;
        }


        public ActionResult QuestionSave()
        {
            var sod = GetParams();
            var isOk = _dal.InsertOrUpdate<NursingQuestion>(sod) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "未知错误");
        }

        public ActionResult QuestionDelete()
        {
            var sod = GetParams();
            var isOk = _dal.Delete(sod);
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "未知错误");
        }

        public ActionResult ExistReference()
        {
            var sod = GetParams();
            var resutl = _dal.ExistReference(sod);
            return resutl ? this.MyJson(0, "该项被引用") : this.MyJson(1, "");
        }

        /// <summary>
        /// 保存护理问题关联 
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionRelated()
        {
            var sod = GetParams();
            var qId = sod.GetString("QuestionID");
            var funIds = sod.GetObject("FunIds") as object[];

            var result = _dal.SvaeRelationship(qId, funIds);
            return result ? this.MyJson(1, "保存成功") : this.MyJson(0, "未知错误");
        }

        /// <summary>
        /// 查询问题关联 参数(问题ID)
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadQuestionRelation()
        {
            //问题ID
            var sod = GetParams();
            var result = _dal.GetQuestionRelation(sod);
            return this.MyJson(result);
        }

        public ActionResult GetQuestionRelationship()
        {
            //问题ID
            var sod = GetParams();
            var result = _dal.GetQuestionRelationship(sod);
            return this.MyJson(result);
        }

        /// <summary>
        /// Params:InputCode  ||TypeID
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionSearch()
        {
            var sod = GetParams();

            var codeKey = "InputCode";
            var typeIDKey = "TypeId";
            var data = _dal.LstNurseQuestion();
            IEnumerable<NursingQuestion> lstQuestion = null;

            //当有TypeID传入时,查询的对象是诊断标准
            if (sod.ContainsKey(typeIDKey) && sod.ContainsKey(codeKey))
            {
                var code = sod.GetString(codeKey);
                if (string.IsNullOrEmpty(code))
                    lstQuestion = data.Where(d => d.TypeId == sod.GetInt(typeIDKey));
                else
                    lstQuestion = data.Where(d => d.TypeId == sod.GetInt(typeIDKey) && d.InputCode.Contains(code));

                return this.MyJson(1, lstQuestion);
            }
            else //查询问题
            {
                var code = sod.GetString(codeKey);
                if (string.IsNullOrEmpty(code))
                    lstQuestion = data.Where(d => d.TypeId == null);
            }

            return this.MyJson(1, lstQuestion);
        }

        public ActionResult ExistsFourElement()
        {
            var sod = GetParams();
            return this.MyJson(_dal.ExistsFourElement(sod));
        }
    }
}
