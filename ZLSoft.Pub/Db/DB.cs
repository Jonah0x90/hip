using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;
using ZLSoft.Pub.Constant;
using IBatisNet.DataMapper;
using System.Data;
using System.Collections;

namespace ZLSoft.Pub.Db
{
    public static class DB
    {
        public static ISqlMapper CurrentSqlMapper { get; set; }
        public static T Load<T, V>(V v) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            T arg_2E_0;
            if ((arg_2E_0 = DaoBase.getInstance().ExecuteQueryForObject<T>(t.MAP_LOAD, v)) == null)
            {
                //arg_2E_0 = t;
            }
            return arg_2E_0;
        }

        public static int Count<T>(object param) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            return DaoBase.getInstance().ExecuteQueryForObject<int>(t.MAP_COUNT, param);
        }

        public static object Scalar(string mapId,StrObjectDict obj)
        {
            return DaoBase.getInstance().ExecuteScalar(mapId, obj);
        }

        public static T LoadSod<T, V>(string mapID,V v) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            T arg_2E_0;
            if ((arg_2E_0 = DaoBase.getInstance().ExecuteQueryForObject<T>(mapID, v)) == null)
            {
                //arg_2E_0 = t;
            }
            return arg_2E_0;
        }

        public static T LoadSodAsync<T, V>(string mapID, V v) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            T arg_2E_0;
            if ((arg_2E_0 = DaoBase.getInstance().ExecuteQueryForObjectAsync<T>(mapID, v)) == null)
            {
                //arg_2E_0 = t;
            }
            return arg_2E_0;
        }


        public static DaoBase GetDbBase()
        {
            return DaoBase.getInstance(null);
        }

        public static DataTable Meta(string conn,IDictionary<string,object> obj)
        {
            return DaoBase.getInstance(conn).QueryForDataTable("PubSql_GetMeta",obj);
        }

        public static IList<StrObjectDict> Select(string sql)
        {
            return DB.Select(null, sql);
        }
        public static IList<StrObjectDict> Select(string conn, string sql)
        {
            return DaoBase.getInstance(conn).ExecuteQueryForList<StrObjectDict>("ExecuteForQuery", new
            {
                sql
            });
        }

        public static IList<StrObjectDict> SelectAsync(string sql)
        {
            return DB.SelectAsync(null, sql);
        }

        public static IList<StrObjectDict> SelectAsync(string conn, string sql)
        {
            return DaoBase.getInstance(conn).ExecuteQueryForListAsync<StrObjectDict>("ExecuteForQuery", new
            {
                sql
            });
        }

        public static DataTable SelectDataTable(string sql)
        {
            return DB.SelectDataTable(null, sql);
        }
        public static DataTable SelectDataTable(string conn, string sql)
        {
            return DaoBase.getInstance(conn).QueryForDataTable("ExecuteForQuery", new
            {
                sql
            });
        }
        public static StrObjectDict SelectRow(string sql)
        {
            IList<StrObjectDict> list = DB.Select(null, sql);
            StrObjectDict result;
            if (list.Count > 0)
            {
                result = list.FirstOrDefault<StrObjectDict>();
            }
            else
            {
                result = null;
            }
            return result;
        }
        public static object SelectFirst(string sql)
        {
            return DaoBase.getInstance().ExecuteScalar("ExecuteForQuery", new
            {
                sql
            });
        }
        public static DataTable ListDataTable(string sql)
        {
            return DaoBase.getInstance().QueryForDataTable("ExecuteForQuery", new
            {
                sql
            });
        }
        public static IList<T> List<T>(IDictionary<string, object> dictionary) where T : BaseModel, new()
        {
            return DB.List<T>(null, dictionary);
        }
        public static IList<StrObjectDict> ListSod(string mapid, IDictionary<string, object> dictionary)
        {
            return DB.ListSod(null, mapid, dictionary);
        }
        public static IList<StrObjectDict> ListSod(string mapid, IDictionary<string, object> dictionary, int pageNumber, int pageSize)
        {
            return DB.ListSod(null, mapid, dictionary, pageNumber, pageSize);
        }

        public static IList<StrObjectDict> ListSod(string mapid, object obj)
        {
            return DB.ListSod(null, mapid, obj.toStrObjDict(false));
        }

        public static IList<StrObjectDict> ListSod(string mapid, object obj,int pageNumber,int pageSize)
        {
            return DB.ListSod(null, mapid, obj.toStrObjDict(false),pageNumber,pageSize);
        }
        public static string GetSql(string mapid, object obj)
        {
            string sql;
            if (obj is StrObjectDict)
            {
                sql = DaoBase.getInstance(null).GetSql(mapid, obj, false);
            }
            else
            {
                sql = DaoBase.getInstance(null).GetSql(mapid, obj.toStrObjDict(false), false);
            }
            return sql;
        }

        public static ISqlMapper getSqlMap()
        {
            return DaoBase.getInstance().GetSqlMap();
        }
        public static IList<T> List<T>(string conn, IDictionary<string, object> dictionary) where T : BaseModel, new()
        {
            DaoBase arg_1B_0 = DaoBase.getInstance(conn);
            T t = System.Activator.CreateInstance<T>();
            return arg_1B_0.ExecuteQueryForList<T>(t.MAP_LIST, dictionary);
        }

        public static IList<T> List2<T>(IDictionary<string, object> dictionary) where T : BaseModel, new()
        {
            DaoBase arg_1B_0 = DaoBase.getInstance(null);
            T t = System.Activator.CreateInstance<T>();
            return arg_1B_0.ExecuteQueryForList<T>(t.MAP_LIST2, dictionary);
        }

        public static IList<StrObjectDict> List2<T>(IDictionary<string, object> dictionary,string sendNullIsOK) where T : BaseModel, new()
        {
            DaoBase arg_1B_0 = DaoBase.getInstance(null);
            T t = System.Activator.CreateInstance<T>();
            return arg_1B_0.ExecuteQueryForList<StrObjectDict>(t.MAP_LIST2, dictionary);
        }

        public static IList<T> List<T>(IDictionary<string, object> dictionary,int pageNum,int pageSize) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            return DaoBase.getInstance().GetSqlMap().QueryForList<T>(t.MAP_LIST, dictionary, (pageNum - 1) * pageSize, pageSize);
        }
        public static IList<StrObjectDict> ListSod(string conn, string mapid, IDictionary<string, object> dictionary)
        {
            return DaoBase.getInstance(conn).ExecuteQueryForList<StrObjectDict>(mapid, dictionary);
        }

        public static IList<StrObjectDict> ListSod(string conn, string mapid, IDictionary<string, object> dictionary,int pageNum,int pageSize)
        {
            return DaoBase.getInstance(conn).ExecuteQueryForList<StrObjectDict>(mapid, dictionary, (pageNum - 1) * pageSize, pageSize);
        }

        public static DBSERVER_TYPE GetDbtype()
        {
            return DB.GetDbtype("");
        }
        public static DBSERVER_TYPE GetDbtype(string conn)
        {
            DBSERVER_TYPE result;
            if (DaoBase.getInstance(conn).GetDbtype().IndexOf("sqlServer") != -1)
            {
                result = DBSERVER_TYPE.MSSQL;
            }
            else
            {
                if (DaoBase.getInstance(conn).GetDbtype().IndexOf("oracle") != -1)
                {
                    result = DBSERVER_TYPE.ORACLE;
                }
                else
                {
                    result = DBSERVER_TYPE.MSSQL;
                }
            }
            return result;
        }
        public static void Execute(string conn, string sql)
        {
            DaoBase.getInstance(conn).Execute(sql);
        }
        public static int Execute(string sql)
        {
            return DaoBase.getInstance().Execute(sql);
        }
        public static int ExecuteWithOutTransaction(ISqlMapper sqlMap, IList<string> sqls)
        {
            return DaoBase.getInstance().Execute(sqlMap,sqls,false);
        }

        public static int BatchExecuteWithOutTransaction(ISqlMapper sqlMap, IList<string> sqls)
        {
            return DaoBase.getInstance().Execute(sqlMap, sqls,true);
        }

        public static int ExecuteWithOutTransaction(ISqlMapper sqlMap, DBState state)
        {
            return DaoBase.getInstance().Execute(sqlMap, state);
        }

        public static int Execute(IList<string> sqls)
        {
            List<DBState> list = new List<DBState>();
            foreach (string current in sqls)
            {
                list.Add(new DBState
                {
                    Name = "ExecuteNoneQuery",
                    Param = new
                    {
                        sql = current
                    }
                });
            }
            return DaoBase.getInstance().Execute(list);
        }

        public static int Execute(DBState state)
        {
            return DaoBase.getInstance().Execute(state);
        }

        public static int Execute(IEnumerable<DBState> batchStates)
        {
            return DaoBase.getInstance().Execute(batchStates);
        }
        public static int ExecuteAsync(IEnumerable<DBState> batchStates)
        {
            return DaoBase.getInstance().ExecuteAsync(batchStates);
        }
        public static int ExecuteWithTransaction(ISqlMapper sqlmapper, IEnumerable<DBState> batchStates)
        {
            return DaoBase.getInstance().ExecuteWithTransaction(sqlmapper, batchStates);
        }
        public static int ExecuteWithTransaction(IEnumerable<DBState> batchStates)
        {
            return DaoBase.getInstance().ExecuteWithTransaction(batchStates);
        }
        public static int ExecuteWithOutTransaction(IEnumerable<DBState> batchStates)
        {
            return DaoBase.getInstance().ExecuteWithTransaction(batchStates);
        }
        public static DataTable ExecuteMsSqlStoredProcedureWithOutputCursor(string mapid, StrObjectDict dictionary, IDictionary<string, System.Data.ParameterDirection> dict2, out Hashtable ht)
        {
            return DaoBase.getInstance().QueryForDataTable(mapid, dictionary, dictionary, dict2, out ht);
        }
        public static void ExecuteOracleStoredProcedureWithOutputCursor(string mapid, IDictionary<string, object> dictionary)
        {
            DaoBase.getInstance().ExecuteOracleStoredProcedureWithOutputCursor(mapid, dictionary);
        }

        public static void BeginTransaction()
        {
            CurrentSqlMapper = getSqlMap();
            CurrentSqlMapper.BeginTransaction();
        }

        public static void Commit(ISqlMapper sqlMaper){
            sqlMaper.CommitTransaction();
        }

        public static void RollBackTransaction(ISqlMapper sqlMaper)
        {
            sqlMaper.RollBackTransaction();
            sqlMaper = null;
        }

        
    }
}
