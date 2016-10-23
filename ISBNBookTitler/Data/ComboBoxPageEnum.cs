using FileExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler
{
    /// <summary>
    /// コンボボックスのページパターン表示
    /// </summary>
    public class ComboBoxPageEnum
    {
        public PageMode PageMode { get; set; }
        public string ComboBoxDisplay { get; set; }

        public static List<ComboBoxPageEnum> PageItems { get; set; }

        static ComboBoxPageEnum()
        {
            PageItems = new List<ComboBoxPageEnum>
            {
                new ComboBoxPageEnum()
                {
                    PageMode = PageMode.Start,
                    ComboBoxDisplay = "開始ページのみ"
                },
                new ComboBoxPageEnum()
                {
                    PageMode = PageMode.End,
                    ComboBoxDisplay = "終了ページのみ"
                },
                new ComboBoxPageEnum()
                {
                    PageMode = PageMode.Both,
                    ComboBoxDisplay = "両方"
                }
            };
        }
    }
}
