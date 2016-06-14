using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.Tree;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub;
using ZLSoft.Model.PUB;

namespace ZLSoft.Public.Controllers
{
    //角色相关功能
    public class RoleController : MVCController
    {
        /// <summary>
        /// 获取完整的主菜单和菜单操作
        /// </summary>
        /// <returns>树形结构</returns>
        public ActionResult ListMenu()
        {
            StrObjectDict reqParam = GetParams();
            string moduleCode = reqParam.GetString("ModuleCode");

            IList<StrObjectDict> result = crudManager.List2<Role>(new { ModuleCode = moduleCode }.toStrObjDict(), null);

            Tree tree = new Tree();
            tree.datasList = result;
            tree.TransformTree("SuperID");

            result = tree.data;
            //获取完整菜单
            //Tree tree = TreeManager.Instance.GetTree("2ad0e672-aba8-432e-83d4-7a30a999", "SuperID", new { ModuleCode = moduleCode }.toStrObjDict());
            ////获取所有菜单操作
            //IList<MenuOption> menuOptions = crudManager.List2<MenuOption>(new StrObjectDict());

            //foreach (var levelOne in tree.data)
            //{
            //    if (levelOne.ContainsKey("Children"))
            //    {
            //        List<StrObjectDict> levelTwos = (List<StrObjectDict>)levelOne.GetObject("Children");
            //        foreach (var levelTwo in levelTwos)
            //        {
            //            levelTwo.Add("MenuOptions",
            //                from el in menuOptions
            //                where el.MenuID.ToString() == levelTwo.GetString("ID")
            //                select el);
            //        }
            //    }
            //}

            return this.MyJson(1, tree.data);
        }

        /// <summary>
        /// 获取系统模块信息
        /// </summary>
        public ActionResult ModuleList()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = RoleManager.Instance.GetModelList(dict);

            return this.MyJson(1, list);
        }

        /// <summary>
        /// 获取模块下的角色分组信息
        /// </summary>
        public ActionResult RoleGroupList()
        {
            StrObjectDict dict = GetParams();
            string moduleCode = dict.GetString("ModuleCode");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(moduleCode))
            {
                return this.MyJson(0, "参数错误:ModuleCode");
            }

            #endregion

            IList<StrObjectDict> dicts = RoleManager.Instance.GetModelRoleGroup(new
            {
                ModuleCode = moduleCode
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        /// <summary>
        /// 获取角色组未选人员列表
        /// </summary>
        public ActionResult NotSelectedUserList()
        {
            StrObjectDict dict = GetParams();
            string roleGroupID = dict.GetString("RoleGroupID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(roleGroupID))
            {
                return this.MyJson(0, "参数错误:RoleGroupID");
            }

            #endregion

            IList<StrObjectDict> dicts = RoleManager.Instance.GetNotSelectedUser(new
            {
                RoleGroupID = roleGroupID
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);

        }

        /// <summary>
        /// 获取角色组已选人员列表
        /// </summary>
        public ActionResult SelectedUserList()
        {
            StrObjectDict dict = GetParams();
            string roleGroupID = dict.GetString("RoleGroupID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(roleGroupID))
            {
                return this.MyJson(0, "参数错误:RoleGroupID");
            }

            #endregion

            IList<StrObjectDict> dicts = RoleManager.Instance.GetSelectedUser(new
            {
                RoleGroupID = roleGroupID
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        public ActionResult SetPower()
        {
            StrObjectDict dict = GetParams();
            string roleID = dict.GetString("RoleGroupID");
            string powerID = dict.GetString("PowerID");
            dict["RoleID"] = dict["RoleGroupID"];

            #region 参数有效性检查

            if (string.IsNullOrEmpty(roleID))
            {
                return this.MyJson(0, "参数错误:RoleGroupID");
            }
            if (string.IsNullOrEmpty(powerID))
            {
                return this.MyJson(0, "参数错误:PowerID");
            }

            #endregion

            IList<Object> menuIDs = new List<Object>();
            try
            {
                menuIDs = ((Object[])dict.GetObject("PowerID")).ToList();
            }
            catch
            {
                return this.MyJson(0, "至少保留一个菜单项");
            }
            IList<RolePower> list = new List<RolePower>();
            RolePower rp = new RolePower();
            foreach (var item in menuIDs)
            {
                var powers = item.toStrObjDict();
                string powerId = null;
                string menuId = null;
                foreach (var items in powers)
                {
                    if (items.Key == "btnId")
                    {
                        powerId = items.Value.ToString();
                    }
                    if (items.Key == "menuId")
                    {
                        menuId = items.Value.ToString();
                    }
                }
                rp = new RolePower()
                {
                    RoleID = roleID,
                    PowerID = powerId,
                    MenuID = menuId
                };
                list.Add(rp);
            }
            //查询到已被分配权限，执行Delete + Insert
            //Delete
            var delete = RoleManager.Instance.DeletePowers(dict);
            //Insert
            var result = RoleManager.Instance.InsertPowers(list);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功.");
            }
            else
            {
                return this.MyJson(0, "保存失败.");
            }
        }

        public ActionResult DelPower()
        {
            StrObjectDict dict = GetParams();
            string roleID = dict.GetString("RoleGroupID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(roleID))
            {
                return this.MyJson(0, "参数错误:RoleGroupID");
            }
            #endregion


            var delete = RoleManager.Instance.DeletePowers(dict);

            if (delete > 0)
            {
                return this.MyJson(1, "删除成功.");
            }
            else
            {
                return this.MyJson(0, "删除失败.");
            }
        }

        public ActionResult GetPowerTree()
        {
            StrObjectDict dict = GetParams();

            Tree tree = RoleManager.Instance.GetRoleTree(dict);

            return this.MyJson(1, tree.data);
        }
    }
}
