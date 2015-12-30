using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFExtractor
{
    /// <summary>
    /// PDFtoJPEGインタフェース
    /// </summary>
    public interface IPDFtoJPG
    {
        void ExtractJpgFromPdf(string file, string outputPath, PageMode mode, int pageCount);
    }

    /// <summary>
    /// ページモード
    /// </summary>
    public enum PageMode
    {
        /// <summary>
        /// 開始から
        /// </summary>
        Start,
        /// <summary>
        /// 終了から
        /// </summary>
        End,
        /// <summary>
        /// 両方
        /// </summary>
        Both
    }
}
