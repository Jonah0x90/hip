using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.ThirdInterface;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Pub.Db;

namespace ZLSoft.Third.Controllers
{
    public class ThirdController : BaseController
    {
        /// <summary>
        /// 导入所有三方数据(护理项目,部门，职工，病人)
        /// </summary>
        /// <returns></returns>
        //public ActionResult FetchAll()
        //{
        //    StrObjectDict dict = GetParams();


        //    return null;
        //}
        

        /// <summary>
        /// 导入问题关系
        /// </summary>
        /// <returns></returns>
        public string SaveQuestionRelation()
        {
            try
            {
                var service1 = ThirdServiceContext.FindService("a8e6b69e-6980-4732-bee4-39ca1293bfdc");
                var data = service1.DoServiceAsync(null);

                var lstDbState = new List<DBState>();
                var lstCheck = new List<dynamic>();
                foreach (var item in data)
                {
                    var id = item.GetString("ID");
                    var ys = item.GetString("因素对照").Replace("<ITEM>", "").Replace("</ITEM>", "").Replace("<ITEM/>", "").Replace("\n", "");
                    var cs = item.GetString("措施对照").Replace("<ITEM>", "").Replace("</ITEM>", "").Replace("<ITEM/>", "").Replace("\n", "");
                    var mb = item.GetString("目标对照").Replace("<ITEM>", "").Replace("</ITEM>", "").Replace("<ITEM/>", "").Replace("\n", "");
                    var pg = item.GetString("评估对照").Replace("<ITEM>", "").Replace("</ITEM>", "").Replace("<ITEM/>", "").Replace("\n", "");
                    var result = GetRelation(id, ys, lstCheck);
                    if (result != null && result.Count != 0)
                        lstDbState.AddRange(result);
                    result = GetRelation(id, cs, lstCheck);
                    if (result != null && result.Count != 0)
                        lstDbState.AddRange(result);
                    result = GetRelation(id, mb, lstCheck);
                    if (result != null && result.Count != 0)
                        lstDbState.AddRange(result);
                    result = GetRelation(id, pg, lstCheck);
                    if (result != null && result.Count != 0)
                        lstDbState.AddRange(result);
                }

                DB.ExecuteAsync(lstDbState);
            }
            catch (Exception msg )
            {
                return msg.Message;
                throw;
            }
            return null;
        }

        private List<DBState> GetRelation(string id, string cId, List<dynamic> lstCheck)
        {
            if (string.IsNullOrEmpty(cId))
                return null;
            var arr = cId.Split(',');
            var lstDbState = new List<DBState>();
            foreach (var item in arr)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                var sod = new
                {
                    FunID = item,
                    QuestionID = id
                };
                if (lstCheck.Any(c => c.FunID == item && c.QuestionID == id))
                    continue;
                lstCheck.Add(sod);

                lstDbState.Add(new DBState()
                {
                    Name = "INSERT_QuestionRelation",
                    Param = sod,
                    Type = ESqlType.INSERT
                });
            }

            return lstDbState;
        }

        string DoService(string id)
        {
            var service = ThirdServiceContext.FindService(id);
            try
            {
                service.DoServiceAsync(null);
            }
            catch (Exception msg)
            {

                return msg.Message;
            }
            return null;
        }

