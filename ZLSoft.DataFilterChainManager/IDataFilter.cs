using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;

namespace ZLSoft.DataFilterChainManager
{
    public interface IDataFilter
    {
        IList<StrObjectDict> filterSource(IList<StrObjectDict> source, string expression);
        string filterSql(string source, string expression);
    }
}
