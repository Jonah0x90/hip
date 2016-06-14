using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.PUB;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Public.Controllers
{
    public class ReportFormController : MVCController
    {
        public ActionResult Analysis()
        {
            StrObjectDict dict = GetParams();

            string sql = dict.GetString("SQL");
            string datasource = dict.GetString("DataSource");
            if (string.IsNullOrEmpty(sql))
            {
                return this.MyJson(0, "参数不能为空：SQL");
            }

            sql = sql.ToUpper();
            //string Table = sql.Substring(sql.LastIndexOf("FROM") + 4);
            //Table = Table.Trim();       

            if (sql.Contains("SELECT") && !sql.Contains("UPDATE") && !sql.Contains("INSERT") && !sql.Contains("DELETE") && !sql.Contains("DROP"))
            {
                IList<string> list = new List<string>();
                try
                {
                    list = ReportFormManager.Instance.SqlAnalysis(datasource, sql);
                }
                catch(Exception msg)
                {
                    return this.MyJson(0, msg.Message);
                }
                return this.MyJson(1, list);
            }
            else
            {
                return this.MyJson(0, "仅限执行SELECT语句");
            }
        }

        public ActionResult SqlExcute()
        {
            StrObjectDict dict = GetParams();

            string sql = dict.GetString("SQL");
            string datasource = dict.GetString("DataSource");
            if (string.IsNullOrEmpty(sql))
            {
                return this.MyJson(0, "参数不能为空：SQL");
            }

            var check = sql.ToUpper();

            if (check.Contains("SELECT") && !check.Contains("UPDATE") && !check.Contains("INSERT") && !check.Contains("DELETE") && !check.Contains("DROP"))
            {
                IList<StrObjectDict> list = new List<StrObjectDict>();
                try
                {
                    list = ReportFormManager.Instance.SqlExcute(datasource, sql);
                }
                catch
                {
                    return this.MyJson(0, "查询失败");
                }
                return this.MyJson(1, list);
            }
            else
            {
                return this.MyJson(0, "仅限执行SELECT语句");
            }
        }

        //public ActionResult GetDataSource()
        //{
        //    StrObjectDict dict = GetParams();

        //    IList<StrObjectDict> list = ReportFormManager.Instance.GetDateSource(dict);

        //    return this.MyJson(1, list);
        //}

        public ActionResult TypeSave()
        {
            StrObjectDict dict = GetParams();
            if (dict.ContainsKey("ID"))
            {
                dict["UpdateUser"] = LoginSession.Current.NAME;
                dict["ModifyTime"] = DateTime.Now;
            }

            int result = ReportFormManager.Instance.TypeInsertOrUpdate(dict);
            if (result > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public ActionResult TypeDel()
        {
            StrObjectDict dict = GetParams();

            int result = ReportFormManager.Instance.TypeDel(dict);
            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult TypeList()
        {
            StrObjectDict dict = GetParams();

            IList<ReportFormType> list = ReportFormManager.Instance.TypeList(dict);

            return this.MyJson(1, list);
        }

        public ActionResult TypePage()
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
                    PageData<StrObjectDict> pd = ReportFormManager.Instance.TypePage(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }
            return this.MyJson(0, "参数不正确");
        }

        public ActionResult TypeData()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = ReportFormManager.Instance.TypeData(dict);

            return this.MyJson(1, list);
        }

        public ActionResult SourcesSave()
        {
            StrObjectDict dict = GetParams();
            string Name = dict.GetString("Name");
            if (string.IsNullOrEmpty(Name))
            {
                return this.MyJson(0, "参数不能为空:Name");
            }

            if (dict.ContainsKey("ID"))
            {
                dict["UpdateUser"] = LoginSession.Current.NAME;
                dict["ModifyTime"] = DateTime.Now;
            }

            string result = ReportFormManager.Instance.SourcesInsertOrUpdate(dict);
            if (result != "Error")
            {
                StrObjectDict succeed = new StrObjectDict();
                succeed["ID"] = result;
                return this.MyJson(1, succeed);
            }
            else
            {
                return this.MyJson(0, "保存失败");
            }
        }

        public ActionResult SourcesDel()
        {
            StrObjectDict dict = GetParams();

            int result = ReportFormManager.Instance.SourcesDel(dict);
            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult SourcesList()
        {
            StrObjectDict dict = GetParams();

            string ReportID = dict.GetString("ReportID");
            if (string.IsNullOrEmpty(ReportID))
            {
                return this.MyJson(0, "参数不能为空:ReportID");
            }

            IList<StrObjectDict> list = ReportFormManager.Instance.SourcesList(dict);

            return this.MyJson(1, list);
        }

        public ActionResult SourcesPage()
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
                    PageData<StrObjectDict> pd = ReportFormManager.Instance.SourcesPage(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }
            return this.MyJson(0, "参数不正确");
        }

        public ActionResult SourcesData()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = ReportFormManager.Instance.SourcesData(dict);

            return this.MyJson(1, list);
        }

        public ActionResult FileSave()
        {
            StrObjectDict dict = GetParams();

            if (dict.ContainsKey("ID"))
            {
                dict["UpdateUser"] = LoginSession.Current.NAME;
                dict["ModifyTime"] = DateTime.Now;
            }
            string result = null;
            try
            {
                result = ReportFormManager.Instance.FileInsertOrUpdate(dict);
            }
            catch (Exception msg)
            {
                return this.MyJson(0,msg.Message);
            }
            return this.MyJson(1,result);
        }

        public ActionResult FileDel()
        {
            StrObjectDict dict = GetParams();

            int result = ReportFormManager.Instance.FileDel(dict);
            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "删除失败");
            }
        }

        public ActionResult FileList()
        {
            StrObjectDict dict = GetParams();

            IList<ReportFormFile> list = ReportFormManager.Instance.FileList(dict);

            return this.MyJson(1, list);
        }

        public ActionResult FilePage()
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
                    PageData<StrObjectDict> pd = ReportFormManager.Instance.FilePage(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }
            return this.MyJson(0, "参数不正确");
        }

        public ActionResult FileData()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = ReportFormManager.Instance.FileData(dict);

            return this.MyJson(1, list);
        }
    }
}
