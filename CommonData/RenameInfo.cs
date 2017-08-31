using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonData
{
    public class RenameInfo
    {
        public static List<string> GetKeys()
        {
            var paramList = new List<string>();
            paramList.Add("@[t]");//タイトル
            paramList.Add("@[a]");//著者
            paramList.Add("@[g]");//ジャンル
            paramList.Add("@[p]");//出版社
            paramList.Add("@[d]");//出版日付
            return paramList;
        }

        public static Dictionary<string, string> GetReplaceParam(BookInfo renameInfo)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("@[t]", renameInfo.Title ?? string.Empty);
            dic.Add("@[a]", renameInfo.Author ?? string.Empty);
            dic.Add("@[g]", renameInfo.Genre ?? string.Empty);
            dic.Add("@[p]", renameInfo.Publisher ?? string.Empty);
            dic.Add("@[d]", renameInfo.Date ?? string.Empty);

            return dic;
        }

        public static string GetRuleName()
        {
            return "@[t]=本タイトル,@[a]=著者名,@[g]=分類,@[p]=出版社,@[d]=出版日付";

        }
    }
}
