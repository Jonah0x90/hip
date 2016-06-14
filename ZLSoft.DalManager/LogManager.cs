using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public class LogManager:CRUDManager
    {
        #region 构造
        private static LogManager _Instance = new LogManager();

        public static LogManager Instance
        {
            get
            {
                return LogManager._Instance;
            }
        }
       

        private LogManager()
        {

        }

        #endregion

        

        #region 方法

        private void Log(Log log)
        {
            log.ModifyTime = DateTime.Now;
            if (log.Content.Length > 3990)
            {
                log.Content = log.Content.Substring(0, 3990);
            }
            int result = InsertOrUpdate<Log>(log);
        }

        /// <summary>
        /// 日志级别:错误
        /// </summary>
        public void Error(Log log)
        {
           
            log.Level = 1;
            Log(log);

        }

        /// <summary>
        /// 日志级别:警告
        /// </summary>
        public void Warning(Log log)
        {
            log.Level = 2;
            Log(log);
        }
        /// <summary>
        /// 日志级别:信息
        /// </summary>
        public void Info(Log log)
        {
            log.Level = 3;
            Log(log);
        }


        /// <summary>
        /// 日志级别:成功
        /// </summary>
        public void Success(Log log)
        {
            log.Level = 4;
            Log(log);
        }
        

        #endregion

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<Log> Pages(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_Log", obj);
            IList<Log> listData = DB.List<Log>(obj, p.PageNumber, p.PageSize);
            return new PageData<Log>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }
    }
}
