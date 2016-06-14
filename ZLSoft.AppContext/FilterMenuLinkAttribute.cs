using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.AppContext
{
   [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class FilterMenuLinkAttribute : FilterAttribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			ControllerContext controllerContext = filterContext.Controller.ControllerContext;
			HttpRequestBase request = controllerContext.HttpContext.Request;
			if (!request.IsAjaxRequest())
			{
				string text = request.Req("menuid");
				if (!string.IsNullOrEmpty(text))
				{
					System.Collections.Generic.IList<StrObjectDict> list = DB.Select("select linkmc,menuid,linkid,pxxh,url from xt_menu_link where menuid='" + text + "' order by pxxh");
					string text2 = "";
					string text3 = "";
					for (int i = 0; i < list.Count; i++)
					{
						string text4 = text2;
						text2 = string.Concat(new string[]
						{
							text4,
							"<div iconcls=\"icon-menulist\" id=\"MenuLink_",
							Utils.GetString(list[i]["LINKID"]),
							"\">",
							Utils.GetString(list[i]["LINKMC"]),
							"</div>"
						});
						text3 = text3 + " case \"" + Utils.GetString(list[i]["LINKMC"]) + "\":\r\n";
						text4 = text3;
						text3 = string.Concat(new string[]
						{
							text4,
							"top.newtabs(new Date().getTime(), \"",
							Utils.GetString(list[i]["LINKMC"]),
							"\", \"",
							Utils.GetString(list[i]["URL"]),
							"\");\r\n"
						});
						text3 += "break;\r\n";
					}
					if (!string.IsNullOrEmpty(text2))
					{
						text2 = "<a href=\"javascript:void(0)\" id=\"btnMenuLink\" class=\"easyui-menubutton\" menu=\"#MenuLink\"  iconcls=\"icon-other\">相关链接</a><div id=\"MenuLink\" style=\"width: 130px; display: none;\">" + text2 + "</div>";
						text2 = text2 + "<script>$(function(){$('#btnMenuLink').menubutton({menu: '#MenuLink',plain: false });}) \r\n $('#MenuLink').menu({ \r\n onClick: function(item) { \r\n switch(item.text) {\r\n" + text3 + "\r\n }}});</script>";
					}
                    filterContext.Controller.ViewBag.MenuLink = text2;
				}
			}
		}
		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}
	}
}