        /// <summary>
        /// 从三方接口抓取病人资料数据
        /// </summary>
        /// <returns></returns>
        public ActionResult FetchInpationList()
        {
            //IThirdService service = ThirdServiceContext.FindService("90335235-effd-44d8-927e-b17ccdde6844");//测试性能
            HttpContext.Cache["Current"] = 0;
            HttpContext.Cache["Sum"] = 10;
            int count = 0;
            Task.Factory.StartNew(() =>
            {
                //同步护理项目
                var msg = DoService("90335235-effd-44d8-927e-b17ccdde686f");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            })
            .ContinueWith(a =>
            {
                //同步用户信息
                var msg =  DoService("b5cef49a-220d-4cce-a5fe-dd0f9312c3e6");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;

            })
            .ContinueWith(a =>
            {
                //同步部门人员关联
                var msg =  DoService("8e52be05-e537-4617-92d9-fb4a948f8139");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            })
            .ContinueWith(a =>
            {
                //同步科室病区
                var msg = DoService("fe6502a3-0240-428a-bcfd-0afccb10ce5f");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            })
            .ContinueWith(a =>
            {
                //同步病人信息
                var msg = DoService("bcb9fa31-9939-44bd-b56e-53a693ac89a4");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            })
            .ContinueWith(a =>
            {
                //同步职工信息
                var msg = DoService("a4007ce5-afa6-4cbc-9322-fc2c3a733d8d");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            })
            .ContinueWith(a =>
            {
                //同步组织机构
                var msg = DoService("01b81cac-6ad1-4d3d-9797-c093d28042a3");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            })
            .ContinueWith(a =>
            {
                //导入病人变动记录
                var msg = DoService("082c8f5a-5f1d-471b-84ac-10049117cd1c");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            }).ContinueWith(a =>
            {
                //导入标准计划
                 var msg = DoService("5e02ca68-3eed-493b-b375-e665fe3708af");
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            }).ContinueWith(a =>
            {
                //导入问题关系
                var msg = SaveQuestionRelation();
                HttpContext.Cache["Current"] = ++count;
                HttpContext.Cache["Msg"] = msg;
            });
            return this.MyJson(0);
        }


        public ActionResult GetProgressing()
        {
            var p = Session["Current"];
            var m = Session["Msg"];
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("current", p);
            obj.Add("msg", m);
            return this.MyJson(obj);
        }

        public ActionResult Test()
        {
            IThirdService service = ThirdServiceContext.FindService("90335235-effd-44d8-927e-b17ccdde6844");//测试性能

            service.DoService(null);
            return null;
        }

        /// <summary>
        /// 输液巡视
        /// </summary>
        /// <returns></returns>
        public ActionResult Driplist()
        {
            var sod = GetParams();
            if (!sod.ContainsKey("StartTime"))
                sod["StartTime"] = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            if (!sod.ContainsKey("EndTime"))
                sod["EndTime"] = DateTime.Now.ToString("yyyy-MM-dd");

            var service = ThirdServiceContext.FindService("1bd9454c-a9a2-4514-a1c2-fe0c040665d0");
            if (service == null)
                return this.MyJson(0, "服务定义引用错误。");
            //var list = service.DoService(new
            //{
            //    HomepageID = 1,
            //    PatientID = 52134,
            //    Baby = 0,
            //    StartTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
            //    EndTime = DateTime.Now.ToString("yyyy-MM-dd")
            //}.toStrObjDict());
            var list = service.DoService(sod);

            //按照医嘱内容分组
            var grouping = list.Where(a => !string.IsNullOrEmpty(a.GetString("RelatID")))
                               .GroupBy(a => new
            {
                AdviceContent = a.GetString("AdviceContent")
            });

            //查找相关ID为null，获取滴速
            var parents = list.Where(a => string.IsNullOrEmpty(a.GetString("RelatID")));
            var result = new List<dynamic>();

            var enumerable = parents as StrObjectDict[] ?? parents.ToArray();
            foreach (var item in grouping)
            {
                var lstAdvice = new List<StrObjectDict>();
                foreach (var child in item)
                {
                    var p = enumerable.First(a => a.GetString("ID").Equals(child.GetString("RelatID")));
                    child["DS"] = p.GetString("DoctorAdvice");
                    lstAdvice.Add(child);
                }
                result.Add(new
                {
                    AdviceContent = item.Key.AdviceContent,
                    AdviceInfo = lstAdvice
                });
            }

            return this.MyJson(result);
        }

