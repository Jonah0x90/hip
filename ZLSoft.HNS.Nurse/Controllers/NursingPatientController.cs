using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;

namespace ZLSoft.HNS.Nurse.Controllers
{
    public class NursingPatientController : MVCController
    {
        NursingPatientManager _dal;
        public NursingPatientController()
        {
            _dal = NursingPatientManager.Instance;
        }

        public override ActionResult List()
        {
            var sod = GetParams();
            var pageInfo = GetPageInfo();

            if (sod.GetInt("Type") == 1)
                sod["Nurse"] = LoginSession.Current.NAME;
            var result = _dal.List1(sod, pageInfo);
            return this.MyJson(1, result.DataList, result.PageInfo);
        }

        public ActionResult GetPatients()
        {
            var sod = GetParams();
            var pageInfo = GetPageInfo();
            return this.MyJson(_dal.GetPatients(sod, pageInfo));
        }

        public ActionResult GetNurseLeavelCount()
        {
            var sod = GetParams();
            var result = _dal.GetNurseLeavelCount(sod);
            return this.MyJson(result);
        }

        public ActionResult Getpatiplan()
        {
            var sod = GetParams();
            sod["DeptID"] = LoginSession.Current.DEPTID;
            return this.MyJson(_dal.ListSod("Getpatiplan", sod));
        }
    }
}
