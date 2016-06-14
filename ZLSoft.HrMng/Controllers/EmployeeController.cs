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
    public class EmployeeController:MVCController
    {
        public override ActionResult Index()
        {
            StrObjectDict dict = GetParams();
            string orgID = dict.GetString("OrgID");
            string keyword = dict.GetString("KeyWord");

            #region 参数有效性校验
            if(string.IsNullOrEmpty(orgID)){
                return this.MyJson(0,"参数错误:OrgID");
            }

            #endregion

            IList<StrObjectDict> list = EmployeeManager.Instance.GetEmpListSod(new
            {
                KeyWord = keyword,
                OrgID = orgID
            }.toStrObjDict());

            return this.MyJson(1,list);
        }


        /// <summary>
        /// 根据输入码获取人事职工列表
        /// </summary>
        /// <returns></returns>
        [AuthIgnore]
        public ActionResult SimpleEmpList()
        {
            StrObjectDict dict = GetParams();
            string keyword = dict.GetString("InputCode");

            #region 入参正确性校验


            //if (string.IsNullOrEmpty(keyword))
            //{
            //    return this.MyJson(0, "参数为空:InputCode");
            //}

            #endregion

            IList<StrObjectDict> list = EmployeeManager.Instance.GetSimpleEmpListSod(new
            {
                KeyWord = keyword
            }.toStrObjDict(false));



            return this.MyJson(1, list);
        }


    }
}
