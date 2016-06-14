using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    public class CustomerManager : CRUDManager
    {
        #region
        private static CustomerManager _Instance = new CustomerManager();
        public static CustomerManager Instance
        {
            get
            {
                return CustomerManager._Instance;
            }
        }
        private CustomerManager()
        {
        }

        #endregion


        /// <summary>
        /// 根据条件返回病人列表
        /// </summary>
        /// <returns></returns>
        public IList<StrObjectDict> Search(StrObjectDict obj)
        {
            if (obj.GetString("Filter") == "1")
            {
                //所有病人
                return this.ListSod("LIST_Customer", obj);
            }
            else if (obj.GetString("Filter") == "2")
            {
                obj["ClientManager"] = obj["Name"];
                return this.ListSod("LIST_Customer", obj);
            }
            else
            {
                return this.ListSod("LIST_Customer", obj);
            }
        }

        public object GetIndexID(string id)
        {
            return DB.Scalar("SELECT_IndexID", new
            {

                ID = id
            }.toStrObjDict());
        }

        /// <summary>
        /// 病人入院
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CustomerEnter(Customer obj)
        {
            IList<DBState> dblist = new List<DBState>();
            if (string.IsNullOrEmpty(obj.ID))
            {
                obj.ID = Utils.getGUID();
                dblist.Add(new DBState
                {
                    Name = obj.MAP_INSERT,
                    Param = obj.ToDict(),
                    Type = ESqlType.INSERT
                });
                dblist.Add(new DBState
                {
                    Name = "INSERT_Customer_State",
                    Param = new
                    {
                        ID = Utils.getGUID(),
                        RelatID = obj.ID,
                        State = 1,
                        StartTime = DateTime.Now,
                        EndTime = DBNull.Value,
                        ModifyUser = obj.ModifyUser,
                        IsBaseThird = 0,
                        ImportTime = DBNull.Value,
                        IndexID = obj.IndexID,
                        WardID = obj.WardID,
                        DeptID = obj.DeptID,
                        CardNo = obj.CardNo,
                        ClientManager = obj.ClientManager
                    }.toStrObjDict(),
                    Type = ESqlType.INSERT
                });
            }
            return DB.Execute(dblist);
        }

        public StrObjectDict GetSystemElement(IDictionary<string,object> obj)
        {
            var data = new StrObjectDict();
            var i = 0;
            IList<StrObjectDict> list = new List<StrObjectDict>();
            List<Object> result = ((Object[])obj.GetObject("SysID")).ToList();
            foreach (var item in result)
            {
                //查询系统项Name
                obj["NameID"] = item;
                var temp = DB.ListSod("Get_SystemElementName", obj);
                //查找系统项Content
                obj["Name"] = temp[0]["Name"].ToString();
                list = DB.ListSod("Get_SystemElement", obj);
                if (list.Count > 0)
                {
                    list[0]["SysID"] = obj["NameID"];
                    data.Add(i.ToString(), list);
                    i++;
                }
                else
                {
                    var id = obj["PatientID"].ToString();
                    obj["ID"] = id.Substring(0, id.IndexOf("."));
                    var datalist = DB.ListSod("Get_SysPatient", obj);
                    if (datalist.Count > 0)
                    {
                        foreach (var items in datalist)
                        {
                            foreach (var info in items)
                            {
                                if (info.Key == obj["Name"].ToString())
                                {
                                    temp[0]["SysID"] = obj["NameID"];
                                    temp[0]["Name"] = obj["Name"];
                                    temp[0]["Value"] = info.Value;
                                    data.Add(i.ToString(), temp);
                                    i++;
                                }
                            }
                        }
                    }
                    else
                    {
                        data.Add(obj["NameID"].ToString(), "查询不到相关记录");
                    }
                }
            }
            return data;
        }
    }
}
