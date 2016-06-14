using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess;
using IBatisNet.DataMapper;
using IBatisNet.DataAccess.DaoSessionHandlers;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.Configuration.Statements;
using System.Data;
using System.Collections;
using IBatisNet.DataMapper.SessionStore;


namespace ZLSoft.Pub.Db
{
    public class DaoBase : IDao
    {
        private static Dictionary<string, DaoBase> _instance = new Dictionary<string, DaoBase>();

        public string ContextName
        {
            get;
            set;
        }
        public string ConnectString
        {
            get;
            set;
        }

        public static DaoBase getInstance()
        {
            if (!DaoBase._instance.ContainsKey("DEFAULT"))
            {
                DaoBase._instance["DEFAULT"] = new DaoBase();
                DaoBase._instance["DEFAULT"].ContextName = null;
            }
            return DaoBase._instance["DEFAULT"];
        }

        public static DaoBase getInstance(string as_ContextName)
        {
            string key;
            if (string.IsNullOrEmpty(as_ContextName))
            {
                key = "DEFAULT";
            }
            else
            {
                key = as_ContextName;
            }
            if (!DaoBase._instance.ContainsKey(key))
            {
                DaoBase._instance[key] = new DaoBase();
                DaoBase._instance[key].ContextName = as_ContextName;
            }
            return DaoBase._instance[key];
        }

        public string GetDbtype()
        {
            IDaoManager instance;
            if (string.IsNullOrEmpty(this.ContextName))
            {
                instance = DaoManager.GetInstance();
            }
            else
            {
                instance = DaoManager.GetInstance(this.ContextName);
            }
            return instance.LocalDataSource.DbProvider.Name;
        }

        public ISqlMapper GetSqlMap()
        {
            IDaoManager instance;
            if (string.IsNullOrEmpty(this.ContextName))
            {
                instance = DaoManager.GetInstance();
            }
            else
            {
                instance = DaoManager.GetInstance(this.ContextName);
            }
            ISqlMapper sqlMap = (instance.GetDaoSession() as SqlMapDaoSession).SqlMap;
            if (!string.IsNullOrEmpty(this.ConnectString))
            {
                sqlMap.DataSource.ConnectionString = this.ConnectString;
            }
            return sqlMap;
        }

        public IDaoManager getDaoManager(string name)
        {
            IDaoManager instance;
            if (string.IsNullOrEmpty(name))
            {
                instance = DaoManager.GetInstance();
            }
            else
            {
                instance = DaoManager.GetInstance(name);
            }
            return instance;
        }

