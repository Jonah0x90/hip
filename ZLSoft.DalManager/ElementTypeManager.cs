using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    /// <summary>
    /// 表单元素Manager
    /// </summary>
    public class ElementTypeManager : CRUDManager
    {
        private static ElementTypeManager _Instance = new ElementTypeManager();
        public static ElementTypeManager Instance
        {
            get
            {
                return ElementTypeManager._Instance;
            }
        }
        private ElementTypeManager()
        {
        }

        /// <summary>
        /// 请求表单元素
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetElementTypes(IDictionary<string, object> obj)
        {
            return DB.ListSod("LISTOF_ElementType", obj);
        }

        /// <summary>
        /// 请求表单元素组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetElementTypesGruop(IDictionary<string, object> obj)
        {
            return DB.ListSod("LISTOF_ElementTypeGroup", obj);
        }
    }
}
