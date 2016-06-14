using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.HNS.NurseDalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Model.Tree;
using ZLSoft.Pub;


namespace ZLSoft.HNS.Nurse.Controllers
{
    public class NursingDocmentController : MVCController
    {
        public ActionResult GetFormTree()
        {
            StrObjectDict dict = GetParams();

            string DepartmentID = dict.GetString("DepartmentID");
            string PatientID = dict.GetString("PatientID");
            string Type = dict.GetString("Type");

            if (string.IsNullOrEmpty(DepartmentID))
            {
                return this.MyJson(0, "参数不能为空:DepartmentID");
            }
            if (string.IsNullOrEmpty(PatientID))
            {
                return this.MyJson(0, "参数不能为空:PatientID");
            }

            IList<StrObjectDict> list = NursingDocmentManager.Instance.GetFormTree(dict);

            Tree tree = NursingDocmentManager.Instance.GetTree(list, "SuperID", dict);

            return this.MyJson(1, tree.data);
        }
    }

}
