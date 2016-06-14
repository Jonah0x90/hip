using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;

namespace ZLSoft.Platform.Controllers
{
    /// <summary>
    /// 部门管理
    /// </summary>
    public class DeptController:BaseController
    {
        /// <summary>
        /// 入口，根据传入部门机构ID返回下辖子部门
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StrObjectDict dict = GetParams();
            string deptID = dict.GetString("DeptID");

            #region 入参有效性校验

            if(string.IsNullOrEmpty(deptID)){
                deptID = "ROOT";
            }
               
            #endregion

            IList<StrObjectDict> list = OrganizationManager.Instance.GetOrgList(new
            {
                SuperID = deptID == "ROOT" ? "" : deptID
            }.toStrObjDict(false));


            return this.MyJson(1,list);
        }

        /// <summary>
        /// 获取部门详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            #region 参数有效性检查

            if(string.IsNullOrEmpty(id)){
                return this.MyJson(0,"参数错误:ID");
            }

            #endregion

            Organization org = OrganizationManager.Instance.LoadByID<Organization>(id);
            return this.MyJson(1,org);
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Save()
        {
            Organization org = new Organization().Bind<Organization>(base.Request);

            #region 检查入参有效性
            //string id = dict.GetString("ID");
            //string name = dict.GetString("Name");
            //string simple = dict.GetString("Simple");
            //string remark = dict.GetString("Remark");
            //string superID = dict.GetString("SuperID");
            //string areaID = dict.GetString("AreaID");
            //string deptType = dict.GetString("DeptType");
            //string deptGrade = dict.GetString("deptGrade");
            //string isFinal = dict.GetString("IsFinal");
            //string isInvalid = dict.GetString("IsInvalid");
            //string inputCode = dict.GetString("InputCode");
            //string seqNo = dict.GetString("SeqNo");
            //string address = dict.GetString("Address");

            #endregion

            if(org.IsInvalid == 1){
                org.InvalidTime = DateTime.Now;
            }

            org.ModifyTime = DateTime.Now;
            org.ModifyUser = LoginSession.Current.NAME;

            bool isOk = OrganizationManager.Instance.InsertOrUpdateOranization(org);

            if (isOk)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            } 
        }


        /// <summary>
        /// 根据ID删除部门
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数错误:ID");
            }

            #endregion

            int isOk = OrganizationManager.Instance.Delete<Organization>(id);
            if(isOk>0){
                return this.MyJson(1,"删除成功");
            }
            else
            {
                return this.MyJson(0,"操作失败");
            }
        }
    }
}
