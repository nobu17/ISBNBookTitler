using BookTitleGetter;
using ISBNBookTitler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler.Logic
{
    public interface IBookRenameService
    {
        ConvertInfo RenameBookFile(string file, BookInfo renameInfo, string renameRule);
    }
}
