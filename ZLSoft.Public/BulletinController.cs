using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.PUB;
using ZLSoft.Model.Tree;

namespace ZLSoft.Public.Controllers
{
    public class BulletinController : CRUDController
    {
        public ActionResult GetList()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> list = BulletinManager.Instance.GetList(dict);

            return this.MyJson(1, list);
        }

        public ActionResult ShowData()
        {
            StrObjectDict dict = GetParams();

            var all = dict.GetString("ALL");

            if (dict.ContainsKey("ALL"))
            {
                IList<StrObjectDict> Results = BulletinManager.Instance.GetSQLAll(dict);
                IList<StrObjectDict> SQLALL = new List<StrObjectDict>();
                for (int i = 0; i < Results.Count; i++)
                {
                    try
                    {
                        IList<StrObjectDict> SQLRequest = BulletinManager.Instance.RequestSQL(Results[i]["ExtSQL"].ToString());
                        SQLALL.Add(SQLRequest[0]);
                    }
                    catch
                    {
                        return this.MyJson(-1, "调用失败：重要事情说三遍！！！请检查设置的SQL查询语句是否正确！请检查设置的SQL查询语句是否正确！请检查设置的SQL查询语句是否正确！");
                    }
                }
                return this.MyJson(1, SQLALL);
            }
            else
            {
                IList<StrObjectDict> Result = BulletinManager.Instance.GetSQL(dict);

                if (Result.Count > 0)
                {
                    string SQL = Result[0]["ExtSQL"].ToString();
                    if (!string.IsNullOrEmpty(SQL))
                    {
                        try
                        {
                            IList<StrObjectDict> SQLRequest = BulletinManager.Instance.RequestSQL(SQL);
                            return this.MyJson(1, SQLRequest);
                        }
                        catch
                        {
                            return this.MyJson(-1, "调用失败：重要事情说三遍！！！请检查设置的SQL查询语句是否正确！请检查设置的SQL查询语句是否正确！请检查设置的SQL查询语句是否正确！");
                        }
                    }
                    else
                    {
                        return this.MyJson(-1, "调用失败");
                    }
                }
                else
                {
                    return this.MyJson(-1, "调用失败");
                }
            }
        }

        public ActionResult GetID()
        {
            StrObjectDict dict = GetParams();

            IList<StrObjectDict> Result = BulletinManager.Instance.GetID(dict);

            return this.MyJson(1, Result);
        }

        public ActionResult GetTree()
        {
            StrObjectDict dict = GetParams();

            Tree tree = BulletinManager.Instance.GetTree(dict);

            return this.MyJson(1, tree.data);
        }

    }
}
