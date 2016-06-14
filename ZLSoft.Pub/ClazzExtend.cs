using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ZLSoft.Pub
{
    public static class ClazzExtend
    {
        public static StrObjectDict HttpDataToDict(this HttpRequestBase request, bool emptyAsKey)
        {
            //return new StrObjectDict().Merger(request.Form);
            if (request.ContentType.IndexOf("application/json") > -1)
            {
                try
                {
                    StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    string req = reader.ReadToEnd();
                    object oi = JsonAdapter.FromJsonAsDictionary(req);
                    return StrObjectDict.FromVariable(oi,emptyAsKey);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            var o = StrObjectDict.FromVariable(request.Form,emptyAsKey);
            StrObjectDict sod = new StrObjectDict();
            sod.Add("Params", o.Merger(request.QueryString));
            return sod;
        }
        public static StrObjectDict HttpDataToDict(this HttpRequestBase request)
        {
           return  request.HttpDataToDict(false);
        }


        public static string ToJson<T, V>(this IDictionary<T, V> dictionary)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach (T current in dictionary.Keys)
            {
                string arg = "";
                if (dictionary[current] != null)
                {
                    V v = dictionary[current];
                    arg = v.ToString();
                }
                stringBuilder.AppendFormat(",{0}:\"{1}\"", current.ToString(), arg);
            }
            if (stringBuilder.Length > 0)
            {
                stringBuilder = stringBuilder.Remove(0, 1);
            }
            return "{" + stringBuilder.ToString() + "}";
        }


        public static List<SelectListItem> ToSelectListItem(this System.Collections.Generic.IEnumerable<StrObjectDict> objs, string ITEM_ID, string ITEM_NAME, string SelectedValue)
        {
            System.Collections.Generic.List<SelectListItem> list = new System.Collections.Generic.List<SelectListItem>();
            foreach (StrObjectDict current in objs)
            {
                list.Add(new SelectListItem
                {
                    Text = Utils.GetString(current[ITEM_NAME]),
                    Value = Utils.GetString(current[ITEM_ID]),
                    Selected = Utils.GetString(current[ITEM_ID]) == SelectedValue
                });
            }
            return list;
        }


        public static List<SelectListItem> ToSelectListItem(this IEnumerable<ListItem> objs, string SelectedValue)
        {
            System.Collections.Generic.List<SelectListItem> list = new System.Collections.Generic.List<SelectListItem>();
            foreach (ListItem current in objs)
            {
                list.Add(new SelectListItem
                {
                    Text = Utils.GetString(current.Text),
                    Value = Utils.GetString(current.Value),
                    Selected = Utils.GetString(current.Value) == SelectedValue
                });
            }
            return list;
        }

        public static List<SelectListItem> ToSelectListItem(this IEnumerable<StrObjectDict> objs, string ITEM_ID, string ITEM_NAME)
        {
            System.Collections.Generic.List<SelectListItem> list = new System.Collections.Generic.List<SelectListItem>();
            foreach (StrObjectDict current in objs)
            {
                list.Add(new SelectListItem
                {
                    Text = Utils.GetString(current[ITEM_NAME]),
                    Value = Utils.GetString(current[ITEM_ID])
                });
            }
            return list;
        }

        public static IList<StrObjectDict> FindAllBy(this IEnumerable<StrObjectDict> objs, string item_name, string value)
        {
            if(objs is List<StrObjectDict>){
                IList<StrObjectDict> ll = ((List<StrObjectDict>)objs).FindAll(delegate(StrObjectDict obj)
                {
                    return ""+obj[item_name] == value || value.Equals(obj[item_name]);
                });
                return ll;
            }

            return new List<StrObjectDict>();
        }

        public static IList<StrObjectDict> FindAllBy(this IEnumerable<StrObjectDict> objs, string[] columns,string[] values)
        {
            if (objs is List<StrObjectDict>)
            {
                string temp_values = values.ObjectToString();
                IList<StrObjectDict> ll = ((List<StrObjectDict>)objs).FindAll(delegate(StrObjectDict obj)
                {
    
                    string temp_values2 = obj.GetValues(columns);
                    return temp_values == temp_values2;

                });
                return ll;
            }

            return new List<StrObjectDict>();
        }

        public static StrObjectDict FindOnlyBy(this IEnumerable<StrObjectDict> objs, string item_name, string value)
        {
            if (objs is List<StrObjectDict>)
            {
                StrObjectDict ll = ((List<StrObjectDict>)objs).Find(delegate(StrObjectDict obj)
                {
                    return "" + obj[item_name] == value || value.Equals(obj[item_name]);
                });
                return ll;
            }

            return new StrObjectDict();
        }

        public static StrObjectDict FindOnlyBy(this IEnumerable<StrObjectDict> objs, string[] columns, string[] values)
        {
            if (objs is List<StrObjectDict>)
            {
                string temp_values = values.ObjectToString();
                StrObjectDict ll = ((List<StrObjectDict>)objs).Find(delegate(StrObjectDict obj)
                {

                    string temp_values2 = obj.GetValues(columns);
                    return temp_values == temp_values2;

                });
                return ll;
            }

            return new StrObjectDict();
        }

        public static string ObjectToString(this string[] obj)
        {
            string result = string.Empty;
            foreach (var item in obj)
            {
                result += item;
            }
            return result;
        }

        //public static JsonResult JsonBig(this ControllerBase controller, object data)
        //{
        //    return new BigJsonResult
        //    {
        //        Data = data,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        public static JsonResult MyJson(this ControllerBase controller, object data)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = 1,
                    Msg = "",
                    Output = new
                    {
                        Data = data
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public static JsonResult MyJson(this ControllerBase controller, int flag)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = flag,
                    Msg = "",
                    Output = ""
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult MyJson(this ControllerBase controller, int flag, string msg)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = flag,
                    Msg = msg,
                    Output = ""
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static string MyJson(this IEnumerable<StrObjectDict> obj)
        {
            return obj.ToJson();
        }

        public static JsonResult MyJson(this ControllerBase controller,int flag, object data)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = flag,
                    Msg = "",
                    Output = new
                    {
                        Data = data
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult MyJson(this ControllerBase controller, int flag, object data,Object page)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = flag,
                    Msg = "",
                    Output = new
                    {
                        Data = data,
                        PageInfo = page
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult MyJson(this ControllerBase controller, int flag,string msg, object output)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = flag,
                    Msg = msg,
                    Output = output
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult MyJson(this ControllerBase controller,int flag, object data, string ContentType)
        {
            return new BigJsonResult
            {
                Data = new
                {
                    Flag = flag,
                    Msg = "",
                    Output = new
                    {
                        Data = data
                    }
                },
                ContentType = ContentType,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stringBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        stringBuilder.Append(string.Concat(new string[]
						{
							"\"",
							dt.Columns[j].ColumnName.ToString(),
							"\":\"",
							dt.Rows[i][j].ToString(),
							"\""
						}));
                        if (j < dt.Columns.Count - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                    stringBuilder.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
            }
            stringBuilder.Append("]}");
            return stringBuilder.ToString();
        }

        public static string ToJson(this IEnumerable<StrObjectDict> obj)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength *= 100;
            return javaScriptSerializer.Serialize(obj);
        }

        public static string ToJson(this object obj)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength *= 100;
            return javaScriptSerializer.Serialize(obj);
        }

        public static string ObjectToJson<T>(string jsonName, IList<T> IL)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{\"" + jsonName + "\":[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    T t = System.Activator.CreateInstance<T>();
                    System.Type type = t.GetType();
                    System.Reflection.PropertyInfo[] properties = type.GetProperties();
                    stringBuilder.Append("{");
                    for (int j = 0; j < properties.Length; j++)
                    {
                        stringBuilder.Append(string.Concat(new object[]
						{
							"\"",
							properties[j].Name.ToString(),
							"\":\"",
							properties[j].GetValue(IL[i], null),
							"\""
						}));
                        if (j < properties.Length - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                    stringBuilder.Append("}");
                    if (i < IL.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
            }
            stringBuilder.Append("]}");
            return stringBuilder.ToString();
        }

        public static string Req(this HttpRequestBase requestBase, string key)
		{
			return requestBase.Req(key, string.Empty);
		}

        public static string Req(this HttpRequestBase requestBase, string key, string defaultValue)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(requestBase.Form[key]))
            {
                string a = requestBase.Form[key];
                if (a == "false")
                {
                    result = "0";
                }
                else
                {
                    if (a == "true,false")
                    {
                        result = "1";
                    }
                    else
                    {
                        result = requestBase.Form[key];
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(requestBase.QueryString[key]))
                {
                    result = requestBase.QueryString[key];
                }
            }
            return result;
        }

        public static IDictionary<string, object> MergerJson(this IDictionary<string, object> dictionary, string jsonstr)
        {
            return dictionary.MergerJson(jsonstr, false);
        }

        public static System.Collections.Generic.IDictionary<string, object> MergerJson(this IDictionary<string, object> dictionary, string jsonstr, bool nullValueAsKey)
        {
            return dictionary.MergerJson(jsonstr, nullValueAsKey, ECase.NORMAL);
        }

        public static System.Collections.Generic.IDictionary<string, object> MergerJson(this IDictionary<string, object> dictionary, string jsonstr, bool nullValueAsKey, ECase keyECase)
        {
            return dictionary.Merger(JsonAdapter.FromJsonAsDictionary(jsonstr), nullValueAsKey, keyECase);
        }

        public static IDictionary<string, string> MergerJson(this IDictionary<string, string> dictionary, string jsonstr)
        {
            return dictionary.MergerJson(jsonstr, false);
        }

        public static IDictionary<string, string> MergerJson(this IDictionary<string, string> dictionary, string jsonstr, bool nullValueAsKey)
        {
            return dictionary.MergerJson(jsonstr, nullValueAsKey, ECase.NORMAL);
        }

        public static IDictionary<string, string> MergerJson(this IDictionary<string, string> dictionary, string jsonstr, bool nullValueAsKey, ECase keyECase)
        {
            return dictionary.Merger(JsonAdapter.FromJsonAsDictionary(jsonstr), nullValueAsKey, keyECase);
        }

        public static IDictionary<string, object> Merger(this IDictionary<string, object> dictionary, object o)
        {
            return dictionary.Merger(o, true);
        }

        public static IDictionary<string, object> Merger(this IDictionary<string, object> dictionary, object o, bool nullValueAsKey)
        {
            return dictionary.Merger(o, nullValueAsKey, ECase.NORMAL);
        }

        public static IDictionary<string, object> Merger(this IDictionary<string, object> dictionary, object o, bool nullValueAsKey, ECase keyECase)
        {
            return dictionary.Merger(o, nullValueAsKey, keyECase, true);
        }

        public static IDictionary<string, object> Merger(this IDictionary<string, object> dictionary, object o, bool nullValueAsKey, ECase keyECase, bool includeInheritedProperty)
        {
            if (o != null)
            {
                if (o is IDictionary<string, object>)
                {
                    IDictionary<string, object> dictionary2 = o as IDictionary<string, object>;
                    string key = null;
                    foreach (string text in dictionary2.Keys)
                    {
                        key = text;
                        if (keyECase == ECase.UPPER)
                        {
                            key = text.ToUpper();
                        }
                        else
                        {
                            if (keyECase == ECase.LOWER)
                            {
                                key = text.ToLower();
                            }
                        }
                        object value = dictionary2[key];
                        if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        {
                            if (dictionary.ContainsKey(key))
                            {
                                dictionary[key] = value;
                            }
                            else
                            {
                                dictionary.Add(key, value);
                            }
                        }
                        else
                        {
                            if (nullValueAsKey)
                            {
                                if (dictionary.ContainsKey(key))
                                {
                                    dictionary[key] = value;
                                }
                                else
                                {
                                    dictionary.Add(key, value);
                                }
                            }
                        }
                    }
                }
                else if (o is NameValueCollection)
                {
                    NameValueCollection nvc = o as NameValueCollection;
                    foreach (var key in nvc.AllKeys)
                    {
                        //dict.Add(k, col[k]);
                        string[] values = nvc.GetValues(key);
                        if (values.Length == 1)
                        {
                            dictionary.Add(key, values[0]);
                        }
                        else
                        {
                            dictionary.Add(key, values);
                        }
                    }
                }
                else
                {
                    PropertyInfo[] array = includeInheritedProperty ? o.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public) : o.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    PropertyInfo[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        System.Reflection.PropertyInfo propertyInfo = array2[i];
                        string text = propertyInfo.Name;
                        if (keyECase == ECase.UPPER)
                        {
                            text = text.ToUpper();
                        }
                        else
                        {
                            if (keyECase == ECase.LOWER)
                            {
                                text = text.ToLower();
                            }
                        }
                        object value = propertyInfo.GetValue(o, null);
                        if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        {
                            if (!dictionary.ContainsKey(text))
                            {
                                dictionary.Add(text, value);
                            }
                            else
                            {
                                dictionary[text] = value;
                            }
                        }
                        else
                        {
                            if (nullValueAsKey)
                            {
                                if (!dictionary.ContainsKey(text))
                                {
                                    dictionary.Add(text, null);
                                }
                                else
                                {
                                    dictionary[text] = null;
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }


        public static IDictionary<string, string> Merger(this IDictionary<string, string> dictionary, object o)
        {
            return dictionary.Merger(o, false);
        }

        public static IDictionary<string, string> Merger(this IDictionary<string, string> dictionary, object o, bool nullValueAsKey)
        {
            return dictionary.Merger(o, nullValueAsKey, ECase.NORMAL);
        }

        public static IDictionary<string, string> Merger(this IDictionary<string, string> dictionary, object o, bool nullValueAsKey, ECase keyECase)
        {
            return dictionary.Merger(o, nullValueAsKey, keyECase, false);
        }

        public static System.Collections.Generic.IDictionary<string, string> Merger(this System.Collections.Generic.IDictionary<string, string> dictionary, object o, bool nullValueAsKey, ECase keyECase, bool includeInheritedProperty)
        {
            if (o != null)
            {
                if (o is System.Collections.Generic.IDictionary<string, string>)
                {
                    System.Collections.Generic.IDictionary<string, string> dictionary2 = o as System.Collections.Generic.IDictionary<string, string>;
                    foreach (string text in dictionary2.Keys)
                    {
                        if (dictionary.ContainsKey(text.ToUpper()))
                        {
                            dictionary[text.ToUpper()] = dictionary2[text];
                        }
                        else
                        {
                            dictionary.Add(text.ToUpper(), dictionary2[text]);
                        }
                    }
                }
                else
                {
                    System.Reflection.PropertyInfo[] array = includeInheritedProperty ? o.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public) : o.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    System.Reflection.PropertyInfo[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        System.Reflection.PropertyInfo propertyInfo = array2[i];
                        string text = propertyInfo.Name;
                        if (keyECase == ECase.UPPER)
                        {
                            text = text.ToUpper();
                        }
                        else
                        {
                            if (keyECase == ECase.LOWER)
                            {
                                text = text.ToLower();
                            }
                        }
                        object value = propertyInfo.GetValue(o, null);
                        if (value != null)
                        {
                            if (!dictionary.ContainsKey(text))
                            {
                                dictionary.Add(text, value.ToString());
                            }
                            else
                            {
                                dictionary[text] = value.ToString();
                            }
                        }
                        else
                        {
                            if (nullValueAsKey)
                            {
                                if (!dictionary.ContainsKey(text))
                                {
                                    dictionary.Add(text, null);
                                }
                                else
                                {
                                    dictionary[text] = null;
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        public static StrObjectDict toStrObjDict(this object o)
        {
            return StrObjectDict.FromVariable(o);
        }


        public static StrObjectDict toStrObjDict(this object o, bool nullValueAsKey)
        {
            //Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //return dictionary.Merger(o, nullValueAsKey) ;
            return StrObjectDict.FromVariable(o, nullValueAsKey);
        }

        public static string ToFormatString(this IDictionary<string, object> dictionary, string format)
        {
            return dictionary.ToFormatString(format, ";");
        }

        public static string ToFormatString(this IDictionary<string, object> dictionary, string format, string itemSeparator)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            string newValue = "";
            foreach (string current in dictionary.Keys)
            {
                if (dictionary[current] != null)
                {
                    newValue = dictionary[current].ToString();
                }
                list.Add(format.Replace("%KEY%", current).Replace("%VALUE%", newValue));
            }
            return string.Join(itemSeparator, list.ToArray());
        }

        public static string GetFormatValueString(this System.Collections.Generic.IDictionary<string, object> dictionary, string formatString)
        {
            return dictionary.GetFormatValueString(formatString, null);
        }

        public static string GetFormatValueString(this System.Collections.Generic.IDictionary<string, object> dictionary, string formatString, string captureRule)
        {
            System.Collections.Generic.IDictionary<string, string> dictionary2 = FormatParser.CaptureFormatKey(formatString, captureRule);
            if (dictionary2.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary2)
                {
                    if (dictionary.ContainsKey(current.Key))
                    {
                        formatString = formatString.Replace(current.Value, Utils.GetString(dictionary[current.Key]));
                    }
                }
            }
            else
            {
                formatString = Utils.GetString(dictionary[formatString.ToUpper()]);
            }
            return formatString;
        }

        public static string ToFormatString(this System.Collections.Generic.IDictionary<string, string> dictionary, string format)
        {
            return dictionary.ToFormatString(format, ";");
        }

        public static string ToFormatString(this System.Collections.Generic.IDictionary<string, string> dictionary, string format, string itemSeparator)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            foreach (string current in dictionary.Keys)
            {
                list.Add(format.Replace("%KEY%", current).Replace("%VALUE%", dictionary[current]));
            }
            return string.Join(itemSeparator, list.ToArray());
        }

        public static string GetString(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.GetString(key, ECase.NORMAL);
        }

        public static string GetString(this IDictionary<string, object> dictionary, string key,ECase keyECase)
        {
            if (keyECase == ECase.LOWER)
            {
                key = key.ToLower();
            }else if(keyECase == ECase.UPPER){
                key = key.ToUpper();
            }
            string result;
            if (dictionary.ContainsKey(key))
            {
                result = dictionary[key] == null ? "" : dictionary[key].ToString();
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static int? GetInt(this IDictionary<string, object> dictionary, string key)
        {
            object result = dictionary.GetObject(key, ECase.NORMAL);
            if(result != null){
                return int.Parse(result.ToString());
            }
            return null;
        }


        public static object GetObject(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.GetObject(key, ECase.NORMAL);
        }

        public static object GetObject(this IDictionary<string, object> dictionary, string key, ECase keyECase)
        {
            if (keyECase == ECase.LOWER)
            {
                key = key.ToLower();
            }
            else if (keyECase == ECase.UPPER)
            {
                key = key.ToUpper();
            }
            object result;
            if (dictionary.ContainsKey(key))
            {
                result = dictionary[key];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static DateTime? GetDate(this IDictionary<string, object> dictionary, string key)
        {
            System.DateTime? dateTime = null;
            //key = key.ToUpper();
            if (dictionary.ContainsKey(key))
            {
                string @string = Utils.GetString(dictionary[key]);
                dateTime = ((@string == "0001-01-01" || @string == "1900-01-01" || @string == "") ? dateTime : new System.DateTime?(System.Convert.ToDateTime(dictionary[key])));
            }
            return dateTime;
        }

        public static decimal GetDec(this IDictionary<string, object> dictionary, string key)
        {
            key = key.ToUpper();
            decimal result;
            if (dictionary.ContainsKey(key))
            {
                result = ((Utils.GetString(dictionary[key]) == "") ? 0m : System.Convert.ToDecimal(dictionary[key]));
            }
            else
            {
                result = 0m;
            }
            return result;
        }

        public static StrObjectDict Union2<T>(this IDictionary<string, object> dictionary1, T dictionary2) where T : System.Collections.Generic.IDictionary<string, object>
        {
            StrObjectDict strObjDict = new StrObjectDict();
            foreach (KeyValuePair<string, object> current in dictionary1.Union(dictionary2))
            {
                strObjDict.Add(current.Key, current.Value);
            }
            return strObjDict;
        }

        public static bool IsDateTime(this object o)
        {
            return o.IsDateTime(false);
        }

        public static bool IsDateTime(this object o, bool strictly)
        {
            bool result;
            if (strictly)
            {
                Regex regex = new Regex("^(19|20)[0-9]{2}[- /.](0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])\\s*([0-1]?\\d|2[0-3])?(:[0-5]?\\d)?(:[0-5]?\\d)?$");
                result = regex.IsMatch(Utils.GetString(o).Trim());
            }
            else
            {
                System.DateTime dateTime;
                result = System.DateTime.TryParse(Utils.GetString(o), out dateTime);
            }
            return result;
        }

        public static bool IsNumeric(this object o)
        {
            bool result = false;
            int num;
            if (int.TryParse(o.ToString(), out num))
            {
                if (o.ToString().Length == 1)
                {
                    result = true;
                }
                else
                {
                    if (o.ToString().Length > 1)
                    {
                        result = !o.ToString().StartsWith("0");
                    }
                }
            }
            return result;
        }

        public static bool IsDecimal(this object o)
        {
            bool result = false;
            decimal num;
            if (decimal.TryParse(o.ToString(), out num))
            {
                result = true;
            }
            return result;
        }
        public static System.Reflection.PropertyInfo[] GetPublic_Instance_DeclaredOnly_PropertyInfo(this object o)
        {
            return o.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        }

        public static System.Collections.Generic.List<ListItem> ToListItem(this System.Collections.Generic.IList<StrObjectDict> records)
        {
            return (
                from record in records
                select new ListItem
                {
                    Text = record["TEXT"].ToString(),
                    Value = record["VALUE"]
                }).ToList<ListItem>();
        }

        public static System.Collections.Generic.List<ListItem> ToListItem(this System.Collections.Generic.IList<StrObjectDict> records, string textfieldname, string valuefieldname)
        {
            return (
                from record in records
                select new ListItem
                {
                    Text = record.GetFormatValueString(textfieldname),
                    Value = record.GetFormatValueString(valuefieldname)
                }).ToList<ListItem>();
        }

        public static string ToFormatString(this System.Collections.Generic.IEnumerable<StrObjectDict> records, string rowSpliter)
        {
            return records.ToFormatString(rowSpliter, "%TEXT%:%VALUE%");
        }

        public static string ToFormatString(this System.Collections.Generic.IEnumerable<StrObjectDict> records, string rowSpliter, string format)
        {
            string result = string.Empty;
            int num = records.Count<StrObjectDict>();
            if (num > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                Regex regex = new Regex("%([0-9A-Za-z_]*)%");
                MatchCollection matchCollection = regex.Matches(format);
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
                foreach (Match match in matchCollection)
                {
                    string text = match.Groups[1].Value.ToUpper();
                    format = format.Replace(match.Groups[1].Value, text);
                    list2.Add(text);
                }
                string text2 = format;
                foreach (StrObjectDict current in records)
                {
                    foreach (string current2 in list2)
                    {
                        text2 = text2.Replace("%" + current2 + "%", current[current2].ToString());
                    }
                    list.Add(text2);
                    text2 = format;
                }
                result = string.Join(rowSpliter, list.ToArray());
            }
            return result;
        }

        public static System.Collections.Generic.IEnumerable<T> Sort<T>(this System.Collections.Generic.IEnumerable<T> source, string sortDirection, string sortKey)
        {
            sortKey = sortKey.ToUpper();
            Func<T, string> keySelector = delegate(T item)
            {
                System.Collections.Generic.IDictionary<string, object> dictionary;
                if (item is System.Collections.Generic.IDictionary<string, object>)
                {
                    dictionary = (item as System.Collections.Generic.IDictionary<string, object>);
                }
                else
                {
                    dictionary = item.toStrObjDict();
                }
                string result;
                if (dictionary.ContainsKey(sortKey))
                {
                    result = Utils.GetString(dictionary[sortKey]);
                }
                else
                {
                    result = string.Empty;
                }
                return result;
            };
            return source.Sort(sortDirection, keySelector, delegate(string a, string b)
            {
                int result;
                if (a.IsDateTime() && b.IsDateTime())
                {
                    result = System.Convert.ToDateTime(a).CompareTo(System.Convert.ToDateTime(b));
                }
                else
                {
                    if (a.IsNumeric() && b.IsNumeric())
                    {
                        result = System.Convert.ToInt32(a).CompareTo(System.Convert.ToInt32(b));
                    }
                    else
                    {
                        result = a.CompareTo(b);
                    }
                }
                return result;
            });
        }

        public static System.Collections.Generic.IEnumerable<T> Sort<T, TKey>(this System.Collections.Generic.IEnumerable<T> source, string sortDirection, Func<T, TKey> keySelector, Func<TKey, TKey, int> comparerMethod)
        {
            System.Collections.Generic.IEnumerable<T> result;
            if (sortDirection == "ASC")
            {
                result = source.OrderBy(keySelector, new MyComparer<TKey>(comparerMethod));
            }
            else
            {
                result = source.OrderByDescending(keySelector, new MyComparer<TKey>(comparerMethod));
            }
            return result;
        }

        public static T ToObject<T>(this IDictionary<string, string> source)
        where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, string> item in source)
            {
                someObjectType.GetProperty(item.Key).SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            if (source is IDictionary<string, object>)
            {
                return source as IDictionary<string, object>;
            }


            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static IDictionary<string, StrObjectDict> AsDictionary(this IList<StrObjectDict> source, string[] keys)
        {
            string key = string.Empty;
            IDictionary<string, StrObjectDict> dict = new Dictionary<string, StrObjectDict>();
            foreach (var item in source)
            {
                key = item.GetValues(keys);
                if(!dict.ContainsKey(key)){
                    dict.Add(key,item);
                }
            }
            return dict;
        }
    }
}
