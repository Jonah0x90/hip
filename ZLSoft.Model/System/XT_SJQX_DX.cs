using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_SJQX_DX : BuzzModel
    {
        public string FAID
        {
            get;
            set;
        }
        public string DXID
        {
            get;
            set;
        }
        public string DXMC
        {
            get;
            set;
        }
        public string FWSQL
        {
            get;
            set;
        }
        public string ZRSQL
        {
            get;
            set;
        }
        public string GXSQL
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
            return "XT_SJQX_DX";
        }
        public override string GetTableName()
        {
            return "XT_SJQX_DX";
        }
    }
}
