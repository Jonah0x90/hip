using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Script.Serialization;

namespace ZLSoft.Pub
{
    public class BigJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = ((!string.IsNullOrEmpty(base.ContentType)) ? base.ContentType : "application/json");
            if (base.ContentEncoding != null)
            {
                response.ContentEncoding = base.ContentEncoding;
            }
            if (base.Data != null)
            {
                JavaScriptSerializer javaScriptSerializer = this.CreateJsonSerializer();
                response.Write(javaScriptSerializer.Serialize(base.Data));
            }
        }
        private JavaScriptSerializer CreateJsonSerializer()
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength *= 100;
            return javaScriptSerializer;
        }
    }
}
