using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Pub
{
    [System.Serializable]
    public class StrObjectDict : Dictionary<string, object>
    {
        public static StrObjectDict FromVariable(object o)
        {
           
            return StrObjectDict.FromVariable(o, true);
        }
        public static StrObjectDict FromVariable(object o, bool nullValueAsKey)
        {
            return StrObjectDict.FromVariable(o, nullValueAsKey, ECase.NORMAL);
        }
        public static StrObjectDict FromVariable(object o, bool nullValueAsKey, ECase keyCaseSensitive)
        {
            return new StrObjectDict().Merger(o, nullValueAsKey, keyCaseSensitive) as StrObjectDict;
        }
        public StrObjectDict()
        {
        }
        public StrObjectDict(StrObjectDict dictionary)
            : base(dictionary)
        {
        }

        public string GetValues(string[] keys)
        {
            string values = string.Empty;
            foreach (var item in keys)
            {
                values += this[item];
            }
            return values;
        }
    }
}
