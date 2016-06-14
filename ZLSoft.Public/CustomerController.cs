using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;

namespace ZLSoft.Public.Controllers
{
    /// <summary>
    /// 客户信息功能
    /// </summary>
    public class CustomerController:MVCController
    {
        /// <summary>
        /// 获取客户(病人)列表,根据组织机构ID和自定义查询条件
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            StrObjectDict dict = GetParams();
            string orgID = dict.GetString("OrgID");
            string inputCode = dict.GetString("InputCode");
            string filter = dict.GetString("Filter");

            #region 入参有效性验证

            if (string.IsNullOrEmpty(orgID))
            {
                return this.MyJson(0, "参数错误:OrgID");
            }
            if (string.IsNullOrEmpty(inputCode))
            {
                if (dict.ContainsKey("InputCode"))
                {
                    dict.Remove("InputCode");
                }
            }
            else
            {
                var temp = inputCode.ToUpper();
                dict["InputCode"] = temp;
            }
            if(string.IsNullOrEmpty(filter))
            {
                //return this.MyJson(0, "参数错误:Filter");
                filter = "1";
            }

            #endregion

            dict["Name"] = LoginSession.Current.NAME;

            IList<StrObjectDict> list = CustomerManager.Instance.Search(dict);

            return this.MyJson(1,list);
        }



        public ActionResult Save()
        {
            Customer c = new Customer().Bind<Customer>(base.Request);


            #region 数据正确性校验

            int indexID = 1;

            if (!string.IsNullOrEmpty(c.ID))
            {
                //办理入院
                //检查病人是否存在，同时提取住院次数
                Customer c2 = CustomerManager.Instance.LoadObject<Customer, StrObjectDict>("LOAD2_Customer",new
                {
                    ID = c.ID,
                    IN = 1
                }.toStrObjDict());

                if(c!=null){
                    return this.MyJson(0,"当前病人还在院,不允许办理入院");
                }

                object obj = CustomerManager.Instance.GetIndexID(c.ID);
                if(obj == null){
                    return this.MyJson(0,"没有找到该病人");
                }

                indexID = int.Parse(obj.ToString()) + 1;
            }
            #endregion

            int isOk = CustomerManager.Instance.CustomerEnter(c);

            if(isOk>0){
                return this.MyJson(1,"保存成功");
            }

            return this.MyJson(0, "发生错误");
        }


        //public ActionResult FetchData()
        //{

        //}

        public ActionResult GetSystemElement()
        {
            StrObjectDict dict = GetParams();

            string PatientID = dict.GetString("PatientID");
            if (string.IsNullOrEmpty(PatientID))
            { 
                return this.MyJson(0,"参数不能为空：PatientID");
            }

            var list = CustomerManager.Instance.GetSystemElement(dict);

            return this.MyJson(1, list);
        }
    }
}
