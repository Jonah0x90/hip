using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.AppContext;
using ZLSoft.DataFilterChainManager;

namespace ZLSoft.Public.Controllers
{
    public class ShujuqxController:Controller
    {
        public ActionResult Index()
        {
            string GLJB = base.Request.Req("GLJB");
            string KSID = base.Request.Req("KSID");
            string NY = base.Request.Req("NY");
            if (string.IsNullOrEmpty(GLJB))
            {
                GLJB = "0";
            }
            IList<StrObjectDict> ksids = DB.ListSod("LIST2_HL_HLDY", new
            {
                ZFPB = 0,
                ORDER_BY_CLAUSE = "PXXH,SRM1"
            }.toStrObjDict());
            if (GLJB != "2")
            {
                ksids = DataFilterChainsManager.Instance.filterSourceByIDataLimits(ksids, new DataFilterProject("164", "HL_HLDY_GLQX", null), "KSID");
            }
            return null;
        }
    }
}
