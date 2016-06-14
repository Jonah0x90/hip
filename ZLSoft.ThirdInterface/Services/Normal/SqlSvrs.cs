using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using IBatisNet.DataMapper.SessionStore;
using ZLSoft.Cache;
using ZLSoft.DalManager;
using ZLSoft.Model.THIRD;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.ThirdInterface.Services
{
    public class SqlSvrs : BaseService, IThirdService
    {
        public IList<StrObjectDict> DoService(StrObjectDict obj)
        {
            if (OtherProperties != null)
            {
                object isAsync = OtherProperties["IsAsync"];
                if ("1".Equals(isAsync.ToString()))
                {
                    //异步访问,普通模式不支持异步访问
                    return null;
                }
                else
                {
                    //同步访问
                    string dataSourceID = OtherProperties["DataSource"].ToString();
                    string sql = OtherProperties["InputTemplate"].ToString();
                    sql = sql.Replace("<IN>", "");
                    sql = sql.Replace("</IN>", "");
                    sql = sql.Replace("&apos;", "'");
                    sql = sql.Replace("&gt;", ">");
                    sql = sql.Replace("&lt;", "<");
                    sql = sql.Replace("&quot;", "\"");

                    if (obj != null)
                    {
                        foreach (var item in obj)
                        {
                            var p = string.Format("${0}$", item.Key);
                            sql = sql.Replace(p, item.Value.ToString());
                        }
                    }

                    int isLocalStorage = int.Parse(OtherProperties["IsLocalStorage"].ToString());
                    if (isLocalStorage == 1)
                    {
                        long startTime = DateTime.Now.Ticks;
                        int update_count = 0;
                        int insert_count = 0;
                        //本地对照表对象
                        string localTbID = OtherProperties["RefObject"].ToString();
                        //获取真实的本地表对象
                        ImportObject imp = ThirdDataManager.Instance.LoadObject<ImportObject, PK_ImportObject>(new PK_ImportObject
                        {
                            ID = localTbID
                        });

                        if (sql.IndexOf("$UPDATETIME$") > -1)
                        {
                            if (imp.ImportTime == null)
                            {
                                imp.ImportTime = DateTime.MinValue;
                            }
                            sql = sql.Replace("$UPDATETIME$", imp.ImportTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        }

                        IList<StrObjectDict> list = DB.Select(dataSourceID, sql);

                        if (list != null && list.Count > 0)
                        {
                            //映射到本地对照表对象
                            string sqls = @"SELECT a.字段,a.三方字段 FROM 系统_三方服务字段对照 a WHERE a.服务ID = '" + ID + "'";
                            IList<StrObjectDict> listRefColumns = DB.Select(sqls);
                            StrObjectDict dict = null;
                            StrObjectDict dictExtend = null;
                            IList<StrObjectDict> remoteDataList = new List<StrObjectDict>();
                            IList<StrObjectDict> remoteDataListExtend = new List<StrObjectDict>();



                            foreach (var item in list)
                            {
                                dict = new StrObjectDict();
                                dictExtend = new StrObjectDict();
                                foreach (var key in item.Keys)
                                {
                                    IList<StrObjectDict> l = listRefColumns.FindAllBy("三方字段", key);
                                    if (l == null || l.Count == 0)
                                    {
                                        dictExtend[key] = item[key];
                                    }
                                    else if (l[0]["字段"].ToString() == "0")
                                    {
                                        dictExtend[l[0]["三方字段"].ToString()] = item[key];
                                    }
                                    else if (l.Count > 1)
                                    {
                                        if (l[0]["字段"].ToString() == "相关ID")
                                        {
                                            dictExtend[l[0]["字段"].ToString()] = item[key];
                                        }
                                        else if (l[1]["字段"].ToString() == "相关ID")
                                        {
                                            dictExtend[l[1]["字段"].ToString()] = item[key];
                                        }
                                        dict[l[0]["字段"].ToString()] = item[key];
                                        dict[l[1]["字段"].ToString()] = item[key];
                                        if (l.Count > 2)
                                        {
                                            dict[l[2]["字段"].ToString()] = item[key];
                                        }
                                    }
                                    else if (l.Count > 0)
                                    {
                                        dict[l[0]["字段"].ToString()] = item[key];
                                    }
                                }

                                if (!string.IsNullOrEmpty(imp.Identifying) && !string.IsNullOrEmpty(imp.ExtendTable))
                                {
                                    if (!dictExtend.ContainsKey("Sign"))
                                    {
                                        dictExtend.Add("Sign", "");
                                    }
                                    string[] columns = imp.Identifying.Split(new char[] { ',' });
                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        dictExtend["Sign"] += dict[columns[j]] + ".";
                                    }
                                }

                                remoteDataListExtend.Add(dictExtend);
                                remoteDataList.Add(dict);
                            }
                            IList<StrObjectDict> cache = CacheIO.Get(ID) as IList<StrObjectDict>;

                            //和本地数据表合并
                            if (cache == null || cache.Count == 0)
                            {
                                //获取本地的数据
                                //生成查询列
                                string colums = string.Empty;
                                foreach (var item in listRefColumns)
                                {
                                    colums += item["字段"] + " AS \"" + item["字段"] + "\",";
                                }
                                if (colums.EndsWith(","))
                                {
                                    colums = colums.Substring(0, colums.Length - 1);
                                }
                                string localsql = @"SELECT {0} FROM " + imp.TableName;
                                localsql = string.Format(localsql, colums);

                                //查找本地库中的数据，并放入缓存中
                                IList<StrObjectDict> localDataList = DB.Select(localsql);
                                //CacheIO.Insert(ID,localDataList);
                                cache = localDataList;
                            }

                            IList<string> dblist = new List<string>();

                            StrObjectDict tempObj = null;
                            IList<StrObjectDict> updateList = new List<StrObjectDict>();

                            IList<IList<string>> batchSqlList = new List<IList<string>>();

                            if (!string.IsNullOrEmpty(imp.Identifying) || !string.IsNullOrEmpty(imp.ExtendTable))
                            {
                                string[] columns = imp.Identifying.Split(new char[] { ',' });

                                //string[] columns_values = new string[columns.Length];

                                //for (int j = 0; j < columns.Length; j++)
                                //{
                                //    IList<StrObjectDict> dict_temp = listRefColumns.FindBy("字段", columns[j]);
                                //    columns_values[j] = dict_temp[0]["三方字段"].ToString();
                                //}

                                string[] columns_values = new string[columns.Length];

                                for (int i = 0; i < cache.Count; i++)
                                {

                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        columns_values[j] = cache[i][columns[j]].ToString();
                                    }

                                    tempObj = remoteDataList.FindOnlyBy(columns, columns_values);
                                    if (tempObj != null)
                                    {
                                        updateList.Add(tempObj);
                                        ((List<StrObjectDict>)remoteDataList).RemoveAll(v => v.GetValues(columns) == tempObj.GetValues(columns));
                                        //i--;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < cache.Count; i++)
                                {
                                    tempObj = remoteDataList.FindOnlyBy("相关ID", cache[i]["相关ID"].ToString());
                                    if (tempObj != null)
                                    {
                                        updateList.Add(tempObj);
                                        ((List<StrObjectDict>)remoteDataList).RemoveAll(v => v["相关ID"].ToString() == tempObj["相关ID"].ToString());
                                        //i--;
                                    }
                                }
                            }

                            string sql_insert = @"INSERT INTO {0}({1}) VALUES({2});";
                            string sql_update = @"UPDATE {0} SET {1} WHERE {2};";
                            string insert_Colums = "";

                            foreach (var item in listRefColumns)
                            {
                                insert_Colums += item["字段"] + ",";
                            }
                            if (insert_Colums.EndsWith(","))
                            {
                                insert_Colums = insert_Colums.Substring(0, insert_Colums.Length - 1);
                            }

                            string values = string.Empty;
                            foreach (var item in remoteDataList)
                            {
                                foreach (var item2 in listRefColumns)
                                {
                                    object v = item[item2["字段"].ToString()];
                                    if (v.IsDateTime(true))
                                    {
                                        values += (v == null ? "''" : "TO_DATE('" + v + "'") + ",'yyyy-mm-dd hh24:mi:ss'),";
                                    }
                                    else
                                    {
                                        values += (v == null ? "''" : "'" + v + "'") + ",";
                                    }
                                }
                                if (dblist.Count >= 500)
                                {
                                    batchSqlList.Add(dblist);
                                    dblist = new List<string>();
                                }
                                dblist.Add(string.Format(sql_insert, imp.TableName, insert_Colums, values.Substring(0, values.Length - 1)));
                                values = string.Empty;
                            }
                            insert_count += remoteDataList.Count;

                            string update_Colums = string.Empty;
                            foreach (var item in updateList)
                            {
                                foreach (var item2 in listRefColumns)
                                {
                                    if (item2["字段"].ToString() == "ID" || item2["字段"].ToString() == "RelatID")
                                    {
                                        continue;
                                    }
                                    if (item[item2["字段"].ToString()].IsDateTime(true))
                                    {
                                        update_Colums += item2["字段"].ToString() + " = TO_DATE('" + item[item2["字段"].ToString()] + "','yyyy-mm-dd hh24:mi:ss'),";
                                    }
                                    else
                                        update_Colums += item2["字段"].ToString() + " = '" + item[item2["字段"].ToString()] + "',";
                                }
                                string strWhere = string.Empty;
                                if (!string.IsNullOrEmpty(imp.Identifying))
                                {
                                    string[] columns = imp.Identifying.Split(new char[] { ',' });
                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        if (j == columns.Length - 1)
                                            strWhere += columns[j] + "='" + item[columns[j]] + "'";
                                        else
                                            strWhere += columns[j] + "='" + item[columns[j]] + "' AND ";
                                    }
                                }
                                else
                                {
                                    strWhere = "相关ID = '" + item["ID"] + "'";
                                }
                                if (dblist.Count >= 500)
                                {
                                    batchSqlList.Add(dblist);
                                    dblist = new List<string>();
                                }

                                dblist.Add(string.Format(sql_update, imp.TableName, update_Colums.Substring(0, update_Colums.Length - 1), strWhere));
                                update_Colums = string.Empty;
                            }

                            update_count += updateList.Count();

                            if (dblist.Count > 0)
                                batchSqlList.Add(dblist);
                            dblist = null;

                            int isOk = 0;

                            //包含扩展信息sql
                            if (imp.ExtendTable != null && "" != imp.ExtendTable)
                            {
                                dblist = new List<string>();
                                //把扩展信息转换成多行数据
                                IList<StrObjectDict> remoteExtendDataListKeyValues = new List<StrObjectDict>();
                                StrObjectDict keyValues = null;



                                foreach (var item in remoteDataListExtend)
                                {
                                    foreach (var key in item.Keys)
                                    {
                                        if (key == "相关ID" || key == "ID")
                                        {
                                            continue;
                                        }
                                        keyValues = new StrObjectDict();
                                        keyValues["ObjectID"] = item["Sign"];//item["相关ID"];
                                        keyValues["ExtendName"] = key;
                                        keyValues["ExtendValue"] = item[key];
                                        remoteExtendDataListKeyValues.Add(keyValues);
                                    }
                                }
                                if (CacheIO.Get(imp.ExtendTable) == null)
                                {
                                    IList<StrObjectDict> localExtendDataList = DB.Select("SELECT * FROM " + imp.ExtendTable);
                                    if (localExtendDataList != null && localExtendDataList.Count > 0)
                                    {
                                        //CacheIO.Insert(imp.ExtendTable,localExtendDataList);
                                    }

                                    IDictionary<string, StrObjectDict> temp_extend_dict = remoteExtendDataListKeyValues.AsDictionary(new string[] { "ObjectID", "ExtendName" });

                                    remoteExtendDataListKeyValues.Clear();
                                    for (int i = 0; i < localExtendDataList.Count; i++)
                                    {
                                        //tempObj = remoteExtendDataListKeyValues.FindOnlyBy(new string[] { "ObjectID", "ExtendName" }, new string[] { localExtendDataList[i]["对象相关ID"].ToString(), localExtendDataList[i]["信息名"].ToString() });
                                        var key = localExtendDataList[i].GetValues(new string[] { "对象相关ID", "信息名" });

                                        tempObj = temp_extend_dict[key];
                                        if (tempObj != null)
                                        {

                                            if (dblist.Count >= 500)
                                            {
                                                batchSqlList.Add(dblist);
                                                dblist = new List<string>();
                                            }
                                            dblist.Add(string.Format(sql_update, imp.ExtendTable, "信息值='" + tempObj["ExtendValue"] + "'", "对象相关ID = '" + tempObj["ObjectID"].ToString() + "' AND 信息名='" + tempObj["ExtendName"].ToString() + "'"));
                                            //((List<StrObjectDict>)remoteExtendDataListKeyValues).RemoveAll(v => ((v["ObjectID"].ToString() == tempObj["ObjectID"].ToString() && (v["ExtendName"].ToString() == tempObj["ExtendName"]))));
                                            temp_extend_dict.Remove(localExtendDataList[i].GetValues(new string[] { "对象相关ID", "信息名" }));
                                            update_count++;
                                        }
                                    }
                                    //((List<StrObjectDict>)remoteExtendDataListKeyValues).AddRange(temp_extend_dict.Values);
                                    //temp_extend_dict.Clear();

                                    foreach (var key in temp_extend_dict.Keys)
                                    {
                                        tempObj = temp_extend_dict[key];
                                        if (dblist.Count >= 500)
                                        {
                                            batchSqlList.Add(dblist);
                                            dblist = new List<string>();
                                        }
                                        string temp_values = "'" + tempObj["ObjectID"].ToString() + "','" + tempObj["ExtendName"].ToString() + "','" + (tempObj["ExtendValue"] == null ? null : tempObj["ExtendValue"].ToString()) + "'";
                                        dblist.Add(string.Format(sql_insert, imp.ExtendTable, "对象相关ID,信息名,信息值", temp_values));
                                        insert_count++;
                                    }
                                    if (dblist.Count > 0 && dblist.Count < 500)
                                    {
                                        batchSqlList.Add(dblist);
                                    }
                                    dblist = null;
                                }

                                int rows = 0;
                                try
                                {
                                    #region 直接sql语句分批提交，结合内存比较，效率较高

                                    foreach (var item in batchSqlList)
                                    {
                                        DB.BeginTransaction();
                                        int rs = DB.BatchExecuteWithOutTransaction(DB.CurrentSqlMapper, item);
                                        rows += Math.Abs(rs);

                                        DB.Commit(DB.CurrentSqlMapper);
                                    }
                                    #endregion

                                    #region 匿名块方式,耗时较长

                                    //IList<object> listExtends = new List<object>();
                                    //IList<DBState> listDbState = new List<DBState>();
                                    //for (int i = 0; i < remoteDataListExtend.Count; i++)
                                    //{
                                    //    foreach (var key in remoteDataListExtend[i].Keys)
                                    //    {
                                    //        if (key == "相关ID" || key == "ID")
                                    //        {
                                    //            continue;
                                    //        }

                                    //        listExtends.Add(new
                                    //        {
                                    //            ObjectID = remoteDataListExtend[i]["相关ID"],
                                    //            ExtendName = key,
                                    //            ExtendValue = remoteDataListExtend[i][key]
                                    //        }); 
                                    //        if (listExtends.Count >= 1000)
                                    //        {
                                    //            listDbState.Add(new DBState
                                    //            {
                                    //                Name = "BatchExtendExecuteNoneQuery2",
                                    //                Param = listExtends,
                                    //                Type = ESqlType.UPDATE
                                    //            });
                                    //            listExtends = new List<object>();
                                    //        }
                                    //    }
                                    //}

                                    //if (listExtends.Count > 0 && listExtends.Count < 1000)
                                    //{
                                    //    listDbState.Add(new DBState
                                    //    {
                                    //        Name = "BatchExtendExecuteNoneQuery2",
                                    //        Param = listExtends,
                                    //        Type = ESqlType.UPDATE
                                    //    });
                                    //}

                                    //foreach (var item in listDbState)
                                    //{
                                    //    DB.BeginTransaction();
                                    //    DB.ExecuteWithOutTransaction(DB.CurrentSqlMapper,item);
                                    //    DB.Commit(DB.CurrentSqlMapper);
                                    //}
                                    #endregion

                                }
                                catch (Exception)
                                {
                                    DB.RollBackTransaction(DB.CurrentSqlMapper);
                                }
                                isOk = rows;
                            }
                            else
                            {
                                try
                                {
                                    foreach (var item in batchSqlList)
                                    {
                                        DB.BeginTransaction();
                                        int rs = DB.BatchExecuteWithOutTransaction(DB.CurrentSqlMapper, item);
                                        isOk += Math.Abs(rs);

                                        DB.Commit(DB.CurrentSqlMapper);
                                    }
                                }
                                catch (Exception)
                                {
                                    DB.RollBackTransaction(DB.CurrentSqlMapper);
                                }
                            }

                            if (isOk >= 0)
                            {
                                //修改最后导入数据时间和导入数据条数

                                long endTime = DateTime.Now.Ticks;
                                long takeTime = endTime - startTime;

                                double takeSec = takeTime / 10000000.0;

                                int resultUpdate = ThirdDataManager.Instance.InsertOrUpdate<ImportObject>(new
                                {
                                    ID = localTbID,
                                    ImportDataNumber = insert_count,
                                    UpdateDataNumber = update_count,
                                    ImportTime = DateTime.Now,
                                    TakeTime = takeSec
                                }.toStrObjDict());

                                //recordSql = string.Format(recordSql, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remoteDataList.Count,updateList.Count, localTbID);
                                //int resultUpdate = DB.Execute(recordSql);
                                StrObjectDict resultSod = new
                                {
                                    ImportResult = "数据同步成功"
                                }.toStrObjDict();

                                //更新缓存
                                //string colums = string.Empty;
                                //foreach (var item in listRefColumns)
                                //{
                                //    colums += item["字段"] + " AS \"" + item["字段"] + "\",";
                                //}
                                //if (colums.EndsWith(","))
                                //{
                                //    colums = colums.Substring(0, colums.Length - 1);
                                //}
                                //string localsql = @"SELECT {0} FROM " + imp.TableName;
                                //localsql = string.Format(localsql, colums);

                                //IList<StrObjectDict> localDataList = DB.Select(localsql);
                                //CacheIO.Remove(ID);
                                //CacheIO.Insert(ID,localDataList);

                                return new List<StrObjectDict>() { resultSod };
                            }
                            else
                            {
                                StrObjectDict resultSod = new
                                {
                                    ImportResult = "没有更新到数据"
                                }.toStrObjDict();
                                return new List<StrObjectDict>() { resultSod };
                            }
                        }
                        else
                        {
                            long endTime = DateTime.Now.Ticks;
                            long takeTime = endTime - startTime;

                            double takeSec = takeTime / 10000000.0;
                            int resultUpdate = ThirdDataManager.Instance.InsertOrUpdate<ImportObject>(new
                            {
                                ID = localTbID,
                                ImportDataNumber = 0,
                                UpdateDataNumber = 0,
                                ImportTime = DateTime.Now,
                                TakeTime = takeSec
                            }.toStrObjDict());

                            StrObjectDict resultSod = new
                            {
                                ImportResult = "没有更新到数据"
                            }.toStrObjDict();
                            return new List<StrObjectDict>() { resultSod };
                        }
                    }
                    else
                    {
                        string sql_Columns = @"SELECT a.字段,a.三方字段,b.表字段,b.应用字段 FROM 系统_三方服务字段对照 a,系统_元数据索引 b where a.字段 = b.表字段 AND a.服务ID = '" + ID + "'";

                        IList<StrObjectDict> list = DB.Select(dataSourceID, sql);

                        //IList<StrObjectDict> listRefColumns = DB.Select(sql_Columns);
                        //StrObjectDict dict = null;
                        //IList<StrObjectDict> remoteDataList = new List<StrObjectDict>();
                        //foreach (var item in list)
                        //{
                        //    dict = new StrObjectDict();
                        //    foreach (var key in item.Keys)
                        //    {
                        //        IList<StrObjectDict> l = listRefColumns.FindAllBy("三方字段", key);
                        //        if (l.Count > 1)
                        //        {
                        //            dict[l[0]["应用字段"].ToString()] = item[key];
                        //            dict[l[1]["应用字段"].ToString()] = item[key];
                        //        }
                        //        else if (l.Count > 0)
                        //        {
                        //            dict[l[0]["应用字段"].ToString()] = item[key];
                        //        }
                        //    }
                        //    remoteDataList.Add(dict);
                        //}

                        // return remoteDataList;
                        return list;

                    }
                }
            }
            else
            {
                return null;
            }
        }


        public IList<StrObjectDict> DoServiceAsync(StrObjectDict obj)
        {
            if (OtherProperties != null)
            {
                object isAsync = OtherProperties["IsAsync"];
                if ("1".Equals(isAsync.ToString()))
                {
                    //异步访问,普通模式不支持异步访问
                    return null;
                }
                else
                {
                    //同步访问
                    string dataSourceID = OtherProperties["DataSource"].ToString();
                    string sql = OtherProperties["InputTemplate"].ToString();
                    sql = sql.Replace("<IN>", "");
                    sql = sql.Replace("</IN>", "");
                    sql = sql.Replace("&apos;", "'");
                    sql = sql.Replace("&gt;", ">");
                    sql = sql.Replace("&lt;", "<");
                    sql = sql.Replace("&quot;", "\"");

                    if (obj != null)
                    {
                        foreach (var item in obj)
                        {
                            var p = string.Format("${0}$", item.Key);
                            sql = sql.Replace(p, item.Value.ToString());
                        }
                    }

                    int isLocalStorage = int.Parse(OtherProperties["IsLocalStorage"].ToString());
                    if (isLocalStorage == 1)
                    {
                        long startTime = DateTime.Now.Ticks;
                        int update_count = 0;
                        int insert_count = 0;
                        //本地对照表对象
                        string localTbID = OtherProperties["RefObject"].ToString();
                        //获取真实的本地表对象
                        ImportObject imp = ThirdDataManager.Instance.LoadObjectAsync<ImportObject, PK_ImportObject>(new PK_ImportObject
                        {
                            ID = localTbID
                        });

                        if (sql.IndexOf("$UPDATETIME$") > -1)
                        {
                            if (imp.ImportTime == null)
                            {
                                imp.ImportTime = DateTime.MinValue;
                            }
                            sql = sql.Replace("$UPDATETIME$", imp.ImportTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        }

                        IList<StrObjectDict> list = DB.SelectAsync(dataSourceID, sql);
                        var localMap = DB.getSqlMap();
                        if (list != null && list.Count > 0)
                        {
                            //映射到本地对照表对象
                            string sqls = @"SELECT a.字段,a.三方字段 FROM 系统_三方服务字段对照 a WHERE a.服务ID = '" + ID + "'";
                            IList<StrObjectDict> listRefColumns = DB.SelectAsync(sqls);
                            StrObjectDict dict = null;
                            StrObjectDict dictExtend = null;
                            IList<StrObjectDict> remoteDataList = new List<StrObjectDict>();
                            IList<StrObjectDict> remoteDataListExtend = new List<StrObjectDict>();



                            foreach (var item in list)
                            {
                                dict = new StrObjectDict();
                                dictExtend = new StrObjectDict();
                                foreach (var key in item.Keys)
                                {
                                    IList<StrObjectDict> l = listRefColumns.FindAllBy("三方字段", key);
                                    if (l == null || l.Count == 0)
                                    {
                                        dictExtend[key] = item[key];
                                    }
                                    else if (l[0]["字段"].ToString() == "0")
                                    {
                                        dictExtend[l[0]["三方字段"].ToString()] = item[key];
                                    }
                                    else if (l.Count > 1)
                                    {
                                        if (l[0]["字段"].ToString() == "相关ID")
                                        {
                                            dictExtend[l[0]["字段"].ToString()] = item[key];
                                        }
                                        else if (l[1]["字段"].ToString() == "相关ID")
                                        {
                                            dictExtend[l[1]["字段"].ToString()] = item[key];
                                        }
                                        dict[l[0]["字段"].ToString()] = item[key];
                                        dict[l[1]["字段"].ToString()] = item[key];
                                        if (l.Count > 2)
                                        {
                                            dict[l[2]["字段"].ToString()] = item[key];
                                        }
                                    }
                                    else if (l.Count > 0)
                                    {
                                        dict[l[0]["字段"].ToString()] = item[key];
                                    }
                                }

                                if (!string.IsNullOrEmpty(imp.Identifying) && !string.IsNullOrEmpty(imp.ExtendTable))
                                {
                                    if (!dictExtend.ContainsKey("Sign"))
                                    {
                                        dictExtend.Add("Sign", "");
                                    }
                                    string[] columns = imp.Identifying.Split(new char[] { ',' });
                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        dictExtend["Sign"] += dict[columns[j]] + ".";
                                    }
                                }

                                remoteDataListExtend.Add(dictExtend);
                                remoteDataList.Add(dict);
                            }

                            var cache = HttpRuntime.Cache.Get(ID) as IList<StrObjectDict>;
                            // IList<StrObjectDict> cache = CacheIO.Get(ID) as IList<StrObjectDict>;

                            //和本地数据表合并
                            if (cache == null || cache.Count == 0)
                            {
                                //获取本地的数据
                                //生成查询列
                                string colums = string.Empty;
                                foreach (var item in listRefColumns)
                                {
                                    colums += item["字段"] + " AS \"" + item["字段"] + "\",";
                                }
                                if (colums.EndsWith(","))
                                {
                                    colums = colums.Substring(0, colums.Length - 1);
                                }
                                string localsql = @"SELECT {0} FROM " + imp.TableName;
                                localsql = string.Format(localsql, colums);

                                //查找本地库中的数据，并放入缓存中
                                IList<StrObjectDict> localDataList = DB.SelectAsync(localsql);
                                //CacheIO.Insert(ID,localDataList);
                                cache = localDataList;
                            }

                            IList<string> dblist = new List<string>();

                            StrObjectDict tempObj = null;
                            IList<StrObjectDict> updateList = new List<StrObjectDict>();

                            IList<IList<string>> batchSqlList = new List<IList<string>>();

                            if (!string.IsNullOrEmpty(imp.Identifying) || !string.IsNullOrEmpty(imp.ExtendTable))
                            {
                                string[] columns = imp.Identifying.Split(new char[] { ',' });

                                //string[] columns_values = new string[columns.Length];

                                //for (int j = 0; j < columns.Length; j++)
                                //{
                                //    IList<StrObjectDict> dict_temp = listRefColumns.FindBy("字段", columns[j]);
                                //    columns_values[j] = dict_temp[0]["三方字段"].ToString();
                                //}

                                string[] columns_values = new string[columns.Length];

                                for (int i = 0; i < cache.Count; i++)
                                {

                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        columns_values[j] = cache[i][columns[j]].ToString();
                                    }

                                    tempObj = remoteDataList.FindOnlyBy(columns, columns_values);
                                    if (tempObj != null)
                                    {
                                        updateList.Add(tempObj);
                                        ((List<StrObjectDict>)remoteDataList).RemoveAll(v => v.GetValues(columns) == tempObj.GetValues(columns));
                                        //i--;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < cache.Count; i++)
                                {
                                    tempObj = remoteDataList.FindOnlyBy("相关ID", cache[i]["相关ID"].ToString());
                                    if (tempObj != null)
                                    {
                                        updateList.Add(tempObj);
                                        ((List<StrObjectDict>)remoteDataList).RemoveAll(v => v["相关ID"].ToString() == tempObj["相关ID"].ToString());
                                        //i--;
                                    }
                                }
                            }

                            string sql_insert = @"INSERT INTO {0}({1}) VALUES({2});";
                            string sql_update = @"UPDATE {0} SET {1} WHERE {2};";
                            string insert_Colums = "";

                            foreach (var item in listRefColumns)
                            {
                                insert_Colums += item["字段"] + ",";
                            }
                            if (insert_Colums.EndsWith(","))
                            {
                                insert_Colums = insert_Colums.Substring(0, insert_Colums.Length - 1);
                            }

                            string values = string.Empty;
                            foreach (var item in remoteDataList)
                            {
                                foreach (var item2 in listRefColumns)
                                {
                                    object v = item[item2["字段"].ToString()];
                                    if (v.IsDateTime(true))
                                    {
                                        values += (v == null ? "''" : "TO_DATE('" + v + "'") + ",'yyyy-mm-dd hh24:mi:ss'),";
                                    }
                                    else
                                    {
                                        if (v != null)
                                        {
                                            if (v.ToString().Contains("?"))
                                            {
                                                v = v.ToString().Replace("?", "&iexcl;");
                                            }
                                        }
                                        else
                                        {
                                            v = string.Empty;
                                        }
                                        values += "'" + v + "',";
                                    }
                                }
                                if (dblist.Count >= 500)
                                {
                                    batchSqlList.Add(dblist);
                                    dblist = new List<string>();
                                }
                                dblist.Add(string.Format(sql_insert, imp.TableName, insert_Colums, values.Substring(0, values.Length - 1)));
                                values = string.Empty;
                            }
                            insert_count += remoteDataList.Count;

                            string update_Colums = string.Empty;
                            foreach (var item in updateList)
                            {
                                foreach (var item2 in listRefColumns)
                                {
                                    if (item2["字段"].ToString() == "ID" || item2["字段"].ToString() == "RelatID")
                                    {
                                        continue;
                                    }
                                    if (item[item2["字段"].ToString()].IsDateTime(true))
                                    {
                                        update_Colums += item2["字段"].ToString() + " = TO_DATE('" + item[item2["字段"].ToString()] + "','yyyy-mm-dd hh24:mi:ss'),";
                                    }
                                    else
                                        update_Colums += item2["字段"].ToString() + " = '" + item[item2["字段"].ToString()] + "',";
                                }
                                string strWhere = string.Empty;
                                if (!string.IsNullOrEmpty(imp.Identifying))
                                {
                                    string[] columns = imp.Identifying.Split(new char[] { ',' });
                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        if (j == columns.Length - 1)
                                            strWhere += columns[j] + "='" + item[columns[j]] + "'";
                                        else
                                            strWhere += columns[j] + "='" + item[columns[j]] + "' AND ";
                                    }
                                }
                                else
                                {
                                    strWhere = "相关ID = '" + item["ID"] + "'";
                                }
                                if (dblist.Count >= 500)
                                {
                                    batchSqlList.Add(dblist);
                                    dblist = new List<string>();
                                }

                                dblist.Add(string.Format(sql_update, imp.TableName, update_Colums.Substring(0, update_Colums.Length - 1), strWhere));
                                update_Colums = string.Empty;
                            }

                            update_count += updateList.Count();

                            if (dblist.Count > 0)
                                batchSqlList.Add(dblist);
                            dblist = null;

                            int isOk = 0;

                            //包含扩展信息sql
                            if (imp.ExtendTable != null && "" != imp.ExtendTable)
                            {
                                dblist = new List<string>();
                                //把扩展信息转换成多行数据
                                IList<StrObjectDict> remoteExtendDataListKeyValues = new List<StrObjectDict>();
                                StrObjectDict keyValues = null;



                                foreach (var item in remoteDataListExtend)
                                {
                                    foreach (var key in item.Keys)
                                    {
                                        if (key == "相关ID" || key == "ID")
                                        {
                                            continue;
                                        }
                                        keyValues = new StrObjectDict();
                                        keyValues["ObjectID"] = item["Sign"];//item["相关ID"];
                                        keyValues["ExtendName"] = key;
                                        keyValues["ExtendValue"] = item[key];
                                        remoteExtendDataListKeyValues.Add(keyValues);
                                    }
                                }
                                if (HttpRuntime.Cache.Get(imp.ExtendTable) == null)
                                {
                                    IList<StrObjectDict> localExtendDataList = DB.SelectAsync("SELECT * FROM " + imp.ExtendTable);
                                    if (localExtendDataList != null && localExtendDataList.Count > 0)
                                    {
                                        //CacheIO.Insert(imp.ExtendTable,localExtendDataList);
                                    }

                                    IDictionary<string, StrObjectDict> temp_extend_dict = remoteExtendDataListKeyValues.AsDictionary(new string[] { "ObjectID", "ExtendName" });

                                    remoteExtendDataListKeyValues.Clear();
                                    for (int i = 0; i < localExtendDataList.Count; i++)
                                    {
                                        //tempObj = remoteExtendDataListKeyValues.FindOnlyBy(new string[] { "ObjectID", "ExtendName" }, new string[] { localExtendDataList[i]["对象相关ID"].ToString(), localExtendDataList[i]["信息名"].ToString() });
                                        var key = localExtendDataList[i].GetValues(new string[] { "对象相关ID", "信息名" });

                                        tempObj = temp_extend_dict[key];
                                        if (tempObj != null)
                                        {

                                            if (dblist.Count >= 500)
                                            {
                                                batchSqlList.Add(dblist);
                                                dblist = new List<string>();
                                            }
                                            dblist.Add(string.Format(sql_update, imp.ExtendTable, "信息值='" + tempObj["ExtendValue"] + "'", "对象相关ID = '" + tempObj["ObjectID"].ToString() + "' AND 信息名='" + tempObj["ExtendName"].ToString() + "'"));
                                            //((List<StrObjectDict>)remoteExtendDataListKeyValues).RemoveAll(v => ((v["ObjectID"].ToString() == tempObj["ObjectID"].ToString() && (v["ExtendName"].ToString() == tempObj["ExtendName"]))));
                                            temp_extend_dict.Remove(localExtendDataList[i].GetValues(new string[] { "对象相关ID", "信息名" }));
                                            update_count++;
                                        }
                                    }
                                    //((List<StrObjectDict>)remoteExtendDataListKeyValues).AddRange(temp_extend_dict.Values);
                                    //temp_extend_dict.Clear();

                                    foreach (var key in temp_extend_dict.Keys)
                                    {
                                        tempObj = temp_extend_dict[key];
                                        if (dblist.Count >= 500)
                                        {
                                            batchSqlList.Add(dblist);
                                            dblist = new List<string>();
                                        }
                                        string temp_values = "'" + tempObj["ObjectID"].ToString() + "','" + tempObj["ExtendName"].ToString() + "','" + (tempObj["ExtendValue"] == null ? null : tempObj["ExtendValue"].ToString()) + "'";
                                        dblist.Add(string.Format(sql_insert, imp.ExtendTable, "对象相关ID,信息名,信息值", temp_values));
                                        insert_count++;
                                    }
                                    if (dblist.Count > 0 && dblist.Count < 500)
                                    {
                                        batchSqlList.Add(dblist);
                                    }
                                    dblist = null;
                                }

                                int rows = 0;
                                try
                                {
                                    #region 直接sql语句分批提交，结合内存比较，效率较高

                                    foreach (var item in batchSqlList)
                                    {
                                        localMap.SessionStore = new CallContextSessionStore(localMap.Id);
                                        localMap.BeginTransaction();
                                        int rs = DB.BatchExecuteWithOutTransaction(localMap, item);
                                        rows += Math.Abs(rs);

                                        DB.Commit(localMap);
                                    }
                                    #endregion

                                    #region 匿名块方式,耗时较长

                                    //IList<object> listExtends = new List<object>();
                                    //IList<DBState> listDbState = new List<DBState>();
                                    //for (int i = 0; i < remoteDataListExtend.Count; i++)
                                    //{
                                    //    foreach (var key in remoteDataListExtend[i].Keys)
                                    //    {
                                    //        if (key == "相关ID" || key == "ID")
                                    //        {
                                    //            continue;
                                    //        }

                                    //        listExtends.Add(new
                                    //        {
                                    //            ObjectID = remoteDataListExtend[i]["相关ID"],
                                    //            ExtendName = key,
                                    //            ExtendValue = remoteDataListExtend[i][key]
                                    //        }); 
                                    //        if (listExtends.Count >= 1000)
                                    //        {
                                    //            listDbState.Add(new DBState
                                    //            {
                                    //                Name = "BatchExtendExecuteNoneQuery2",
                                    //                Param = listExtends,
                                    //                Type = ESqlType.UPDATE
                                    //            });
                                    //            listExtends = new List<object>();
                                    //        }
                                    //    }
                                    //}

                                    //if (listExtends.Count > 0 && listExtends.Count < 1000)
                                    //{
                                    //    listDbState.Add(new DBState
                                    //    {
                                    //        Name = "BatchExtendExecuteNoneQuery2",
                                    //        Param = listExtends,
                                    //        Type = ESqlType.UPDATE
                                    //    });
                                    //}

                                    //foreach (var item in listDbState)
                                    //{
                                    //    DB.BeginTransaction();
                                    //    DB.ExecuteWithOutTransaction(DB.CurrentSqlMapper,item);
                                    //    DB.Commit(DB.CurrentSqlMapper);
                                    //}
                                    #endregion

                                }
                                catch (Exception)
                                {
                                    DB.RollBackTransaction(localMap);
                                }
                                isOk = rows;
                            }
                            else
                            {
                                try
                                {
                                    foreach (var item in batchSqlList)
                                    {
                                        localMap.SessionStore = new CallContextSessionStore(localMap.Id);
                                        localMap.BeginTransaction();
                                        int rs = DB.BatchExecuteWithOutTransaction(localMap, item);
                                        isOk += Math.Abs(rs);

                                        DB.Commit(localMap);
                                    }
                                }
                                catch (Exception)
                                {
                                    DB.RollBackTransaction(localMap);
                                }
                            }

                            if (isOk >= 0)
                            {
                                //修改最后导入数据时间和导入数据条数

                                long endTime = DateTime.Now.Ticks;
                                long takeTime = endTime - startTime;

                                double takeSec = takeTime / 10000000.0;

                                int resultUpdate = ThirdDataManager.Instance.InsertOrUpdate<ImportObject>(new
                                {
                                    ID = localTbID,
                                    ImportDataNumber = insert_count,
                                    UpdateDataNumber = update_count,
                                    ImportTime = DateTime.Now,
                                    TakeTime = takeSec
                                }.toStrObjDict());

                                //recordSql = string.Format(recordSql, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remoteDataList.Count,updateList.Count, localTbID);
                                //int resultUpdate = DB.Execute(recordSql);
                                StrObjectDict resultSod = new
                                {
                                    ImportResult = "数据同步成功"
                                }.toStrObjDict();

                                //更新缓存
                                //string colums = string.Empty;
                                //foreach (var item in listRefColumns)
                                //{
                                //    colums += item["字段"] + " AS \"" + item["字段"] + "\",";
                                //}
                                //if (colums.EndsWith(","))
                                //{
                                //    colums = colums.Substring(0, colums.Length - 1);
                                //}
                                //string localsql = @"SELECT {0} FROM " + imp.TableName;
                                //localsql = string.Format(localsql, colums);

                                //IList<StrObjectDict> localDataList = DB.Select(localsql);
                                //CacheIO.Remove(ID);
                                //CacheIO.Insert(ID,localDataList);

                                return new List<StrObjectDict>() { resultSod };
                            }
                            else
                            {
                                StrObjectDict resultSod = new
                                {
                                    ImportResult = "没有更新到数据"
                                }.toStrObjDict();
                                return new List<StrObjectDict>() { resultSod };
                            }
                        }
                        else
                        {
                            long endTime = DateTime.Now.Ticks;
                            long takeTime = endTime - startTime;

                            double takeSec = takeTime / 10000000.0;
                            int resultUpdate = ThirdDataManager.Instance.InsertOrUpdate<ImportObject>(new
                            {
                                ID = localTbID,
                                ImportDataNumber = 0,
                                UpdateDataNumber = 0,
                                ImportTime = DateTime.Now,
                                TakeTime = takeSec
                            }.toStrObjDict());

                            StrObjectDict resultSod = new
                            {
                                ImportResult = "没有更新到数据"
                            }.toStrObjDict();
                            return new List<StrObjectDict>() { resultSod };
                        }
                    }
                    else
                    {
                        string sql_Columns = @"SELECT a.字段,a.三方字段,b.表字段,b.应用字段 FROM 系统_三方服务字段对照 a,系统_元数据索引 b where a.字段 = b.表字段 AND a.服务ID = '" + ID + "'";

                        IList<StrObjectDict> list = DB.SelectAsync(dataSourceID, sql);

                        //IList<StrObjectDict> listRefColumns = DB.Select(sql_Columns);
                        //StrObjectDict dict = null;
                        //IList<StrObjectDict> remoteDataList = new List<StrObjectDict>();
                        //foreach (var item in list)
                        //{
                        //    dict = new StrObjectDict();
                        //    foreach (var key in item.Keys)
                        //    {
                        //        IList<StrObjectDict> l = listRefColumns.FindAllBy("三方字段", key);
                        //        if (l.Count > 1)
                        //        {
                        //            dict[l[0]["应用字段"].ToString()] = item[key];
                        //            dict[l[1]["应用字段"].ToString()] = item[key];
                        //        }
                        //        else if (l.Count > 0)
                        //        {
                        //            dict[l[0]["应用字段"].ToString()] = item[key];
                        //        }
                        //    }
                        //    remoteDataList.Add(dict);
                        //}

                        // return remoteDataList;
                        return list;

                    }
                }
            }
            else
            {
                return null;
            }
        }
        public string AsynDoService(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }
    }
}
