using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZLSoft.Model.THIRD;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    public class ThirdDataManager:CRUDManager
    {
        private static ThirdDataManager _Instance = new ThirdDataManager();

        public static ThirdDataManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        private ThirdDataManager()
        {

        }

        public IList<ImportObject> GetImportDataObjectList()
        {
            return this.List<ImportObject>(null);
        }

        //public IList<ImportObject> GetImportDataObjectList2()
        //{
        //    return DB.Select("d6ef68ec-d1df-45b8-af85-651103a192fd", "Select '1','d' from dual");
        //}


        public IList<StrObjectDict> GetImportDataObjectColumns(string tableName)
        {
            return this.ListSod("LIST_ImportObjectColumns", new
            {
                TableName = tableName
            }.toStrObjDict());
        }

        public IList<string> GetSqlMeta(string dataSource,string sql)
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
    }
}
