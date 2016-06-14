using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Model.Tree;
using ZLSoft.Pub;

namespace ZLSoft.Platform.Controllers
{
    public class BPMController:MVCController
    {
        public override ActionResult Index()
        {
            IList<StrObjectDict> list = BPMManager.Instance.ListSod<BPM>(null);
            Tree tree = new Tree();
            tree.datasList = list;
            tree.TransformTree("SuperID");
            return this.MyJson(1, tree.data);
        }
    }
}
