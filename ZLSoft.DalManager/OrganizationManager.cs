using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{

    /// <summary>
    ///
    /// </summary>
    public class OrganizationManager:CRUDManager
    {
         private static OrganizationManager _Instance = new OrganizationManager();
        public static OrganizationManager Instance
        {
            get
            {
                return OrganizationManager._Instance;
            }
        }
        private OrganizationManager()
        {
        }


        public IList<StrObjectDict> CheckedName(IDictionary<string, object> obj)
        {
            return DB.ListSod("Checked_Organization", obj);
        }

        /// <summary>
        /// 根据条件获取组织机构列表
        /// </summary>
        /// <returns></returns>
        public IList<StrObjectDict> GetOrgList(IDictionary<string,object> obj)
        {
            return DB.ListSod("LIST_SIMPLE_Organization", obj);
        }


        /// <summary>
        /// 获取组织机构关联列表
        /// </summary>
        /// <returns></returns>
        public IList<StrObjectDict> GetOrgRltList(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_RelationList", obj);
        }


        /// <summary>
        /// 根据用户获取组织机构列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public IList<StrObjectDict> GetOrgListByUser(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_Organization_ByUser", obj);
        }


        public bool InsertOrUpdateOranization(Organization org)
        {
            return this.InsertOrUpdate<Organization>(org.toStrObjDict()) > 0 ? true : false;
        }

        public int InsertRelation(StrObjectDict dict)
        {
            DBState state = null;
            string LesionID = dict["ID"].ToString();
            string DepartmentID = dict["ID"].ToString();
            try
            {
                switch (dict["DepartType"].ToString())
                {
                    case "1":
                        DepartmentID = dict["RelationID"].ToString();
                        break;
                    case "2":
                        LesionID = dict["RelationID"].ToString();
                        break;
                    default:
                        break;
                }
            }
            catch
            { }
            state = new DBState
            {
                Name = "INSERT_OrganizationRelation",
                Param = new
                {
                    LesionID = LesionID,
                    DepartmentID = DepartmentID,
                    IsBaseThird = "0"
                }.toStrObjDict(),
                Type = ESqlType.INSERT
            };
            return DB.Execute(state);
        }

        public IList<StrObjectDict> ListHospital(IDictionary<string, object> obj)
        {
            return DB.ListSod("Get_YQList", obj);
        }
    }
}
