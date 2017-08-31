using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using CommonData;
using BookUtil;

namespace FileInfoChange
{
    /// <summary>
    /// PDFファイルの情報変更
    /// </summary>
    internal class PdfFileChange : IFileInfoChange
    {
        public FileInfoChangeResult ChangeFIleInfo(string filePath, BookInfo changeInfo, PDFFileChangeSetting changeSetting)
        {
            var res = new FileInfoChangeResult();

            try
            {
                var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Modify);

                var changeData = RenameInfo.GetReplaceParam(changeInfo); 
                doc.Info.Title = EncodingUnicode(GetChanedData(changeSetting.Title, changeData));
                doc.Info.Author = EncodingUnicode(GetChanedData(changeSetting.Author, changeData));
                doc.Info.Keywords = EncodingUnicode(GetChanedData(changeSetting.Keyword, changeData));
                doc.Info.Subject = EncodingUnicode(GetChanedData(changeSetting.SubTitle, changeData));
                //出版年
                var time = DateTime.Now;
                if(DateTime.TryParse(GetChanedData(changeSetting.CreationDate, changeData), out time))
                {
                    doc.Info.CreationDate = time;
                }


                doc.Save(filePath);

                res.IsSuccess = true;
            }
            catch(Exception e)
            {
                res.IsSuccess = false;
                res.Message = e.ToString();
            }

            return res;
        }

        private string GetChanedData(string baseInfom, Dictionary<string, string> parameter)
        {
            return CommonStringReplace.GetReplaceString(baseInfom ?? string.Empty, parameter);
        }

        /// <summary>
        /// 日本語文字化け対策（参考：http://forum.pdfsharp.net/viewtopic.php?p=9523#p9523
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string EncodingUnicode(string str)
        {
            var encoding = Encoding.BigEndianUnicode;
            var bytes = encoding.GetBytes(str);
            var sb = new StringBuilder();
            sb.Append((char)254);
            sb.Append((char)255);
            for (int i = 0; i < bytes.Length; ++i)
            {
                sb.Append((char)bytes[i]);
            }
            return sb.ToString();
        }
    }
}
