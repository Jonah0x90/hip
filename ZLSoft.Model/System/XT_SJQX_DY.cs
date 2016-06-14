using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_SJQX_DY : BuzzModel
	{
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
		public string DYMC
		{
			get;
			set;
		}
		public string KCSQL
		{
			get;
			set;
		}
		public string FYMC
		{
			get;
			set;
		}
		public string MRZ
		{
			get;
			set;
		}
		public int? PXXH
		{
			get;
			set;
		}
		public string XGR
		{
			get;
			set;
		}
		public DateTime? XGRQ
		{
			get;
			set;
		}
		public override string GetModelName()
		{
			return "XT_SJQX_DY";
		}
		public override string GetTableName()
		{
			return "XT_SJQX_DY";
		}
	}
}
