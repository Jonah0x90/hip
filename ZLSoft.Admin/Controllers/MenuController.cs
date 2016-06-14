using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Pub.PageData;
using ZLSoft.Model.PLATFORM;

namespace ZLSoft.Platform.Controllers
{
    //菜单相关功能
    public class MenuController : MVCController
    {
        public ActionResult Save()
        {
            StrObjectDict dict = GetParams();
            StrObjectDict menuoption = GetData("Control");

            #region 检查入参有效性
            string Code = dict.GetString("Code");
            string FunID = dict.GetString("FunID");
            string IsInvalid = dict.GetString("IsInvalid");
            string SuperID = dict.GetString("SuperID");
            string IsRelatFun = dict.GetString("IsRelatFun");
            //string FuncOptID = menuoption.GetString("FuncOptID");

            if (string.IsNullOrEmpty(Code))
            {
                return this.MyJson(0, "参数错误:Code");
            }
            if (string.IsNullOrEmpty(IsInvalid))
            {
                return this.MyJson(0, "参数错误:IsInvalid");
            }
            if (string.IsNullOrEmpty(SuperID))
            {
                return this.MyJson(0, "参数错误:SuperID");
            }


            if (IsRelatFun == "1")
            {
                if (string.IsNullOrEmpty(FunID))
                {
                    return this.MyJson(0, "参数错误:FunID");
                }
            }

            #endregion

            int result = MenuManager.Instance.InsertOrUpdate(dict, menuoption);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }
 
        public ActionResult MenuOptionList()
        {
            StrObjectDict dict = GetParams();

            #region 入参检查
            string MenuID = dict.GetString("MenuID");
            if (string.IsNullOrEmpty(MenuID))
            {
                return this.MyJson(0, "参数不能为空：MenuID");
            }
            #endregion

            IList<MenuOption> list = MenuManager.Instance.List<MenuOption>(dict);

            return this.MyJson(1, list);
        }

    }
}
