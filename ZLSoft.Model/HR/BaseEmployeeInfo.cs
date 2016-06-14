using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;
using ZLSoft.Model.PUB;

namespace ZLSoft.Model.HR
{
    /// <summary>
    ///人事_职工基本档案
    /// <summary>
    public class BaseEmployeeInfo:ReferModel
    {
        /// <summary>
        /// 用户ID
        /// <summary>
        public string UserID { get; set; }

        /// <summary>
        /// 身份证
        /// <summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 性别
        /// <summary>
        public string Sex { get; set; }

        /// <summary>
        /// 民族
        /// <summary>
        public string Nation { get; set; }

        /// <summary>
        /// 出生日期
        /// <summary>
        public string Birday { get; set; }

        /// <summary>
        /// 籍贯
        /// <summary>
        public string PlaceOfOrigin { get; set; }

        /// <summary>
        /// 参加工作时间
        /// <summary>
        public string TakeWorkDate { get; set; }

        /// <summary>
        /// 到院时间
        /// <summary>
        public string ToHospitalDate { get; set; }

        /// <summary>
        /// 职工性质
        /// <summary>
        public string EmployeeNature { get; set; }

        /// <summary>
        /// 岗位类别
        /// <summary>
        public string JobCategory { get; set; }

        /// <summary>
        /// 职工类别
        /// <summary>
        public string EmployeeCategory { get; set; }

        /// <summary>
        /// 职工类别2
        /// <summary>
        public string EmployeeCategory2 { get; set; }

        /// <summary>
        /// 人事科室ID
        /// <summary>
        public string PersonDepartID { get; set; }

        /// <summary>
        /// 业务单元ID
        /// <summary>
        public string BusinessUnitID { get; set; }

        /// <summary>
        /// 业务组织ID
        /// <summary>
        public string BusinessOrgID { get; set; }

        /// <summary>
        /// 临时组织ID
        /// <summary>
        public string TempOrgID { get; set; }

        /// <summary>
        /// 其他组织ID
        /// <summary>
        public string OtherOrgID { get; set; }

        /// <summary>
        /// 人员编制
        /// <summary>
        public string Staffing { get; set; }

        /// <summary>
        /// 备注
        /// <summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 家庭地址
        /// <summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 家庭邮编
        /// <summary>
        public string HomePostCode { get; set; }

        /// <summary>
        /// 家庭电话
        /// <summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// 手机号
        /// <summary>
        public string Phone { get; set; }

        /// <summary>
        /// 工作电话
        /// <summary>
        public string WorkPhone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// <summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// 英语等级
        /// <summary>
        public string EnglishLevel { get; set; }

        /// <summary>
        /// 计算机等级
        /// <summary>
        public string PCLevel { get; set; }

        /// <summary>
        /// 退休时间
        /// <summary>
        public string RetiredDate { get; set; }

        /// <summary>
        /// 返聘到期时间
        /// <summary>
        public string RestartEndDate { get; set; }

        /// <summary>
        /// 预定离院日期
        /// <summary>
        public string PreLeaveDate { get; set; }

        /// <summary>
        /// 是否注销
        /// <summary>
        public string IsCancellation { get; set; }

        /// <summary>
        /// 注销日期
        /// <summary>
        public string CancellationDate { get; set; }

        /// <summary>
        /// 注销原因
        /// <summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// 拼音码
        /// <summary>
        public string PYDate { get; set; }

        /// <summary>
        /// 政治面貌
        /// <summary>
        public string PoliticalStatus { get; set; }

        /// <summary>
        /// 入党团时间
        /// <summary>
        public string JoinGroupDate { get; set; }

        /// <summary>
        /// 党团内职务
        /// <summary>
        public string GroupJob { get; set; }

        /// <summary>
        /// 党内任职时间
        /// <summary>
        public string GroupJobDate { get; set; }

        /// <summary>
        /// 学历
        /// <summary>
        public string Education { get; set; }

        /// <summary>
        /// 毕业时间
        /// <summary>
        public string GraduationDate { get; set; }

        /// <summary>
        /// 毕业学校
        /// <summary>
        public string GraduationSchool { get; set; }

        /// <summary>
        /// 所学专业
        /// <summary>
        public string Professional { get; set; }

        /// <summary>
        /// 教育类别
        /// <summary>
        public string EducationCategory { get; set; }

        /// <summary>
        /// 学制
        /// <summary>
        public string LengthOfSchooling { get; set; }

        /// <summary>
        /// 学位
        /// <summary>
        public string Degree { get; set; }

        /// <summary>
        /// 学位时间
        /// <summary>
        public string DegreeDate { get; set; }

        /// <summary>
        /// 基础学历
        /// <summary>
        public string BaseEducation { get; set; }

        /// <summary>
        /// 户口性质
        /// <summary>
        public string AccountOfNature { get; set; }

        /// <summary>
        /// 户口所在地
        /// <summary>
        public string AccountLocation { get; set; }

        /// <summary>
        /// 婚姻状况
        /// <summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// 工龄
        /// <summary>
        public string LengthOfService { get; set; }

        /// <summary>
        /// 修改日期
        /// <summary>
        public string ModifyDate { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "人事_职工基本档案";
        }
    }
}
