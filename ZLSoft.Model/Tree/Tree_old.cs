using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZLSoft.Pub;

namespace ZLSoft.Model.Tree
{
    public class Tree_old
    {
        public IList<StrObjectDict> datas;
        public IList<StrObjectDict> datas_tree;

        public string root_show_flag = "1";
        private string showcheckbox = "0";
        public string ShowCheckBox
        {
            get
            {
                return showcheckbox;
            }
            set
            {
                showcheckbox = value;
            }
        }

        public string Root_show_flag
        {
            get
            {
                return root_show_flag;
            }
            set
            {
                root_show_flag = value;
            }
        }

        public IList<StrObjectDict> ToList()
        {
           IList<StrObjectDict> list = new List<StrObjectDict>();
            IEnumerable<StrObjectDict> enumerable =
                from a in this.datas
                where Utils.GetString(a["ID"]) != "ROOT"
                select a;
            foreach (StrObjectDict current in enumerable)
            {
                list.Add(new StrObjectDict
				{

					{
						"VALUE",
						Utils.GetString(current["ID"])
					},

					{
						"TEXT",
						Utils.GetString(current["MC"])
					}
				});
            }
            return list;
        }
        public string ToComboJson(string keyword)
        {
            string srm = "SRM1";
            string result;
            if (this.datas.Count > 0 && !string.IsNullOrEmpty(keyword))
            {
                if (string.IsNullOrEmpty(srm))
                {
                    srm = "SRM1";
                }
                IEnumerable<StrObjectDict> obj =
                    from a in this.datas
                    where Utils.GetString(a["ID"]) != "ROOT" && (Utils.GetString(a[srm]).ToUpper().StartsWith(keyword.ToUpper()) || Utils.GetString(a["MC"]).Contains(keyword.ToUpper()) || Utils.GetString(a["SRM3"]).ToUpper().StartsWith(keyword.ToUpper()))
                    select a;
                result = obj.ToJson();
            }
            else
            {
                result = "[]";
            }
            return result;
        }


