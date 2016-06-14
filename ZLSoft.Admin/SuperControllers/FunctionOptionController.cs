using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;

namespace ZLSoft.Platform.SuperControllers
{
    public class FunctionOptionController:CRUDController
    {
        public override ActionResult List()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> dicts = FunctionManager.Instance.GetOptionList(dict);

            return this.MyJson(dicts);
        }
    }
}