        /// <summary>
        /// 待配液清单
        /// LesionID,PatientID,HomepageID,Baby,RequestTime
        /// </summary>
        /// <returns></returns>
        public ActionResult AdviceGetdosagelistBq()
        {
            var sod = GetParams();
            //入参没有包含时间,则传入当天时间
            if (!sod.ContainsKey("StartTime"))
                sod["StartTime"] = DateTime.Now;
            if (!sod.ContainsKey("EndTime"))
                sod["EndTime"] = DateTime.Now;

            var service = ThirdServiceContext.FindService("3e4f3d0f-5950-4107-a4ed-ab12d7d1a569");
            if (service == null)
                return this.MyJson(0, "服务定义引用错误。");
            //var list = service.DoService(new
            //{
            //    HomePageID = 1,
            //    PatientID = 52134,
            //    Baby = 0,
            //    StartTime = DateTime.Now,
            //    EndTime = DateTime.Now,
            //    LesionID = 426,
            //}.toStrObjDict());
            var list = service.DoService(sod);

            var group = list.GroupBy(g => new
            {
                RequestTime = g["RequestTime"],
                RelatID = g["RelatID"]
            }).Where(g => g.Key.RelatID != null);

            var result = new List<dynamic>();
            foreach (var item in group)
            {
                var info = item.Select(CreateDosageBq).Cast<dynamic>().ToList();
                var parent = list.FirstOrDefault(a => a.GetString("ID").Equals(item.Key.RelatID.ToString()) &&
                                                    a.GetString("RequestTime").Equals(item.Key.RequestTime.ToString()) &&
                                                    string.IsNullOrEmpty(a.GetString("RelatID")));
                info.Add(CreateDosageBq(parent));
                result.Add(new
                {
                    info = info,
                    FullName = parent.GetString("FullName"),
                    ExcuteRate = parent.GetString("ExcuteRate"),
                    RequestTime = parent.GetString("RequestTime")
                });
            }
            return this.MyJson(result);
        }

        object CreateDosageBq(StrObjectDict g)
        {
            return new
            {
                ID = g.GetString("ID"),
                RelatID = g.GetString("RelatID"),
                Nurse = g.GetString("Nurse"),
                PatientID = g.GetString("PatientID"),
                HomepageID = g.GetString("HomepageID"),
                Baby = g.GetString("Baby"),
                FullName = g.GetString("FullName"),
                Sex = g.GetString("Sex"),
                Age = g.GetString("Age"),
                OutHospitalBed = g.GetString("OutHospitalBed"),
                SendNumber = g.GetString("SendNumber"),
                RequestTime = g.GetString("RequestTime"),
                AdviceDeadline = g.GetString("AdviceDeadline"),
                DiagnoseCategory = g.GetString("DiagnoseCategory"),
                OperationCategory = g.GetString("OperationCategory"),
                ExecuteCategory = g.GetString("ExecuteCategory"),

                AdviceContent = g.GetString("AdviceContent"),
                DoctorAdvice = g.GetString("DoctorAdvice"),
                Name = g.GetString("Name"),
                Norms = g.GetString("Norms"),
                Gross = g.GetString("Gross"),
                OnceYield = g.GetString("OnceYield"),
                GrossUnit = g.GetString("GrossUnit"),
                Unit = g.GetString("Unit"),

                ExcuteTime = g.GetString("ExcuteTime"),
                ExcuteRate = g.GetString("ExcuteRate"),
                ExcuteTimeScheme = g.GetString("ExcuteTimeScheme"),
                CreateAdvDoctor = g.GetString("CreateAdvDoctor"),
                CreateAdvTime = g.GetString("CreateAdvTime"),
                StopAdvDoctor = g.GetString("StopAdvDoctor"),
                StopAdvTime = g.GetString("StopAdvTime"),
                UrgentMark = g.GetString("UrgentMark"),
                DemoCode = g.GetString("DemoCode"),
                Orgin = g.GetString("Orgin")
            };
        }

