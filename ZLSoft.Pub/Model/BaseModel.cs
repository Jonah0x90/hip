using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;


namespace ZLSoft.Pub.Model
{
    public abstract class BaseModel
    {
        [ScriptIgnore]
        public virtual string MAP_INSERT
        {
            get
            {
                return "INSERT_" + this.GetModelName();
            }
        }

        [ScriptIgnore]
        public virtual string MAP_COUNT
        {
            get
            {
                return "COUNT_" + this.GetModelName();
            }
        }

         [ScriptIgnore]
        public virtual string MAP_UPDATE
        {
            get
            {
                return "UPDATE_" + this.GetModelName();
            }
        }

         [ScriptIgnore]
        public virtual string MAP_DELETE
        {
            get
            {
                return "DELETE_" + this.GetModelName();
            }
        }

         [ScriptIgnore]
        public virtual string MAP_LOAD
        {
            get
            {
                return "LOAD_" + this.GetModelName();
            }
        }

         [ScriptIgnore]
        public virtual string MAP_LIST
        {
            get
            {
                return "LIST_" + this.GetModelName();
            }
        }

         [ScriptIgnore]
         public virtual string MAP_LISTSOD
         {
             get
             {
                 return "LISTSOD_" + this.GetModelName();
             }
         }

         [ScriptIgnore]
        public virtual string MAP_LIST2
        {
            get
            {
                return "LIST2_" + this.GetModelName();
            }
        }

         [ScriptIgnore]
         public virtual string MAP_NAMEVALUE
         {
             get
             {
                 return "NAMEVALUE_" + this.GetModelName();
             }
         }

        public abstract string GetModelName();
        public abstract string GetTableName();
        public T Bind<T>(HttpRequestBase request) where T : BuzzModel
        {
            PropertyInfo[] public_Instance_DeclaredOnly_PropertyInfo = this.GetPublic_Instance_DeclaredOnly_PropertyInfo();
            PropertyInfo[] array = public_Instance_DeclaredOnly_PropertyInfo;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                string value = request.Req(propertyInfo.Name, null);
                Type type = propertyInfo.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    type = Nullable.GetUnderlyingType(type);
                }
                if (!string.IsNullOrEmpty(value))
                {
                    object value2 = Convert.ChangeType(value, type);
                    propertyInfo.SetValue(this, value2, null);
                }
                else
                {
                    propertyInfo.SetValue(this, null, null);
                }
            }
            return this as T;
        }


        public T Bind<T>(IDictionary<string, object> dictionary) where T : BuzzModel
        {
            PropertyInfo[] public_Instance_DeclaredOnly_PropertyInfo = this.GetPublic_Instance_DeclaredOnly_PropertyInfo();
            PropertyInfo[] array = public_Instance_DeclaredOnly_PropertyInfo;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                if (dictionary.Keys.Contains(propertyInfo.Name))
                {
                    string value = dictionary[propertyInfo.Name].ToString();
                    Type type = propertyInfo.PropertyType;
                    if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        object value2 = System.Convert.ChangeType(value, type);
                        propertyInfo.SetValue(this, value2, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(this, null, null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(this, null, null);
                }
            }
            return this as T;
        }
        public IDictionary<string, object> Bind(HttpRequestBase request)
        {
            return StrObjectDict.FromVariable(this.Bind<BuzzModel>(request));
        }
        public IDictionary<string, object> Bind(object o)
        {
            return StrObjectDict.FromVariable(this).Merger(o);
        }
        public StrObjectDict ToDict()
        {
            return this.ToDict(false);
        }
        public StrObjectDict ToDict(bool nullValueAsKey)
        {
            return this.ToDict(nullValueAsKey, ECase.NORMAL);
        }
        public StrObjectDict ToDict(bool nullValueAsKey, ECase keyCaseSensitive)
        {
            return StrObjectDict.FromVariable(this, nullValueAsKey, keyCaseSensitive);
        }


        #region 公共属性






        /// <summary>
        /// 修改用户
        /// </summary>
        public string ModifyUser { get; set; }


        public string ModifyTimeTxt
        {
            get
            {
                if (ModifyTime == null)
                {
                    return string.Empty;
                }
                return ModifyTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        [ScriptIgnore]
        public DateTime? ModifyTime
        {
            get;
            set;
        }



        public virtual string ID
        {
            get;
            set;
        }

        #endregion
    }
}
