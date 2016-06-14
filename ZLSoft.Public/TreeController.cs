using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.Tree;
using ZLSoft.Pub;

namespace ZLSoft.Public.Controllers
{
    /// <summary>
    /// 控制所有树形结构数据
    /// </summary>
    public class TreeController:SessionController
    {
        /// <summary>
        /// 组装所有树形结构
        /// </summary>
        /// <returns></returns>
        public ActionResult Data()
        {
            StrObjectDict dict = GetParams();

            string moduleCode = dict.GetString("ModuleCode");//应用模块代码
            string ID = dict.GetString("ID");//树ID
            string groupID = dict.GetString("GroupID");//角色组ID

            dict["UserID"] = LoginSession.Current.USERID;

            #region 入参合法性校验

            if(string.IsNullOrEmpty(moduleCode)){
                //return this.MyJson(0, "参数不能为空:ModuleCode");
            }

            if (string.IsNullOrEmpty(ID))
            {
                return this.MyJson(0, "参数不能为空:ID");
            }


            #endregion

            //Object param = null;

            //switch (ID)
            //{
            //    case "1ad0e672-aba8-432e-83d4-7a30a999":   //主菜单
            //        param = new 
            //        {
            //            UserID = LoginSession.Current.USERID,
            //            ModuleCode = moduleCode 
            //        };
            //        break;
            //    case "1ad0e672-aba8-432e-73d4-8a30a699":  //组织机构
            //        goto case "1ad0e672-aba8-432e-83d4-7a30a999";
            //    case "2ad0e672-aba8-432e-83d4-7a30a999":  //主菜单_无权限
            //        param = new 
            //        { 
            //            ModuleCode = moduleCode
            //        };
            //        break;
            //    case "3ad0e672-aba8-432e-83d4-7a30a999": //菜单权限维护
            //        param = new
            //        {
            //            ModuleCode = moduleCode,
            //            GroupID = groupID
            //        };
            //        break;
            //    default:
            //        break;
            //}

            

            //主菜单树形结构ID为1ad0e672-aba8-432e-83d4-7a30a999
            //组织机构树形结构为1ad0e672-aba8-432e-73d4-8a30a699
            //Tree tree = TreeManager.Instance.GetTree(ID, "SuperID", param.toStrObjDict());
            Tree tree = TreeManager.Instance.GetTree(ID, "SuperID", dict);

            return this.MyJson(1,tree.data);
        }

        public ActionResult Index()
        {
            return this.Data();
        }

        public ActionResult CommonCodeDetail()
        {
            Tree tree = TreeManager.Instance.GetTree("1ad0e672-aba8-432e-83d4-7a30a699", "SuperiorID", null);

            return this.MyJson(1, tree.data);
        }


        /// <summary>
        /// 根据菜单ID获取菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            Tree tree = TreeManager.Instance.GetTree("1ad0e672-aba8-432e-83d4-7a30a999", "SuperiorID", new
            {
                UserID = "123"
            }.toStrObjDict());

            return this.MyJson(1, tree.data);
        }

        public ActionResult Query()
        {
            return this.MyJson(1, "");
        }

        //SELECT A.ID, "名称" as "Name", A."上级ID" as "SuperID",A."图标" as "PicUrl", decode((SELECT "菜单操作ID" FROM "系统_角色权限" WHERE "角色ID"='$GroupID$' AND "菜单ID"= A.ID AND ROWNUM = '1' ),Null,0,1)AS "IsChecked" FROM "系统_菜单" A where "模块代码"='$ModuleCode$' order by "排序序号" asc

    }
}
