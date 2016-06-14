using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using IBatisNet.DataMapper.SessionStore;
using ZLSoft.AppContext;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.Enum;
using ZLSoft.ThirdInterface;

namespace ZLSoft.Sys.Controllers
{
    public class InstallController : BaseController
    {

        private int _count;

        #region Action

        public ActionResult Log()
        {
            return this.MyJson(1);
        }


        /// <summary>
        /// 修改连接字符串
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateConfig()
        {
            var result = 1;
            var message = string.Empty;
            try
            {
                var strConn = string.Format("Data Source={0};Persist Security Info=True;User ID={1};Password={2};",
                                                    Request.Form["DataSource"],
                                                    Request.Form["UserID"],
                                                    Request.Form["Password"]);

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                var pathDao = string.Format(@"{0}dbconfig\dao.config", baseDirectory);
                UpdateConnectionString(pathDao, "database", "IWELL", strConn);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.ToString();
            }


            return this.MyJson(new { message, result });
        }

        /// <summary>
        /// 修改三方数据源
        /// </summary>
        /// <returns></returns>
        public ActionResult DataSource()
        {
            var dict = GetParams();
            //(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.89)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = ydtest)))
            var dataSource = string.Format("(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = {2})))",
                                            dict.GetString("Host"), dict.GetString("Port"), dict.GetString("SID"));
            var url = string.Format("Data Source={0};User ID={1};Password={2};", dataSource, dict.GetString("UserName"), dict.GetString("Password"));

            var p = new
            {
                Provider = dict.GetString("Provider"),
                Url = url,
                UserName = dict.GetString("UserName"),
                Password = dict.GetString("Password"),
                Database = dataSource,
                ID = "ef090fc0-a178-42a7-81da-5f6d5021bf81"
            };

            var result = DB.Execute(new DBState
             {
                 Name = "UPDATE_DataSource",
                 Param = p.toStrObjDict(),
                 Type = ESqlType.UPDATE
             });
            if (result > 0)
            {
                WebAppContextInit.ThirdDbInit();
                ThirdServiceContext.Initializer(DeployMode.MODE_NORMAL);
            }
            return this.MyJson(result);
        }

        /// <summary>
        /// 创建表以及添加基础数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Excute()
        {
            var server = AppDomain.CurrentDomain.BaseDirectory;

            var path = string.Format(@"{0}Resources\DBConfig", server);
            //创建表
            var fileT = Path.Combine(path, "Tables.sql");
            var sqlBatchT = GetSqlBatch(fileT, 100, true);

            //添加基础数据
            var fileI = Path.Combine(path, "Insert.sql");
            var sqlBatchI = GetSqlBatch(fileI, 100, false);

            var fileJ = Path.Combine(path, "服务定义_交班报告.sql");
            var sqlBatchJ = GetSqlBatch(fileJ, 1, false);
            HttpContext.Cache["Sum"] = sqlBatchT.Count + sqlBatchI.Count + 1;


            var map = DB.getSqlMap();
            Task.Factory.StartNew(() =>
             {
                 try
                 {
                     foreach (var item in sqlBatchT)
                     {
                         map.SessionStore = new CallContextSessionStore(map.Id);
                         map.BeginTransaction();
                         DB.BatchExecuteWithOutTransaction(map, item);
                         DB.Commit(map);
                         HttpContext.Cache["Current"] = ++_count;
                     }
                 }
                 catch (Exception)
                 {
                     DB.RollBackTransaction(map);
                 }
             }).ContinueWith(a =>
            {
                try
                {
                    foreach (var item in sqlBatchI)
                    {
                        map.SessionStore = new CallContextSessionStore(map.Id);
                        map.BeginTransaction();
                        DB.BatchExecuteWithOutTransaction(map, item);
                        DB.Commit(map);
                        HttpContext.Cache["Current"] = ++_count;
                    }

                    DB.Execute(new DBState()
                    {
                        Name = "ExecuteNoneQuery",
                        Param = sqlBatchJ.First().First(),
                        Type = ESqlType.INSERT
                    });
                    HttpContext.Cache["Current"] = ++_count;
                }
                catch (Exception)
                {
                    DB.RollBackTransaction(map);
                }
            });
            //new { result = Math.Abs(result) == 2 ? 1 : 0 }
            return this.MyJson(0);
        }

        #endregion

        #region Fun
        private List<List<string>> GetSqlBatch(string file, int num, bool isCreate)
        {
            var sql = GetSql(file);
            //手动更改Tables脚本中的分号为@&; ,用作分隔符
            var separator = new[] { "@&;" };
            var arr = sql.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var tmp = arr.Select((cmdText, index) => new { cmdText, index });
            var sqlBatch = new List<List<string>>();
            var lstx = new List<string>();
            foreach (var item in tmp)
            {
                if (lstx.Count == num)
                {
                    sqlBatch.Add(lstx);
                    lstx = new List<string>();
                }
                if (isCreate)
                {
                    if (!string.IsNullOrEmpty(item.cmdText.Replace("\r", "").Replace("\n", "")))
                        lstx.Add(string.Format("execute immediate '{0}';", item.cmdText.Replace("'", "''")));
                }
                else
                {
                    if (!(item.cmdText.Contains("REM INSERTING into") || string.IsNullOrEmpty(item.cmdText.Replace("\r", "").Replace("\n", ""))))
                        lstx.Add(string.Format("{0};", item.cmdText));
                }
            }
            if (lstx.Count > 0)
                sqlBatch.Add(lstx);
            return sqlBatch;
        }

        //public int ExcuteData(List<List<string>> batchSqlList)
        //{
        //    var map = DB.getSqlMap();
        //    Task.Factory.StartNew(() =>
        //    {
        //        var result = 1;
        //        try
        //        {
        //            foreach (var item in batchSqlList)
        //            {
        //                map.SessionStore = new CallContextSessionStore(map.Id);
        //                map.BeginTransaction();
        //                DB.BatchExecuteWithOutTransaction(map, item);
        //                DB.Commit(map);
        //                HttpContext.Cache["Current"] = ++_count;
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            DB.RollBackTransaction(map);
        //            result = 0;
        //        }
        //    }).Start();
        //}
        void _SetSession()
        {
            Session["Current"] = ++_count;
        }
        private string GetSql(string file)
        {
            string sql;
            using (var rs = new StreamReader(file, Encoding.Default))
            {
                sql = rs.ReadToEnd();
                rs.Close();
            }

            return sql;
        }

        private void UpdateConnectionString(string fileName, string nodeName, string key, string value)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            var xmlNode = doc.GetElementsByTagName(nodeName).Item(0);
            if (xmlNode != null)
            {
                var nodes = xmlNode.ChildNodes;
                for (var i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Attributes == null)
                        continue;
                    var name = nodes[i].Attributes["name"];

                    if (name == null || name.Value != key) continue;

                    name = nodes[i].Attributes["connectionString"];
                    name.Value = value;
                    break;
                }
            }
            doc.Save(fileName);
        }

        #endregion
    }

    public class InstallProssController : AsyncController
    {
        public void GetProgressingAsync()
        {
            AsyncManager.OutstandingOperations.Increment();
            AsyncManager.Parameters["Current"] = HttpContext.Cache["Current"];
            AsyncManager.Parameters["Sum"] = HttpContext.Cache["Sum"];
            AsyncManager.OutstandingOperations.Decrement();
        }

        public ActionResult GetProgressingCompleted(object current, object sum)
        {
            return Json(new { sum, current }, JsonRequestBehavior.AllowGet);
        }
    }
}
