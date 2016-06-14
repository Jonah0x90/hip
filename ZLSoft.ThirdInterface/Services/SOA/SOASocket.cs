using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;

namespace ZLSoft.ThirdInterface.Services
{
    public class SOASocket : BaseService, IThirdService
    {

        public IList<StrObjectDict> DoService(Pub.StrObjectDict obj)
        {
            throw new NotImplementedException();
        }

        public IList<StrObjectDict> DoServiceAsync(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }

        public string AsynDoService(Pub.StrObjectDict obj)
        {
            throw new NotImplementedException();
        }
    }
}