        public System.Data.DataTable QueryForDataTable(string statementName, object paramObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            bool flag = false;
            System.DateTime now = System.DateTime.Now;
            System.Data.DataTable result;
            try
            {
                if (!sqlMap.IsSessionStarted)
                {
                    sqlMap.OpenConnection();
                    flag = true;
                }
                System.Data.DataSet dataSet = new System.Data.DataSet();
                IMappedStatement mappedStatement = sqlMap.GetMappedStatement(statementName);
                RequestScope requestScope = mappedStatement.Statement.Sql.GetRequestScope(mappedStatement, paramObject, sqlMap.LocalSession);
                mappedStatement.PreparedCommand.Create(requestScope, sqlMap.LocalSession, mappedStatement.Statement, paramObject);
                System.Reflection.FieldInfo field = requestScope.IDbCommand.GetType().GetField("_innerDbCommand", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                using (System.Data.IDbCommand dbCommand = (System.Data.IDbCommand)field.GetValue(requestScope.IDbCommand))
                {
                    int num = sqlMap.LocalSession.CreateDataAdapter(dbCommand).Fill(dataSet);
                }
                result = dataSet.Tables[0];
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (flag)
                {
                    sqlMap.CloseConnection();
                }
            }
            return result;
        }

        public System.Data.DataTable QueryForDataTable(string statementName, object paramObject, System.Collections.Generic.IDictionary<string, object> outputParameters)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.Data.DataTable result;
            try
            {
                result = sqlMap.QueryForDataTable(statementName, paramObject, outputParameters);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public System.Data.IDbCommand GetDbCommand(ISqlMapper sqlMapper, string statementName, object paramObject)
        {
            IStatement statement = sqlMapper.GetMappedStatement(statementName).Statement;
            IMappedStatement mappedStatement = sqlMapper.GetMappedStatement(statementName);
            ISqlMapSession sqlMapSession = new SqlMapSession(sqlMapper);
            if (sqlMapper.LocalSession != null)
            {
                sqlMapSession = sqlMapper.LocalSession;
            }
            else
            {
                sqlMapSession = sqlMapper.OpenConnection();
            }
            RequestScope requestScope = statement.Sql.GetRequestScope(mappedStatement, paramObject, sqlMapSession);
            mappedStatement.PreparedCommand.Create(requestScope, sqlMapSession, statement, paramObject);
            System.Data.IDbCommand dbCommand = sqlMapSession.CreateCommand(System.Data.CommandType.Text);
            dbCommand.CommandText = requestScope.IDbCommand.CommandText;
            return dbCommand;
        }

        private System.Data.IDbCommand GetDbCommand(ISqlMapper sqlMapper, string statementName, StrObjectDict paramObject, StrObjectDict dictParam, System.Collections.Generic.IDictionary<string, System.Data.ParameterDirection> dictParmDirection, System.Data.CommandType cmdType)
        {
            System.Data.IDbCommand result;
            if (cmdType == System.Data.CommandType.Text)
            {
                result = this.GetDbCommand(sqlMapper, statementName, paramObject);
            }
            else
            {
                IStatement statement = sqlMapper.GetMappedStatement(statementName).Statement;
                IMappedStatement mappedStatement = sqlMapper.GetMappedStatement(statementName);
                ISqlMapSession sqlMapSession = new SqlMapSession(sqlMapper);
                if (sqlMapper.LocalSession != null)
                {
                    sqlMapSession = sqlMapper.LocalSession;
                }
                else
                {
                    sqlMapSession = sqlMapper.OpenConnection();
                }
                RequestScope requestScope = statement.Sql.GetRequestScope(mappedStatement, paramObject, sqlMapSession);
                mappedStatement.PreparedCommand.Create(requestScope, sqlMapSession, statement, paramObject);
                System.Data.IDbCommand dbCommand = sqlMapSession.CreateCommand(cmdType);
                dbCommand.CommandText = requestScope.IDbCommand.CommandText;
                if (cmdType != System.Data.CommandType.StoredProcedure || dictParam == null)
                {
                    result = dbCommand;
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, object> current in dictParam)
                    {
                        string text = current.Key.ToString();
                        System.Data.IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
                        dbDataParameter.ParameterName = text;
                        dbDataParameter.Value = current.Value;
                        if (dictParmDirection != null && dictParmDirection.ContainsKey(text))
                        {
                            dbDataParameter.Direction = dictParmDirection[text];
                        }
                        dbCommand.Parameters.Add(dbDataParameter);
                    }
                    result = dbCommand;
                }
            }
            return result;
        }

        public System.Data.DataTable QueryForDataTable(string statementName, StrObjectDict paramObject, StrObjectDict dictParam, System.Collections.Generic.IDictionary<string, System.Data.ParameterDirection> dictParamDirection, out System.Collections.Hashtable htOutPutParameter)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.Data.DataSet dataSet = new System.Data.DataSet();
            bool flag = false;
            ISqlMapSession sqlMapSession = sqlMap.LocalSession;
            if (sqlMapSession == null)
            {
                sqlMapSession = new SqlMapSession(sqlMap);
                sqlMapSession.OpenConnection();
                flag = true;
            }
            System.Data.IDbCommand dbCommand = this.GetDbCommand(sqlMap, statementName, paramObject, dictParam, dictParamDirection, System.Data.CommandType.StoredProcedure);
            try
            {
                dbCommand.Connection = sqlMapSession.Connection;
                System.Data.IDbDataAdapter dbDataAdapter = sqlMapSession.CreateDataAdapter(dbCommand);
                dbDataAdapter.Fill(dataSet);
            }
            finally
            {
                if (flag)
                {
                    sqlMapSession.CloseConnection();
                }
            }
            htOutPutParameter = new System.Collections.Hashtable();
            foreach (System.Data.IDataParameter dataParameter in dbCommand.Parameters)
            {
                if (dataParameter.Direction == System.Data.ParameterDirection.Output)
                {
                    htOutPutParameter[dataParameter.ParameterName] = dataParameter.Value;
                }
            }
            return dataSet.Tables[0];
        }

        public System.Collections.Generic.IDictionary<K, V> ExecuteQueryForDictionary<K, V>(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.Collections.Generic.IDictionary<K, V> result;
            try
            {
                result = sqlMap.QueryForDictionary<K, V>(statementName, parameterObject, "ROWNUM");
            }
            catch
            {
                throw;
            }
            return result;
        }

