using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace ZLSoft.Pub
{
    public class Utils
    {
        public static string GetString(object as_str)
        {
            string result;
            if (as_str == System.DBNull.Value || as_str == null)
            {
                result = "";
            }
            else
            {
                result = as_str.ToString();
            }
            return result;
        }

        public static string Escape(string str)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                stringBuilder.Append((char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == ' ' || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
            }
            return stringBuilder.ToString();
        }

        public static string GetPageSql_Ora(string Sql, int DataCount, int Page, int PageRows)
        {
            return string.Concat(new object[]
			{
				"SELECT *  FROM (SELECT ROWNUM AS NUM, x.*  FROM (",
				Sql,
				") x ) WHERE NUM > ",
				(Page - 1) * PageRows,
				" AND NUM<=",
				Page * PageRows
			});
        }

        public static string FillLeftString(string a_str, char a_char, int a_len)
        {
            string result;
            if (a_str.Length > a_len)
            {
                result = a_str;
            }
            else
            {
                int num = a_len - a_str.Length;
                for (int i = 0; i < num; i++)
                {
                    a_str = a_char + a_str;
                }
                result = a_str;
            }
            return result;
        }

        public static string GetString(StrObjectDict dict, string as_str)
        {
            string result;
            if (dict.ContainsKey(as_str))
            {
                result = Utils.GetString(dict[as_str]);
            }
            else
            {
                result = "";
            }
            return result;
        }

        public static string UTF8ByteArrayToString(byte[] characters)
        {
            System.Text.UTF8Encoding uTF8Encoding = new System.Text.UTF8Encoding();
            return uTF8Encoding.GetString(characters);
        }

        public static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            System.Text.UTF8Encoding uTF8Encoding = new System.Text.UTF8Encoding();
            return uTF8Encoding.GetBytes(pXmlString);
        }

        public static string GetSrmLike(string srm, string keyword, string SRM_COL, string NAME_COL, string CODE_COL, string OTHER_COL)
        {
            string text = "";
            keyword = keyword.ToUpper();
            string text2 = srm.ToUpper();
            if (string.IsNullOrEmpty(srm))
            {
                text2 = "SRM1";
            }
            text2 = text2.Substring(text2.Length - 1);
            if (!string.IsNullOrEmpty(SRM_COL))
            {
                string text3 = text;
                text = string.Concat(new string[]
				{
					text3,
					" or  UPPER(",
					SRM_COL,
					text2,
					") LIKE '%",
					keyword,
					"%'"
				});
            }
            if (!string.IsNullOrEmpty(NAME_COL))
            {
                string text3 = text;
                text = string.Concat(new string[]
				{
					text3,
					" or UPPER(",
					NAME_COL,
					") LIKE '%",
					keyword,
					"%'"
				});
            }
            if (!string.IsNullOrEmpty(CODE_COL))
            {
                string text3 = text;
                text = string.Concat(new string[]
				{
					text3,
					" or UPPER(",
					CODE_COL,
					") LIKE '%",
					keyword,
					"%'"
				});
            }
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    string text3 = text;
                    text = string.Concat(new string[]
					{
						text3,
						" or UPPER(",
						array[i],
						") LIKE '%",
						keyword,
						"%'"
					});
                }
            }
            return " ( 1=0 " + text + ") ";
        }

        public static string GetSrmOrderBy_Mssql(string SRM, string KEYWORD, string INPUT_CODE_COL, string NAME_COL, string CODE_COL, string OTHER_COL)
        {
            string text = " CAST((CASE ";
            string text2 = SRM;
            KEYWORD = KEYWORD.ToUpper();
            INPUT_CODE_COL = INPUT_CODE_COL.ToUpper();
            if (string.IsNullOrEmpty(text2))
            {
                text2 = "SRM1";
            }
            text2 = text2.Substring(text2.Length - 1);
            Utils.getorder1(ref text, INPUT_CODE_COL + text2, KEYWORD);
            if (INPUT_CODE_COL.IndexOf("SRM") >= 0)
            {
                Utils.getorder1(ref text, INPUT_CODE_COL + "3", KEYWORD);
            }
            Utils.getorder1(ref text, NAME_COL, KEYWORD);
            Utils.getorder1(ref text, CODE_COL, KEYWORD);
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    Utils.getorder1(ref text, array[i], KEYWORD);
                }
            }
            text += " END) as varchar) ";
            text += " + '_' + CAST(len(CASE ";
            Utils.getorder2(ref text, INPUT_CODE_COL + text2, KEYWORD);
            if (INPUT_CODE_COL.IndexOf("SRM") >= 0)
            {
                Utils.getorder2(ref text, INPUT_CODE_COL + "3", KEYWORD);
            }
            Utils.getorder2(ref text, NAME_COL, KEYWORD);
            Utils.getorder2(ref text, CODE_COL, KEYWORD);
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    Utils.getorder2(ref text, array[i], KEYWORD);
                }
            }
            text += " END) as varchar ) ";
            text += " + '_' +  (CASE ";
            Utils.getorder2(ref text, INPUT_CODE_COL + text2, KEYWORD);
            if (INPUT_CODE_COL.IndexOf("SRM") >= 0)
            {
                Utils.getorder2(ref text, INPUT_CODE_COL + "3", KEYWORD);
            }
            Utils.getorder2(ref text, NAME_COL, KEYWORD);
            Utils.getorder2(ref text, CODE_COL, KEYWORD);
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    Utils.getorder2(ref text, array[i], KEYWORD);
                }
            }
            text += " END) ";
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    text = text + " + '_' +" + array[i];
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(NAME_COL))
                {
                    text = text + " +  '_' + " + NAME_COL;
                }
                if (!string.IsNullOrEmpty(CODE_COL))
                {
                    text = text + " +  '_' + " + CODE_COL;
                }
            }
            return text;
        }

        public static string GetSrmOrderBy_Ora(string SRM, string KEYWORD, string INPUT_CODE_COL, string NAME_COL, string CODE_COL, string OTHER_COL)
        {
            string text = " TO_CHAR((CASE ";
            string text2 = SRM;
            KEYWORD = KEYWORD.ToUpper();
            INPUT_CODE_COL = INPUT_CODE_COL.ToUpper();
            if (string.IsNullOrEmpty(text2))
            {
                text2 = "SRM1";
            }
            text2 = text2.Substring(text2.Length - 1);
            Utils.getorder1(ref text, INPUT_CODE_COL + text2, KEYWORD);
            if (INPUT_CODE_COL.IndexOf("SRM") >= 0)
            {
                Utils.getorder1(ref text, INPUT_CODE_COL + "3", KEYWORD);
            }
            Utils.getorder1(ref text, NAME_COL, KEYWORD);
            Utils.getorder1(ref text, CODE_COL, KEYWORD);
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    Utils.getorder1(ref text, array[i], KEYWORD);
                }
            }
            text += " END),'00') ";
            text += " || '_' || TO_CHAR(length(CASE ";
            Utils.getorder2(ref text, INPUT_CODE_COL + text2, KEYWORD);
            if (INPUT_CODE_COL.IndexOf("SRM") >= 0)
            {
                Utils.getorder2(ref text, INPUT_CODE_COL + "3", KEYWORD);
            }
            Utils.getorder2(ref text, NAME_COL, KEYWORD);
            Utils.getorder2(ref text, CODE_COL, KEYWORD);
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    Utils.getorder2(ref text, array[i], KEYWORD);
                }
            }
            text += " END),'000') ";
            text += " || '_' ||  (CASE ";
            Utils.getorder2(ref text, INPUT_CODE_COL + text2, KEYWORD);
            if (INPUT_CODE_COL.IndexOf("SRM") >= 0)
            {
                Utils.getorder2(ref text, INPUT_CODE_COL + "3", KEYWORD);
            }
            Utils.getorder2(ref text, NAME_COL, KEYWORD);
            Utils.getorder2(ref text, CODE_COL, KEYWORD);
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    Utils.getorder2(ref text, array[i], KEYWORD);
                }
            }
            text += " END) ";
            if (!string.IsNullOrEmpty(OTHER_COL))
            {
                string[] array = OTHER_COL.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    text = text + " || '_' ||" + array[i];
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(NAME_COL))
                {
                    text = text + " ||  '_' || " + NAME_COL;
                }
                if (!string.IsNullOrEmpty(CODE_COL))
                {
                    text = text + " ||  '_' || " + CODE_COL;
                }
            }
            return text;
        }

        private static void getorder1(ref string OrderByStr, string COL, string keyword)
        {
            if (!string.IsNullOrEmpty(COL))
            {
                string text = OrderByStr;
                OrderByStr = string.Concat(new string[]
				{
					text,
					" WHEN  CHARINDEX( UPPER(",
					COL,
					") , '",
					keyword,
					"')>0 THEN  CHARINDEX( UPPER(",
					COL,
					") , '",
					keyword,
					"') "
				});
            }
        }

        private static void getorder2(ref string OrderByStr, string COL, string keyword)
        {
            if (!string.IsNullOrEmpty(COL))
            {
                string text = OrderByStr;
                OrderByStr = string.Concat(new string[]
				{
					text,
					" WHEN  CHARINDEX( UPPER(",
					COL,
					") , '",
					keyword,
					"')>0 THEN  ",
					COL,
					" "
				});
            }
        }

        public static EmptyResult ExportExcel(string HtmlString)
        {
            return Utils.ExportExcel(HtmlString, "export");
        }

        public static EmptyResult ExportExcel(string HtmlString, string ExcelName)
		{
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ClearHeaders();
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.Charset = "UTF-8";
			HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(ExcelName, System.Text.Encoding.UTF8) + ".xls");
			HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
			HttpContext.Current.Response.ContentType = "application/ms-excel";
			HttpContext.Current.Response.Write("<html><head><meta http-equiv=Content-Type content='text/html; charset=utf-8'></head><body>");
			HttpContext.Current.Response.Write(HtmlString);
			HttpContext.Current.Response.Write("</body></html>");
			HttpContext.Current.Response.End();
			return new EmptyResult();
		}

        /// <summary>
        /// 生成GUID
        /// </summary>
        /// <returns>GUID</returns>
        public static string getGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        
	}
}