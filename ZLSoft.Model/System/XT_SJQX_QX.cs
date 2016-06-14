using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_SJQX_QX : BuzzModel
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
        public string DXMXID
        {
            get;
            set;
        }
        public string DYID
        {
            get;
            set;
        }
        public string KCID
        {
            get;
            set;
        }
        public string XXID
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
            return "XT_SJQX_QX";
        }
        public override string GetTableName()
        {
            return "XT_SJQX_QX";
        }
    }
}
