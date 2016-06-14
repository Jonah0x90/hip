using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Public.Controllers
{
    //角色相关功能
    public class RoleMemberController : MVCController
    {

        public override ActionResult List()
        {
            //获取参数
            StrObjectDict reqParam = GetParams();

            var result = crudManager.ListSod<RoleMember>(reqParam);
            //分页
            return this.MyJson(1, result);
        }

        public ActionResult NotSelectedrList()
        {
            StrObjectDict reqParam = GetParams();

            var result = crudManager.ListSod("LISTOTHERSOD_RoleMember", reqParam);

            return this.MyJson(1, result);
        }

        public override ActionResult Page()
        {
            //获取参数
            StrObjectDict reqParam = GetParams();
            StrObjectDict reqPage = GetPageInfo();


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
                    PageData<StrObjectDict> result = RoleManager.Instance.ListSod<RoleMember>(reqParam, page);
                    return this.MyJson(1, result.DataList, result.PageInfo);
                }
            }

            return this.MyJson(0, "参数错误!");
        }


        public  ActionResult Insert()
        {
            StrObjectDict reqParam = GetParams();
            string roleID = reqParam.GetString("RoleID");
            IList<RoleMember> list = new List<RoleMember>();

            List<Object> userIDs = ((Object[])reqParam.GetObject("UserIDs")).ToList();

            foreach (var item in userIDs)
            {
                RoleMember rm = new RoleMember() 
                { 
                    RoleID = roleID,
                    UserID = item.ToString()
                };
                list.Add(rm);

            }

            //批量插入
            var result = RoleManager.Instance.InsertBatchs(list);
            return this.MyJson(1, result);
        }


        public override ActionResult Delete()
        {
            StrObjectDict reqParam = GetParams();
            string roleID = reqParam.GetString("RoleID");
            IList<RoleMember> list = new List<RoleMember>();


            List<Object> userIDs = ((Object[])reqParam.GetObject("UserIDs")).ToList();

            foreach (var item in userIDs)
            {
                RoleMember rm = new RoleMember()
                {
                    RoleID = roleID,
                    UserID = item.ToString()
                };
                list.Add(rm);
            }
            var result = RoleManager.Instance.DeleteBatchs(list);
            return this.MyJson(1, result);
        }
    }
}
