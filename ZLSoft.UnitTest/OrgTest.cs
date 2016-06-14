using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.DalManager;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;

namespace ZLSoft.UnitTest
{
     [TestFixture]
    public class OrgTest
    {
        private static DomDaoManagerBuilder dal = new DomDaoManagerBuilder();

        [SetUp]
        public void Init()
        {
            dal.Configure("dbconfig/dao.config");
        }

         [Test]
        public void ListOrg()
        {
            IList<StrObjectDict> list = OrganizationManager.Instance.GetOrgList(new
            {
                DepartType = 2
            }.toStrObjDict());
           Assert.True(list.Count>0);
        }
    }
}
