using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Model.THIRD;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.Platform.SuperControllers
{
    public class DataServiceController:MVCController
    {

        public override ActionResult Index()
        {
            IList<DataService> list = DataServiceManager.Instance.List<DataService>(null);

            return this.MyJson(0,list);
        }

        public ActionResult Save()
        {
            StrObjectDict sod = GetParams();
            int isOk = 0;
            if (sod.GetInt("Type") == 4 && sod.GetInt("IsLocalStorage") == 1 && sod.GetInt("IsRef") == 1)
            {
                if (!sod.ContainsKey("Reference") || sod["Reference"] == null)
                {
                    return this.MyJson(0, "该方式必须输入列对照");
                }
                //使用数据源服务方式,需要保存列对照
                isOk = DataServiceManager.Instance.SaveThirdServiceSqlType(sod);
            }
            else
            {
                sod["IsLocalStorage"] = 0;
                sod["IsRef"] = 0;
                sod["RefObject"] = null;
                isOk = DataServiceManager.Instance.InsertOrUpdate<DataService>(sod);
            }

            if (isOk > 0)
            {
                return this.MyJson(1, "保存成功");
            }

            return this.MyJson(0, "操作失败");
        }

        /// <summary>
        /// 获得服务列数据对照(sql方式)
        /// </summary>
        /// <returns></returns>
        public ActionResult Columns()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            IList<DataServiceColumnMap> list = DataServiceManager.Instance.GetColumnMapList(id);

            return this.MyJson(1,list);
        }


        /// <summary>
        /// 获取sql语句查询列
        /// </summary>
        /// <returns></returns>
        public ActionResult SqlDataColumns()
        {
            StrObjectDict dict = GetParams();

            string datasource = dict.GetString("dataSource");
            string sql = dict.GetString("InputTemplate");

            IList<string> list = ThirdDataManager.Instance.GetSqlMeta(datasource, sql);

            return this.MyJson(1,list);
        }
    }
}
