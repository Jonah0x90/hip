using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.DalManager;

namespace ZLSoft.HNS.Nurse.Controllers
{
    public class NursingAssessmentController : MVCController
    {
        public ActionResult GetEvaluateFile()
        {
            StrObjectDict dict = GetParams();

            string patientID = dict.GetString("PatientID");
            string type = dict.GetString("Type");
            string departmentID = dict.GetString("DepartmentID");

            if (string.IsNullOrEmpty(patientID))
            {
                return this.MyJson(0, "参数不能为空:PatientID");
            }
            if(string.IsNullOrEmpty(type))
            {
                return this.MyJson(0, "参数不能为空:Type");
            }
            if (string.IsNullOrEmpty(departmentID))
            {
                return this.MyJson(0, "参数不能为空:DepartmentID");
            }

            IList<StrObjectDict> list = NursingAssessmentManager.Instance.GetEvaluateFile(dict);

            return this.MyJson(1, list);
        }

        public ActionResult AddEvaluateFile()
        {
            //StrObjectDict dict = GetParams();

            //string patientID = dict.GetString("PatientID");
            //string formID = dict.GetString("FormID");
            //string departmentID = dict.GetString("DepartmentID");
            //dict["UnitID"] = departmentID;

            //if (string.IsNullOrEmpty(patientID))
            //{
            //    return this.MyJson(0, "参数不能为空:PatientID");
            //}
            //if (string.IsNullOrEmpty(formID))
            //{
            //    return this.MyJson(0, "参数不能为空:FormID");
            //}
            //if (string.IsNullOrEmpty(departmentID))
            //{
            //    return this.MyJson(0, "参数不能为空:DepartmentID");
            //}

            //IList<StrObjectDict> version = NursingAssessmentManager.Instance.GetVersion(dict);
            //if (!string.IsNullOrEmpty(version[0]["MAX(版本号)"].ToString()))
            //{
            //    dict["VersionCode"] = version[0]["MAX(版本号)"];
            //    dict["ModifyUser"] = LoginSession.Current.USERID;
            //    dict["ModifyTime"] = DateTime.Now;

            //    string result = NursingAssessmentManager.Instance.InSertOrUpdateEvaluateFile(dict);

            //    IList<StrObjectDict> list = new List<StrObjectDict>();
            //    switch (result)
            //    { 
            //        case "保存失败":
            //            return this.MyJson(0, "病人护理表单信息保存失败");
            //        case "修改失败":
            //            return this.MyJson(0, "病人护理表单信息修改失败");
            //        case "修改成功":
            //            list = NursingAssessmentManager.Instance.EvaluateFileResult(dict);
            //            return this.MyJson(1, list);
            //        default:
            //            dict["ID"] = result;
            //            list = NursingAssessmentManager.Instance.EvaluateFileResult(dict);
            //            return this.MyJson(1, list);
            //    }             
            //}
            //else
            //{
            //    return this.MyJson(-1, "版本号请求失败");
            //}

            return null;
        }

        public ActionResult GetEvaluateData()
        {
            StrObjectDict dict = GetParams();

            string ID = dict.GetString("ID");
            string pages = dict.GetString("Pages");
            var client = dict.GetString("Client");

            if (string.IsNullOrEmpty(pages))
            {
                return this.MyJson(0, "参数不能为空:Pages");
            }
            if (string.IsNullOrEmpty(ID))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }
            if (string.IsNullOrEmpty(client))
            {
                
            }

            dict["FormID"] = dict["ID"];

            IList<StrObjectDict> info = FormManager.Instance.ClientStyleInfo(dict);
            if (!string.IsNullOrEmpty(client))
            {
                if (client == "PC")
                {
                    if (info[0]["DS"] == null)
                    {
                        return this.MyJson(0, "PC端样式尚未设置");
                    }
                    dict["Agent"] = info[0]["DS"].ToString();
                }
                if (client == "Mobile")
                {
                    if (info[0]["MS"] == null)
                    {
                        return this.MyJson(0, "移动端样式尚未设置");
                    }
                    dict["Agent"] = info[0]["MS"].ToString();
                }
                if (string.IsNullOrEmpty(client))
                {
                    dict["Agent"] = "N/A";
                }
            }

