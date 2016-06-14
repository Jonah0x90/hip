using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Model.Tree;

namespace ZLSoft.Platform.Controllers
{
    public class CommonCodeDetailController : CRUDController
    {
        

        public override ActionResult List()
        {
            StrObjectDict reqData = base.GetHttpData();
            StrObjectDict reqParam = reqData.GetObject("Params").toStrObjDict(false);


            IList<StrObjectDict> result = base.crudManager.ListSod<CommonCodeDetail>(reqParam);
            Tree tree = new Tree();
            tree.datasList = result;
            tree.TransformTree("SuperID");
            return this.MyJson(1, tree.data);
        }

        public ActionResult GetFromRelation()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("CodeID");

            #region 参数有效性检查

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数错误:CodeID");
            }

            #endregion


            IList<StrObjectDict> dicts = CommCodeManager.Instance.getFromRelation(new
            {
                CodeID = id
            }.toStrObjDict(false));
            return this.MyJson(1, dicts);
        }

        public ActionResult Del()
        {
            StrObjectDict dict = GetParams();
            string id = dict.GetString("ID");

            if(string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数不能为空：ID");
            }

            int result = CommCodeManager.Instance.Dels(dict);
            if (result > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "参数失败");
            }
        }
    }
}