        /// <summary>
        /// 已配液
        /// Operator  RequestTime  LesionID
        /// </summary>
        /// <returns></returns>
        public ActionResult AdviceGetdosagelog()
        {
            var sod = GetParams();
            //入参没有包含时间,则传入当天时间
            if (!sod.ContainsKey("StartTime"))
                sod["StartTime"] = DateTime.Now;
            if (!sod.ContainsKey("EndTime"))
                sod["EndTime"] = DateTime.Now;

            var service = ThirdServiceContext.FindService("72f681b9-f524-440a-9094-aceb7144f5fa");
            if (service == null)
                return this.MyJson(0, "服务定义引用错误。");
            //var list = service.DoService(new
            //{
            //    HomePageID = 1,
            //    PatientID = 52134,
            //    Baby = 0,
            //    StartTime = DateTime.Now,
            //    EndTime = DateTime.Now,
            //    LesionID = 426
            //}.toStrObjDict());

            var list = service.DoService(sod);

            var group = list.GroupBy(g => new { RequestTime = g["RequestTime"], RelatID = g["RelatID"] }).
                            Where(g => g.Key.RelatID != null);

            var result = new List<dynamic>();
            foreach (var item in group)
            {
                var info = new List<dynamic>();
                foreach (var g in item)
                {
                    info.Add(CreateDosage(g));
                }
                var parent = list.FirstOrDefault(a => a.GetString("ID").Equals(item.Key.RelatID.ToString()) &&
                                                    a.GetString("RequestTime").Equals(item.Key.RequestTime.ToString()) &&
                                                    string.IsNullOrEmpty(a.GetString("RelatID")));
                info.Add(CreateDosage(parent));
                result.Add(new
                {
                    info = info,
                    FullName = parent.GetString("FullName"),
                    ExcuteRate = parent.GetString("ExcuteRate"),
                    RequestTime = parent.GetString("RequestTime"),
                    DosingTime = parent.GetString("DosingTime")
                });
            }
            return this.MyJson(result);
        }

        object CreateDosage(StrObjectDict sod)
        {
            return new
            {
                PatientID = sod.GetString("PatientID"),
                HomePageID = sod.GetString("HomePageID"),
                Baby = sod.GetString("Baby"),
                FullName = sod.GetString("FullName"),
                Sex = sod.GetString("Sex"),
                Age = sod.GetString("Age"),
                OutHospitalBed = sod.GetString("OutHospitalBed"),
                ID = sod.GetString("ID"),
                RelatID = sod.GetString("RelatID"),
                SendNumber = sod.GetString("SendNumber"),
                RequestTime = sod.GetString("RequestTime"),
                AdviceDeadline = sod.GetString("AdviceDeadline"),
                DiagnoseCategory = sod.GetString("DiagnoseCategory"),
                OperationCategory = sod.GetString("OperationCategory"),
                ExecuteCategory = sod.GetString("ExecuteCategory"),
                AdviceContent = sod.GetString("AdviceContent"),
                DoctorAdvice = sod.GetString("DoctorAdvice"),
                Name = sod.GetString("Name"),
                Norms = sod.GetString("Norms"),
                Gross = sod.GetString("Gross"),
                OnceYield = sod.GetString("OnceYield"),
                GrossUnit = sod.GetString("GrossUnit"),
                Unit = sod.GetString("Unit"),
                ExcuteTime = sod.GetString("ExcuteTime"),
                ExcuteRate = sod.GetString("ExcuteRate"),
                CreateAdvDoctor = sod.GetString("CreateAdvDoctor"),
                CreateAdvTime = sod.GetString("CreateAdvTime"),
                StopAdvDoctor = sod.GetString("StopAdvDoctor"),
                StopAdvTime = sod.GetString("StopAdvTime"),
                UrgentMark = sod.GetString("UrgentMark"),
                DosingPerson = sod.GetString("DosingPerson"),
                Examiner = sod.GetString("Examiner"),
                DosingTime = sod.GetString("DosingTime"),
                Orgin = sod.GetString("Orgin"),
            };
        }

