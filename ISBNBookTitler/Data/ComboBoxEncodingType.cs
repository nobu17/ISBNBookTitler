using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler
{
    public class ComboBoxEncodingType
    {
        public ReadFileEncodingType SelectEncoding { get; set; }

        public string ComboBoxDisplay { get; set; }

        public static List<ComboBoxEncodingType> PageItems { get; set; }

        static ComboBoxEncodingType()
        {
            PageItems = new List<ComboBoxEncodingType>
            {
                new ComboBoxEncodingType()
                {
                    SelectEncoding = ReadFileEncodingType.UTF_8,
                    ComboBoxDisplay = "UTF-8"
                },
                new ComboBoxEncodingType()
                {
                    SelectEncoding = ReadFileEncodingType.S_JIS,
                    ComboBoxDisplay = "S-JIS"
                },
                new ComboBoxEncodingType()
                {
                    SelectEncoding = ReadFileEncodingType.UTF_8AndS_JIS,
                    ComboBoxDisplay = "UTF-8とS-JIS"
                }
            };
        }
    }



}
