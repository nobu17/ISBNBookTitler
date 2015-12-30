using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUtil
{
    public class IsbnConverter
    {
        public static string GetIsbn10FromIsbn13(string isbn13)
        {
            if(isbn13.Length != 13)
            {
                throw new ArgumentException("Isbn13");
            }
            //頭3桁と末尾1桁の切捨て
            var cuttedIsbn = isbn13.Remove(0, 3).Remove(9, 1);

            //各桁の計算
            var sums = Enumerable.Range(0, 9).Select(x => int.Parse(cuttedIsbn[x].ToString()) * (10 - x)).Sum();
            //余りを11で割ったもの
            var digit = (11 - sums % 11).ToString();
            //11と10の場合
            if (digit == "10") digit = "X";
            else if (digit == "11") digit = "0";

            return cuttedIsbn + digit;
        }
    }
}