        /// <summary>
        /// 护理巡视
        /// </summary>
        /// <returns></returns>
        public ActionResult ScoutSickroomGetlists()
        {
            var sod = GetParams();
            //入参没有包含时间,则传入当天时间
            if (!sod.ContainsKey("StartTime"))
                sod["StartTime"] = DateTime.Now;
            if (!sod.ContainsKey("EndTime"))
                sod["EndTime"] = DateTime.Now;

            var service = ThirdServiceContext.FindService("3cc6de9c-96e1-4aaa-bdb7-7bd374537df2");
            if (service == null)
                return this.MyJson(0, "服务定义引用错误。");

            var list = service.DoService(sod);
            return this.MyJson(list);
        }

        /// <summary>
        /// 输液状态
        /// </summary>
        /// <returns></returns>
        public ActionResult TransfusionStatus()
        {
            var service = ThirdServiceContext.FindService("79584c92-57f7-4d20-bca4-8a6aaf1dcfff");
            if (service == null)
                return this.MyJson(0, "服务定义引用错误。");
            var sod = GetParams();
            var list = service.DoService(sod);

            var grouping = list.GroupBy(a => new
            {
                //ID = a.GetString("ID"),
                //Usage = a.GetString("Usage"),
                //RequestTime = a.GetString("RequestTime"),
                //ExcuteRate = a.GetString("ExcuteRate"),
                Status = a.GetString("Status")
            });

            var objs = new List<dynamic>();

            foreach (var item in grouping)
            {
                var drugInfo = new List<dynamic>();
                var drugCount = 0;
                var drugGroruping = item.GroupBy(a => new
                {
                    ID = a.GetString("ID"),
                    Usage = a.GetString("Usage"),
                    RequestTime = a.GetString("RequestTime"),
                    ExcuteRate = a.GetString("ExcuteRate")
                });

                foreach (var drugItem in drugGroruping)
                {
                    var drugList = new List<dynamic>();
                    foreach (var drug in drugItem)
                    {
                        drugList.Add(new
                        {
                            DrugID = drug.GetString("DrugID"),
                            AdviceContent = drug.GetString("AdviceContent"),
                            OnceYield = drug.GetString("OnceYield")
                        });
                        drugCount++;
                    }
                    drugInfo.Add(new
                    {
                        ID = drugItem.Key.ID,
                        Usage = drugItem.Key.Usage,
                        RequestTime = drugItem.Key.RequestTime,
                        ExcuteRate = drugItem.Key.ExcuteRate,
                        DrugList = drugList
                    });
                }
                objs.Add(new
                {
                    Status = item.Key.Status,
                    DrugCount = drugCount,
                    DrugInfo = drugInfo
                });
            }

            return this.MyJson(objs);
        }



