using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.HNS;

namespace ZLSoft.HNS.Controllers
{
    public class NursingFormController : MVCController
    {
        public ActionResult GetFormType()
        {
            StrObjectDict dict = GetParams();
            string formType = dict.GetString("FormType");
            string fastCode = dict.GetString("FastCode");

            IList<StrObjectDict> dicts = NursingFormManager.Instance.Hns_GetFormType(new
            {
                FormType = formType,
                FastCode = fastCode
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        public ActionResult GetForm()
        {
            StrObjectDict dict = GetParams();
            string versionCode = dict.GetString("VersionCode");
            string id = dict.GetString("ID");

            IList<StrObjectDict> dicts = NursingFormManager.Instance.Hns_GetForm(new
            {
                VersionCode = versionCode,
                FormID = id
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        public override ActionResult InsertOrUpdate()
        {
            StrObjectDict dict = GetParams();
            
            #region NursingForm
		    string name = dict.GetString("Name");
            string fastCode = dict.GetString("FastCode");
            string formType = dict.GetString("FormType");
            string markCode = dict.GetString("MarkCode");
            string modifyUser = LoginSession.Current.USERID;
            DateTime modifyTime = DateTime.Now;
            string deptRange = dict.GetString("DeptRange");
            string formAgainst = dict.GetString("FormAgainst");
            string newVersionCode = dict.GetString("NewVersionCode");
            string designVersion = dict.GetString("DesignVersion");
            string resolution = dict.GetString("Resolution");
            DateTime invalidTime = DateTime.Now;
            string isInvalid = dict.GetString("IsInvalid"); 
	        #endregion

            #region NursingFormVersion
            string updateUser = LoginSession.Current.USERID;
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
            string formStruct2 = dict.GetString("FormStruct2");
            string formStruct3 = dict.GetString("FormStruct3"); 
	        #endregion 

            int result = NursingFormManager.Instance.NursingFormInsertOrUpdate(new NursingForm
            {
                Name = name,
                FastCode = fastCode,
                FormType = int.Parse(formType),
                MarkCode = int.Parse(markCode),
                ModifyUser = modifyUser,
                ModifyTime = DateTime.Now,
                DeptRange = deptRange,
                FormAgainst = formAgainst,
                NewVersionCode = newVersionCode,
                DesignVersion = designVersion,
                Resolution = resolution,
                InvalidTime = invalidTime,
                IsInvalid = int.Parse(isInvalid)
            },
            new NursingFormVersion
            {
                VersionCode = version,
                UpdateUser = updateUser,
                UpdateRemark = updateRemark,
                Issuer = issuer,
                IssueDateTime = issueDateTime,
                IssueRemark =issueRemark
            },
            new NursingFormFile
            {
                PageNumber = int.Parse(pageNumber),
                PageNo = int.Parse(pageNo),
                PageContent = pageContent,
                Docment = docment,
                VersionCode = versionCode,
                FormStruct = formStruct,
                FormStruct2 = formStruct2,
                FormStruct3 = formStruct3    
            });

            if (result > 0)
            {
                return this.MyJson(1, "保存成功!");
            }
            else
            {
                return this.MyJson(0, "保存失败!");
            }

        }

        public override ActionResult Delete()
        {
            StrObjectDict dict = GetParams();

            string id = dict.GetString("ID");
            int result = NursingFormManager.Instance.NursingFormInsertDelete(id);
            if (result > 0)
            {
                return this.MyJson(1, "删除成功!");
            }
            else
            {
                return this.MyJson(0, "删除失败!");
            }
        }

        public ActionResult Publish()
        {
            StrObjectDict dict = GetParams();
            string formType = dict.GetString("FormType");
            string fastCode = dict.GetString("FastCode");

            int result = NursingFormManager.Instance.Hns_PublishInsert(dict);
            if (result > 0)
            {
                IList<StrObjectDict> dicts = NursingFormManager.Instance.Hns_Publish(dict);
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
            if (!string.IsNullOrEmpty(isInvalid))
            {
                state = "0";
            }
            else
            {
                state = "1";
            }

            IList<StrObjectDict> dicts = NursingFormManager.Instance.Hns_SetFormState(new
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

            IList<StrObjectDict> dicts = NursingFormManager.Instance.HNS_GetDepartmentPersonnel(dict);

            return this.MyJson(1, dicts);
        }

        public ActionResult SetFormDepts()
        {
            StrObjectDict dict = GetParams();

            int result = NursingFormManager.Instance.Hns_setFormDepts(dict);
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

            IList<StrObjectDict> dicts = NursingFormManager.Instance.HNS_GetEvaluateStyle(dict);

            return this.MyJson(1, dicts);
        }

        public ActionResult SetFormMap()
        {
            StrObjectDict dict = GetParams();

            int result = NursingFormManager.Instance.Hns_setFormMap(dict);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功!");
            }
            else
            {
                return this.MyJson(2, "保存失败!");
            }
        }
    }
}
