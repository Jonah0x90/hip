using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.System
{
    public class XT_THIRD_CON:BuzzModel
    {
        public string CONN_ID
        {
            get;
            set;
        }
        public string CONN_NAME
        {
            get;
            set;
        }
        public string PROVIDER_NAME
        {
            get;
            set;
        }
        public string SERVER_NAME
        {
            get;
            set;
        }
        public string USERNAME
        {
            get;
            set;
        }
        public string PASSWORD
        {
            get;
            set;
        }
        public string DATABASE
        {
            get;
            set;
        }
        public string PARAM
        {
            get;
            set;
        }
        public string MAPSFILE
        {
            get;
            set;
        }
        public override string GetModelName()
        {
            return "XT_THIRD_CON";
        }
        public override string GetTableName()
        {
            return "XT_THIRD_CON";
        }
    }
}
