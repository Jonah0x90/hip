using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Pub
{
    public static class MD5Encode
    {
        public static string Encode(string input)
        {
            string result;
            if (string.IsNullOrEmpty(input))
            {
                result = string.Empty;
            }
            else
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                System.Security.Cryptography.MD5 mD = System.Security.Cryptography.MD5.Create();
                byte[] array = mD.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }
    }
}
