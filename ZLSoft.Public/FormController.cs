using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.PUB;

namespace ZLSoft.Public.Controllers
{
    public class FormController : MVCController
    {
        public ActionResult GetFormType()
        {
            StrObjectDict dict = GetParams();
            string formType = dict.GetString("FormType");
            string fastCode = dict.GetString("FastCode");

            IList<StrObjectDict> dicts = FormManager.Instance.Hns_GetFormType(new
            {
                FormType = formType,
                FastCode = fastCode
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        public ActionResult ClientFormType()
        {
            StrObjectDict dict = GetParams();
            string formType = dict.GetString("FormType");
            string DeptRange = dict.GetString("DeptRange");
            if (string.IsNullOrEmpty(formType))
            {
                return this.MyJson(0, "参数不能为空:FormType");
            }
            if (string.IsNullOrEmpty(DeptRange))
            {
                return this.MyJson(0, "参数不能为空:DeptRange");
            }

            IList<StrObjectDict> list = FormManager.Instance.Client_GetFormType(dict);

            return this.MyJson(1, list);
        }

        public ActionResult GetForm()
        {
            StrObjectDict dict = GetParams();
            string versionCode = dict.GetString("VersionCode");
            string id = dict.GetString("ID");
            string client = dict.GetString("Client");

            if(string.IsNullOrEmpty(id))
            {
                return this.MyJson(0,"参数不能为空:ID");
            }

            dict["FormID"] = dict["ID"];
            dict.Remove("ID");

            if (!string.IsNullOrEmpty(versionCode))
            {
                IList<StrObjectDict> info = FormManager.Instance.StyleInfo(dict);
                if (client == "PC")
                {
                    dict["Agent"] = info[0]["DS"].ToString();  
                }
                if (client == "Mobile")
                {
                    dict["Agent"] = info[0]["MS"].ToString();
                }
                if (string.IsNullOrEmpty(client))
                {
                    dict["Agent"] = "N/A";
                }
                else
                {
                    dict["Agent"] = client;
                }
            }

            IList<StrObjectDict> dicts = FormManager.Instance.Hns_GetForm(dict);
            dicts.Last().Add("Last", 1);
            return this.MyJson(1, dicts);
        }

        public ActionResult Client_GetForm()
        {
            StrObjectDict dict = GetParams();
            dict["FormID"] = dict["ID"];
            IList<StrObjectDict> result = FormManager.Instance.CheckVerCode(dict);
            try
            {
                if (string.IsNullOrEmpty(result[0]["VersionCode"].ToString()))
                {
                    return this.MyJson(0, "接口错误：请求最新版本号失败");
                }
            }
            catch
            {
                return this.MyJson(0, "接口错误：请求最新版本号失败");
            }
            string versionCode = result[0]["VersionCode"].ToString();
            string id = dict.GetString("ID");

            IList<StrObjectDict> dicts = FormManager.Instance.Hns_GetForm(new
            {
                VersionCode = versionCode,
                FormID = id
            }.toStrObjDict(false));
            dicts.Last().Add("Last", 1);
            return this.MyJson(1, dicts);
        }

        public override ActionResult InsertOrUpdate()
        {
            StrObjectDict dict = GetParams();

            string Struct = dict.GetString("FormStruct");
            string formstyle = dict.GetString("FormStyle");

            if (!dict.ContainsKey("VersionCode"))
            {
                dict["VersionCode"] = "1.0";
            }
            //if (string.IsNullOrEmpty(dict["Pages"].ToString()))
            //{
            //    return this.MyJson(0, "参数不能为空:Pages");
            //}

            dict["ModifyTime"] = DateTime.Now;
            dict["ModifyUser"] = LoginSession.Current.NAME;
            dict["InvalidTime"] = DateTime.Now;
            dict["UpdateUser"] = LoginSession.Current.NAME;
            dict["PageNo"] = "1";

            #region NursingForm
            string name = dict.GetString("Name");
            string fastCode = dict.GetString("FastCode");
            string formType = dict.GetString("FormType");
            string markCode = dict.GetString("MarkCode");
            string modifyUser = LoginSession.Current.NAME;
            DateTime modifyTime = DateTime.Now;
            string deptRange = dict.GetString("DeptRange");
            string formAgainst = dict.GetString("FormAgainst");
            string newVersionCode = dict.GetString("NewVersionCode");
            string designVersion = dict.GetString("DesignVersion");
            string resolution = dict.GetString("Resolution");
            DateTime invalidTime = DateTime.Now;
            string isInvalid = dict.GetString("IsInvalid");
            string ID = dict.GetString("ID");
            #endregion

            #region NursingFormVersion
            string updateUser = LoginSession.Current.NAME;
            string version = dict.GetString("VersionCode");
            string updateRemark = dict.GetString("UpdateRemark");
            string issuer = dict.GetString("Issuer");
            DateTime issueDateTime = DateTime.Now;
            string issueRemark = dict.GetString("IssueRemark");
            #endregion

            #region NursingFormFile
            string pageNumber = dict.GetString("PageNumber");
            string pageNo = dict.GetString("PageNo");
            string pageContent = dict.GetString("PageContent");
            string docment = dict.GetString("Docment");
            string versionCode = dict.GetString("VersionCode");
            string formStruct = dict.GetString("FormStruct");
            #endregion

            if (formType != "2")
            {
                if (string.IsNullOrEmpty(formstyle))
                {
                    return this.MyJson(0, "参数不能为空：FormStyle");
                }
            }
            else
            {
                dict["FormStyle"] = "N/A";
            }

            if (string.IsNullOrEmpty(formType))
            {
                return this.MyJson(0, "参数不能为空：FormType");
            }

            if (string.IsNullOrEmpty(ID))
            {
                string id = Utils.getGUID();
                dict["ID"] = id;
                dict["FormID"] = id;
                switch (dict["FormStyle"].ToString())
                { 
                    //样式A
                    case "4f31eca8-ff29-404f-99f7-8cc8afe395a5":
                        dict["FormStructA"] = dict["FormStruct"];
                        dict["PageContentA"] = dict["PageContent"];
                        dict.Remove("FormStruct");
                        dict.Remove("PageContent");
                        break;
                    //样式B
                    case "c4bbc193-9f77-408c-909c-4073a9da2d01":
                        dict["FormStructB"] = dict["FormStruct"];
                        dict["PageContentB"] = dict["PageContent"];
                        dict.Remove("FormStruct");
                        dict.Remove("PageContent");
                        break;
                    //样式C
                    case "7f9b2933-fe36-45b0-9442-d3fb7384d90e":
                        dict["FormStructC"] = dict["FormStruct"];
                        dict["PageContentC"] = dict["PageContent"];
                        dict.Remove("FormStruct");
                        dict.Remove("PageContent");
                        break;
                    //自适应
                    default:
                        break;
                }
                try
                {
                    var result = FormManager.Instance.FormInsert(dict);
                }
                catch
                {
                    //执行失败删除插入内容
                    var del = FormManager.Instance.NursingFormInsertDelete(id);
                    return this.MyJson(-1, "调用失败：请检查入参是否符合规范或缺失必要入参或表单名称是否重复。");    
                }
                if (dict["FormType"].ToString().Trim() != "2")
                {
                    try
                    {
                        StrObjectDict list = JsonAdapter.FromJsonAsDictionary(Struct).toStrObjDict();
                        var data = FormManager.Instance.InsertData(list, dict);
                    }
                    catch
                    {
                        //执行失败删除插入内容
                        var del = FormManager.Instance.NursingFormInsertDelete(id);
                        return this.MyJson(-1, "InsertOrUpdate接口的错误信息：检测到[报表统计数据]ID存在重复,违反唯一约束条件。数据进行回滚操作。");
                    }
                }
                return this.MyJson(1, "保存成功!");
            }
            else
            {
                switch (dict["FormStyle"].ToString())
                {
                    //样式A
                    case "4f31eca8-ff29-404f-99f7-8cc8afe395a5":
                        dict["FormStructA"] = dict["FormStruct"];
                        dict["PageContentA"] = dict["PageContent"];
                        dict.Remove("FormStruct");
                        dict.Remove("PageContent");
                        break;
                    //样式B
                    case "c4bbc193-9f77-408c-909c-4073a9da2d01":
                        dict["FormStructB"] = dict["FormStruct"];
                        dict["PageContentB"] = dict["PageContent"];
                        dict.Remove("FormStruct");
                        dict.Remove("PageContent");
                        break;
                    //样式C
                    case "7f9b2933-fe36-45b0-9442-d3fb7384d90e":
                        dict["FormStructC"] = dict["FormStruct"];
                        dict["PageContentC"] = dict["PageContent"];
                        dict.Remove("FormStruct");
                        dict.Remove("PageContent");
                        break;
                    //自适应
                    default:
                        break;
                }
                try
                {
                    var result = FormManager.Instance.FormUpdate(dict);
                }
                catch
                {
                    return this.MyJson(-1, "调用失败：请检查入参是否符合规范或缺失必要入参或表单名称是否重复。");
                }
                if (dict["FormType"].ToString().Trim() != "2")
                {
                    try
                    {
                        StrObjectDict list = JsonAdapter.FromJsonAsDictionary(Struct).toStrObjDict();
                        //清除键值对备份数据
                        var del = FormManager.Instance.DelData(dict);
                        //保存键值对备份数据
                        var data = FormManager.Instance.InsertData(list, dict);
                    }
                    catch(Exception msg)
                    {
                        return this.MyJson(-1, msg.Message);
                    }
                }
                return this.MyJson(1, "保存成功!");
            }

        }

        public override ActionResult Delete()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");
            int result = FormManager.Instance.NursingFormInsertDelete(id);
            //清除键值对备份数据
            int data = FormManager.Instance.DelData(dict);
            if (result > 0)
            {
                return this.MyJson(1, "删除成功!");
            }
            else
            {
                return this.MyJson(0, "删除失败!");
            }
        }

        /// <summary>
        /// Type 0 取消 |  1 发布
        /// </summary>
        /// <returns></returns>
        public ActionResult Publish()
        {
            StrObjectDict dict = GetParams();
            var type = dict.GetInt("Type");
            var formId = dict.GetString("FormID");
            var versionCode = dict.GetString("VersionCode");
            if (string.IsNullOrEmpty(formId))
            {
                return this.MyJson(0, "参数不能为空：FormID");
            }
            if (string.IsNullOrEmpty(versionCode))
            {
                return this.MyJson(0, "参数不能为空：VersionCode");
            }

            if (type == 1)
            {
                string formType = dict.GetString("FormType");
                string fastCode = dict.GetString("FastCode");
                dict["Issuer"] = LoginSession.Current.NAME;
            }
            if (type == 0)
            { 
                //检查发布表单状态
                IList<StrObjectDict> state = FormManager.Instance.CheckedPublish(dict);
                //应用中
                if (state.Count > 0)
                {
                    return this.MyJson(0, "表单已应用,无法取消发布.");
                }
            }

            int result = FormManager.Instance.Hns_PublishInsert(dict);
            if (result > 0)
            {
                IList<StrObjectDict> dicts = FormManager.Instance.Hns_Publish(dict);
                return this.MyJson(1, dicts);
            }
            else
            {
                return this.MyJson(0, "接口错误!请重试!");
            }
        }

        public ActionResult SetFormState()
        {
            StrObjectDict dict = GetParams();
            string isInvalid = dict.GetString("IsInvalid");
            string state;
            DateTime invalidTime = DateTime.Now;
            if (!string.IsNullOrEmpty(isInvalid) || isInvalid == "1")
            {
                state = "0";
                isInvalid = "1";
            }
            else
            {
                state = "1";
            }

            IList<StrObjectDict> dicts = FormManager.Instance.Hns_SetFormState(new
            {
                State = state,
                IsInvalid = isInvalid,
                InvalidTime = invalidTime
            }.toStrObjDict(false));

            return this.MyJson(1, dicts);
        }

        public ActionResult GetDepartmentPersonnel()
        {
            StrObjectDict dict = GetParams();

            string typeID = dict.GetString("TypeID");

            if (string.IsNullOrEmpty(typeID))
            {
                return this.MyJson(0, "参数不能为空:TypeID");
            }

            if(typeID == "0")
            {
                dict.Remove("TypeID");
            }

            IList<StrObjectDict> dicts = FormManager.Instance.HNS_GetDepartmentPersonnel(dict);

            return this.MyJson(1, dicts);
        }

        public ActionResult SetFormDepts()
        {
            StrObjectDict dict = GetParams();

            int result = FormManager.Instance.Hns_setFormDepts(dict);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功!");
            }
            else
            {
                return this.MyJson(2, "保存失败!");
            }
        }

        public ActionResult GetEvaluateStyle()
        {
            StrObjectDict dict = GetParams();

            if (!dict.ContainsKey("FormID"))
            {
                dict["Type"] = "2";
                var dicts = FormManager.Instance.HNS_GetEvaluateStyle(dict);
                return this.MyJson(1, dicts);
            }
            dict["ID"] = dict["FormID"];
            dict.Remove("FormID");
            var list = FormManager.Instance.HNS_GetEvaluateStyleByID(dict);
            return this.MyJson(1, list);      
        }

        public ActionResult SetFormMap()
        {
            StrObjectDict dict = GetParams();

            int result = FormManager.Instance.Hns_setFormMap(dict);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功!");
            }
            else
            {
                return this.MyJson(2, "保存失败!");
            }
        }

