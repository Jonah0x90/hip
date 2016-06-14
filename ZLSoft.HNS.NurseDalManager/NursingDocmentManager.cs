using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.Tree;

namespace ZLSoft.HNS.NurseDalManager
{
    public class NursingDocmentManager : CRUDManager
    { 
        #region 构造

        private static NursingDocmentManager _instance = new NursingDocmentManager();
        public static NursingDocmentManager Instance
        {
            get
            {
                return NursingDocmentManager._instance;
            }
        }
        private NursingDocmentManager()
        {
        }

        #endregion

        public IList<StrObjectDict> GetFormTree(IDictionary<string, object> obj)
        {
            IList<StrObjectDict> list = DB.ListSod("GetListTree_FormFileData", obj);
            for (int i = 0; i < list.Count; i++)
            {
                list[i]["SuperID"] = "Top";
            }
            StrObjectDict temp = new StrObjectDict();
            temp.Add("ID", "Top");
            temp.Add("Name", "评估信息");
            temp.Add("SuperID", "ROOT");
            list.Add(temp);
            return list;
        }

        public Tree GetTree(IList<StrObjectDict> data, string relColumn, StrObjectDict paramDict)
        {
            Tree tree = new Tree();
            tree.datasList = data;
            tree.TransformTree(relColumn);
            return tree;
        }
    }
}
