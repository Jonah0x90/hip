using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.Tree;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.Constant;
using ZLSoft.Model.SYS;

namespace ZLSoft.DalManager
{
    public class TreeManager
    {
        #region 构造
        private static TreeManager _Instance = new TreeManager();
        public static TreeManager Instance
        {
            get
            {
                return TreeManager._Instance;
            }
        }
        private TreeManager()
        {
        }
        #endregion

        public Tree GetTree(string id,string relColumn,StrObjectDict paramDict)
        {
            Tree tree = new Tree();
            DBSERVER_TYPE dbType = DB.GetDbtype();
            string sql = "";
            TreeDefinition t = DB.Load<TreeDefinition,PK_TreeDefinition>(new PK_TreeDefinition
            {
                ID = id
            });

            switch (dbType)
            {
                case DBSERVER_TYPE.ORACLE:
                    sql = t.OrcScript;
                    break;
                case DBSERVER_TYPE.MSSQL:
                    sql = t.MssqlScript;
                    break;
                case DBSERVER_TYPE.MYSQL:
                    sql = t.MysqlScript;
                    break;
                case DBSERVER_TYPE.UNDEF:
                    sql = t.OrcScript;
                    break;
                default:
                    sql = t.OrcScript;
                    break;
            }
            sql = sql.Replace("&iexcl;", "?");
            if (paramDict != null)
            {
                foreach (var item in paramDict.Keys)
                {
                    sql = sql.Replace("$" + item + "$", "" + paramDict[item]);
                }
            }


            tree.datasList = DB.Select(sql);
            tree.TransformTree(relColumn);
            return tree;
        }

//         public Tree GetTree(string treedm, StrObjectDict dict, StrObjectDict dict_filter, string checkbox, string root_selected)
//         {
//             string text = "";
//             Tree tree = new Tree();
//             tree.datas_tree = DB.Select("select * from xt_tree where dm='" + treedm + "'");
//             tree.Root_show_flag = Utils.GetString(tree.datas_tree.FirstOrDefault<StrObjectDict>()["ROOT_SHOW_FLAG"]);
//             string text2 = "";
//             if (tree.datas_tree.Count > 0)
//             {
//                 if (DB.GetDbtype() == DBSERVER_TYPE.ORACLE)
//                 {
//                     text = Utils.GetString(tree.datas_tree.FirstOrDefault<StrObjectDict>()["SQL_ORACLE"]);
//                 }
//                 else
//                 {
//                     text = Utils.GetString(tree.datas_tree.FirstOrDefault<StrObjectDict>()["SQL_MSSQL"]);
//                 }
//                 text2 = Utils.GetString(tree.datas_tree.FirstOrDefault<StrObjectDict>()["PXZD"]);
//             }
//             foreach (KeyValuePair<string, object> current in dict)
//             {
//                 text = text.Replace(current.Key, Utils.GetString(current.Value));
//             }
//             string text3 = " 1=1 ";
//             if (dict_filter != null)
//             {
//                 if (dict_filter.Count > 0)
//                 {
//                     foreach (System.Collections.Generic.KeyValuePair<string, object> current in dict_filter)
//                     {
//                         string text4 = text3;
//                         text3 = string.Concat(new string[]
// 						{
// 							text4,
// 							" and  tbl.",
// 							current.Key,
// 							" = '",
// 							Utils.GetString(current.Value),
// 							"'"
// 						});
//                     }
//                 }
//             }
//             text = " select  tbl.* from   (" + text + ")  tbl where " + text3;
//             if (!string.IsNullOrEmpty(text2))
//             {
//                 text = text + " order by " + text2;
//             }
//             tree.datas = DB.Select(text);
//             if (tree.root_show_flag == "1")
//             {
//                 tree.datas.Add(StrObjectDict.FromVariable(new
//                 {
//                     SJID = "",
//                     ID = "ROOT",
//                     MC = Utils.GetString(tree.datas_tree.FirstOrDefault<StrObjectDict>()["ROOT_NAME"]),
//                     JB = 0,
//                     SRM1 = "",
//                     SRM2 = "",
//                     SRM3 = "",
//                     DM = "ROOT",
//                     ZFPB = 0,
//                     MJPB = 0,
//                     SELECTED = root_selected
//                 }));
//             }
//             tree.ShowCheckBox = checkbox;
//             return tree;
//         }
    }
}
