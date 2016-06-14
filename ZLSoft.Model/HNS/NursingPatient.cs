using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///PUB_病人基本信息
    /// <summary>
    public class NursingPatient : BuzzModel
    {

        /// <summary>
        /// 相关ID
        /// <summary>
        public string RelatID { get; set; }

        /// <summary>
        /// 住院号
        /// <summary>
        public string AdmissionNumber { get; set; }

        /// <summary>
        /// 姓名
        /// <summary>
        public string FullName { get; set; }

        /// <summary>
        /// 性别
        /// <summary>
        public string Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// <summary>
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// 入院日期
        /// <summary>
        public DateTime InDay { get; set; }

        /// <summary>
        /// 出院日期
        /// <summary>
        public DateTime OutDay { get; set; }

        /// <summary>
        /// 当前病区相关ID
        /// <summary>
        public string LesionID { get; set; }

        /// <summary>
        /// 当前科室相关ID
        /// <summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 责任护士
        /// <summary>
        public string Nurse { get; set; }

        /// <summary>
        /// 床号
        /// <summary>
        public string BedNumber { get; set; }

        /// <summary>
        /// 状态
        /// <summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否对照
        /// <summary>
        public int IsBaseThird { get; set; }

        /// <summary>
        /// 是否危重
        /// <summary>
        public int IsCritical { get; set; }

        /// <summary>
        /// 护理等级
        /// <summary>
        public string NursingLevel { get; set; }

        /// <summary>
        /// 年龄
        /// <summary>
        public string Age { get; set; }

        /// <summary>
        /// 相关住院次数
        /// </summary>
        public int InTimes { get; set; }

        public string Doctor { get; set; }

        public int BabyNum { get; set; }

        public override string GetModelName()
        {
            return "NursingPatient";
        }

        public override string GetTableName()
        {
            return "PUB_病人基本信息";
        }
    }
}
