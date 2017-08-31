using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileUtil
    {
        private const string PDFExtension = ".pdf";
        private const string ZipExtension = ".zip";

        /// <summary>
        /// PDFファイルかどうか
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsPdfFile(string filePath)
        {
            if(filePath.ToLower().EndsWith(PDFExtension))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ZIPファイルかどうか
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsZipFile(string filePath)
        {
            if (filePath.ToLower().EndsWith(ZipExtension))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 対象ファイルかどうか
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsTargetFile(string filePath)
        {
            if(IsZipFile(filePath))
            {
                return true;
            }
            else if(IsPdfFile(filePath))
            {
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
