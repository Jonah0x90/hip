using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Pub.Db
{
    public class DBState
    {
        public string Name
        {
            get;
            set;
        }
        public object Param
        {
            get;
            set;
        }
        public ESqlType Type
        {
            get;
            set;
        }
    }
}
