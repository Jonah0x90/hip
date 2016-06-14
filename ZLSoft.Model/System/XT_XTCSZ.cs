using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_XTCSZ : BuzzModel
    {
        public string CSID
        {
            get;
            set;
        }
        public string SLID
        {
            get;
            set;
        }
        public string SLMC
        {
            get;
            set;
        }
        public string CSZ
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
            return "XT_CSZ";
        }
        public override string GetTableName()
        {
            return "XT_CSZ";
        }
    }
}
