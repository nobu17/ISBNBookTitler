using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTitleGetter
{
    public interface IBookInfoGet
    {
        BookInfo GetBookInfo(string isbn13);
    }
}
