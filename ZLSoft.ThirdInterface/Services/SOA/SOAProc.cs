using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;

namespace ZLSoft.ThirdInterface.Services
{
    public class SOAProc:BaseService, IThirdService
    {
        public IList<StrObjectDict> DoService(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }

        public IList<StrObjectDict> DoServiceAsync(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }

        public StrObjectDict AsynDoService(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }
    }
}
