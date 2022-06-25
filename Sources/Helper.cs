using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PHChecker
{
    public class Helper
    {
        public static string Pars(string strSource, string strStart, string strEnd, int startPos = 0)
        {
            string result = string.Empty;
            try
            {
                int length = strStart.Length,
                    num = strSource.IndexOf(strStart, startPos),
                    num2 = strSource.IndexOf(strEnd, num + length);
                if (num != -1 & num2 != -1)
                    result = strSource.Substring(num + length, num2 - (num + length));
            }
            catch (Exception ex) { File.WriteAllText("ParsError.txt", ex.Message); }
            return result;
        }
    }
}