        public ActionResult CheckFormName()
        {
            StrObjectDict dict = GetParams();

            if (string.IsNullOrEmpty(dict["Name"].ToString()))
            {
                return this.MyJson(0, "参数不能为空:Name");
            }

            IList<StrObjectDict> dicts = FormManager.Instance.CheckName(dict);

            if (dicts.Count > 0)
            {
                return this.MyJson(0, "名字已重复,请更换表单名称.");
            }
            else
            {
                return this.MyJson(1, "表单名称效验通过.");
            }
        }

        public ActionResult GetElementTypes()
        {
            StrObjectDict dict = GetParams();

            string type = dict.GetString("Type");
            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(type))
            {
                return this.MyJson(0, "参数不能为空:Type");
            }
            if (string.IsNullOrEmpty(id))
            {
                //return this.MyJson(0, "参数不能为空:ID");
            }

            IList<StrObjectDict> dicts = FormManager.Instance.Hns_GetElementTypes(dict);

            return this.MyJson(1, dicts);
        }

        public ActionResult GetTypeElements()
        {
            StrObjectDict dict = GetParams();

            if (dict.GetString("Type") != "0")
            {
                if (string.IsNullOrEmpty(dict.GetString("TypeID")))
                {
                    return this.MyJson(0, "参数不能为空:TypeID");
                }
            }
            if (string.IsNullOrEmpty(dict.GetString("FastCode")))
            {
                dict.Remove("FastCode");
            }

            IList<StrObjectDict> dicts = FormManager.Instance.Hns_GetTypeElements(dict);

            return this.MyJson(1, dicts);
        }

