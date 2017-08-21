using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ImageUtil
    {
        private static List<string> ImageExtensions = new List<string> (){ "jpg", "jpeg" };

        /// <summary>
        /// 画像ファイルかどうか判定
        /// </summary>
        /// <param name="filaName"></param>
        /// <returns></returns>
        public static bool IsImageFile(string filaName)
        {
            if(!string.IsNullOrWhiteSpace(filaName) && ImageExtensions.Any(x => filaName.ToLower().EndsWith(x)))
            {
                return true;
            }
            return false;
        }
    }
}
