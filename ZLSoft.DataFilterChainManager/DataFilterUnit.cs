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
    public class DataFilterUnit:IDataFilter
    {
        public DBSERVER_TYPE dbserver_type;
		public string dual_sql = "";
		public IDictionary<string, DataFilterExt> Dkzs = new Dictionary<string, DataFilterExt>();
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
		public string SUFFIX
		{
			get;
			set;
		}
		public bool HasKz
		{
			get;
			set;
		}
		public string KZ_ZSQL
		{
			get;
			set;
		}
        public DataFilterUnit(string YHID, string FAID, string DYID, string SUFFIX = null)
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
			this.SUFFIX = SUFFIX;
			this.HasKz = false;
			XT_SJQX_DY xT_SJQX_DY = DB.Load<XT_SJQX_DY, PK_XT_SJQX_DY>(new PK_XT_SJQX_DY
			{
				FAID = FAID,
				DYID = DYID
			});
			if (!string.IsNullOrEmpty(xT_SJQX_DY.KCSQL))
			{
				IList<StrObjectDict> list = DB.Select(xT_SJQX_DY.KCSQL);
				if (list.Count > 0)
				{
					foreach (StrObjectDict current in list)
					{
						this.Dkzs.Add(Utils.GetString(current["ID"]), new DataFilterExt(YHID, FAID, DYID, Utils.GetString(current["ID"]), SUFFIX));
					}
					this.HasKz = true;
					this.initKzs(xT_SJQX_DY, list);
				}
			}
			if (this.Dkzs.Count == 0)
			{
                this.Dkzs.Add("ALL", new DataFilterExt(YHID, FAID, DYID, "ALL", SUFFIX));
			}
		}
		private void initKzs(XT_SJQX_DY dy, IList<StrObjectDict> kzs)
		{
			string format = "SELECT XXMXID {0} FROM ({1}) A  GROUP BY XXMXID";
			IList<XT_SJQX_DX> list = DB.List<XT_SJQX_DX>(new
			{
				FAID = this.FAID,
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
                text = (string.IsNullOrEmpty(text) ? "" : " UNION ") + DataFiltersAction.ChuliSql(text2, this.YHID, this.FAID, current.DXID);
			}
			IList<StrObjectDict> list2 = DB.ListSod("LIST_XT_SJQX_QX_BY_DY", new
			{
				FAID = this.FAID,
				DYID = this.DYID,
				DXSQL = text
			}.toStrObjDict());
			if (list2.Count > 0)
			{
				string PXXH = Utils.GetString(list2.First<StrObjectDict>()["PXXH"]);
				string text3 = string.Format("SELECT DISTINCT A.XXMXID, A.XXID, A.KCID FROM XT_SJQX_QXMX A ,({0}) B,XT_SJQX_DY_XX C WHERE  A.DXID=B.DXID AND A.DXMXID=B.DXMXID  AND A.FAID=C.FAID AND C.DYID=A.DYID AND C.XXID=A.XXID AND  A.FAID='{1}'AND A.DYID='{2}' AND B.PXXH='{3}' AND C.XXLX=2", new object[]
				{
					text,
					this.FAID,
					this.DYID,
					PXXH
				});
				List<StrObjectDict> source = (
					from item in list2
					where Utils.GetString(item["PXXH"]) == PXXH
					select item).ToList<StrObjectDict>();
				string text4 = "";
				foreach (StrObjectDict kz in kzs)
				{
					if ((
						from item in source
						where Utils.GetString(item["XXLX"]) == "0" && Utils.GetString(item["KCID"]) == Utils.GetString(kz["ID"])
						select item).Count<StrObjectDict>() == 0)
					{
						string text5 = text4;
						text4 = string.Concat(new string[]
						{
							text5,
							",(case when sum(case when kcid ='",
							Utils.GetString(kz["ID"]),
							"' then 1 else 0 end)>0 then 1 else 0 end) AS ",
							string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX,
							"_KZ_",
							Utils.GetString(kz["ID"])
						});
						foreach (StrObjectDict current2 in 
							from item in source
							where Utils.GetString(item["XXLX"]) == "1" && Utils.GetString(item["KCID"]) == Utils.GetString(kz["ID"])
							select item)
						{
                            text3 += string.Format(" UNION SELECT A.ID AS XXMXID,'{0}' AS XXID,'{1}' AS KCID FROM ({2}) A ", Utils.GetString(current2["XXID"]), Utils.GetString(kz["ID"]), DataFiltersAction.ChuliSql(Utils.GetString(current2["ZSQL"]), this.YHID, this.FAID));
						}
						foreach (StrObjectDict current2 in 
							from item in source
							where Utils.GetString(item["XXLX"]) == "3" && Utils.GetString(item["KCID"]) == Utils.GetString(kz["ID"])
							select item)
						{
                            string arg = DataFiltersAction.ChuliSql(Utils.GetString(current2["GLSQL"]), this.YHID, this.FAID);
							string arg2 = string.Format("SELECT DISTINCT A.XXMXID, A.XXID, A.KCID FROM XT_SJQX_QXMX A ,({0}) B,XT_SJQX_DY_XX C WHERE  A.DXID=B.DXID AND A.DXMXID=B.DXMXID  AND A.FAID=C.FAID AND C.DYID=A.DYID AND C.XXID=A.XXID AND  A.FAID='{1}'AND A.DYID='{2}' AND A.KCID='{3}' AND A.XXID='{4}'", new object[]
							{
								text,
								this.FAID,
								this.DYID,
								Utils.GetString(kz["ID"]),
								Utils.GetString(current2["XXID"])
							});
							text3 += string.Format(" UNION SELECT A.XXMXID,B.XXID,B.KCID FROM ({0}) A INNER JOIN ({1}) B ON A.FLID = B.XXMXID", arg, arg2);
						}
					}
					else
					{
						StrObjectDict strObjDict = (
							from item in source
							where Utils.GetString(item["XXLX"]) == "0" && Utils.GetString(item["KCID"]) == Utils.GetString(kz["ID"])
							select item).ToList<StrObjectDict>()[0];
						string text5 = text4;
						text4 = string.Concat(new string[]
						{
							text5,
							",1 as ",
							string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX,
							"_KZ_",
							Utils.GetString(kz["ID"])
						});
                        text3 += string.Format(" UNION SELECT A.ID AS XXMXID,'{0}' AS XXID,'{1}' AS KCID FROM ({2}) A ", Utils.GetString(strObjDict["XXID"]), Utils.GetString(kz["ID"]), DataFiltersAction.ChuliSql(Utils.GetString(strObjDict["ZSQL"]), this.YHID, this.FAID));
					}
				}
				this.KZ_ZSQL = string.Format(format, text4, text3);
			}
			else
			{
				if (!string.IsNullOrEmpty(dy.MRZ))
				{
					XT_SJQX_DY_XX xT_SJQX_DY_XX = DB.Load<XT_SJQX_DY_XX, PK_XT_SJQX_DY_XX>(new PK_XT_SJQX_DY_XX
					{
						FAID = this.FAID,
						DYID = this.DYID,
						XXID = dy.MRZ
					});
					if (xT_SJQX_DY_XX.XXLX == 0 || xT_SJQX_DY_XX.XXLX == 1)
					{
						string text4 = "";
						foreach (StrObjectDict current3 in kzs)
						{
							string text5 = text4;
							text4 = string.Concat(new string[]
							{
								text5,
								",1 as ",
								string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX,
								"_KZ_",
								Utils.GetString(current3["ID"])
							});
						}
                        this.KZ_ZSQL = string.Format(" SELECT A.ID AS XXMXID,{0} FROM ({1}) A ", text4, DataFiltersAction.ChuliSql(Utils.GetString(xT_SJQX_DY_XX.ZSQL), this.YHID, this.FAID));
						return;
					}
				}
				if (string.IsNullOrEmpty(this.KZ_ZSQL))
				{
					string text4 = "";
					foreach (StrObjectDict current3 in kzs)
					{
						string text5 = text4;
						text4 = string.Concat(new string[]
						{
							text5,
							",0 as ",
							string.IsNullOrEmpty(this.SUFFIX) ? (this.FAID + "_" + this.DYID) : this.SUFFIX,
							"_KZ_",
							Utils.GetString(current3["ID"])
						});
					}
					this.KZ_ZSQL = string.Format(" SELECT NULL AS XXMXID{0}  " + this.dual_sql, text4);
				}
			}
		}
		public IList<StrObjectDict> getDataPermissble()
		{
			return DB.Select(this.KZ_ZSQL);
		}
		public IList<StrObjectDict> filterSource(IList<StrObjectDict> source, string expression)
		{
			IList<StrObjectDict> result;
			if (!this.HasKz)
			{
				result = this.Dkzs["ALL"].filterSource(source, expression);
			}
			else
			{
				IList<StrObjectDict> dataPermissble = this.getDataPermissble();
				string[] array = expression.Split("|".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 1)
				{
					IEnumerable<StrObjectDict> source2 = 
						from s in source
						join f in dataPermissble on Utils.GetString(s[expression]) equals Utils.GetString(f["XXMXID"]) into fj
						from f in 
							from f in fj
							select f
						select s.Union2(f);
					result = source2.ToList<StrObjectDict>();
				}
				else
				{
					IList<StrObjectDict> list = new List<StrObjectDict>();
					string[] array2 = array;
					string exp;
					for (int i = 0; i < array2.Length; i++)
					{
						exp = array2[i];
						IEnumerable<StrObjectDict> source2 = 
							from s in source
							join f in dataPermissble on Utils.GetString(s[exp]) equals Utils.GetString(f["XXMXID"]) into fj
							from f in 
								from f in fj
								select f
							select s.Union2(f);
						list = list.Union(source2.ToList<StrObjectDict>()).ToList<StrObjectDict>();
					}
					result = list;
				}
			}
			return result;
		}
		public string filterSql(string source, string expression)
		{
			string result;
			if (!this.HasKz)
			{
				result = this.Dkzs["ALL"].filterSql(source, expression);
			}
			else
			{
				string[] array = expression.Split("|".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 1)
				{
					result = string.Format("SELECT A.* ,B.* FROM ({0}) A,({1}) B WHERE A.{2}=B.XXMXID", source, this.KZ_ZSQL, expression);
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
					string arg2 = string.Format("SELECT DISTINCT {3} ,B.* FROM ({0}) A,({1}) B WHERE {2}", new object[]
					{
						source,
						this.KZ_ZSQL,
						text,
						text3
					});
					result = string.Format("SELECT A.* ,B.* FROM ({0}) A,({1}) B WHERE A.{2}=B.XXMXID", source, arg2, text2);
				}
			}
			return result;
		}
    }
}
