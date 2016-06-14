using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.Pub;
using ZLSoft.Pub.Constant;
using ZLSoft.Pub.Db;
using ZLSoft.DalManager;

namespace ZLSoft.Public.Controllers
{
    /// <summary>
    /// 系统运行公共方法
    /// </summary>
    public class SysCommonController:BaseController
    {
        public ActionResult GetInputCode(){

            StrObjectDict dict = GetParams();
            string chnr = dict.GetString("Chnr");
            string code = "拼音码";

            #region 检查参数有效性
            if (string.IsNullOrEmpty(chnr)){
                return this.MyJson(0,"参数错误:Chnr");
            }

            //if (string.IsNullOrEmpty(code))
            //{
            //    return this.MyJson(0, "参数错误:Code");
            //}

            #endregion

            string mapid = "PubSql_GetSrm";
            //if (DB.GetDbtype() == DBSERVER_TYPE.ORACLE)
            //{
            //    mapid = "PubSql_GetSrm";
            //}
            //else
            //{
            //    mapid = "PubSql_GetSrm_Mssql";
            //}
			IList<StrObjectDict> source = DB.ListSod(mapid, new
			{
                Chnr = chnr,
				Code = code
			}.toStrObjDict(true));

			StrObjectDict strObjDict = source.FirstOrDefault<StrObjectDict>();
			string result;
			if (strObjDict != null)
			{
                result = Utils.GetString(strObjDict["InputCode"]);
			}
			else
			{
				result = "";
			}
            return this.MyJson(1, new
            {
                InputCode = result
            });
        }


        public ActionResult Test()
        {
            string regstr = @"^(\d{4})-([0-1]\d)-([0-3]\d)\s([0-5]\d):([0-5]\d):([0-5]\d)$";
            Regex reg = new Regex(regstr);
            bool result = reg.Match("1987-09-11 17:22:22").Success;
            bool result2 = reg.Match("asdfasdfa").Success;
            return null;
        }

        public ActionResult UUID()
        {
            string guid = Utils.getGUID();
            return this.MyJson(1,guid);
        }

        public ActionResult Demo()
        {
            StrObjectDict dict = GetParams();

            string temp = dict.GetString("FormStruct");
            string FormID = dict.GetString("FormID");
            string VersionCode = dict.GetString("VersionCode");

            StrObjectDict tes1 = JsonAdapter.FromJsonAsDictionary(temp).toStrObjDict();

            string result = temp.ToJson();


            //string[] array = temp.Trim().Split(',');

            //string temp2 = "";

            //for (int i = 0; i < array.Length; i++)
            //{
            //    array[i] += ",";
            //    temp2 += array[i];
            //}

            //temp2 = temp2.Substring(0, temp2.Length - 1);

            //IList<string> lis = new List<string>(array);

            int temps = FormManager.Instance.InsertData(tes1,dict);

            //return this.MyJson(1, lis);

            return this.MyJson(1, tes1);
        }

      
    }
}
