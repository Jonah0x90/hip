using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Platform.SuperControllers
{
    public class FunctionController:CRUDController
    {
        public ActionResult Save()
        {
            StrObjectDict dict = GetParams();
            StrObjectDict option = GetData("Option");

            #region 入参检查
            string Name = dict.GetString("Name");
            if (string.IsNullOrEmpty(Name))
            {
                return this.MyJson(0, "参数不能为空：Name");
            }
            #endregion

            int result = FunctionManager.Instance.InsertOrUpdate(dict, option);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }

        }

        //public  override ActionResult Delete()
        //{
        //    StrObjectDict dict = GetParams();
        //    StrObjectDict option = GetData("Option");

        //    #region 入参检查
        //    string id = dict.GetString("ID");
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return this.MyJson(0, "参数不能为空：ID");
        //    }
        //    #endregion

        //    int result = FunctionManager.Instance.Delete(dict,option);
        //    if (result > 0)
        //    {
        //        return this.MyJson(1, "删除成功");
        //    }
        //    else
        //    {
        //        return this.MyJson(0, "删除失败");
        //    }
        //}

        public override ActionResult Page()
        {
            StrObjectDict reqParam = GetParams();
            StrObjectDict reqPage = GetPageInfo();

            string ModifyUser = reqParam.GetString("ModifyUser");
            string StartDate = reqParam.GetString("StartDate");
            string EndDate = reqParam.GetString("EndDate");

            if (string.IsNullOrEmpty(ModifyUser))
            {
                reqParam.Remove("ModifyUser");
            }
            if (string.IsNullOrEmpty(StartDate))
            {
                reqParam.Remove("StartDate");
            }
            if (string.IsNullOrEmpty(EndDate))
            {
                reqParam.Remove("EndDate");
            }

            //分页
            Page page = new Page();
            int pageNum = 0;
            int pageSize = 0;
            if (int.TryParse(reqPage.GetString("PageNum"), out pageNum) && int.TryParse(reqPage.GetString("PageSize"), out pageSize))
            {
                page.PageNumber = pageNum;
                page.PageSize = pageSize;
                if (page.PageSize > 0 && page.PageNumber > 0)
                {
                    PageData<Function> pd = FunctionManager.Instance.Pages(reqParam, page);
                    return this.MyJson(1, pd.DataList, pd.PageInfo);
                }

            }

            return this.MyJson(0, "参数不正确");


        }
    }
}
