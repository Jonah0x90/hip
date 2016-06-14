using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Pub.PageData
{
    public class PageData<T>
    {
        private IList<T> _DataList = null;

        public IList<T> DataList
        {
            get { return _DataList; }
            set { _DataList = value; }
        }

        private Page _PageInfo = null;

        public Page PageInfo
        {
            get { return _PageInfo; }
            set { _PageInfo = value; }
        }


        
        public PageData(IList<T> data,int currentNumber,int pageSize,int listCount)
        {
            _DataList = data;

            _PageInfo = new Page(currentNumber, pageSize, listCount);
        }

        public PageData(IList<T> data, int currentNumber, int listCount)
        {
            _DataList = data;

            _PageInfo = new Page(currentNumber, 20, listCount);
        }

        public PageData(IList<T> data)
        {
            _DataList = data;
        }

    }
}
