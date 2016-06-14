using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;

namespace ZLSoft.Pub.PageData
{
    public class Page
    {
        public Page()
        {


        }
        public Page(int pageNum,int pageSize,int dataCount)
        {
            this._PageNumber = pageNum;
            this._PageSize = pageSize;
            this._DataCount = dataCount;
        }

        #region 变量

        private int _PageNumber = -1;

        public int PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }


        private int _PageSize = -1;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        private int _PageCount = -1;

        public int PageCount
        {
            get
            {
                if (_PageNumber == -1)
                {
                    return -1;
                }
                return _DataCount % PageSize == 0 ? _DataCount / PageSize : (_DataCount / PageSize + 1);
            }
        }


        private int _DataCount = 0;

        public int DataCount
        {
            get { return _DataCount; }
            set { _DataCount = value; }
        }

        #endregion

        #region 方法

       

        #endregion




    }
}