        /// <summary>
        /// 交班报告  48373493-109f-4af6-b9dd-64bf4166d345
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeShiftsReport()
        {
            // WebAppContextInit.DbInit();
            var service = ThirdServiceContext.FindService("48373493-109f-4af6-b9dd-64bf4166d345");
            if (service == null)
                return this.MyJson(0, "服务定义引用错误。");
            var sod = GetParams();
            var lesionId = sod.GetString("LesionID");

            //班次 ID为公共代码ID
            var parentID = "485045d0-1fe9-4553-be68-921e3bc44bb4";
            var parameter = new StrObjectDict { { "CodeID", parentID } };
            var listComm = DataServiceManager.Instance.ListSod("LISTSOD_CommonCodeDetail", parameter);

            //交班记录
            var pLog = new StrObjectDict { { "LesionID", lesionId } };
            var changesTationLogs = DataServiceManager.Instance.ListSod("LIST_ChangesTationLogRec", pLog);

            //查询时间对应的报告
            const string dateFormat = "yyyy-MM-dd";
            var date = DateTime.Now.ToString(dateFormat);
            var pDate = sod.GetDate("Date");
            if (pDate != null)
                date = pDate.Value.ToString(dateFormat);
            sod.Remove("Date");
            DateTime rsTime;
            DateTime reTime;
            var rCulCount = new List<dynamic>();//保存各班次人员情况
            var all = new List<StrObjectDict>();
            var generalReport = new List<StrObjectDict>();
            foreach (var common in listComm)
            {
                var remark = common.GetString("Remarks");
                if (string.IsNullOrEmpty(remark))
                    continue;

                var timeArr = remark.Split(',');
                if (timeArr.Length != 2)
                    continue;

                var sTime = string.Format("{0} {1}", date, timeArr[0]);
                var eTime = string.Format("{0} {1}", date, timeArr[1]);
                if (!(DateTime.TryParse(sTime, out rsTime) && DateTime.TryParse(eTime, out reTime)))
                    continue;

                //构造sql查询入参
                sod["StartTime"] = sTime;
                sod["EndTime"] = eTime;

                //查找出在该班次下的记录
                var t = changesTationLogs.Where(g =>
                {
                    var dateTime = g.GetDate("RecordTime");
                    return dateTime != null && (DateTime.Compare(dateTime.Value, rsTime) >= 0 &&
                                                               DateTime.Compare(dateTime.Value, reTime) < 0);
                }).ToList();

                var now = DateTime.Now;
                //判断当前时间是否在班次时间内
                var isCurTime = now.Hour >= rsTime.Hour && now.Hour <= reTime.Hour;

                var lst = service.DoService(sod);
                rCulCount.Add(Convert(lst, sTime, isCurTime, t));

                var grParameter = new StrObjectDict 
                { 
                        { "LesionID", lesionId }, 
                        { "StartTime", sTime }, 
                        { "EndTime", eTime } 
                };
                generalReport.AddRange(GeneralReport(grParameter));

                all.AddRange(lst);
            }

            var group = all.GroupBy(g => new
            {
                PatientID = g.GetString("PatientID"),
                HomePageID = g.GetString("HomePageID"),
                FullName = g.GetString("FullName"),
                BedNumber = g.GetString("BedNumber"),
                Description = g.GetString("Description")
            });

            var patients = new List<dynamic>();
            foreach (var item in group)
            {
                var lstG = new List<StrObjectDict>();
                var oIndex = 0;
                foreach (var g in item.OrderBy(o => o.GetDate("TimeOffset")))
                {
                    var sysContent = string.Format("{0} {1} {2} {3} {4}", g.GetString("BBT"),
                                                                      g.GetString("P"),
                                                                      g.GetString("R"),
                                                                      g.GetString("BP"),
                                                                      g.GetString("NurseDigest"));

                    if (string.IsNullOrEmpty(sysContent) || sysContent.Trim().Equals("/"))
                        sysContent = string.Empty;
                    g.Add("SysContent", sysContent);
                    g.Add("Order", oIndex);
                    lstG.Add(g);
                    oIndex++;
                }
                patients.Add(new
                {
                    Name = item.Key.FullName,
                    BedNumber = item.Key.BedNumber,
                    Description = item.Key.Description,
                    Info = lstG
                });
            }

            //统计总人数
            var gIndex = 0;
            foreach (var item in generalReport.OrderBy(g => g.GetDate("StartTime")))
            {
                item.Add("Order", gIndex);
                var sTime = item.GetDate("StartTime");
                var eTime = item.GetDate("EndTime");
                if (sTime != null) item.Add("StartHour", sTime.Value.Hour);
                if (eTime != null) item.Add("EndHour", eTime.Value.Hour);
                gIndex++;
            }

            return this.MyJson(new { Report = rCulCount.OrderBy(o => o.Times), RangePatients = patients, GeneralReport = generalReport.OrderBy(o => o.GetInt("Order")) });
        }

