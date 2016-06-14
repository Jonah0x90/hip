using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;

namespace ZLSoft.Platform.Controllers
{
    public class SysParametersController : CRUDController
    {
        public ActionResult GetList()
        {
            var dict = GetParams();

            var list = ParmsManager.Instance.list(dict);

            return this.MyJson(1, list);
        }

        public ActionResult Save()
        {
            return null;
        }
    }
}
