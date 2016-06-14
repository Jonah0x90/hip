using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_GGDM : BuzzModel
    {
        public string ID
        {
            get;
            set;
        }
        public string MC
        {
            get;
            set;
        }
        public string LBID
        {
            get;
            set;
        }
        public string XTID
        {
            get;
            set;
        }
        public int? SFZF
        {
            get;
            set;
        }
        public int? XTPB
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
            return "XT_GGDM";
        }
        public override string GetTableName()
        {
            return "XT_GGDM";
        }
    }
}
