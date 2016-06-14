using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HNS.Controllers
{
    public class ElementTypesController : MVCController
    {
        public ActionResult GetElementType()
        { 
            StrObjectDict dict = GetParams();
            string type = dict.GetString("Type");

            if (string.IsNullOrEmpty(type))
            {
                IList<StrObjectDict> list = ElementTypeManager.Instance.GetElementTypes(dict);
                return this.MyJson(1, list);
            }
            else if (type == "1")
            {
                IList<StrObjectDict> list = ElementTypeManager.Instance.GetElementTypesGruop(dict);
                return this.MyJson(1, list);
            }
            else
            {
                return this.MyJson(0, "Type参数错误。");
            }
        }

    }
}
