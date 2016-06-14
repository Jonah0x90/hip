using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_TREE : BuzzModel
    {
        public string TREEDM
        {
            get;
            set;
        }
        public string TREEMC
        {
            get;
            set;
        }
        public int? ZFPB
        {
            get;
            set;
        }
        public int? XTPB
        {
            get;
            set;
        }
        public string BZ
        {
            get;
            set;
        }
        public int? ROOT_SHOW_FLAG
        {
            get;
            set;
        }
        public string ROOT_NAME
        {
            get;
            set;
        }
        public string SHOW_FORMAT
        {
            get;
            set;
        }
        public string COL_ID
        {
            get;
            set;
        }
        public string COL_NAME
        {
            get;
            set;
        }
        public string COL_WIDTH
        {
            get;
            set;
        }
        public string SRM1
        {
            get;
            set;
        }
        public string SRM2
        {
            get;
            set;
        }
        public string SRM3
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
        public int? PXXH
        {
            get;
            set;
        }
        public string FLID
        {
            get;
            set;
        }
        public string CONN_ID
        {
            get;
            set;
        }
        public string SQL_ORACLE
        {
            get;
            set;
        }
        public string SQL_MSSQL
        {
            get;
            set;
        }
        public string PXZD
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_TREE";
        }
        public override string GetTableName()
        {
            return "XT_TREE";
        }
    }
}
