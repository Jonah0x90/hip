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
using ZLSoft.Model.Tree;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Platform.Controllers
{
    public class CommonCodeController:CRUDController
    {


        public ActionResult ListCommonCode()
        {

            StrObjectDict reqParam = GetParams();


            IList<StrObjectDict> result = base.crudManager.ListSod<CommonCode>(reqParam);
            Tree tree = new Tree();
            tree.datasList = result;
            tree.TransformTree("SuperID");
            return this.MyJson(1, tree.data);
        }


        public ActionResult GetFromTypeList()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = CommCodeManager.Instance.getCommCodeFrom(dict);

            return this.MyJson(1, list);
        }

        public ActionResult Save()
        {
            StrObjectDict dict = GetParams();

            #region 检查入参有效性
            string CateID = dict.GetString("CateID");
            string IsInvalid = dict.GetString("IsInvalid");
            string Name = dict.GetString("Name");
            string SuperID = dict.GetString("SuperID");
            string CodeID = dict.GetString("CodeID");

            if (string.IsNullOrEmpty(CateID))
            {
                return this.MyJson(0, "参数错误:CateID");
            }
            if (string.IsNullOrEmpty(IsInvalid))
            {
                return this.MyJson(0, "参数错误:IsInvalid");
            }
            if (string.IsNullOrEmpty(Name))
            {
                return this.MyJson(0, "参数错误:Name");
            }
            if (string.IsNullOrEmpty(SuperID))
            {
                return this.MyJson(0, "参数错误:SuperID");
            }
            if (string.IsNullOrEmpty(CodeID))
            {
                return this.MyJson(0, "参数错误:CodeID");
            }

            #endregion
   
            if (dict["IsInvalid"].ToString() == "1")
            {
                dict["InvalidTime"] = DateTime.Now;
            }

            dict["ModifyTime"] = DateTime.Now;
            dict["ModifyUser"] = LoginSession.Current.NAME;

            int result = CommCodeManager.Instance.InsertOrUpdate<CommonCode>(dict);
            int results = CommCodeManager.Instance.InsertOrUpdate<CommonCodeDetail>(dict);
            if (result > 0 && results > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        //public ActionResult Delete()
        //{
        //    StrObjectDict dict = GetParams();

        //    #region 入参检查
        //    string id = dict.GetString("ID");
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return this.MyJson(0, "参数不能为空：ID");
        //    } 
        //    #endregion

        //    int result = CommCodeManager.Instance.Delete<CommonCode>(dict);
        //    int results = CommCodeManager.Instance.Delete<CommonCodeDetail>(dict);
        //    if (result > 0 && results > 0)
        //    {
        //        return this.MyJson(1, "删除成功");
        //    }
        //    else
        //    {
        //        return this.MyJson(0, "删除失败");
        //    }
        //}

        public ActionResult ListCommonCodeDetail()
        {
            StrObjectDict reqData = base.GetHttpData();
            StrObjectDict reqParam = reqData.GetObject("Params").toStrObjDict(false);


            IList<StrObjectDict> result = base.crudManager.ListSod<CommonCodeDetail>(reqParam);
            Tree tree = new Tree();
            tree.datasList = result;
            tree.TransformTree("SuperID");
            return this.MyJson(1, tree.data);
        }

        public ActionResult GetFromRelation()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("CodeID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数错误:CodeID");
            }

            #endregion


            IList<StrObjectDict> dicts = CommCodeManager.Instance.getFromRelation(new
            {
                CodeID = id
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        public ActionResult PagesCommonCode()
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
                    PageData<StrObjectDict> pd = CommCodeManager.Instance.PagesCommCode(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }

            return this.MyJson(0, "参数不正确");
        }

        public ActionResult PageCommonCodeDetail()
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
                    PageData<StrObjectDict> pd = CommCodeManager.Instance.PagesCommCodeDetail(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }

            return this.MyJson(0, "参数不正确");
        }
    }
}
