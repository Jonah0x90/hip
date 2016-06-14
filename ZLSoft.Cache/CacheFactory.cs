using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Cache.Default;

namespace ZLSoft.Cache
{
    public class CacheFactory
    {
        public static ICache CreateInstance()
        {
            return new DefaultCache();
        }
    }
}