        public string ToTreeJson(string showroot, string sjid, string async, int expanddeep)
        {
            this.m_async = async;
            this.m_sjid = sjid;
            this.m_expanddeep = expanddeep;
            int num = 1;
            this.m_pos = 0;
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(sjid))
            {
                IEnumerable<StrObjectDict> enumerable;
                if (showroot == "1")
                {
                    enumerable =
                        from a in this.datas
                        where Utils.GetString(a["SJID"]) == sjid
                        select a;
                }
                else
                {
                    enumerable =
                        from a in this.datas
                        where Utils.GetString(a["SJID"]) == "ROOT"
                        select a;
                }
                foreach (StrObjectDict Node in enumerable)
                {
                    Node["Binding_Levels"] = num;
                    if (this.datas.Count((StrObjectDict a) => Utils.GetString(a["SJID"]) == Utils.GetString(Node["ID"])) > 0)
                    {
                        Node["Binding_End_Flag"] = 0;
                    }
                    else
                    {
                        Node["Binding_End_Flag"] = 1;
                    }
                    list.Add(this.GetSubJsonTree(Node, sjid, async));
                }
            }
            else
            {
                string[] array = sjid.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] array2 = array;
                string item;
                for (int i = 0; i < array2.Length; i++)
                {
                    item = array2[i];
                    IEnumerable<StrObjectDict> enumerable =
                        from a in this.datas
                        where Utils.GetString(a["ID"]) == item
                        select a;
                    foreach (StrObjectDict Node in enumerable)
                    {
                        Node["Binding_Levels"] = num;
                        if (this.datas.Count((StrObjectDict a) => Utils.GetString(a["SJID"]) == Utils.GetString(Node["ID"])) > 0)
                        {
                            Node["Binding_End_Flag"] = 0;
                        }
                        else
                        {
                            Node["Binding_End_Flag"] = 1;
                        }
                        list.Add(this.GetSubJsonTree(Node, item, async));
                    }
                }
            }
            return "[" + string.Join(",", list.ToArray()) + "]";
        }
        public string ToGridJson()
        {
            List<string> list = new List<string>();
            foreach (StrObjectDict current in this.datas)
            {
                list.Add(string.Concat(new string[]
				{
					"\"",
					Utils.GetString(current["ID"]),
					"\":\"",
					Utils.GetString(current["SJID"]),
					"\""
				}));
            }
            return "{" + string.Join(",", list.ToArray()) + "}";
        }
        private string GetSubJsonTree(StrObjectDict node, string as_sjid, string async)
        {
            this.m_pos++;
            string @string = Utils.GetString(node["SJID"]);
            string result;
            if (Utils.GetString(node["Binding_End_Flag"]) == "1" || (this.m_pos == 2 && async == "1"))
            {
                string jsonNode = this.GetJsonNode(node, true);
                this.m_pos--;
                result = jsonNode;
            }
            else
            {
                List<string> list = new List<string>();
                foreach (StrObjectDict nodetemp in
                    from a in this.datas
                    where Utils.GetString(a["SJID"]) == Utils.GetString(node["ID"])
                    select a)
                {
                    if (this.datas.Count((StrObjectDict a) => Utils.GetString(a["SJID"]) == Utils.GetString(nodetemp["ID"])) > 0)
                    {
                        nodetemp["Binding_End_Flag"] = 0;
                    }
                    else
                    {
                        nodetemp["Binding_End_Flag"] = 1;
                    }
                    nodetemp["Binding_Levels"] = Convert.ToInt32(node["Binding_Levels"]) + 1;
                    list.Add(this.GetSubJsonTree(nodetemp, as_sjid, async));
                }
                string text = this.GetJsonNode(node, false).Replace("{children_" + Utils.GetString(node["ID"]) + "}", string.Join(",", list.ToArray())).ToString();
                this.m_pos--;
                result = text;
            }
            return result;
        }
        private string GetJsonNode(StrObjectDict Node, bool type)
        {
            string @string = Utils.GetString(Node["ID"]);
            string string2 = Utils.GetString(Node["Binding_End_Flag"]);
            string str;
            if (type)
            {
                str = "";
            }
            else
            {
                str = ((string2 == "1") ? "" : (",children:[{children_" + @string + "}]"));
            }
            string text = "{attributes:{id:\"{id}\"},complete:" + ((string2 == "1" || this.m_async == "0") ? "true" : "false") + ",data:{title:\"{title}\",attributes:{{attributes}}},state:\"{state}\",initcheckstate:\"{initcheckstate}\",selectable:\"{selectable}\"";
            string newValue;
            if (string2 == "1")
            {
                newValue = "leaf";
            }
            else
            {
                if (Utils.GetString(Node["SJID"]) == "ROOT" || Utils.GetString(Node["SJID"]) == "" || (this.m_sjid == "" && this.m_expanddeep == this.m_pos))
                {
                    newValue = "open";
                }
                else
                {
                    newValue = ",closed";
                }
            }
            string text2 = Utils.GetString(Node["MC"]);
            if (Utils.GetString(this.datas_tree.FirstOrDefault<StrObjectDict>(), "SHOW_FORMAT") != "" && Convert.ToString(Node["ID"]) != "ROOT")
            {
                text2 = Utils.GetString(this.datas_tree.FirstOrDefault<StrObjectDict>(), "SHOW_FORMAT").ToUpper();
                Regex regex = new Regex("#([^#]*)#");
                string input = Utils.GetString(this.datas_tree.FirstOrDefault<StrObjectDict>(), "SHOW_FORMAT").ToUpper();
                MatchCollection matchCollection = regex.Matches(input);
                if (matchCollection.Count > 0)
                {
                    foreach (Match match in matchCollection)
                    {
                        text2 = text2.Replace("#" + match.Groups[1].Value + "#", Convert.ToString(Node[match.Groups[1].Value]));
                    }
                }
            }
            string newValue2 = "disabled";
            if (this.showcheckbox == "1")
            {
                if (Utils.GetString(Node, "CHECKED") == "1")
                {
                    newValue2 = "checked";
                }
                else
                {
                    newValue2 = "unchecked";
                }
            }
            List<string> list = new List<string>();
            foreach (string current in Node.Keys)
            {
                list.Add(string.Concat(new string[]
				{
					"\"",
					current.ToLower(),
					"\":\"",
					Convert.ToString(Node[current]),
					"\""
				}));
            }
            string newValue3 = string.Join(",", list.ToArray());
            return text.Replace("{id}", @string).Replace("{title}", text2).Replace("{attributes}", newValue3).Replace("{state}", newValue).Replace("{initcheckstate}", newValue2).Replace("{selectable}", "1") + str + "}";
        }


        private int m_pos;
        private string m_async;
        private string m_sjid;
        private int m_expanddeep;
    }
}
