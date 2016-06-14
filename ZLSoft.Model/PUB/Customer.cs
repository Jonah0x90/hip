using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{

    /// <summary>
    /// 公共_客户(病人信息)
    /// </summary>
    public class Customer : ReferModel
    {
        /// <summary>
        /// 主页ID
        /// </summary>
        public int IndexID { get; set; }


        /// <summary>
        /// 客户代码(住院号)
        /// </summary>
        public string CustomerNo{ get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDay { get; set; }


        /// <summary>
        /// 注册日期(入院日期)
        /// </summary>
        public DateTime? RegDate { get; set; }


        /// <summary>
        /// 注销时间(出院时间)
        /// </summary>
        public DateTime? OutDate { get; set; }

        /// <summary>
        /// 所属部门ID(病区相关ID)
        /// </summary>
        public int WardID { get; set; }

        /// <summary>
        /// 所属机构ID(科室相关ID)
        /// </summary>
        public int DeptID { get; set; }


        /// <summary>
        /// 客户经理(责任护士)
        /// </summary>
        public string ClientManager { get; set; }

        /// <summary>
        /// 用户卡号(床号)
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 是否VIP(是否危重)
        /// </summary>
        public int IsVip { get; set; }


        /// <summary>
        /// 会员等级(护理等级)
        /// </summary>
        public int Grade { get; set; }

        public override string GetModelName()
        {
            return this.GetType().Name;
        }

        public override string GetTableName()
        {
            return "PUB_病人基本信息";
        }
    }
}
