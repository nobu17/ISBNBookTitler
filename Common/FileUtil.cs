using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileUtil
    {
        private const string PDFExtension = ".pdf";
        private const string ZipExtension = ".zip";
        private const string RarExtension = ".rar";

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
        /// RARファイルかどうか
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsRarFile(string filePath)
        {
            if (filePath.ToLower().EndsWith(RarExtension))
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

        /// <summary>
        /// 重複時にカウントアップしたユニークなファイル名を取得します。
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetUniqueFilename(string fullPath)
        {
            if (!Path.IsPathRooted(fullPath))
            {
                fullPath = Path.GetFullPath(fullPath);
            }

            if (File.Exists(fullPath))
            {
                String filename = Path.GetFileName(fullPath);
                String path = fullPath.Substring(0, fullPath.Length - filename.Length);
                String filenameWOExt = Path.GetFileNameWithoutExtension(fullPath);
                String ext = Path.GetExtension(fullPath);
                int n = 1;
                do
                {
                    fullPath = Path.Combine(path, String.Format("{0} ({1}){2}", filenameWOExt, (n++), ext));
                }
                while (File.Exists(fullPath));
            }
            return fullPath;
        }
    }
}
