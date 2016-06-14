using System;
using System.IO;
using System.Text;

using HybridDSP.Net.HTTP;

namespace ZLSoft.Soa.ServiceBus
{
    class GuidHandler : IHTTPRequestHandler
    {
        private RequestHandlerFactory.WriteLogDelegate _writeLogDelegate;
        public GuidHandler(RequestHandlerFactory.WriteLogDelegate d)
        {
            _writeLogDelegate = d;
        }

        #region IHTTPRequestHandler Members

        public void HandleRequest(HTTPServerRequest request, HTTPServerResponse response)
        {
            StreamReader reader = new StreamReader(request.GetRequestStream(),Encoding.UTF8);
            string req = reader.ReadToEnd();
            string n = request.Get("name");
            if (_writeLogDelegate != null)
                _writeLogDelegate("Handling GUID request");

            if (request.URI == "/")
            {
                /**
                 * In this example we'll write the body to a memorystream before
                 * we send the whole buffer to the response. This way the KeepAlive
                 * can stay true since the length of the body is known when the 
                 * response header is sent.
                 **/
                using (MemoryStream ostr = new MemoryStream())
                {
                    using (TextWriter tw = new StreamWriter(ostr))
                    {
                        tw.WriteLine("<html>");
                        tw.WriteLine("<header><title>GUID Server</title></header>");
                        tw.WriteLine("<body>");
                        tw.WriteLine(Guid.NewGuid().ToString());
                        tw.WriteLine("</body>");
                        tw.WriteLine("</html>");
                    }

                    response.SendBuffer(ostr.ToArray(), "text/html");
                }
            }
            else
            {
                response.StatusAndReason = HTTPServerResponse.HTTPStatus.HTTP_NOT_FOUND;
                response.Send();
            }
        }

        #endregion
    }
}
