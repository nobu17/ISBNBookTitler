using CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoChange
{
    public interface IFileInfoChange
    {
        FileInfoChangeResult ChangeFIleInfo(string filePath, BookInfo changeInfo, PDFFileChangeSetting changeSetting);
    }
}
