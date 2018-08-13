using BookTitleGetter;
using Common;
using CommonData;
using FileExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler.Logic
{
    public interface IFiledBookInfoGetService
    {
        void InitService(string ghostScriptPath, string zbarImagePath, IBookInfoGet bookinfoGetService);
        BookInfo GetBookInfo(string outputPathRoot, string filePath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode);
    }
}
