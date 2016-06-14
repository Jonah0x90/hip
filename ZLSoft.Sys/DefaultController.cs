using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Sys;

namespace ZLSoft.Web
{
    public class DefaultController : BaseController
    {
        public ActionResult Index()
        {
            //string filePath = base.HttpContext.Request.FilePath;
            //ActionResult result;
            //if (filePath.EndsWith("/"))
            //{
            //    result = this.Redirect(filePath + "sys/login/");
            //}
            //else
            //{
            //    result = this.Redirect(filePath + "/sys/login/");
            //}
            //return result;
            
            Success("Login");

            return null;
        }


        



      
    }
}
