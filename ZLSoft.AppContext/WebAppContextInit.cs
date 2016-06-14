using System;
using System.Collections;
using System.Collections.Generic;
using IBatisNet.DataAccess.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IBatisNet.DataAccess;
using ZLSoft.Model.System;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.Db;
using IBatisNet.DataMapper.Customize;
using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess.Interfaces;
using ZLSoft.Pub.Mvc;
using ZLSoft.ThirdInterface;
using ZLSoft.Pub.Enum;

namespace ZLSoft.AppContext
{
    public static class WebAppContextInit
    {
        private static DomDaoManagerBuilder dal = new DomDaoManagerBuilder();
        public static DomDaoManagerBuilder DomDaoManagerBuilder
        {
            get
            {
                return WebAppContextInit.dal;
            }
        }


        public static void RoutesInit(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("DEFAULT", "", new
            {
                controller = "Default",
                action = "Index"
            });
            routes.MapRoute("SUPER", "super/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
			{
				"ZLSoft.Platform.SuperControllers"
			});
            routes.MapRoute("LOGIN", "sys/login/{action}", new
            {
                controller = "Login",
                action = "Index"
            }, new string[]
			{
				"ZLSoft.Sys.Controllers"
			});
            routes.MapRoute("MAIN", "sys/main/{action}", new
            {
                controller = "Main",
                action = "Index"
            }, new string[]
			{
				"ZLSoft.Sys.Controllers"
			});
            routes.MapRoute("SYS", "sys/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
			{
				"ZLSoft.Sys.Controllers"
			});
            routes.MapRoute("PLATFORM", "platform/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
            {
                "ZLSoft.Platform.Controllers"
            });
            routes.MapRoute("PUB", "pub/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
            {
                "ZLSoft.Public.Controllers"
            });
            routes.MapRoute("HR", "hr/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
			{
				"ZLSoft.HrMng.Controllers"
			});
            routes.MapRoute("THIRD", "third/{controller}/{action}", new
            {
                controller = "Third",
                action = "Index"
            }, new string[]
            {
                "ZLSoft.Third.Controllers"
            });
            routes.MapRoute("Mobile", "Mobile/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
			{
				"IWell.Mobile.Controllers"
			});
            routes.MapRoute("Radiation", "Radiation/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
			{
				"IWell.Radiation.Controllers"
			});
            routes.MapRoute("ERROR", "error/{action}", new
            {
                controller = "Error",
                action = "PageNotFound"
            });

            routes.MapRoute("Nurse", "nurse/{controller}/{action}", new
            {
                action = "Index"
            }, new string[]
            {
                "ZLSoft.HNS.Nurse.Controllers"
            });
        }

        public static void LocalDbInit()
        {
            try
            {
                dal.Configure("dbconfig/dao.config");
            }
            catch
            {
            }
        }

        public static void ThirdDbInit()
        {
            try
            {
                object[] args = new object[0];
                IList<ZLSoft.Model.PLATFORM.DataSource> list = DB.List<ZLSoft.Model.PLATFORM.DataSource>(null);
                foreach (ZLSoft.Model.PLATFORM.DataSource current in list)
                {
                    if (DaoManager.GetInstance(current.ID) == null)
                    {
                        if (dal.TheConfigurationScope.Providers.Contains(current.Provider))
                        {
                            ExecuteSqlProviderAttacher.DBType dBType =
                                ExecuteSqlProviderAttacher.CheckDBType(current.Provider);
                            string sERVER_NAME = current.DataSourceName;
                            string dATABASE = current.Database;
                            string uSERNAME = current.UserName;
                            string pASSWORD = current.Password;
                            string key = "resource";
                            string mAPSFILE = "dbconfig/SqlMap_Interface.config";
                            string connectionString;
                            if (dBType.ToString() == "NONE" || dBType == ExecuteSqlProviderAttacher.DBType.ORACLE)
                            {
                                connectionString = "Data Source={0};User ID={1};Password={2};";
                                connectionString = string.Format(connectionString, dATABASE, uSERNAME, pASSWORD);
                            }
                            else
                            {
                                if (dBType != ExecuteSqlProviderAttacher.DBType.SQLSERVER)
                                {
                                    continue;
                                }
                                connectionString = ExecuteSqlProviderAttacher.GetSqlServerConnectString(sERVER_NAME,
                                    dATABASE, uSERNAME, pASSWORD);
                            }
                            DaoManager daoManager = DaoManager.NewInstance(current.ID);
                            daoManager.IsDefault = false;
                            daoManager.DbProvider =
                                (dal.TheConfigurationScope.Providers[current.Provider] as IDbProvider);
                            daoManager.DataSource = new IBatisNet.Common.DataSource
                            {
                                Name = sERVER_NAME,
                                ConnectionString = connectionString,
                                DbProvider = daoManager.DbProvider
                            };
                            IDictionary dictionary = new Hashtable();
                            dictionary.Add("DataSource", daoManager.DataSource);
                            dictionary.Add("UseConfigFileWatcher", false);
                            dictionary.Add(key, mAPSFILE);
                            Type type =
                                TypeUtils.ResolveType("IBatisNet.DataAccess.DaoSessionHandlers.SqlMapDaoSessionHandler");
                            IDaoSessionHandler daoSessionHandler =
                                System.Activator.CreateInstance(type, args) as IDaoSessionHandler;
                            daoSessionHandler.Configure(dal.TheConfigurationScope.Properties, dictionary);
                            daoManager.DaoSessionHandler = daoSessionHandler;
                            DaoManager.RegisterDaoManager(daoManager.Id, daoManager);
                        }
                    }
                }

            }
            catch
            {
            }
        }

        public static void ViewInit()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MvcViewEngine());
        }

        public static void OtherInit()
        {
            try
            {
                ThirdServiceContext.Initializer(DeployMode.MODE_NORMAL);
                LoginSession.Init();
                if (!CheckRegCode())
                {
                    HttpContext.Current.Application["REGCODE"] = "false";
                }
                else
                {
                    HttpContext.Current.Application["REGCODE"] = "true";
                }
            }
            catch { }

        }

        public static bool CheckRegCode()
        {
            //RegCode regCode = new RegCode();
            //string b = regCode.Sern();
            //string relativeSearchPath = System.AppDomain.CurrentDomain.RelativeSearchPath;
            //string path = relativeSearchPath.Substring(0, relativeSearchPath.LastIndexOf("\\")) + "\\RegCode.key";
            //bool result;
            //if (!System.IO.File.Exists(path))
            //{
            //    result = false;
            //}
            //else
            //{
            //    string a = System.IO.File.ReadAllText(path);
            //    result = (a == b);
            //}
            //return result;
            return true;
        }
    }
}
