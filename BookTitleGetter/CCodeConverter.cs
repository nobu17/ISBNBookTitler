using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTitleGetter
{
    public class CCodeConverter
    {
        /// <summary>
        /// Cコードから本ジャンルを取得
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetBookGenreString(string code)
        {
            if(!string.IsNullOrWhiteSpace(code) && code.Length == 4)
            {
                var str = string.Empty;
                //2桁目のみ使用
                var sec = code[1];
                switch(sec)
                {
                    case '0':
                        str = "単行本";
                        break;
                    case '1':
                        str = "文庫";
                        break;
                    case '2':
                        str = "新書";
                        break;
                    case '3':
                        str = "全集・双書";
                        break;
                    case '4':
                        str = "ムック・その他";
                        break;
                    case '5':
                        str = "事・辞典";
                        break;
                    case '6':
                        str = "図鑑";
                        break;
                    case '7':
                        str = "絵本";
                        break;
                    case '8':
                        str = "磁性媒体など";
                        break;
                    case '9':
                        str = "コミック";
                        break;
                    default:
                        str = "不正なコード";
                        break;

                }
                return str;
            }
            else
            {
                return "不明(判断不可)";
            }

        }
    }
}
