using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTitleGetter
{
    /// <summary>
    /// ProductFormからジャンルを推定
    /// </summary>
    public class ProductFormConverter
    {
        private static Dictionary<string, string> CodeList = new Dictionary<string, string>()
        {
            { "B108","単行本" },
            { "B109","単行本" },
            { "B110","単行本" },

            { "B111","文庫" },
            { "B112","新書" }
        };

        public static string GetGenreStringFromFormCode(string code)
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
