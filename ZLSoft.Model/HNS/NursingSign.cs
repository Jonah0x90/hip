using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///HNS_病人护理签名
    /// <summary>
    public class NursingSign : BuzzModel
    {
        /// <summary>
        /// 性质 1为计划签名，2为评估签名
        /// <summary>
        public int Nature { get; set; }

        /// <summary>
        /// 源ID
        /// <summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 证书ID
        /// <summary>
        public string CertificateID { get; set; }

        /// <summary>
        /// 时间戳
        /// <summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 签名人
        /// <summary>
        public string Signer { get; set; }

        /// <summary>
        /// 签名时间
        /// <summary>
        public DateTime SignedTime { get; set; }

        /// <summary>
        /// 签名内容
        /// <summary>
        public string SignContent { get; set; }

        public override string GetModelName()
        {
            return "NursingSign";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理签名";
        }
    }
}
