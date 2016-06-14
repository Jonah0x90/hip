using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.HR;
using ZLSoft.Model.PUB;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Public.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class PubUserController:MVCController
    {
        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            StrObjectDict dict = GetParams();
            string keyword = dict.GetString("KeyWord");
            #region 入参正确性校验


            //if (string.IsNullOrEmpty(keyword))
            //{
            //    return this.MyJson(0, "参数为空:DeptID");
            //}

            #endregion

            IList<StrObjectDict> list = UserManager.Instance.Search(keyword);

            return this.MyJson(list);
        }




        /// <summary>
        /// 获取用户列表(分页)
        /// </summary>
        public override ActionResult Page()
        {
            StrObjectDict reqParam = GetParams();
            StrObjectDict reqPage = GetPageInfo(); ;



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
                    PageData<StrObjectDict> pd = UserManager.Instance.ListSod(reqParam, page);
                    return this.MyJson(1, pd);
                }

            }

            return this.MyJson(0, "参数不正确");
        }


        /// <summary>
        /// 获取人员详细信息
        /// </summary>
        /// <returns></returns>
        public override ActionResult Load()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            #region 入参正确性校验


            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数为空:ID");
            }

            #endregion

            PubUser pu = UserManager.Instance.LoadByID<PubUser>(id);
            return this.MyJson(1, pu);
        }

        /// <summary>
        /// 保存人员信息
        /// </summary>
        /// <returns></returns>
        public override ActionResult InsertOrUpdate()
        {

            StrObjectDict reqParam = GetParams();
            var pwd = reqParam.GetString("Password");
            reqParam.Remove("Password");
            reqParam["Password"] = MD5Encode.Encode(pwd);

            int result = UserManager.Instance.InsertOrUpdate<PubUser>(reqParam);
            if (result > 0)
            {
                return this.MyJson(1,result);
            }
            return this.MyJson(0, "保存失败");
        }

        /// <summary>
        /// 根据删除人员信息
        /// </summary>
        /// <returns></returns>
        public override ActionResult Delete()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            #region 入参正确性校验


            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数为空:ID");
            }
            #endregion

            int isOk = UserManager.Instance.Delete<PubUser>(id);

            if (isOk > 0)
            {
                return this.MyJson(1, "保存成功");
            }
            else
            {
                return this.MyJson(0, "操作失败");
            }
        }
    }
}
