using Common;
using CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileInfoChange
{
    /// <summary>
    /// ファイル情報の変更
    /// </summary>
    public class FileInfoChangeService : IFileInfoChange
    {
        private readonly IFileInfoChange _pdfChangeService = new PdfFileChange();

        public FileInfoChangeResult ChangeFIleInfo(string filePath, BookInfo changeInfo, PDFFileChangeSetting changeSetting)
        {
            if(FileUtil.IsPdfFile(filePath))
            {
                //PDFファイルのみ中身を書き換える
                return _pdfChangeService.ChangeFIleInfo(filePath, changeInfo, changeSetting);
            }
            else
            {
                return new FileInfoChangeResult() { IsSuccess = true, Message = String.Empty};
            }
        }
    }
}
