using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ZLSoft.Pub.Mvc
{
    public class MvcViewEngine: RazorViewEngine
	{
		private string _AppPath = string.Empty;
		private static string[] NewViewFormats = new string[]
		{
			"~/Views/Sys/{1}/{0}.cshtml",
			"~/Views/Nursing/{1}/{0}.cshtml",
			"~/Views/Hr/{1}/{0}.cshtml",
			"~/Views/Mobile/{1}/{0}.cshtml",
			"~/Views/Radiation/{1}/{0}.cshtml"
		};
		private static string[] NewViewFormats2 = new string[]
		{
			"~/Views/Sys/{1}/{0}.cshtml",
			"~/Views/Nursing/{1}/{0}.cshtml",
			"~/Views/Hr/{1}/{0}.cshtml",
			"~/Views/Mobile/{1}/{0}.cshtml",
			"~/Views/Radiation/{1}/{0}.cshtml"
		};
		public MvcViewEngine()
		{
            base.ViewLocationFormats = base.ViewLocationFormats.Union(MvcViewEngine.NewViewFormats).ToArray<string>();
            base.PartialViewLocationFormats = base.ViewLocationFormats.Union(MvcViewEngine.NewViewFormats2).ToArray<string>();
		}
	}
}
