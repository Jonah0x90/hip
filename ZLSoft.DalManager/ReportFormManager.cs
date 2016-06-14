using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public class ReportFormManager : CRUDManager
    {

        private static ReportFormManager _Instance = new ReportFormManager();

        public static ReportFormManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        private ReportFormManager()
        {

        }

        /// <summary>
        /// SQL 查询分析器
        /// </summary>
        /// <param name="dataSource">数据源ID</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public IList<string> SqlAnalysis(string dataSource,string sql)
        {
            DataTable dt = DB.Meta(dataSource,new
            {
                SQL = sql
            }.toStrObjDict());

            IList<string> list = new List<string>();

            for (int i = 0; i < dt.Columns.Count; i++)
			{
                list.Add(dt.Columns[i].ColumnName);
			}

            return list;
        }

        public IList<StrObjectDict> SqlExcute(string dataSource, string sql)
        {
            return DB.Select(dataSource, sql);          
        }

        /// <summary>
        /// 获取数据源ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetDateSource(IDictionary<string,object> obj)
        {
            return DB.ListSod("Get_DataSource", obj);
        }

        /// <summary>
        /// 保存报表分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int TypeInsertOrUpdate(IDictionary<string,object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                obj["ID"] = Utils.getGUID();
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ReportFormType",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ReportFormType",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
        }

        /// <summary>
        /// 删除报表分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int TypeDel(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ReportFormType",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state); 
        }

        /// <summary>
        /// 请求报表分类（分页）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<StrObjectDict> TypePage(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_ReportFormType", obj);
            IList<StrObjectDict> listData = DB.ListSod("Info_ReportFormType", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }

        /// <summary>
        /// 请求报表分类List
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<ReportFormType> TypeList(IDictionary<string, object> obj)
        {
            return DB.List<ReportFormType>(obj);
        }

        /// <summary>
        /// 请求报表分类详细数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> TypeData(IDictionary<string, object> obj)
        {
            return DB.ListSod("Info_ReportFormType", obj);
        }

        /// <summary>
        /// 保存报表数据源
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SourcesInsertOrUpdate(IDictionary<string, object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                string ID = Utils.getGUID();
                obj["ID"] = ID;
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ReportFormSources",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                if (DB.Execute(state) > 0)
                {
                    return ID;
                }
                else
                {
                    return "Error";
                }
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ReportFormSources",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                if (DB.Execute(state) > 0)
                {
                    return obj["ID"].ToString();
                }
                else
                {
                    return "Error";
                }
            }
        }

        /// <summary>
        /// 删除报表数据源
        /// </summary>
        /// <returns></returns>
        public int SourcesDel(IDictionary<string,object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ReportFormSources",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state); 
        }

        /// <summary>
        /// 请求报表数据源（分页）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<StrObjectDict> SourcesPage(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_ReportFormSources", obj);
            IList<StrObjectDict> listData = DB.ListSod("Info_ReportFormSources", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }

        /// <summary>
        /// 请求报表数据源List
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> SourcesList(IDictionary<string, object> obj)
        {
            return DB.ListSod("Info_ReportFormSources", obj);
        }

        /// <summary>
        /// 请求报表数据源详细数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> SourcesData(IDictionary<string, object> obj)
        {
            return DB.ListSod("Info_ReportFormSources", obj);
        }


        /// <summary>
        /// 保存报表文件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string FileInsertOrUpdate(IDictionary<string, object> obj)
        {

            var id = Utils.getGUID();

            if (!obj.ContainsKey("ID"))
            {
                obj["ID"] = id;
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ReportFormFile",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                DB.Execute(state);
                return id;
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ReportFormFile",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                DB.Execute(state);
                return null;
            }
        }

        /// <summary>
        /// 删除报表文件
        /// </summary>
        /// <returns></returns>
        public int FileDel(IDictionary<string,object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ReportFormFile",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 请求报表文件（分页）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<StrObjectDict> FilePage(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_ReportFormFile", obj);
            IList<StrObjectDict> listData = DB.ListSod("Info_ReportFormFile", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }

        /// <summary>
        /// 请求报表文件List
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<ReportFormFile> FileList(IDictionary<string, object> obj)
        {
            return DB.List<ReportFormFile>(obj);
        }

        /// <summary>
        /// 请求报表文件详细数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> FileData(IDictionary<string, object> obj)
        {
            return DB.ListSod("Info_ReportFormFile", obj);
        }



    }
}
