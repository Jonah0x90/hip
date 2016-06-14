using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_护理项目
    /// <summary>
    public class WorkItem : ReferModel,IImportObject
    {

      
        /// <summary>
        /// 编码
        /// <summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位
        /// <summary>
        public string Unit { get; set; }

        /// <summary>
        /// 说明
        /// <summary>
        public string Remark { get; set; }


        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "PUB_护理项目";
        }

        public string MAP_ImportInsert
        {
            get { return "IMPORT_INSERT_" + GetModelName(); }
        }

        public string MAP_ImportUpdate
        {
            get { return "IMPORT_UPDATE_" + GetModelName(); }
        }
    }
}
