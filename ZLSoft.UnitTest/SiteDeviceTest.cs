using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.UnitTest
{
    [TestFixture]
    public class SiteDeviceTest
    {

        private static DomDaoManagerBuilder dal = new DomDaoManagerBuilder();

        [SetUp]
        public void Init()
        {
            dal.Configure("dbconfig/dao.config");
        }

        [Test]
        public void InsertDevice()
        {
//             int result = SiteDeviceManager.Instance.Add(new SiteDevice
//             {
//                 DeviceID = pointID.ToString(),
//                 DeviceInfo = pointInfo.ToString(),
//                 Enable = 1,
//                 IsMobile = 1,
//                 RegTime = DateTime.Now,
//                 Remark = "",
//                 DeptID = orgID.ToString()
//             }, moduleCode.ToString());
// 
//             Assert.True(result > 0);
        }
    }
}
