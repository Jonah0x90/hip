using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.HR;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HrMng.Controllers
{
    public class BaseEmloyeeInfoController:MVCController
    {
        public override ActionResult Index()
        {
            return null;
        }

        /// <summary>
        /// 获取人员基本档案信息
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        public override ActionResult Load()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            #region 入参正确性校验


            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数为空:ID");
            }

            #endregion

            BaseEmployeeInfo info = BaseEmployeeInfoManager.Instance.LoadByID<BaseEmployeeInfo>(id);
            return this.MyJson(1, info);
        }

    }
}
