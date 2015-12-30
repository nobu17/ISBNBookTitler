using BookTitleGetter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler.Data
{
    public class RenameInfo
    {
        public static List<string> GetKeys()
        {
            var paramList = new List<string>();
            paramList.Add("@[t]");
            paramList.Add("@[a]");
            paramList.Add("@[g]");
            paramList.Add("@[p]");
            return paramList;
        }

        public static Dictionary<string, string> GetReplaceParam(BookInfo renameInfo)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("@[t]", renameInfo.Title ?? string.Empty);
            dic.Add("@[a]", renameInfo.Author ?? string.Empty);
            dic.Add("@[g]", renameInfo.Genre ?? string.Empty);
            dic.Add("@[p]", renameInfo.Publisher ?? string.Empty);

            return dic;
        }
    }
}
