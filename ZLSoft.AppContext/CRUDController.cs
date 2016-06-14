using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.PageData;

namespace ZLSoft.AppContext
{
    /// <summary>
    /// 对单边进行增删查改,子类的命名必须跟使用的Model名相同
    /// </summary>
    public class CRUDController:BaseController
    {

        protected CRUDManager crudManager = new CRUDManager();

        private string NULL_MODEL_ERROR = "系统错误,找不到实体类!";


        /// <summary>
        /// 获取model的类型
        /// </summary>
        /// <returns>model的类型</returns>
        private Type getType()
        {
            string modelNameSpace = this.GetType().Namespace;
            string middleName = "";
            Match match_en = Regex.Match(modelNameSpace, @"\.([\w]*)\.", RegexOptions.IgnoreCase);
            if (match_en.Success)
            {
                middleName = match_en.Groups[1].Value;
            }
            //根据controller的命名空间获取model的命名空间
            switch (middleName.ToUpper())
            {
                case "PLATFORM":
                    middleName = "PLATFORM";
                    break;
                case "SYS":
                    middleName = "SYS";
                    break;
                case "PUBLIC":
                    middleName = "PUB";
                    break;
                case "HrMng":
                    middleName = "HR";
                    break;
                default:
                    break;
            }
            string strClass = "ZLSoft.Model." + middleName.ToUpper() + "." + this.GetType().Name.Replace("Controller", "");
            return Type.GetType(strClass + ",ZlSoft.Model");
        }
        /// <summary>
        /// 获取键值对,如下拉框的数据
        /// </summary>
        /// <returns></returns>
        [AuthIgnore]
        public virtual ActionResult NameValue()
        {
            //获取参数
            StrObjectDict reqParam = GetParams();


            //获取实体类
            Type t = this.getType();
            if (t == null)
            {
                return this.MyJson(0, this.NULL_MODEL_ERROR);
            }

            var result = crudManager.GetType().GetMethod("NameValue", new Type[] { typeof(StrObjectDict) }).MakeGenericMethod(t).Invoke(crudManager, new object[] { reqParam });
            return this.MyJson(1, result);
        }

        /// <summary>
        /// 读取表中的所有信息
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public virtual ActionResult List()
        {
            //获取参数
            StrObjectDict reqParam = GetParams(); 


            //获取实体类
            Type t = this.getType();
            if (t == null)
            {
                return this.MyJson(0, this.NULL_MODEL_ERROR); 
            }

            //分页
            
            var result = crudManager.GetType().GetMethod("List", new Type[] { typeof(IDictionary<string, object>) }).MakeGenericMethod(t).Invoke(crudManager, new object[] { reqParam });
            return this.MyJson(1, result);
        }
        /// <summary>
        /// 根据ID获取单条数据
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Load()
        {
            //获取参数
            string ID = GetParams().GetString("ID");


            //获取实体类
            Type t = this.getType();
            if (t == null)
            {
                return this.MyJson(0, this.NULL_MODEL_ERROR);
            }


            //分页

            var result = crudManager.GetType().GetMethod("LoadByID").MakeGenericMethod(new Type[]{t}).Invoke(crudManager, new object[] { ID });
            return this.MyJson(1, result);
        }

        /// <summary>
        /// 列表信息(分页)
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Page()
        {
            //获取参数
            StrObjectDict reqParam = GetParams();
            StrObjectDict reqPage = GetPageInfo();

            //获取实体类
            Type t = this.getType();
            if (t == null)
            {
                return this.MyJson(0, this.NULL_MODEL_ERROR);
            }

            //分页
            Page page = new Page();
            int pageNum = 0;
            int pageSize = 0;
            if (int.TryParse(reqPage.GetString("PageNum"), out pageNum) && int.TryParse(reqPage.GetString("PageSize"), out pageSize))
            {
                page.PageNumber = pageNum;
                page.PageSize = pageSize;
                if (page.PageSize > 0 && page.PageNumber > 0)
                {
                    object[] args = new object[] { reqParam, page };
                    var result = crudManager.GetType().GetMethod("List", new Type[] { typeof(IDictionary<string, object>), typeof(Page) }).MakeGenericMethod(t).Invoke(crudManager, args);
                    return this.MyJson(1, result.GetType().GetProperty("DataList").GetValue(result, null), result.GetType().GetProperty("PageInfo").GetValue(result, null));
                }
            }
           
            return this.MyJson(0,"参数错误!");
        }
        /// <summary>
        /// 插入或者更新
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public virtual ActionResult InsertOrUpdate()
        {
            //获取参数
            StrObjectDict reqParam = GetParams(true);
            //获取实体类
            Type t = this.getType();
            if (t == null)
            {
                return this.MyJson(0, this.NULL_MODEL_ERROR);
            }
            int result = (int)crudManager.GetType().GetMethod("InsertOrUpdate", new Type[] { typeof(StrObjectDict) }).MakeGenericMethod(t).Invoke(crudManager, new object[] { reqParam });

            if (result > 0)
            {
                return this.MyJson(1);
            }
            else
            {
                return this.MyJson(0,"sorry!try again!");
            }
            
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public virtual ActionResult Delete()
        {
            //获取参数
            StrObjectDict reqParam = GetParams(true);
            //获取实体类
            Type t = this.getType();
            if (t == null)
            {
                return this.MyJson(0, this.NULL_MODEL_ERROR);
            }
            int result = (int)crudManager.GetType().GetMethod("Delete", new Type[] { typeof(System.String) }).MakeGenericMethod(t).Invoke(crudManager, new object[] { reqParam.GetString("ID") });

            if (result > 0)
            {
                return this.MyJson(1);
            }
            else
            {
                return this.MyJson(0, "对不起,删除的记录不存在!");
            }

        }
        /// <summary>
        /// 默认处理
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            return this.List();
        }

    }
}
