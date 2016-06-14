using System.Linq;
using ZLSoft.AppContext;
using ZLSoft.HNS.NurseDalManager;
using System.Web.Mvc;
using ZLSoft.Pub;

namespace ZLSoft.HNS.Nurse.Controllers
{
    public class PatientCareMarkController : MVCController
    {
        private readonly PatientCareMarkManager _dal;


        public PatientCareMarkController()
        {
            _dal = PatientCareMarkManager.Instance;
        }

        public ActionResult Save()
        {
            var sod = GetParams();
            var isOk = _dal.Save(sod, LoginSession.Current.NAME);
            var list = new StrObjectDict();
            if (!sod.ContainsKey("ID"))
            {
                list.Add("ID", sod);
                return this.MyJson(1, list);
            }
            else
            {
                if (isOk == "update")
                {
                    return this.MyJson(1, "保存成功");
                }
                else
                {
                    return this.MyJson(0, "未知错误");
                }
            }
            
        }

        public ActionResult GetMarkInfo()
        {
            var sod = GetParams(); 
            var questions = _dal.ListSod("GetQuestionList", new {IDS = sod.GetString("Qid").Split(',')}.toStrObjDict());
            var markInfo = _dal.ListSod("GetMarkInfo", sod);
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = 1,
                    Msg = "",
                    Output = new
                    {
                        Data = new
                        {
                            MarkInfo = markInfo,
                            Questions = questions
                        }
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public override ActionResult List()
        {
            var sod = GetParams();
            if (sod.Count == 0) 
                return this.MyJson(0, "入参为空。");
            return this.MyJson(_dal.ListSod("LIST", sod));
        }

        /// <summary>
        /// 获取已触发的病人评分结论
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTriggerResult()
        {
            var sod = GetParams();
            if (sod.Count == 0) 
                return this.MyJson(0,"入参为空。");
            return this.MyJson(_dal.ListSod("GetTriggerResult", sod));
        }

        public ActionResult GetResultByScore()
        {
            var sod = GetParams();
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = 1,
                    Msg = "",
                    Output = new
                    {
                        Data = new
                        {
                            MarkVerdict = _dal.GetResultByScore(sod)
                        }
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public override ActionResult Delete()
        {
            var sod = GetParams();
            return _dal.Delelte(sod) ? this.MyJson(1, "删除成功") : this.MyJson(0, "删除失败");
        }
    }
}
