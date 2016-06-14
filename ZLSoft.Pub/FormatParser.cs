using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZLSoft.Pub
{
    public static class FormatParser
    {
        public static string DEFAULT_CAPTURE_RULE = "%([A-Za-z_]*)%";
        public static System.Collections.Generic.IDictionary<string, string> CaptureFormatKey(string formatString, string captureRule)
        {
            captureRule = (string.IsNullOrEmpty(captureRule) ? FormatParser.DEFAULT_CAPTURE_RULE : captureRule);
            Regex regex = new Regex(captureRule);
            MatchCollection matchCollection = regex.Matches(formatString);
            System.Collections.Generic.IDictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
            foreach (Match match in matchCollection)
            {
                dictionary.Add(match.Groups[1].Value.ToUpper(), match.Value);
            }
            return dictionary;
        }
    }
}
