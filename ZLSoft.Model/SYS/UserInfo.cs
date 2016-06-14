using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
   public class UserInfo:BuzzModel
    {
       /// <summary>
       /// 工号
       /// </summary>
       public string JobNumber { get; set; }

       /// <summary>
       /// 姓名
       /// </summary>
       public string Name { get; set; }

       /// <summary>
       /// 性别
       /// </summary>
       public string Gender { get; set; }

       /// <summary>
       /// 密码
       /// </summary>
       public string Password { get; set; }


       /// <summary>
       /// 是否作废
       /// </summary>
       public int IsNullify { get; set; }

       /// <summary>
       /// 输入码
       /// </summary>
       public string InputCodes { get; set; }


       /// <summary>
       /// 修改用户
       /// </summary>
       public string Modifier { get; set; }

       /// <summary>
       /// 修改日期
       /// </summary>
       public DateTime? ModifiDate { get; set; }

       /// <summary>
       /// 作废日期
       /// </summary>
       public DateTime? NullifyDate { get; set; }

       /// <summary>
       /// 是否对照
       /// </summary>
       public int IsContrast { get; set; }

       /// <summary>
       /// 相关ID
       /// </summary>
       public string RelatedID { get; set; }

       public override string GetModelName()
       {
           return "UserInfo";
       }
       public override string GetTableName()
       {
           return "系统_用户信息";
       }
    }
}
