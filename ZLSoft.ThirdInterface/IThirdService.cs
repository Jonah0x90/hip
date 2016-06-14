using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLSoft.Pub;

namespace ZLSoft.ThirdInterface
{
    public interface IThirdService
    {
        IList<StrObjectDict> DoService(StrObjectDict obj);

        IList<StrObjectDict> DoServiceAsync(StrObjectDict obj);
    }
}
