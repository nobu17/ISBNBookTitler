using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler
{
    public class ComboBoxPageCount
    {
        public int Page { get; set; }
        public static List<ComboBoxPageCount> PageItems { get; set; }

        static ComboBoxPageCount()
        {
            PageItems = Enumerable.Range(1, 10).Select(x => new ComboBoxPageCount() { Page = x }).ToList();
        }
    }
}
