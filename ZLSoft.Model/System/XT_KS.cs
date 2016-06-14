using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_KS : BuzzModel
    {
        public string ID
        {
            get;
            set;
        }
        public string DM
        {
            get;
            set;
        }
        public string MC
        {
            get;
            set;
        }
        public string JC
        {
            get;
            set;
        }
        public string SM
        {
            get;
            set;
        }
        public string SJID
        {
            get;
            set;
        }
        public string YQID
        {
            get;
            set;
        }
        public string KSXZ
        {
            get;
            set;
        }
        public int? KSJB
        {
            get;
            set;
        }
        public int? SFMJ
        {
            get;
            set;
        }
        public string SFQYFL
        {
            get;
            set;
        }
        public string SFFLMJ
        {
            get;
            set;
        }
        public string SFFLZF
        {
            get;
            set;
        }
        //public int? LYPB
        //{
        //    get;
        //    set;
        //}
        //public int? EJKPB
        //{
        //    get;
        //    set;
        //}
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
        public string PXXH
        {
            get;
            set;
        }
        public string DZ
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_KS";
        }
        public override string GetTableName()
        {
            return "XT_KS";
        }
    }
}
