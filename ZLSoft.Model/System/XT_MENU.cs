using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_MENU:BuzzModel
    {
        public string ID
        {
            get;
            set;
        }
        public string BM
        {
            get;
            set;
        }
        public string MC
        {
            get;
            set;
        }
        public string YYID
        {
            get;
            set;
        }
        public string MS
        {
            get;
            set;
        }
        public string SJID
        {
            get;
            set;
        }
        public int? JC
        {
            get;
            set;
        }
        public int? SFMJ
        {
            get;
            set;
        }
        public int? GLGN
        {
            get;
            set;
        }
        public string GNID
        {
            get;
            set;
        }
        public string PIC
        {
            get;
            set;
        }
        public string CSZ
        {
            get;
            set;
        }
        public int? DKFS
        {
            get;
            set;
        }
        public int? QXKZPB
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
        public long? PXXH
        {
            get;
            set;
        }
        public string PICBIG
        {
            get;
            set;
        }
        public int? SFM
        {
            get;
            set;
        }
        public string MLBID
        {
            get;
            set;
        }
        public string MMC
        {
            get;
            set;
        }
        public string MCSZ
        {
            get;
            set;
        }
        public long? MPXXH
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_MENU";
        }
        public override string GetTableName()
        {
            return "XT_MENU";
        }
    }
}
