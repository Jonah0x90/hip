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
    public class DataFilterExt:IDataFilter
    {
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
		public string DYID
		{
			get;
			set;
		}
		public string KZID
		{
			get;
			set;
		}
		public string SUFFIX
		{
			get;
			set;
		}
		public string KZ_ZSQL
		{
			get;
			set;
		}
        public DataFilterExt(string YHID, string FAID, string DYID, string KZID, string SUFFIX = null)
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
			this.DYID = DYID;
			this.YHID = YHID;
			this.KZID = KZID;
			this.SUFFIX = SUFFIX;
			string format = "SELECT XXMXID {0} FROM ({1}) A  GROUP BY XXMXID";
			System.Collections.Generic.IList<XT_SJQX_DX> list = DB.List<XT_SJQX_DX>(new
			{
				FAID = FAID,
				ORDER_BY_CLAUSE = " PXXH ASC"
			}.toStrObjDict());
			string text = "";
			foreach (XT_SJQX_DX current in list)
			{
				string text2 = current.GXSQL;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = string.Concat(new object[]
					{
						"SELECT '$DXID$' AS DXID, '$YHID$' AS DXMXID,'$YHID$' AS YHID,",
						current.PXXH,
						" AS PXXH ",
						this.dual_sql
					});
				}
				else
				{
					text2 = string.Concat(new object[]
					{
						"SELECT A.DXID,A.DXMXID,A.YHID,",
						current.PXXH,
						" AS PXXH FROM (",
						text2,
						") A WHERE YHID = '$YHID$'"
					});
				}
                text = (string.IsNullOrEmpty(text) ? "" : " UNION ") + DataFiltersAction.ChuliSql(text2, YHID, FAID, current.DXID);
			}
			System.Collections.Generic.IList<StrObjectDict> list2 = DB.ListSod("LIST_XT_SJQX_QX_BY_DY", new
			{
				FAID = FAID,
				DYID = DYID,
				KZID = KZID,
				DXSQL = text
			}.toStrObjDict());
			if (list2.Count > 0)
			{
                string PXXH = Utils.GetString(list2.First<StrObjectDict>()["PXXH"]);
				string text3 = string.Format("SELECT DISTINCT A.XXMXID, A.XXID, A.KCID FROM XT_SJQX_QXMX A ,({0}) B,XT_SJQX_DY_XX C WHERE  A.DXID=B.DXID AND A.DXMXID=B.DXMXID  AND A.FAID=C.FAID AND C.DYID=A.DYID AND C.XXID=A.XXID AND  A.FAID='{1}'AND A.DYID='{2}' AND A.KCID='{3}' AND B.PXXH='{4}' AND C.XXLX=2", new object[]
				{
					text,
					FAID,
					DYID,
					KZID,
					PXXH
				});
                System.Collections.Generic.List<StrObjectDict> source = (
					from item in list2
					where Utils.GetString(item["PXXH"]) == PXXH
                    select item).ToList<StrObjectDict>();
				string text4 = "";
				if ((
					from item in source
					where Utils.GetString(item["XXLX"]) == "0"
                    select item).Count<StrObjectDict>() == 0)
				{
					string text5 = text4;
					text4 = string.Concat(new string[]
					{
						text5,
						",(case when sum(case when kcid = '",
						KZID,
						"' then 1 else 0 end )>0 then 1 else 0 end) AS ",
						string.IsNullOrEmpty(SUFFIX) ? (FAID + "_" + DYID) : SUFFIX,
						"_KZ_",
						KZID
					});
                    foreach (StrObjectDict current2 in 
						from item in source
						where Utils.GetString(item["XXLX"]) == "1"
						select item)
					{
                        text3 += string.Format(" UNION SELECT A.ID AS XXMXID,'{0}' AS XXID,'{1}' AS KCID FROM ({2}) A ", Utils.GetString(current2["XXID"]), KZID, DataFiltersAction.ChuliSql(Utils.GetString(current2["ZSQL"]), YHID, FAID));
					}
                    foreach (StrObjectDict current2 in 
						from item in source
						where Utils.GetString(item["XXLX"]) == "3"
						select item)
					{
                        string arg = DataFiltersAction.ChuliSql(Utils.GetString(current2["GLSQL"]), YHID, FAID);
						string arg2 = string.Format("SELECT DISTINCT A.XXMXID, A.XXID, A.KCID FROM XT_SJQX_QXMX A ,({0}) B,XT_SJQX_DY_XX C WHERE  A.DXID=B.DXID AND A.DXMXID=B.DXMXID  AND A.FAID=C.FAID AND C.DYID=A.DYID AND C.XXID=A.XXID AND  A.FAID='{1}'AND A.DYID='{2}' AND A.KCID='{3}' AND A.XXID='{4}'", new object[]
						{
							text,
							FAID,
							DYID,
							KZID,
							Utils.GetString(current2["XXID"])
						});
						text3 += string.Format(" UNION SELECT A.XXMXID,B.XXID,B.KCID FROM ({0}) A INNER JOIN ({1}) B ON A.FLID = B.XXMXID", arg, arg2);
					}
				}
				else
				{
                    StrObjectDict strObjDict = (
						from item in source
						where Utils.GetString(item["XXLX"]) == "0"
                        select item).ToList<StrObjectDict>()[0];
					string text5 = text4;
					text4 = string.Concat(new string[]
					{
						text5,
						",1 as ",
						string.IsNullOrEmpty(SUFFIX) ? (FAID + "_" + DYID) : SUFFIX,
						"_KZ_",
						KZID
					});
                    text3 += string.Format(" UNION SELECT A.ID AS XXMXID,'{0}' AS XXID,'{1}' AS KCID FROM ({2}) A ", Utils.GetString(strObjDict["XXID"]), KZID, DataFiltersAction.ChuliSql(Utils.GetString(strObjDict["ZSQL"]), YHID, FAID));
				}
				this.KZ_ZSQL = string.Format(format, text4, text3);
			}
			else
			{
				XT_SJQX_DY xT_SJQX_DY = DB.Load<XT_SJQX_DY, PK_XT_SJQX_DY>(new PK_XT_SJQX_DY
				{
					FAID = FAID,
					DYID = DYID
				});
				if (!string.IsNullOrEmpty(xT_SJQX_DY.MRZ))
				{
					XT_SJQX_DY_XX xT_SJQX_DY_XX = DB.Load<XT_SJQX_DY_XX, PK_XT_SJQX_DY_XX>(new PK_XT_SJQX_DY_XX
					{
						FAID = FAID,
						DYID = DYID,
						XXID = xT_SJQX_DY.MRZ
					});
					if (xT_SJQX_DY_XX.XXLX == 0 || xT_SJQX_DY_XX.XXLX == 1)
					{
                        this.KZ_ZSQL = string.Format(" SELECT A.ID AS XXMXID,1 AS " + (string.IsNullOrEmpty(SUFFIX) ? (FAID + "_" + DYID) : SUFFIX) + "_KZ_{0}  FROM ({1}) A ", KZID, DataFiltersAction.ChuliSql(Utils.GetString(xT_SJQX_DY_XX.ZSQL), YHID, FAID));
						return;
					}
				}
				if (string.IsNullOrEmpty(this.KZ_ZSQL))
				{
					this.KZ_ZSQL = string.Format(" SELECT NULL AS XXMXID, 0 AS " + (string.IsNullOrEmpty(SUFFIX) ? (FAID + "_" + DYID) : SUFFIX) + "_KZ_{0} " + this.dual_sql, KZID);
				}
			}
		}
        public System.Collections.Generic.IList<StrObjectDict> getDataPermissble()
		{
			return DB.Select(this.KZ_ZSQL);
		}
        public System.Collections.Generic.IList<StrObjectDict> filterSource(System.Collections.Generic.IList<StrObjectDict> source, string expression)
		{
            System.Collections.Generic.IList<StrObjectDict> filterData = this.getDataPermissble();
			string[] exps = expression.Split("|".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
			return (
				from item in source
                where filterData.Any(delegate(StrObjectDict item2)
				{
					bool result = false;
					//string[] expss = exps;
					for (int i = 0; i < exps.Length; i++)
					{
						string key = exps[i];
						if (Utils.GetString(item2["XXMXID"]) == Utils.GetString(item[key]))
						{
							result = true;
							break;
						}
					}
					return result;
				})
                select item).ToList<StrObjectDict>();
		}
		public string filterSql(string source, string expression)
		{
			string[] array = expression.Split("|".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
			string result;
			if (array.Length == 1)
			{
				result = string.Format("SELECT A.*,B." + (string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX) + "_KZ_{0} FROM ({1}) A,({2}) B WHERE A.{3}=B.XXMXID", new object[]
				{
					this.KZID,
					source,
					this.KZ_ZSQL,
					expression
				});
			}
			else
			{
				string text = "(";
				string text2 = "";
				string text3 = "";
				int num = 0;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string arg = array2[i];
					if (num++ != 0)
					{
						text += " OR ";
						text2 += " AND ";
						text3 += ",";
					}
					text += string.Format("A.{0}=B.XXMXID", arg);
					text2 += string.Format("(A.{0}=B.{0} OR (A.{0} IS NULL AND B.{0} IS NULL))", arg);
					text3 += string.Format("A.{0}", arg);
				}
				text += ")";
				string text4 = string.Format("SELECT DISTINCT {4},B." + (string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX) + "_KZ_{0} FROM ({1}) A,({2}) B WHERE {3}", new object[]
				{
					this.KZID,
					source,
					this.KZ_ZSQL,
					text,
					text3
				});
				result = string.Format("SELECT A.*,B." + (string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX) + "_KZ_{0} FROM ({1}) A,({2}) B WHERE {3}", new object[]
				{
					this.KZID,
					source,
					text4,
					text2
				});
			}
			return result;
		}
    }
}
