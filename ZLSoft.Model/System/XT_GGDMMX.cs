using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_GGDMMX : BuzzModel
    {
        public string DMID
        {
            get;
            set;
        }
        public string MXID
        {
            get;
            set;
        }
        public string MXMC
        {
            get;
            set;
        }
        public string SJID
        {
            get;
            set;
        }
        public int? JB
        {
            get;
            set;
        }
        public int? XTPB
        {
            get;
            set;
        }
        public int? SFMJ
        {
            get;
            set;
        }
        public long? PXXH
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
        public string CHAR1
        {
            get;
            set;
        }
        public string CHAR2
        {
            get;
            set;
        }
        public string CHAR3
        {
            get;
            set;
        }
        public string CHAR4
        {
            get;
            set;
        }
        public string CHAR5
        {
            get;
            set;
        }
        public string CHAR6
        {
            get;
            set;
        }
        public decimal? NUM1
        {
            get;
            set;
        }
        public decimal? NUM2
        {
            get;
            set;
        }
        public decimal? NUM3
        {
            get;
            set;
        }
        public decimal? NUM4
        {
            get;
            set;
        }
        public decimal? NUM5
        {
            get;
            set;
        }
        public decimal? NUM6
        {
            get;
            set;
        }
        public DateTime? DATE1
        {
            get;
            set;
        }
        public DateTime? DATE2
        {
            get;
            set;
        }
        public DateTime? DATE3
        {
            get;
            set;
        }
        public int? SFZF
        {
            get;
            set;
        }
        public DateTime? ZFRQ
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
        public string BZ
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_GGDMMX";
        }
        public override string GetTableName()
        {
            return "XT_GGDMMX";
        }
    }
}
