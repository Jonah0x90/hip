using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.Pub;
using ZLSoft.DalManager;

namespace ZLSoft.Public.Controllers
{
    public class KeyValueController : SessionController
    {
        public override ActionResult Index()
        {
            return this.List();
        }

        public override ActionResult List()
        {
            StrObjectDict reqParam = GetParams();

            string id =reqParam.GetString("ID") ;

            if(string.IsNullOrEmpty(id)){
                return this.MyJson(0,"参数错误!");
            }

            IList<StrObjectDict> result = KeyValueManager.Instance.List(id);

            return this.MyJson(1,result);
        }
    }
}
