using BookTitleGetter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler
{
    public class ComboBoxBookInfoType
    {
        public BookInfoGetServiceType SelectService { get; set; }

        public string ComboBoxDisplay { get; set; }

        public IBookInfoGet Service { get; set; }

        public static List<ComboBoxBookInfoType> PageItems { get; set; }

        static ComboBoxBookInfoType()
        {
            PageItems = new List<ComboBoxBookInfoType>
            {
                new ComboBoxBookInfoType()
                {
                    SelectService = BookInfoGetServiceType.Amazon,
                    ComboBoxDisplay = "Amazon(非推奨)",
                    Service = new AmazonBookInfoGet()
                },
                new ComboBoxBookInfoType()
                {
                    SelectService = BookInfoGetServiceType.NationalLib,
                    ComboBoxDisplay = "openBD",
                    Service = new OpenBDBookInfoGet()
                },
                new ComboBoxBookInfoType()
                {
                    SelectService = BookInfoGetServiceType.NationalLib,
                    ComboBoxDisplay = "国会図書館",
                    Service = new NationalLibBookInfoGet()
                }
            };
        }
    }

    /// <summary>
    /// 書籍情報取得タイプ
    /// </summary>
    public enum BookInfoGetServiceType
    {
        Amazon,
        NationalLib
    }
}
