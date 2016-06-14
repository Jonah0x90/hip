using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.DalManager;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Platform.Controllers
{
    public class LogController:CRUDController
    {
        public ActionResult Index()
        {
            return null;  
        }

        public override ActionResult Page()
        {
            StrObjectDict reqParam = GetParams();
            StrObjectDict reqPage = GetPageInfo();

            string Function = reqParam.GetString("Function");
            string Content = reqParam.GetString("Content");
            string ModifyUser = reqParam.GetString("ModifyUser");
            string Level = reqParam.GetString("Level");
            string StartDate = reqParam.GetString("StartDate");
            string EndDate = reqParam.GetString("EndDate");

            if (string.IsNullOrEmpty(Function))
            {
                reqParam.Remove("Function");
            }
            if (string.IsNullOrEmpty(Content))
            {
                reqParam.Remove("Content");
            }
            if (string.IsNullOrEmpty(ModifyUser))
            {
                reqParam.Remove("ModifyUser");
            }
            if (string.IsNullOrEmpty(Level))
            {
                reqParam.Remove("Level");
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
                    PageData<Log> pd = LogManager.Instance.Pages(reqParam, page);
                    return this.MyJson(1, pd.DataList, pd.PageInfo);
                }

            }

            return this.MyJson(0, "参数不正确");


        }

    }
}
