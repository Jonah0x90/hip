using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_YHXX:BuzzModel
    {
        public string ID
        {
            get;
            set;
        }
        public string ZH
        {
            get;
            set;
        }
        public string XM
        {
            get;
            set;
        }
        public string XB
        {
            get;
            set;
        }
        public string MM
        {
            get;
            set;
        }
        public string KSID
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
        public int? SFZG
        {
            get;
            set;
        }
        public string ZGID
        {
            get;
            set;
        }
        public string MRSRM
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
        public override string GetModelName()
        {
            return "XT_YHXX";
        }
        public override string GetTableName()
        {
            return "XT_YHXX";
        }
    }
}
