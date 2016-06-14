using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Model.PUB;
using ZLSoft.Model.Tree;

namespace ZLSoft.Public.Controllers
{
    public class OrganizationsController:BaseController
    {

        public ActionResult Index()
        {
            return null;
        }

        /// <summary>
        /// 获取组织机构(所有)
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            //var o = Request.Form;
            //string type = Request.Req("Type");//取病区还是取科室。1科室。2病区
            StrObjectDict dict = GetParams();
            //int depttype = 0;
            //if (!int.TryParse(type, out depttype))
            //{
            //    return Json(new
            //    {
            //        Flag = 0,
            //        Error = "病区类别参数错误"
            //    }, JsonRequestBehavior.AllowGet);
            //}

            IList<StrObjectDict> list = OrganizationManager.Instance.GetOrgList(dict);

            return this.MyJson(1,list);
        }

        public ActionResult Tree()
        {
            //组织机构树形结构ID 1ad0e672-aba8-432e-73d4-8a30a699

            Tree tree = TreeManager.Instance.GetTree("1ad0e672-aba8-432e-73d4-8a30a699", "SuperID", null);

            return this.MyJson(1, tree.data);
        }


        /// <summary>
        /// 获取所分配的病区
        /// </summary>
        /// <returns></returns>
        public ActionResult ListWard()
        {
            string userID = LoginSession.Current.USERID;
            IList<StrObjectDict> dict = OrganizationManager.Instance.GetOrgListByUser(new
            {
                UserID = userID,
                DepartType = 2
            }.toStrObjDict(false));
            return this.MyJson(1,dict);
        }


        /// <summary>
        /// 获取所分配的科室
        /// </summary>
        /// <returns></returns>
        public ActionResult ListDepart()
        {
            string userID = LoginSession.Current.USERID;
            IList<StrObjectDict> dict = OrganizationManager.Instance.GetOrgListByUser(new
            {
                UserID = userID,
                DepartType = 1
            }.toStrObjDict(false));
            return this.MyJson(1, dict);
        }


        /// <summary>
        /// 根据ID组织机构详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ListInfo()
        {

            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数错误:ID");
            }

            #endregion

            Organization org = OrganizationManager.Instance.LoadByID<Organization>(id);
            return this.MyJson(1, org);
        }

        /// <summary>
        /// 获取所有关联病区或科室的组织机构列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RelationList()
        {
            //SELECT * FROM 系统_组织机构 LEFT JOIN 系统_组织机构_病区科室关联 ON 系统_组织机构.ID=系统_组织机构_病区科室关联.病区相关ID OR 系统_组织机构.ID=系统_组织机构_病区科室关联.科室相关ID

            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = OrganizationManager.Instance.GetOrgRltList(dict);

            return this.MyJson(1, list);
        }

        public ActionResult CheckedName()
        {
            StrObjectDict dict = GetParams();

            string Name = dict.GetString("Name");

            if (string.IsNullOrEmpty(Name))
            {
                return this.MyJson(0, "参数不能为空Name");
            }

            IList<StrObjectDict> list = OrganizationManager.Instance.CheckedName(dict);

            if (int.Parse(list[0]["Name"].ToString()) > 0)
            {
                return this.MyJson(0, "效验失败：已存在的名称");
            }
            else
            {
                return this.MyJson(1, "效验通过");
            }
        }
         

        /// <summary>
        /// 保存组织机构信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Save()
        {
            StrObjectDict dict = GetParams();

            #region 检查入参有效性
            string name = dict.GetString("Name");

            if (string.IsNullOrEmpty(name))
            {
                return this.MyJson(0, "参数错误:name");
            }

            #endregion

            #region 关联科室或病区
            // 关联科室或病区验证
            //if (dict["DepartType"].ToString() != "0")
            //{
            //    //读取当前ID、关联ID，oracle触发器写入 系统_组织机构_病区科室关联表
            //    /*
            //        CREATE OR REPLACE TRIGGER tr_organizationsrelation
            //        AFTER INSERT OR UPDATE OF ID,相关ID OR DELETE ON 系统_组织机构
            //        FOR EACH ROW
            //        WHEN (new.部门性质 != 0)
            //        BEGIN
            //            CASE
            //            WHEN INSERTING THEN
            //                INSERT INTO 系统_组织机构_病区科室关联 VALUES (:new.ID,:new.相关ID,'0');
            //            WHEN UPDATING THEN
            //                UPDATE 系统_组织机构_病区科室关联 SET 病区相关ID=:new.ID, 科室相关ID=:new.相关ID WHERE 病区相关ID=:old.ID;
            //            WHEN DELETING THEN
            //                DELETE 系统_组织机构_病区科室关联 WHERE 病区相关ID=:old.ID;
            //            END CASE;
            //        END;
            //     */
            //} 
            #endregion

            if (dict["IsInvalid"].ToString() == "1")
            {
                dict["InvalidTime"] = DateTime.Now;
            }

            dict["ModifyTime"] = DateTime.Now;
            dict["ModifyUser"] = LoginSession.Current.NAME;

            int result = OrganizationManager.Instance.InsertOrUpdate<Organization>(dict);

            if (result > 0)
            {
                if (dict["DepartType"].ToString() != "0")
                {
                    int results = OrganizationManager.Instance.InsertRelation(dict);  //插入关联信息
                }
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }


        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            if (id == "123")
            {
                return this.MyJson(0, "ROOT节点无法删除");
            }

            #region 参数有效性检查

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数错误:ID");
            }

            #endregion

            int isOk = OrganizationManager.Instance.Delete<Organization>(id);
            if (isOk > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "操作失败");
            }
        }

        /// <summary>
        /// 请求院区列表
        /// </summary>
        /// <returns></returns>
        public ActionResult HospotalDistrict()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> dicts = OrganizationManager.Instance.ListHospital(dict);

            return this.MyJson(dicts);
        }

    }
}
