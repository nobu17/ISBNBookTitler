using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTitleGetter
{
    /// <summary>
    /// ジャンルコード変換
    /// </summary>
    public class OpenBDGenreCodeConverter
    {
        private static Dictionary<string, string> CodeList = new Dictionary<string, string>()
        {
            { "01","文芸" },
            { "02","新書" },
            { "03","社会一般" },
            { "04","資格.試験" },
            { "05","ビジネス" },
            { "06","スポーツ.健康" },
            { "07","趣味.実用" },
            { "09","ゲーム" },
            { "10","芸能.タレント" },

            { "11","テレビ.映画化" },
            { "12","芸術" },
            { "13","哲学.宗教" },
            { "14","歴史.地理" },
            { "15","社会科学" },
            { "16","教育" },
            { "17","自然科学" },
            { "18","医学" },
            { "19","工業.工学" },
            { "20","コンピュータ" },

            { "21","語学.辞事典" },
            { "22","学参" },
            { "23","児童図書" },
            { "24","ヤングアダルト" },
            { "29","新刊セット" },
            { "30","全集" },

            { "31","文庫" },
            { "36","コミック文庫" },


            { "41","コミックス(欠番扱)" },
            { "42","コミックス(雑誌扱)" },
            { "43","コミックス(書籍)" },
            { "44","コミックス(廉価版)" },

            { "51","ムック" },
        };

        public static string GetBookGenreString(string code)
        {
            try
            {
                var name = CodeList.FirstOrDefault(x => code.StartsWith(x.Key));
                return name.Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
