using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonData
{
    /// <summary>
    /// PDFファイルの変種設定
    /// </summary>
    public class PDFFileChangeSetting
    {
        public PDFFileChangeSetting()
        {
            Title = "@[t]";
            Author = "@[a]";
            SubTitle = "";
            CreationDate = "@[d]";
            Keyword = "";
        }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 作成者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// サブタイトル
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 作成日
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// キーワード
        /// </summary>
        public string Keyword { get; set; }
    }
}
