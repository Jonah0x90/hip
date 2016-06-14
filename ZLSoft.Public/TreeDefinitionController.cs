using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Model.SYS;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Public.Controllers
{
    public class TreeDefinitionController : MVCController
    {
        public override ActionResult List()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = TreeDefinitionManager.Instance.GetList(dict);

            return this.MyJson(1, list);
        }

        public ActionResult Pages()
        {
            StrObjectDict reqParam = GetParams();
            StrObjectDict reqPage = GetPageInfo(); ;

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
                    PageData<StrObjectDict> pd = TreeDefinitionManager.Instance.ListSod(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }

            return this.MyJson(0, "参数不正确");
        }

        public ActionResult Save()
        {
            StrObjectDict dict = GetParams();

            dict["ModifyUser"] = LoginSession.Current.NAME;
            if (dict["IsInvalid"].ToString() == "1")
            {
                dict["Invalidtime"] = DateTime.Now;
            }
    
            int result = TreeDefinitionManager.Instance.InsertOrUpdate(dict);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public override ActionResult Delete()
        {
            StrObjectDict dict = GetParams();
            int result = TreeDefinitionManager.Instance.Delete(dict);
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
}
