using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;

namespace ZLSoft.DataFilterChainManager
{
    public class DataFilterChainsManager
    {
        private static DataFilterChainsManager _Instance = new DataFilterChainsManager();
		public static DataFilterChainsManager Instance
		{
			get
			{
				return DataFilterChainsManager._Instance;
			}
		}


        private DataFilterChainsManager()
		{
		}


        public IList<StrObjectDict> filterSourceByIDataLimits(System.Collections.Generic.IList<StrObjectDict> source, IDataFilter dataLimits, string expression)
		{
			return dataLimits.filterSource(source, expression);
		}

		public string filterSqlByIDataLimits(string sourceSql, IDataFilter dataLimits, string expression)
		{
			return dataLimits.filterSql(sourceSql, expression);
		}
    }
}
