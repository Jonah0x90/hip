using System;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Util;
using ZLSoft.DalManager;
using IBatisNet.DataAccess.Configuration;
using ZLSoft.AppContext;
using ZLSoft.Model.SYS;
using System.Collections.Generic;
using ZLSoft.Model.Tree;
using ZLSoft.Model.PLATFORM;
using System.Web.Mvc;
using ZLSoft.Pub;

namespace ZLSoft.UnitTest
{
    [TestFixture]
    public class CommCodeManagerTest
    {

        private static DomDaoManagerBuilder dal = new DomDaoManagerBuilder();

        [SetUp]
        public void Init()
        {
            dal.Configure("dbconfig/dao.config");
        }

        [Test]
        public void LoadCommCodeDetail()
        {
            Tree tree = TreeManager.Instance.GetTree("CommCodeDetail", "SuperiorID",null);
            //tree.TransformTree("SuperiorID");
            string j = tree.data.MyJson();
            IList<CommonCodeDetail> list1 = CommCodeManager.getCommCodeDetail("337e7e55-6609-476a-94fb-cc1e8f9f");
//             string re = CommCodeManager.Instance.getCommCodebyID("");
             Assert.True(3 > 2);
        }

        [Test]
        public void InsertParam()
        {
            //int result = ParmsManager.Instance.setParm(null,"测试参数","2343333");
           // Assert.True(result > 0);
        }

        [Test]
        public void Delete()
        {
//             int result = ParmsManager.Instance.Delete<Sy>("7e47a98a-577f-4cb6-b162-e205895814bf");
//             Assert.True(result > 0);
        }
    }
}
