using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_YQ : BuzzModel
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
        public string SM
        {
            get;
            set;
        }
        public string DZ
        {
            get;
            set;
        }
        public string YB
        {
            get;
            set;
        }
        public string DH
        {
            get;
            set;
        }
        public string CZ
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
        public int? PXXH
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_YQ";
        }
        public override string GetTableName()
        {
            return "XT_YQ";
        }
    }
}
