using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_SJQX_DY_XX : BuzzModel
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
        public string XXID
        {
            get;
            set;
        }
        public string XXMC
        {
            get;
            set;
        }
        public int? XXLX
        {
            get;
            set;
        }
        public int? ZXFS
        {
            get;
            set;
        }
        public string TREE_ID
        {
            get;
            set;
        }
        public string TREE_CS
        {
            get;
            set;
        }
        public string TREE_TJ
        {
            get;
            set;
        }
        public string ZSQL
        {
            get;
            set;
        }
        public int? SCFW
        {
            get;
            set;
        }
        public string GLSQL
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
            return "XT_SJQX_DY_XX";
        }
        public override string GetTableName()
        {
            return "XT_SJQX_DY_XX";
        }
    }
}
