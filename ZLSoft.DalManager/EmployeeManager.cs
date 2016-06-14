using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.HR;
using ZLSoft.Pub;

namespace ZLSoft.DalManager
{
    public class EmployeeManager:CRUDManager
    {
        private static EmployeeManager _Instance = new EmployeeManager();

        public static EmployeeManager Instance
        {
            get
            {
                return EmployeeManager._Instance;
            }
        }

        private EmployeeManager()
        {

        }

        public IList<Employee> List(string keyword)
        {
            return this.List<Employee>(new
            {
                InputCode = keyword+"%"
            }.toStrObjDict());
        }

        public IList<StrObjectDict> GetSimpleEmpListSod(StrObjectDict dict)
        {
            return this.ListSod("LIST2_Employee", dict);
        }

        public IList<StrObjectDict> GetEmpListSod(StrObjectDict dict)
        {
            return this.ListSod("LIST3_Employee", dict);
        }
    }
}
