using System;
using System.IO;
using System.Text;

using HybridDSP.Net.HTTP;

namespace ZLSoft.Soa.ServiceBus
{
    class DateTimeHandler : IHTTPRequestHandler
    {
        private RequestHandlerFactory.WriteLogDelegate _writeLogDelegate;
        public DateTimeHandler(RequestHandlerFactory.WriteLogDelegate d)
        {
            _writeLogDelegate = d;
        }

        #region IHTTPRequestHandler Members

        public void HandleRequest(HTTPServerRequest request, HTTPServerResponse response)
        {
            if (_writeLogDelegate != null)
                _writeLogDelegate("Handling DateTime request");

            if (request.URI == "/")
            {
                /**
                 * In this example we'll write the body into the
                 * stream obtained by response.Send(). This will cause the
                 * KeepAlive to be false since the size of the body is not
                 * known when the response header is sent.
                 **/
                response.ContentType = "text/html";
                using (Stream ostr = response.Send())
                using (TextWriter tw = new StreamWriter(ostr))
                {
                    tw.WriteLine("<html>");
                    tw.WriteLine("<head>");
                    tw.WriteLine("<title>Date Time Server</title>");
                    tw.WriteLine("<meta http-equiv=\"refresh\" content=\"2\">");
                    tw.WriteLine("</header>");
                    tw.WriteLine("<body>");
                    tw.WriteLine(DateTime.Now.ToString());
                    tw.WriteLine("</body>");
                    tw.WriteLine("</html>");
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
