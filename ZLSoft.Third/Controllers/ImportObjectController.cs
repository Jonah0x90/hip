using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.THIRD;
using ZLSoft.Pub;

namespace ZLSoft.Third.Controllers
{
    public class ImportObjectController:MVCController
    {


        /// <summary>
        /// 获取平台允许导入的对象列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TableObjectList()
        {
            IList<ImportObject> list = ThirdDataManager.Instance.GetImportDataObjectList();

            return this.MyJson(1, list);
        }



        /// <summary>
        /// 根据导入目标对象获取对象列
        /// </summary>
        /// <returns></returns>
        public ActionResult ObjectColumns()
        {
            StrObjectDict dict = GetParams();
            string tablename = dict.GetString("ObjectName");
            IList<StrObjectDict> list = ThirdDataManager.Instance.GetImportDataObjectColumns(tablename);
            return this.MyJson(1, list);
        }
    }
}
