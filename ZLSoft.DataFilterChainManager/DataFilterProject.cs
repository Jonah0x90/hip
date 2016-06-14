using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Constant;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Model.System;

namespace ZLSoft.DataFilterChainManager
{
    public class DataFilterProject:IDataFilter
    {
        public IDictionary<string, DataFilterUnit> Ddys = new Dictionary<string, DataFilterUnit>();
		public DBSERVER_TYPE dbserver_type;
		public string dual_sql = "";
		public string YHID
		{
			get;
			set;
		}
		public string FAID
		{
			get;
			set;
		}
		public string SUFFIX
		{
			get;
			set;
		}
        public DataFilterProject(string YHID, string FAID, string SUFFIX = null)
		{
			this.dbserver_type = DB.GetDbtype();
			if (this.dbserver_type == DBSERVER_TYPE.ORACLE)
			{
				this.dual_sql = " from dual ";
			}
			else
			{
				this.dual_sql = " ";
			}
			this.FAID = FAID;
			this.YHID = YHID;
			this.SUFFIX = SUFFIX;
			IList<XT_SJQX_DY> list = DB.List<XT_SJQX_DY>(new
			{
				FAID
			}.toStrObjDict());
			foreach (XT_SJQX_DY current in list)
			{
				this.Ddys.Add(current.DYID, new DataFilterUnit(YHID, FAID, current.DYID, SUFFIX));
			}
		}
        public IList<StrObjectDict> filterSource(IList<StrObjectDict> source, string expression)
		{
            IList<StrObjectDict> result;
			if (this.Ddys.Count > 0)
			{
				result = this.Ddys.First<KeyValuePair<string, DataFilterUnit>>().Value.filterSource(source, expression);
			}
			else
			{
				result = source;
			}
			return result;
		}
		public string filterSql(string source, string expression)
		{
			string result;
			if (this.Ddys.Count > 0)
			{
				result = this.Ddys.First<KeyValuePair<string, DataFilterUnit>>().Value.filterSql(source, expression);
			}
			else
			{
				result = source;
			}
			return result;
		}
    }
}
