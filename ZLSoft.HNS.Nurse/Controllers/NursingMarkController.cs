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
    public class NursingMarkController : MVCController
    {
        private NursingMarkManager _dal;
        public NursingMarkController()
        {
            _dal = NursingMarkManager.Instance;
        }
        #region 评分表

        public override ActionResult Index()
        {
            var sod = GetParams();
            sod["IsInvalid"] = 0;
            var data = _dal.List<NursingMark>(sod);
            return data.Count > 0 ? this.MyJson(1, data) : this.MyJson(0, "无数据信息");
        }

        public ActionResult Search()
        {
            var sod = GetParams();
            var lst = _dal.List<NursingMark>(sod);
            return this.MyJson(lst);
        }

        public ActionResult MarkSave()
        {
            var sod = GetParams();
            var names = _dal.List<NursingMark>(new { Name = sod.GetString("Name") }.toStrObjDict());
            if (names.Count > 1 && string.IsNullOrEmpty(sod.GetString("ID")))
                return this.MyJson(0, "名称已存在");
            var isOk = _dal.InsertOrUpdate<NursingMark>(sod) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult MarkDelete()
        {
            var sod = GetParams();
            var isOk = _dal.MarkDelete(sod.GetString("ID"));
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "未知错误");
        }

        public ActionResult IsSubset()
        {
            var sod = GetParams();
            var id = sod.GetString("MarkTabID");
            var result = _dal.IsSubset(id);
            return result ? this.MyJson(1, "") : this.MyJson(0, "无数据信息");
        }

        #endregion


        #region 评分项目

        public ActionResult TargetLoad()
        {
            var sod = GetParams();
            var data = _dal.List<NursingMarkTarget>(sod);

            var lst = new List<dynamic>();
            string strScoreValue, strScoreRemark;

            foreach (var item in data)
            {
                strScoreValue = strScoreRemark = string.Empty;
                var strJson = "[";
                if (!string.IsNullOrEmpty(item.ScoreValue))
                {
                    var scoreValue = item.ScoreValue.Split(',');
                    var valueRange = item.ValueRange.Split(',');
                    var scoreRemark = string.IsNullOrEmpty(item.ScoreRemark) ? null : item.ScoreRemark.Split(',');
                    for (int i = 0; i < scoreValue.Length; i++)
                    {
                        strJson += "{";
                        strJson += string.Format("'fz':'{0}','zy':'{1}','sm':'{2}'",
                                                    scoreValue[i],
                                                    valueRange[i],
                                                    scoreRemark == null ? "" : scoreRemark[i]);
                        strJson += "},";
                        strScoreValue += string.Format("{0}={1},", valueRange[i], scoreValue[i]);
                        strScoreRemark += string.Format("{0}分({1});", scoreValue[i], scoreRemark == null ? "" : scoreRemark[i]);
                    }
                    strScoreValue = strScoreValue.TrimEnd(',');
                    strScoreRemark = strScoreRemark.TrimEnd(';');
                    strJson = strJson.TrimEnd(',');
                }
                strJson += "]";

                var obj = new
                {
                    ID = item.ID,
                    Name = item.Name,
                    MarkTabID = item.MarkTabID,
                    Remark = item.Remark,
                    SerialNumber = item.SerialNumber,
                    IsMultiple = item.IsMultiple,
                    ElementID = item.ElementID,
                    StrScoreValue = strScoreValue,
                    StrScoreRemark = strScoreRemark,
                    score_group = JsonAdapter.FromJsonAsDictionary(strJson)

                };

                lst.Add(obj);
            }
            return this.MyJson(1, lst);

        }

        public ActionResult TargetSave()
        {
            var sod = GetParams();
            var objs = sod.GetObject("score_group") as object[];
            string fz, zy, sm;
            fz = zy = sm = string.Empty;
            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    var dic = obj as Dictionary<string, object>;
                    foreach (var item in dic)
                    {
                        switch (item.Key)
                        {
                            case "fz":
                                fz += item.Value.ToString() + ",";
                                break;
                            case "zy":
                                zy += item.Value.ToString() + ",";
                                break;
                            case "sm":
                                sm += item.Value.ToString() + ",";
                                break;
                        }
                    }
                }
            }

            var param = new
            {
                ID = sod.ContainsKey("ID") ? sod.GetString("ID") : string.Empty,
                Name = sod.GetString("Name"),
                MarkTabID = sod.GetString("MarkTabID"),
                Remark = sod.GetString("Remark"),
                SerialNumber = sod.GetString("SerialNumber"),
                IsMultiple = sod.GetString("IsMultiple"),
                ElementID = sod.GetString("ElementID"),
                ValueRange = zy.TrimEnd(','),
                ScoreValue = fz.TrimEnd(','),
                ScoreRemark = sm.TrimEnd(',')
            }.toStrObjDict();

            var isOk = _dal.InsertOrUpdate<NursingMarkTarget>(param) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }
        public ActionResult TargetDelete()
        {
            var sod = GetParams();
            var isOk = _dal.Delete<NursingMarkTarget>(sod) > 0;
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "未知错误");
        }

        public ActionResult GetMaxSerialNum()
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
                        SerialNumber = _dal.GetMaxCount(sod)
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region 参考

        public ActionResult ReferLoad()
        {
            var sod = GetParams();

            var data = _dal.List<NursingMarkRefer>(sod);
            return this.MyJson(1, data);
        }

        public ActionResult ReferSave()
        {
            var sod = GetParams();
            var id = sod.ContainsKey("ID") ? sod.GetString("ID") : string.Empty;
            var min = float.Parse(sod.GetString("Min"));
            var max = float.Parse(sod.GetString("Max"));
            var result = sod.GetString("Result");
            var markTabID = sod.GetString("MarkTabID");
            if (max < min)
                this.MyJson(0, "最大值不能小于等于最小值！");

            var lstRefer = _dal.List<NursingMarkRefer>(new
            {
                MarkTabID = markTabID
            }.toStrObjDict());

            if (lstRefer.Any(r => r.Result.Equals(result)) && string.IsNullOrEmpty(id) ||
                (!string.IsNullOrEmpty(id) && lstRefer.Any(r => r.Result.Equals(result) && !r.ID.Equals(id))))
                return this.MyJson(0, "结论出现重复！");

            lstRefer = string.IsNullOrEmpty(id) ? lstRefer : lstRefer.Where(r => !r.ID.Equals(id)).ToList();

            foreach (var item in lstRefer)
            {
                if ((max >= item.Min && max <= item.Max) ||
                    (max > item.Max && min < item.Min) ||
                    (min >= item.Min && min <= item.Max))
                    return this.MyJson(0, "评分结论的值域出现重合！");
            }

            var isOk = _dal.InsertOrUpdate<NursingMarkRefer>(sod) > 0;
            return isOk ? this.MyJson(1, "保存成功") : this.MyJson(0, "操作失败");
        }

        public ActionResult ReferDelete()
        {
            var sod = GetParams();
            var isOk = _dal.Delete<NursingMarkRefer>(sod) > 0;
            return isOk ? this.MyJson(1, "删除成功") : this.MyJson(0, "未知错误");
        }

        #endregion
    }
}
