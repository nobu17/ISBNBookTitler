using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUtil
{
    public class CommonStringReplace
    {
        /// <summary>
        /// 文字列の置換
        /// </summary>
        /// <param name="target"></param>
        /// <param name="replaceParam"></param>
        /// <returns></returns>
        public static string GetReplaceString(string target, Dictionary<string, string> replaceParam)
        {
            foreach(var rep in replaceParam)
            {
                target = target.Replace(rep.Key, rep.Value);
            }

            return target;
        }
    }
}
