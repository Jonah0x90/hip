using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.System;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;

namespace ZLSoft.DalManager
{
    public class ParmManager
    {
        private static ParmManager _Instance = new ParmManager();
        public static ParmManager Instance
        {
            get
            {
                return ParmManager._Instance;
            }
        }
        private ParmManager()
        {
        }
        public string getCsz(string CSID)
        {
            XT_XTCS xT_CS = DB.Load<XT_XTCS, PK_XT_XTCS>(new PK_XT_XTCS
            {
                CSID = CSID
            });
            return xT_CS.MRZ;
        }
        public string getCsz(string CSID, string SLID)
        {
            XT_XTCSZ xT_CSZ = DB.Load<XT_XTCSZ, PK_XT_XTCSZ>(new PK_XT_XTCSZ
            {
                CSID = CSID,
                SLID = SLID
            });
            string result;
            if (string.IsNullOrEmpty(xT_CSZ.CSZ))
            {
                result = this.getCsz(CSID);
            }
            else
            {
                result = xT_CSZ.CSZ;
            }
            return result;
        }


        public void setCsz(string CSID, string SLID, string CSZ,string XGR)
        {
            XT_XTCSZ xT_CSZ = DB.Load<XT_XTCSZ, PK_XT_XTCSZ>(new PK_XT_XTCSZ
            {
                CSID = CSID,
                SLID = SLID
            });
            xT_CSZ.CSZ = CSZ;
            xT_CSZ.XGR = XGR;
            xT_CSZ.XGRQ = new System.DateTime?(System.DateTime.Now);
            List<DBState> list = new List<DBState>();
            if (string.IsNullOrEmpty(CSID))
            {
                list.Add(new DBState
                {
                    Name = xT_CSZ.MAP_INSERT,
                    Param = xT_CSZ.ToDict(),
                    Type = ESqlType.INSERT
                });
            }
            else
            {
                list.Add(new DBState
                {
                    Name = xT_CSZ.MAP_UPDATE,
                    Param = xT_CSZ.ToDict(),
                    Type = ESqlType.UPDATE
                });
            }
            DB.Execute(list);
        }
    }
}
