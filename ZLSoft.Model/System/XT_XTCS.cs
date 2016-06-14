using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_XTCS : BuzzModel
    {
        public string CSID
        {
            get;
            set;
        }
        public string CSMC
        {
            get;
            set;
        }
        public string CSSM
        {
            get;
            set;
        }
        public string CSZSM
        {
            get;
            set;
        }
        public string MRZ
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
        public string XGYH
        {
            get;
            set;
        }
        public DateTime? XGRQ
        {
            get;
            set;
        }
        public string FLID
        {
            get;
            set;
        }
        public int? SLBZ
        {
            get;
            set;
        }
        public string SLSQL
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_XTCS";
        }
        public override string GetTableName()
        {
            return "XT_XTCS";
        }
    }
}