            IList<StrObjectDict> list = NursingAssessmentManager.Instance.GetEvaluateData(dict);

            return this.MyJson(1, list);   
        }


        public ActionResult GetSysElement()
        {
            StrObjectDict dict = GetParams();

            string PatientID = dict.GetString("PatientID");
            if (string.IsNullOrEmpty(PatientID))
            {
                return this.MyJson(0, "参数不能为空:PatientID");
            }

            IList<StrObjectDict> list = NursingAssessmentManager.Instance.GetSysElement(dict);

            return this.MyJson(1, list);   
        }

        public ActionResult SaveEvaluateFile()
        {
            StrObjectDict dict = GetParams();

            string FormID = dict.GetString("FormID");
            string pages = dict.GetString("Pages"); 

            if (string.IsNullOrEmpty(pages))
            {
                return this.MyJson(0, "参数不能为空:Pages");
            }
            if (string.IsNullOrEmpty(FormID))
            {
                return this.MyJson(0, "参数不能为空:FormID");
            }

            IList<StrObjectDict> version = NursingAssessmentManager.Instance.GetVersion(dict);

            try
            {
                if (!string.IsNullOrEmpty(version[0]["MAX(版本号)"].ToString()))
                {
                    dict["VersionCode"] = version[0]["MAX(版本号)"];
                    dict["ModifyUser"] = LoginSession.Current.NAME;
                    dict["ModifyTime"] = DateTime.Now;

                    try
                    {
                        string result = NursingAssessmentManager.Instance.InSertOrUpdateEvaluateFile(dict);
                        //保存到表单数据
                        if (result != "1")
                        {
                            //result = 1时，表明为Update 入参包含ID
                            if (!dict.ContainsKey("ID"))
                            {
                                if (string.IsNullOrEmpty(result))
                                {
                                    dict["ID"] = result;
                                }
                            }
                        }
                        var savedata = NursingAssessmentManager.Instance.SaveFormData(dict);
                    }
                    catch(Exception msg)
                    {
                        return this.MyJson(0, msg.Message);
                    }
                    return this.MyJson(1, "保存成功");
                }
                else
                {
                    return this.MyJson(0, "版本号获取失败!请检查版本号是否尚未设置或为空");
                }
            }
            catch(Exception msg)
            {
                return this.MyJson(0, msg.Message);
            }
        }

        public ActionResult DelEvaluateFile()
        {
            StrObjectDict dict = GetParams();

            string ID = dict.GetString("ID");
            if (string.IsNullOrEmpty(ID))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }

            try
            {
                int result = NursingAssessmentManager.Instance.DelEvaluateFile(dict);
                int del = NursingAssessmentManager.Instance.DelFormData(dict);
            }
            catch (Exception msg)
            {
                return this.MyJson(0, msg.Message);
            }

            return this.MyJson(1, "删除成功");
        }

        public ActionResult GetEvaluateStyle()
        {
            StrObjectDict dict = GetParams();

            string FormID = dict.GetString("FormID");
            if (string.IsNullOrEmpty(FormID))
            {
                return this.MyJson(0, "参数不能为空:FormID");
            }

            IList<StrObjectDict> list = NursingAssessmentManager.Instance.GetEvaluateStyle(dict);

            return this.MyJson(1, list);
            
        }

        public ActionResult GetResultData()
        {
            StrObjectDict dict = GetParams();

            string ID = dict.GetString("ID");

            if (string.IsNullOrEmpty(ID))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }

            IList<StrObjectDict> list = NursingAssessmentManager.Instance.GetResultData(dict);

            return this.MyJson(1, list);
        }

    }
}
