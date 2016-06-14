using System;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Util;
using ZLSoft.DalManager;
using IBatisNet.DataAccess.Configuration;
using ZLSoft.AppContext;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;

namespace ZLSoft.UnitTest
{
    [TestFixture]
    public class SysParametersTest
    {

        private static DomDaoManagerBuilder dal = new DomDaoManagerBuilder();

        [SetUp]
        public void Init()
        {
            dal.Configure("dbconfig/dao.config");
        }

        [Test]
        public void LoadParam()
        {
            string re = ParmsManager.Instance.getParmValueByID("bad06e22-935b-4040-9599-b4fcbede5716");
            Assert.AreEqual("2343333", re);
        }

        [Test]
        public void LoadParamByID()
        {
           // string id = "bad06e22-935b-4040-9599-b4fcbede5716";
           // string aa = DB.GetSql("LIST_SysParameters", new { 
           //     ID = id
           // }.toStrObjDict());
           //// SysParameters re = ParmsManager.Instance.LoadByID<SysParameters, string>(id);
           // Assert.AreEqual("2343333", re.ID);
        }

        [Test]
        public void InsertParam()
        {
            //int result = ParmsManager.Instance.setParm(null,"测试参数","2343333");
            int result = ParmsManager.Instance.InsertOrUpdate<SysParameters>(new SysParameters
            {
                ID = null,
                Name = "doubi",
                DefaultValue = "11111111"
            });

            Assert.True(result > 0);
        }

        [Test]
        public void Delete()
        {
            int result = ParmsManager.Instance.Delete<SysParameters>("a6bab6a2-d116-437f-84c8-a6f0183325f8");
            Assert.True(result > 0);
        }

        [Test]
        public void insertAnything()
        {
            CRUDManager cm = new CRUDManager();
            int result = cm.InsertOrUpdate<CommonCodeDetail>(new CommonCodeDetail
            {
                ID = null,
                Name = "test",
            });
        }
    }
}
