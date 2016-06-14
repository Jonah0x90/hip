using System;
using System.Collections.Generic;
using System.Linq;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.HNS.NurseDalManager
{
    public class NursingPatientManager : CRUDManager
    {
        private static NursingPatientManager _instance = new NursingPatientManager();
        public static NursingPatientManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public PageData<StrObjectDict> List1(IDictionary<string, object> sod, IDictionary<string, object> pageInfo)
        {
            #region 构造入参

            object[] lstLevel = null;
            if (sod.ContainsKey("LstLevel"))
            {
                lstLevel = sod["LstLevel"] as object[];
                if (lstLevel.Length == 0)
                {
                    sod.Remove("LstLevel");
                }
                else if (lstLevel.Contains("无护理"))
                {
                    sod["NursingLevel"] = 1;
                }
            }
            //如果IsCritical入参为空，则删除
            if (sod.ContainsKey("IsCritical") && sod["IsCritical"].Equals(string.Empty))
            {
                sod.Remove("IsCritical");
            }
            //IsContain用于在xml中判断是否加括号
            if (lstLevel != null && lstLevel.Length > 0)
            {
                sod["IsContain"] = "";
            }
            else
            {
                sod["IsContain"] = "1";
            }

            if (sod.GetInt("Type") == 4)
            {
                if (sod.ContainsKey("StartTime") && string.IsNullOrEmpty(sod.GetString("StartTime")))
                {
                    sod["StartTime"] = DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (sod.ContainsKey("EndTime") && string.IsNullOrEmpty(sod.GetString("EndTime")))
                {
                    sod["EndTime"] = DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (sod.ContainsKey("AttachType") && !string.IsNullOrEmpty(sod.GetString("AttachType")))
                {
                    var value = sod.GetString("AttachType");
                    var strs = value.Split(',');
                    var attType = int.Parse(strs[0]);
                    if (!string.IsNullOrEmpty(strs[1]))
                    {
                        switch (attType)
                        {
                            case 1:
                                sod["FullName"] = strs[1];
                                break;
                            case 2:
                                sod["BedNumber"] = strs[1];
                                break;
                            case 3:
                                sod["AdmissionNumber"] = strs[1];
                                break;
                        }
                    }
                }
            }

            #endregion

            var pageNumber = pageInfo.GetInt("PageNum").Value;
            var pageSize = pageInfo.GetInt("PageSize").Value;

            var lstCount = Convert.ToInt32(DB.Scalar("COUNT_NursingPatient", sod.toStrObjDict()));

            var listData = DB.ListSod("LIST_NursingPatient", sod, pageNumber, pageSize);
            var groupData = listData.GroupBy(g => g.GetString("ID"));
            var result = new List<StrObjectDict>();
            foreach (var group in groupData)
            {
                var loaded = false;
                var lstForm = new List<string>();
                var lstTodo = new List<StrObjectDict>();
                var itemInfo = new StrObjectDict();
                foreach (var item in group)
                {
                    if (!loaded)
                    {
                        var extraInfo = item["ExtraInfo"].ToString().Split(',');
                        var lstDic = (from childItem in extraInfo
                                      select childItem.Split('_') into lst
                                      where lst.Length == 2 && !string.IsNullOrEmpty(lst[1])
                                      select new Dictionary<string, string> { { lst[0], lst[1] } }).ToList();
                        var nursingLevel = item.GetString("NursingLevel");
                        item["NursingLevelCh"] = GetLeavel(nursingLevel);
                        item["ArrayExtraInfo"] = lstDic;
                        itemInfo = item;
                        loaded = true;
                    }
                    lstForm.Add(item.GetString("FormName"));
                    lstTodo.Add(new
                    {
                        Name = item.GetString("TodoName"),
                        URL = item.GetString("URL"),
                        Parameter = item.GetString("Parameter")
                    }.toStrObjDict());
                }
                itemInfo["LstForm"] = lstForm;
                itemInfo["LstTodo"] = lstTodo;
                result.Add(itemInfo);
            }

            return new PageData<StrObjectDict>(result.OrderBy(s => s["BedNumber"]).ToList(), pageNumber, pageSize, lstCount);
        }

        public List<StrObjectDict> GetNurseLeavelCount(StrObjectDict sod)
        {
            var result = LoadObjectSod("GetNurseLeavelCount", sod);
            int id = 1;
            return result.Select(item => new
            {
                ID = id++,
                Name = item.Key,
                Value = item.Value,
                NursingLevelCh = GetLeavel(item.Key),
                Selected = false
            }.toStrObjDict()).ToList();
        }

        public PageData<StrObjectDict> GetPatients(StrObjectDict sod, StrObjectDict pageInfo)
        {
            var pageNumber = pageInfo.GetInt("PageNum").Value;
            var pageSize = pageInfo.GetInt("PageSize").Value;
            var lstCount = Convert.ToInt32(DB.Scalar("COUNT_NursingPatient1", sod));
            var lst = DB.ListSod("LIST_NursingPatient1", sod, pageNumber, pageSize);
            return new PageData<StrObjectDict>(lst, pageNumber, pageSize, lstCount);
        }

        private string GetLeavel(string nursingLevel)
        {
            switch (nursingLevel)
            {
                case "无护理":
                    return "无";
                case "Ⅰ级护理":
                case "一级护理":
                    return "一";
                case "Ⅱ级护理":
                case "二级护理":
                    return "二";
                case "Ⅲ级护理":
                case "三级护理":
                    return "三";
                case "危重病人":
                    return "危";
                case "特级护理":
                    return "特";
            }
            return "";
        }
    }
}