        /// <summary>
        /// 计算各个班的人数情况
        /// 状态字典 1:入院；2：出院；3：转出；4：转入；5：发热；6：分娩；7：手术；8：死亡；9：病重；10：病危
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="sTime"></param>
        /// <param name="isCurTime"></param>
        /// <param name="changesTationLogs"></param>
        /// <returns></returns>
        object Convert(IList<StrObjectDict> lst, string sTime, bool isCurTime, List<StrObjectDict> changesTationLogs)
        {
            var g = lst.GroupBy(a => a.GetString("Status"));
            var lstGroup = new List<dynamic>();
            var lstNum = new List<int>();
            foreach (var item in g)
            {
                lstGroup.Add(new { Status = item.Key, Count = item.Count(), Name = GetTypeByNum(int.Parse(item.Key)) });
                lstNum.Add(int.Parse(item.Key));
            }
            //补全1-10状态所对应count
            for (var i = 1; i <= 10; i++)
            {
                if (!lstNum.Contains(i))
                    lstGroup.Add(new { Status = i, Count = 0, Name = GetTypeByNum(i) });
            }

            //增加 <时段> 状态
            foreach (var item in lst)
            {
                item.Add("TimeOffset", sTime);
                item.Add("IsCurTime", isCurTime);
                //查询手动输入的数据
                var log = changesTationLogs.FirstOrDefault(c => c.GetString("PatientID").Equals(item.GetString("PatientID")));

                item.Add("Content", log == null ? string.Empty : log.GetString("Content"));
                item.Add("RecordPerson", log == null ? string.Empty : log.GetString("RecordPerson"));
                item.Add("ID", log == null ? string.Empty : log.GetString("ID"));
            }
            return new { Times = sTime, Group = lstGroup.OrderBy(o => int.Parse(o.Status.ToString())) };
        }

        public ActionResult Insert()
        {
            //            var arrayStr = @"医嘱期效 , AdviceDeadline;
            //                            操作类型 , OperationCategory;
            //                            执行分类 , ExecuteCategory;
            //                            医嘱内容 , AdviceContent;
            //                            医生嘱托 , DoctorAdvice;
            //                            规格   ,   Norms;
            //                            开始执行时间, ExcuteTime;
            //                            上次打印时间 ,LastPrintTime;
            //                            执行频次 , ExcuteRate;
            //                            首次时间 ,  FirstTime;
            //                            末次时间 ,  LastTime;
            //                            发送号  ,  SendNumber;
            //                            执行时间方案,  ExcuteTimeScheme;
            //                            频率次数 ,  RateCount;
            //                            频率间隔 ,  RateInterval;
            //                            间隔单位 , IntervalUnit;
            //                            开嘱医生 , CreateAdvDoctor;
            //                            开嘱时间 , CreateAdvTime;
            //                            停嘱医生 , StopAdvDoctor;
            //                            停嘱时间 , StopAdvTime;
            //                            紧急标志 , UrgentMark;
            //                            样本条码 , DemoCode;
            //                            要求时间 , RequestTime;
            //                            开始时间 , BeginTime;
            //                            操作员   , Operator;
            //                            输液反应,  TransfusionReaction;
            //                            总量     , Gross;
            //                            单量   ,   OnceYield;
            //                            总量单位 , GrossUnit;
            //                            单位    ,  Unit";
            var arrayStr = GetParams();
            foreach (var item in arrayStr["Str"].ToString().Split(';'))
            {
                var property = item.Split(',');
                var sql = string.Format("insert into 系统_元数据索引(表字段,应用字段) values('{0}','{1}')",
                                            property[0].Trim(), property[1].Trim());
                Pub.Db.DB.Execute(sql);
            }

            return null;
        }

        private string GetTypeByNum(int num)
        {
            switch (num)
            {
                case 1:
                    return "入院";
                case 2:
                    return "出院";
                case 3:
                    return "转出";
                case 4:
                    return "转入";
                case 5:
                    return "发热";
                case 6:
                    return "分娩";
                case 7:
                    return "手术";
                case 8:
                    return "死亡";
                case 9:
                    return "病重";
                case 10:
                    return "病危";
            }
            return string.Empty;
        }

        /// <summary>
        /// 统计病区患者人数
        /// </summary>
        private IList<StrObjectDict> GeneralReport(StrObjectDict sod)
        {
            var service = ThirdServiceContext.FindService("7ac40243-302e-4cd7-98cb-9dc7ef9c6b6a");
            return service.DoService(sod);
        }
    }
}
