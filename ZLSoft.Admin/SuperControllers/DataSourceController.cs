using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub;

namespace ZLSoft.Platform.SuperControllers
{
    public class DataSourceController:MVCController
    {
        //public ActionResult NameValue()
        //{
        //    StrObjectDict sod = GetParams();
        //    IList<StrObjectDict> list = DataSourceManager.Instance.NameValue<DataSource>(sod);
        //    return this.MyJson(1, list);
        //}

        public override ActionResult Index()
        {
            StrObjectDict sod = GetParams();
            IList<DataSource> list = DataSourceManager.Instance.List<DataSource>(sod);
            return this.MyJson(1,list);
        }


        public ActionResult Save()
        {
            StrObjectDict sod = GetParams();
            int isOk = DataSourceManager.Instance.InsertOrUpdate<DataSource>(sod);
            if(isOk>0){
                return this.MyJson(1,"保存成功");
            }
            return this.MyJson(0,"未知错误");
        }

        public override ActionResult Delete()
        {
            StrObjectDict sod = GetParams();
            int isOk = DataSourceManager.Instance.Delete<DataSource>(sod);
            if (isOk > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            return this.MyJson(0, "未知错误");

        }
    }
}
