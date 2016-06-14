using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using System.Web.Script.Serialization;
using ZLSoft.DalManager;
using System.Security.Policy;
using System.Reflection;
using System.Web.Routing;

namespace ZLSoft.AppContext
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FilterBaseAttribute : FilterAttribute, IActionFilter
    {
        public FilterBaseAttribute()
        {
            base.Order = 10;
        }
        //public void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    ControllerContext controllerContext = filterContext.Controller.ControllerContext;
        //    HttpRequestBase request = controllerContext.HttpContext.Request;
        //    if (!request.IsAjaxRequest())
        //    {
        //        string text = request.Req("menuid");
        //        filterContext.Controller.ViewData["exclude_btn"] = "[]";
        //        if (!string.IsNullOrEmpty(text))
        //        {
        //            IList<StrObjectDict> list = DB.ListSod("list_menu_op_by_yhid", StrObjectDict.FromVariable(new
        //            {
        //                YHID = LoginSession.Current.YHID,
        //                MENUID = text
        //            }));
        //            string text2 = "";
        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                if (i > 0)
        //                {
        //                    text2 += ",";
        //                }
        //                text2 = text2 + "'" + Utils.GetString(list[i]["OPDM"]) + "'";
        //            }
        //            filterContext.Controller.ViewData["exclude_btn"] = "[" + text2 + "]";

        //        }
        //    }
        //}


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            JsonResult json = filterContext.Result as JsonResult;

            if(json == null){
                return;
            }

            PropertyInfo info = json.Data.GetType().GetProperty("Flag");
            if(info == null){
                return;
            }
            object result = info.GetValue(json.Data,null);
            if (result == null || result.ToString() == "0")
            {
                return;
            }

            //检查菜单的操作权限,并返回允许的操作
            ControllerContext controllerContext = filterContext.Controller.ControllerContext;
            HttpRequestBase request = controllerContext.HttpContext.Request;

            //string menuID = request.Req("MenuID");

            //根据url查找功能,如果没有找到，则认为是操作，不进行处理，否则查找功能相应操作是否有相应权限
            string uri = request.Url.PathAndQuery;


            if (!string.IsNullOrEmpty(uri))
            {
                IList<StrObjectDict> menuOpt = MenuManager.Instance.GetMenuOpt(new
                {
                    UserID = "123",//LoginSession.Current.YHID,
                    Url = uri
                }.toStrObjDict());

                if (menuOpt != null && menuOpt.Count() > 0)
                {
                    

                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    javaScriptSerializer.MaxJsonLength *= 100;

                    string s = javaScriptSerializer.Serialize(json.Data);

                    object o = JsonAdapter.FromJsonAsDictionary(s);
                    StrObjectDict dict_filter = StrObjectDict.FromVariable(o);
                    if(dict_filter.ContainsKey("Output")){
                        o = dict_filter["Output"];

                        if(o == null || o.ToString() == ""){
                            o = new
                            {
                                Option = ""
                            };
                        }

                        StrObjectDict output = o.toStrObjDict();
                        
                        output["Option"]=menuOpt;
                        dict_filter["Output"] = output;
                    }
                    filterContext.Result = filterContext.Controller.MyJson(int.Parse(dict_filter["Flag"].ToString()),""+dict_filter["Msg"],dict_filter["Output"]);
                }
            }

        }

        /// <summary>
        /// 检查用户是否有访问权限
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ControllerContext controllerContext = filterContext.Controller.ControllerContext;

            object o = controllerContext.RouteData.Values["action"];
            MethodInfo[] methodinfos = controllerContext.Controller.GetType().GetMethods();
            object[] os = null;
            foreach (var item in methodinfos)
            {
                os = item.GetCustomAttributes(typeof(AuthIgnoreAttribute), true);
                if (os.Length > 0)
                {
                    if(item.Name.ToLower() == o.ToString().ToLower()){
                        return;
                    }
                }
            }


            //HttpRequestBase request = controllerContext.HttpContext.Request;
            
            //string uri = request.Url.PathAndQuery;
            //int start = uri.IndexOf("?MenuID");
            //uri = uri.Substring(0, start);
            //bool isOk = MenuManager.Instance.IsRequestAuth(new
            //{
            //    UserID = "123", //LoginSession.Current.YHID,
            //    ModuleCode = "003",
            //    Url = uri
            //}.toStrObjDict());


            //if (!isOk)
            //{
            //    string str = "你没有权限进行该操作!";
            //    filterContext.Result = filterContext.Controller.MyJson(0, str);
            //    filterContext.HttpContext.Response.Clear();
            //    filterContext.HttpContext.Response.StatusCode = 503;
            //}

            //if(!string.IsNullOrEmpty(uri)){
            //    uri.IndexOf();
            //}
        }
    }
}
