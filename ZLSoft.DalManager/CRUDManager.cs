using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.Model;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public  class CRUDManager
    {

        public virtual IList<StrObjectDict> NameValue<T>(StrObjectDict obj) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            return DB.ListSod(t.MAP_NAMEVALUE, obj);
        }

        /// <summary>
        /// 通过ID获取相应表的一条数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual T LoadByID<T>(string ID) where T : BaseModel, new()
        {
            return DB.Load<T, string>(ID);
            //T t = System.Activator.CreateInstance<T>();
            //T result = t;
            //IList<T> list = DB.List<T>(new
            //{
            //    ID = ID
            //}.toStrObjDict());
            //if (list.Count > 0)
            //{
            //    result = list[0];
            //}
            //return result;
        }
        public virtual T LoadObject<T, V>(string mapID, V obj) where T : BaseModel, new()
        {
             return DB.LoadSod<T,V>(mapID,obj);
        }

        public virtual T LoadObject<T, V>(V obj) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            return DB.LoadSod<T, V>(t.MAP_LOAD, obj);
        }

        public virtual T LoadObjectAsync<T, V>(V obj) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            return DB.LoadSodAsync<T, V>(t.MAP_LOAD, obj);
        }

        public virtual StrObjectDict LoadObjectSod<T>(object obj) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            IList<StrObjectDict> l = DB.ListSod(t.MAP_LOAD, obj);
            if(l.Count > 0){
                return l.FirstOrDefault();
            }
            return null;
        }

        public virtual StrObjectDict LoadObjectSod(string mapID, object obj)
        {
            IList<StrObjectDict> l = DB.ListSod(mapID, obj);
            if (l.Count > 0)
            {
                return l.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 插入或者更新表
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int InsertOrUpdate<T>(T t) where T : BaseModel, new()
        {
            DBState state = null;
            if (string.IsNullOrEmpty(t.ID))
            {
                if(string.IsNullOrEmpty(t.ID)){
                    t.ID = Utils.getGUID();
                }
                state = new DBState
                {
                    Name = t.MAP_INSERT,
                    Param = t.ToDict(),
                    Type = ESqlType.INSERT
                };
            }
            else
            {
                state = new DBState
                {
                    Name = t.MAP_UPDATE,
                    Param = t.ToDict(),
                    Type = ESqlType.UPDATE
                };
            }
            return DB.Execute(state);
        }
        /// <summary>
        /// 插入或者更新表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public virtual int InsertOrUpdate<T>(StrObjectDict dict) where T : BaseModel, new()
        {
            DBState state = null;
            T t = System.Activator.CreateInstance<T>();
            if (!dict.ContainsKey("ID") || dict["ID"] == null)
            {
                dict["ID"] = Utils.getGUID();
                state = new DBState
                {
                    Name = t.MAP_INSERT,
                    Param = dict,
                    Type = ESqlType.INSERT
                };
            }
            else
            {
                state = new DBState
                {
                    Name = t.MAP_UPDATE,
                    Param = dict,
                    Type = ESqlType.UPDATE
                };
            }
            return DB.Execute(state);
        }

        public virtual int InsertOrUpdateBatchs<T>(IList<T> list) where T : BaseModel, new()
        {
            IList<DBState> states = new List<DBState>();
            T t = System.Activator.CreateInstance<T>();
            foreach (var item in list)
            {
                DBState state = null;
                if (string.IsNullOrEmpty(t.ID))
                {
                    if (string.IsNullOrEmpty(t.ID))
                    {
                        t.ID = Utils.getGUID();
                    }
                    state = new DBState
                    {
                        Name = t.MAP_INSERT,
                        Param = item.ToDict(),
                        Type = ESqlType.INSERT
                    };
                }
                else
                {
                    state = new DBState
                    {
                        Name = t.MAP_UPDATE,
                        Param = item.ToDict(),
                        Type = ESqlType.UPDATE
                    };
                }
                states.Add(state);
            }
           
            return DB.ExecuteWithTransaction(states);
        }

        public virtual int Delete<T>(StrObjectDict obj) where T : BaseModel, new()
        {
            T t = System.Activator.CreateInstance<T>();
            DBState state = new DBState
            {
                Name = t.MAP_DELETE,
                Param = obj,
                Type = ESqlType.DELETE
            };

            return DB.Execute(state);
        }
        
        /// <summary>
        /// 通过ID删除表的一条数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual int Delete<T>(string ID) where T : BaseModel, new()
        {
            T t = new T
            {
                ID = ID
            };
            DBState state = new DBState
            {
                Name = t.MAP_DELETE,
                Param = t.ToDict(),
                Type = ESqlType.DELETE
            };

            return DB.Execute(state);
        }


        /// <summary>
        /// 根据条件查询表
        /// </summary>
        /// <param name=""></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public virtual IList<T> List<T>(IDictionary<string, object> dictionary) where T : BaseModel, new()
        {
            return DB.List<T>(null, dictionary);
        }

        public virtual IList<T> List2<T>(IDictionary<string, object> dictionary) where T : BaseModel, new()
        {
            return DB.List2<T>(dictionary);
        }
        public virtual IList<StrObjectDict> List2<T>(IDictionary<string, object> dictionary,string sendNullIsOk) where T : BaseModel, new()
        {
            return DB.List2<T>(dictionary,null);
        }
        public IList<T> List<T>(string conn,IDictionary<string, object> dictionary) where T : BaseModel, new()
        {
            return DB.List<T>(conn, dictionary);
        }

        public virtual PageData<T> List<T>(IDictionary<string, object> dict, Page p) where T : BaseModel, new() 
        {
            int listcount = DB.Count<T>(dict);
            IList<T> listData = DB.List<T>(dict,p.PageNumber,p.PageSize);
            return new PageData<T>(listData,p.PageNumber,p.PageSize,listcount);
        }

        public virtual IList<StrObjectDict> ListSod<T>(IDictionary<string, object> param) where T : BaseModel, new()
        {
             T t = System.Activator.CreateInstance<T>();
             return  DB.ListSod(t.MAP_LISTSOD, param);
        }

        public virtual PageData<StrObjectDict> ListSod<T>(IDictionary<string, object> param, Page p) where T : BaseModel, new() 
        {
            T t = System.Activator.CreateInstance<T>();
            int listcount = DB.Count<T>(param);
            IList<StrObjectDict> listData = DB.ListSod(t.MAP_LISTSOD,param,p.PageNumber,p.PageSize);

            return new PageData<StrObjectDict>(listData,p.PageNumber,p.PageSize,listcount);

        }
        public virtual IList<StrObjectDict> ListSod(string mapid, IDictionary<string, object> param)
        {
            return DB.ListSod(mapid, param);
        }


        public IList<StrObjectDict> ListSod(string conn,string mapid, IDictionary<string, object> param)
        {
            return DB.ListSod(conn,mapid, param);
        }

    }
}