        public ActionResult SaveElement()
        {
            StrObjectDict dict = GetParams();

            try
            {
                int result = FormManager.Instance.Hns_SaveElement(dict);
            }
            catch (Exception msg)
            {
                return this.MyJson(0, msg.Message);
            }

            return this.MyJson(1, "保存成功");
        }

        public ActionResult DelElement()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }

            int result = FormManager.Instance.Hns_DelElement(dict);

            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult SaveElementGroup()
        {

            StrObjectDict dict = GetParams();

            dict["ModifyTime"] = DateTime.Now;
            dict["UpdateUser"] = LoginSession.Current.NAME;

            int result = FormManager.Instance.Hns_SaveElementGroup(dict);

            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public ActionResult DelElementGroup()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }

            int result = FormManager.Instance.Hns_DelElementGroup(dict);

            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult SaveEType()
        {
            StrObjectDict dict = GetParams();

            int result = FormManager.Instance.Hns_SaveEType(dict);

            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public ActionResult DelEType()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }

            int result = FormManager.Instance.Hns_DelEType(dict);

            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult SavePType()
        {
            StrObjectDict dict = GetParams();

            int result = FormManager.Instance.Hns_SavePType(dict);

            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public ActionResult DelPType()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }

            int result = FormManager.Instance.Hns_DelPType(dict);

            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult SelectElementGroup()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }

            IList<StrObjectDict> list = FormManager.Instance.Hns_SelectElementGrup(dict);

            return this.MyJson(1, list);
        }

        public ActionResult InsertVer()
        {
            StrObjectDict dict = GetParams();
            string Struct = dict.GetString("FormStruct");
            string id = dict.GetString("FormID");
            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空：FormID");
            }

            IList<StrObjectDict> checkvercode = FormManager.Instance.CheckVerCode(dict);
            IList<StrObjectDict> formtype = FormManager.Instance.GetFormTypes(dict);

            try
            {
                if (string.IsNullOrEmpty(checkvercode[0]["VersionCode"].ToString()))
                {
                    return this.MyJson(-1, "调用失败");
                }
            }
            catch
            {
                return this.MyJson(0, "最新版本号获取失败");
            }

            try
            {
                if (string.IsNullOrEmpty(formtype[0]["Type"].ToString()))
                {
                    return this.MyJson(-1, "调用失败");
                }
            }
            catch
            {
                return this.MyJson(0, "表单类型获取失败");
            }

            dict["FormType"] = formtype[0]["Type"];

            var version = checkvercode[0]["VersionCode"].ToString();
            string temp = FormManager.Instance.NewVerCode(version);

            if (temp == "Error")
            {
                return this.MyJson(-1, "接口错误");
            }

            dict["VersionCode"] = temp;

            dict["PageNumber"] = "1";
            dict["ID"] = Utils.getGUID();

            var result = FormManager.Instance.FormVerInsert(dict);
            //var results = FormManager.Instance.SetVerCode(dict);
            if (dict["FormType"].ToString().Trim() != "2")
            {
                try
                {
                    StrObjectDict list = JsonAdapter.FromJsonAsDictionary(Struct).toStrObjDict();
                    var data = FormManager.Instance.InsertData(list, dict);
                }
                catch(Exception msg)
                {
                    //执行失败删除插入内容
                    FormManager.Instance.FomrVerDelCatch(dict["ID"].ToString());
                    //恢复原先版本号
                    //dict["VersionCode"] = version;
                    //FormManager.Instance.SetVerCode(dict);
                    return this.MyJson(-1, msg.Message);
                }
            }

            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public ActionResult UpdateVer()
        {
            StrObjectDict dict = GetParams();
            string Struct = dict.GetString("FormStruct");

            string id = dict.GetString("ID");
            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }
            dict["FormID"] = dict["ID"];

            IList<StrObjectDict> checkvercode = FormManager.Instance.CheckVerCode(dict);
            IList<StrObjectDict> formtype = FormManager.Instance.GetFormTypes(dict);

            try
            {
                if (string.IsNullOrEmpty(checkvercode[0]["VersionCode"].ToString()))
                {
                    return this.MyJson(-1, "调用失败");
                }
            }
            catch
            {
                return this.MyJson(0, "最新版本号获取失败");
            }

            try
            {
                if (string.IsNullOrEmpty(formtype[0]["Type"].ToString()))
                {
                    return this.MyJson(-1, "调用失败");
                }
            }
            catch
            {
                return this.MyJson(0, "表单类型获取失败");
            }



            dict["VersionCode"] = checkvercode[0]["VersionCode"];

            dict["PageNumber"] = "1";

            var result = FormManager.Instance.FormVerUpdate(dict);

            if (dict["FormType"].ToString().Trim() != "2")
            {
                try
                {
                    var del = FormManager.Instance.DelData(dict);
                    StrObjectDict list = JsonAdapter.FromJsonAsDictionary(Struct).toStrObjDict();
                    var data = FormManager.Instance.InsertData(list, dict);
                }
                catch
                {
                    //执行失败删除插入内容
                    FormManager.Instance.FormVerDel(dict);
                    return this.MyJson(-1, "UpdateVer接口的错误信息：检测到[报表统计数据]ID存在重复,违反唯一约束条件。数据进行回滚操作。");
                }
            }

            if (result > 0)
            {
                return this.MyJson(1, "更新成功");
            }
            else
            {
                return this.MyJson(0, "更新失败");
            }
        }

        public ActionResult DelVer()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");
            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }
            dict["FormID"] = dict["ID"];

            IList<StrObjectDict> checkvercode = FormManager.Instance.CheckVerCode(dict);

            try
            {
                if (string.IsNullOrEmpty(checkvercode[0]["VersionCode"].ToString()))
                {
                    return this.MyJson(-1, "调用失败");
                }
            }
            catch
            {
                return this.MyJson(0, "最新版本号获取失败");
            }

            //string temp = FormManager.Instance.NewVerCode(checkvercode[0]["VersionCode"].ToString());

            //if (temp == "Error")
            //{
            //    return this.MyJson(-1, "接口错误");
            //}

            dict["VersionCode"] = checkvercode[0]["VersionCode"];

            dict["PageNumber"] = "1";

            if (dict["VersionCode"].ToString() == "1.0" || dict["VersionCode"].ToString() == "1")
            {
                var result = FormManager.Instance.FormVerDel(dict);
                var results = FormManager.Instance.NursingFormInsertDelete(id);
                var del = FormManager.Instance.DelData(dict);

                if (result > 0 && results > 0)
                {
                    return this.MyJson(1, "删除成功");
                }
                else
                {
                    return this.MyJson(0, "删除失败");
                }
            }
            else
            {
                var result = FormManager.Instance.FormVerDel(dict);

                if (result > 0)
                {
                    return this.MyJson(1, "删除成功");
                }
                else
                {
                    return this.MyJson(0, "删除失败");
                }
            }    
        }

        public ActionResult GetFormDept()
        {
            StrObjectDict dict = GetParams();

            string ID = dict.GetString("ID");
            if (string.IsNullOrEmpty(ID))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }

            IList<StrObjectDict> list = FormManager.Instance.GetFormDept(dict);

            return this.MyJson(1, list);
        }

        public ActionResult GetFormStyle()
        {
            StrObjectDict dict = GetParams();

            string TypeID = dict.GetString("TypeID");
            if (string.IsNullOrEmpty(TypeID))
            {
                return this.MyJson(0, "参数不能为空：TypeID");
            }

            IList<StrObjectDict> list = FormManager.Instance.GetFormStyle(dict);

            return this.MyJson(1, list);
        }

        public ActionResult SetMobileStyle()
        {
            StrObjectDict dict = GetParams();

            string MobileStyle = dict.GetString("MobileStyle");
            string VersionCode = dict.GetString("VersionCode");
            string Id = dict.GetString("ID");

            if (string.IsNullOrEmpty(MobileStyle))
            {
                return this.MyJson(0, "参数不能为空：MobileStyle");
            }
            if (string.IsNullOrEmpty(VersionCode))
            {
                return this.MyJson(0, "参数不能为空：VersionCode");
            }
            if (string.IsNullOrEmpty(Id))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }

            int result = FormManager.Instance.SetMobileStyle(dict);
            if (result > 0)
            {
                return this.MyJson(1, "设置成功");
            }
            else
            {
                return this.MyJson(0, "设置失败");
            }
        }

        public ActionResult SetDeskTopStyle()
        {
            StrObjectDict dict = GetParams();

            string DesktopStyle = dict.GetString("DesktopStyle");
            string VersionCode = dict.GetString("VersionCode");
            string Id = dict.GetString("ID");

            if (string.IsNullOrEmpty(DesktopStyle))
            {
                return this.MyJson(0, "参数不能为空：DesktopStyle");
            }
            if (string.IsNullOrEmpty(VersionCode))
            {
                return this.MyJson(0, "参数不能为空：VersionCode");
            }
            if (string.IsNullOrEmpty(Id))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }

            int result = FormManager.Instance.SetDesktopStyle(dict);
            if (result > 0)
            {
                return this.MyJson(1, "设置成功");
            }
            else
            {
                return this.MyJson(0, "设置失败");
            }
        }

    }
}
