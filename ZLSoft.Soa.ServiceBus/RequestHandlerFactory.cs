using System;
using System.Collections.Generic;
using System.Text;

using HybridDSP.Net.HTTP;

namespace ZLSoft.Soa.ServiceBus
{
    class RequestHandlerFactory : IHTTPRequestHandlerFactory
    {
        public enum RequestHandlerType { DateTime, GUID };

        private RequestHandlerType _type = RequestHandlerType.DateTime;
        public RequestHandlerType HandlerType
        {
            get { return _type; }
            set { _type = value; }
        }

        public delegate void WriteLogDelegate(string str);
        private WriteLogDelegate _writeLogDelegate;

        public RequestHandlerFactory(WriteLogDelegate d)
        {
            _writeLogDelegate = d;
        }

        #region IHTTPRequestHandlerFactory Members

        public IHTTPRequestHandler CreateRequestHandler(HTTPServerRequest request)
        {
            switch (_type)
            {
                case RequestHandlerType.DateTime: return new DateTimeHandler(_writeLogDelegate);
                case RequestHandlerType.GUID: return new GuidHandler(_writeLogDelegate);
                default: return null;
            }
        }

        #endregion
    }
}
