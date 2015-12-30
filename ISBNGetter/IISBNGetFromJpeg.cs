using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNGetter
{
    public interface IIsbnGetFromJpeg
    {
        string GetIsbn(string imagePath);
    }
}
