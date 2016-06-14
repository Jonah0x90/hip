using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.DataFilterChainManager
{
    public static class DataFiltersAction
    {
        public static string ChuliSql(string sql, string YHID, string FAID)
        {
            return DataFiltersAction.ChuliSql(sql, YHID, FAID, null);
        }
        public static string ChuliSql(string sql, string YHID, string FAID, string DXID)
        {
            string text = sql.Replace("$YHID$", YHID);
            text = text.Replace("$DXID$", DXID);
            return text.Replace("$FAID$", FAID);
        }
    }
}