        public object ExecuteScalar(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.DateTime now = System.DateTime.Now;
            object result;
            try
            {
                object obj = sqlMap.QueryScalar(statementName, parameterObject);
                result = obj;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            IList<T> result;
            try
            {
                IList<T> list = sqlMap.QueryForList<T>(statementName, parameterObject);
                result = list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public IList<T> ExecuteQueryForListAsync<T>(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            sqlMap.SessionStore = new CallContextSessionStore(sqlMap.Id);
            IList<T> result;
            try
            {
                IList<T> list = sqlMap.QueryForList<T>(statementName, parameterObject);
                result = list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string GetType(ESqlType type)
        {
            string result;
            if (type == ESqlType.INSERT)
            {
                result = "INSERT";
            }
            else
            {
                if (type == ESqlType.UPDATE)
                {
                    result = "UPDATE";
                }
                else
                {
                    if (type == ESqlType.DELETE)
                    {
                        result = "DELETE";
                    }
                    else
                    {
                        result = "SELECT";
                    }
                }
            }
            return result;
        }

        public System.Collections.Generic.IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.DateTime now = System.DateTime.Now;
            System.Collections.Generic.IList<T> result;
            try
            {
                System.Collections.Generic.IList<T> list = sqlMap.QueryForList<T>(statementName, parameterObject, skipResults, maxResults);
                result = list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public System.Collections.IList ExecuteQueryForList(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.Collections.IList result;
            try
            {
                result = sqlMap.QueryForList(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public System.Collections.IList ExecuteQueryForList(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.Collections.IList result;
            try
            {
                result = sqlMap.QueryForList(statementName, parameterObject, skipResults, maxResults);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public T ExecuteQueryForObject<T>(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            T result;
            try
            {
                result = sqlMap.QueryForObject<T>(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public T ExecuteQueryForObjectAsync<T>(string statementName, object parameterObject)
        {
            var sqlMap = this.GetSqlMap();
            sqlMap.SessionStore = new CallContextSessionStore(sqlMap.Id);
            T result;
            try
            {
                result = sqlMap.QueryForObject<T>(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public object ExecuteQueryForObject(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            object result;
            try
            {
                result = sqlMap.QueryForObject(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public int Execute(ISqlMapper sqlMap, DBState state)
        {
            int resultRowNum = 0;
            try
            {
                if (state.Type == ESqlType.INSERT)
                {
                    resultRowNum += sqlMap.Update(state.Name, state.Param);
                }
                else
                {
                    if (state.Type == ESqlType.UPDATE)
                    {
                        string sql = GetSql(state.Name, state.Param, true);
                        resultRowNum += sqlMap.Update(state.Name, state.Param);
                    }
                    else
                    {
                        resultRowNum += sqlMap.Delete(state.Name, state.Param);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultRowNum;
        }

        public int Execute(DBState state)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            int resultRowNum = 0;
            try
            {
                if (state.Type == ESqlType.INSERT)
                {
                    resultRowNum += sqlMap.Update(state.Name, state.Param);
                }
                else
                {
                    if (state.Type == ESqlType.UPDATE)
                    {
                        //string sql = GetSql(state.Name, state.Param,true);
                        resultRowNum += sqlMap.Update(state.Name, state.Param);
                    }
                    else
                    {
                        resultRowNum += sqlMap.Delete(state.Name, state.Param);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultRowNum;
        }

        public int Execute(string sqlStatement)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            string text = sqlStatement.ToUpper().Trim();
            ESqlType eSqlType = ESqlType.INSERT;
            int resultRowNum = 0;
            if (text.StartsWith(ESqlType.INSERT.ToString()))
            {
                eSqlType = ESqlType.INSERT;
            }
            else
            {
                if (text.StartsWith(ESqlType.UPDATE.ToString()))
                {
                    eSqlType = ESqlType.UPDATE;
                }
                else
                {
                    if (text.StartsWith(ESqlType.DELETE.ToString()))
                    {
                        eSqlType = ESqlType.DELETE;
                    }
                }
            }
            DBState dBState = new DBState
            {
                Name = "ExecuteNoneQuery",
                Param = sqlStatement,
                Type = eSqlType
            };
            if (eSqlType == ESqlType.INSERT)
            {
                resultRowNum += sqlMap.Update(dBState.Name, dBState.Param);
            }
            else
            {
                if (eSqlType == ESqlType.UPDATE)
                {
                    resultRowNum += sqlMap.Update(dBState.Name, dBState.Param);
                }
                else
                {
                    resultRowNum += sqlMap.Delete(dBState.Name, dBState.Param);
                }
            }

            return resultRowNum;
        }

        public int ExecuteInsert(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.DateTime now = System.DateTime.Now;
            int resultRowNum;
            try
            {
                resultRowNum = sqlMap.Update(statementName, parameterObject);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return resultRowNum;
        }

        public int ExecuteUpdate(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.DateTime now = System.DateTime.Now;
            int result;
            try
            {
                int num = sqlMap.Update(statementName, parameterObject);
                result = num;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void ExecuteStoredProcedureNoneQuery(string statementName, object parameterObject)
        {
            this.ExecuteUpdate(statementName, parameterObject);
        }

        public int ExecuteDelete(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            System.DateTime now = System.DateTime.Now;
            int result;
            try
            {
                int num = sqlMap.Delete(statementName, parameterObject);
                result = num;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ISqlMapper BeginTransaction()
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            sqlMap.BeginTransaction();
            return sqlMap;
        }

        public void CommitTransaction(ISqlMapper sqlMap)
        {
            sqlMap.CommitTransaction();
        }

        public void RollBackTransaction(ISqlMapper sqlMap)
        {
            sqlMap.RollBackTransaction();
        }

        public IList<StrObjectDict> ExecuteQueryForListWithTransaction(ISqlMapper sqlMap, string statementName, object parameterObject)
        {
            IList<StrObjectDict> result;
            try
            {
                result = sqlMap.QueryForList<StrObjectDict>(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public void ExecuteOracleStoredProcedureWithOutputCursor(string statementName, object parameterObject)
        {
            System.DateTime now = System.DateTime.Now;
            ISqlMapper sqlMap = this.GetSqlMap();
            try
            {
                sqlMap.OracleStoredProcedureWithOutputCursor(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
        }

        public DataTable ExecuteMsSqlStoredProcedureWithOutputCursor(string statementName, object parameterObject)
        {
            DateTime now = System.DateTime.Now;
            ISqlMapper sqlMap = this.GetSqlMap();
            System.Data.DataTable result;
            try
            {
                result = sqlMap.QueryForDataTable(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public void ExecuteMsSqlStoredProcedure(string statementName, object parameterObject)
        {
            DateTime now = System.DateTime.Now;
            ISqlMapper sqlMap = this.GetSqlMap();
            try
            {
                sqlMap.QueryForObject(statementName, parameterObject);
            }
            catch
            {
                throw;
            }
        }

        public int ExecuteWithTransaction(ISqlMapper sqlMap, System.Collections.Generic.IEnumerable<DBState> batchStatements)
        {
            int resultRowNum = 0;
            try
            {
                sqlMap.BeginTransaction();
                foreach (DBState current in batchStatements)
                {
                    if (current.Type == ESqlType.INSERT)
                    {
                        resultRowNum += sqlMap.Update(current.Name, current.Param);

                    }
                    else
                    {
                        if (current.Type == ESqlType.UPDATE)
                        {
                            resultRowNum += sqlMap.Update(current.Name, current.Param);
                        }
                        else
                        {
                            resultRowNum += sqlMap.Delete(current.Name, current.Param);
                        }
                    }
                }
                sqlMap.CommitTransaction();
            }
            catch
            {
                sqlMap.RollBackTransaction();
                throw;
            }

            return resultRowNum;
        }
        public int ExecuteWithTransaction(System.Collections.Generic.IEnumerable<DBState> batchStatements)
        {
            return this.ExecuteWithTransaction(this.GetSqlMap(), batchStatements);
        }

        public int Execute(IEnumerable<DBState> batchStatements)
        {
            //DateTime now = System.DateTime.Now;
            ISqlMapper sqlMap = this.GetSqlMap();
            int resultRowNum = 0;
            try
            {
                sqlMap.BeginTransaction();
                foreach (DBState current in batchStatements)
                {
                    if (current.Type == ESqlType.INSERT)
                    {
                        resultRowNum += sqlMap.Update(current.Name, current.Param);

                    }
                    else
                    {
                        if (current.Type == ESqlType.UPDATE)
                        {
                            resultRowNum += sqlMap.Update(current.Name, current.Param);
                        }
                        else
                        {
                            resultRowNum += sqlMap.Delete(current.Name, current.Param);
                        }
                    }
                }
                sqlMap.CommitTransaction();
            }
            catch (System.Exception ex)
            {
                sqlMap.RollBackTransaction();
                throw ex;
            }

            return resultRowNum;
        }
        public int ExecuteAsync(IEnumerable<DBState> batchStatements)
        {
            //DateTime now = System.DateTime.Now;
            ISqlMapper sqlMap = this.GetSqlMap();
            sqlMap.SessionStore = new CallContextSessionStore(sqlMap.Id);
            int resultRowNum = 0;
            try
            {
                sqlMap.BeginTransaction();
                foreach (DBState current in batchStatements)
                {
                    if (current.Type == ESqlType.INSERT)
                    {
                        resultRowNum += sqlMap.Update(current.Name, current.Param);

                    }
                    else
                    {
                        if (current.Type == ESqlType.UPDATE)
                        {
                            resultRowNum += sqlMap.Update(current.Name, current.Param);
                        }
                        else
                        {
                            resultRowNum += sqlMap.Delete(current.Name, current.Param);
                        }
                    }
                }
                sqlMap.CommitTransaction();
            }
            catch (System.Exception ex)
            {
                sqlMap.RollBackTransaction();
                throw ex;
            }

            return resultRowNum;
        }

        public int Execute(ISqlMapper sqlMap, IEnumerable<DBState> batchStatements)
        {
            //DateTime now = System.DateTime.Now;
            int resultRowNum = 0;
            foreach (DBState current in batchStatements)
            {
                if (current.Type == ESqlType.INSERT)
                {
                    resultRowNum += sqlMap.Update(current.Name, current.Param);

                }
                else
                {
                    if (current.Type == ESqlType.UPDATE)
                    {
                        resultRowNum += sqlMap.Update(current.Name, current.Param);
                    }
                    else
                    {
                        resultRowNum += sqlMap.Delete(current.Name, current.Param);
                    }
                }
            }

            return resultRowNum;
        }

        public int BatchExecute(ISqlMapper sqlMap, DBState state)
        {
            //DateTime now = System.DateTime.Now;
            int resultRowNum = 0;
            resultRowNum = sqlMap.Update(state.Name, state.Param);
            return resultRowNum;
        }


        public int Execute(IEnumerable<string> sqlStatements)
        {
            List<DBState> list = new List<DBState>();
            foreach (string current in sqlStatements)
            {
                string text = current;
                string text2 = text.ToUpper().Trim();
                ESqlType type = ESqlType.INSERT;
                if (text2.StartsWith(ESqlType.INSERT.ToString()))
                {
                    type = ESqlType.INSERT;
                }
                else
                {
                    if (text2.StartsWith(ESqlType.UPDATE.ToString()))
                    {
                        type = ESqlType.UPDATE;
                    }
                    else
                    {
                        if (text2.StartsWith(ESqlType.DELETE.ToString()))
                        {
                            type = ESqlType.DELETE;
                        }
                    }
                }
                list.Add(new DBState
                {
                    Name = "ExecuteNoneQuery",
                    Param = text,
                    Type = type
                });
            }
            return this.Execute(list);
        }

        public int Execute(ISqlMapper sqlMap, IEnumerable<string> sqlStatements, bool isBatch)
        {
            if (isBatch)
            {
                IList list = new ArrayList();
                foreach (var item in sqlStatements)
                {
                    list.Add(new
                    {
                        sql = item
                    });
                }

                return this.BatchExecute(sqlMap, new DBState
                {
                    Name = "BatchExecuteNoneQuery",
                    Param = list
                });
            }
            else
            {
                List<DBState> list = new List<DBState>();
                foreach (string current in sqlStatements)
                {
                    string text = current;
                    string text2 = text.ToUpper().Trim();
                    ESqlType type = ESqlType.INSERT;
                    if (text2.StartsWith(ESqlType.INSERT.ToString()))
                    {
                        type = ESqlType.INSERT;
                    }
                    else
                    {
                        if (text2.StartsWith(ESqlType.UPDATE.ToString()))
                        {
                            type = ESqlType.UPDATE;
                        }
                        else
                        {
                            if (text2.StartsWith(ESqlType.DELETE.ToString()))
                            {
                                type = ESqlType.DELETE;
                            }
                        }
                    }
                    list.Add(new DBState
                    {
                        Name = "ExecuteNoneQuery",
                        Param = new
                        {
                            sql = current
                        },
                        Type = type
                    });
                }
                return this.Execute(sqlMap, list);
            }
        }

        public string GetSql(string statementName, object paramObject, bool isPremitive)
        {
            ISqlMapper sqlMap = this.GetSqlMap();
            string result;
            try
            {
                result = sqlMap.ExtractSql(statementName, paramObject, isPremitive);
            }
            catch
            {
                result = "";
            }
            return result;
        }


    }
}
