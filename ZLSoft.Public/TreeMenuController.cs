using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Model.Tree;

namespace ZLSoft.Public.Controllers
{
    public class TreeMenuController:MVCController
    {
//         [ValidateInput(false)]
//         public ActionResult Load()
//         {
//             base.ValidateRequest = false;
//             string treedm = base.Request.Req("treedm");
//             string sjid = base.Request.Req("sjid");
//             string checkbox = base.Request.Req("checkbox");
//             string async = base.Request.Req("isasync");
//             string text = base.Request.Req("expanddeep");
//             string text2 = base.Request.Req("root_selected");
//             if (string.IsNullOrEmpty(text2))
//             {
//                 text2 = "0";
//             }
//             int expanddeep = 2;
//             if (text != "")
//             {
//                 expanddeep = System.Convert.ToInt32(text);
//             }
//             string text3 = base.Request.Req("PARAMS").Replace("&#39;", "'");
//             StrObjectDict dict = null;
//             text3 = ((text3 == "") ? "{}" : text3);
//             if (text3 != "")
//             {
//                 object o = JsonAdapter.FromJsonAsDictionary(text3);
//                 dict = StrObjectDict.FromVariable(o);
//             }
//             string text4 = base.Request.Req("datafilter").Replace("&#39;", "'");
//             text4 = ((text4 == "") ? "{}" : text4);
//             StrObjectDict dict_filter = null;
//             if (text4 != "")
//             {
//                 object o = JsonAdapter.FromJsonAsDictionary(text4);
//                 dict_filter = StrObjectDict.FromVariable(o);
//             }
//             Tree tree = TreeManager.Instance.GetTree(treedm, dict, dict_filter, checkbox, text2);
//             return base.Content(string.Concat(new string[]
// 			{
// 				"{\"treedata\":",
// 				tree.ToTreeJson(tree.root_show_flag, sjid, async, expanddeep),
// 				",\"flatdata\":",
// 				tree.ToGridJson(),
// 				"}"
// 			}));
//         }
    }
}
