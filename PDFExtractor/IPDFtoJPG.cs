using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtractor
{
    /// <summary>
    /// JPG解凍インタフェース
    /// </summary>
    public interface IExtractJPG
    {
        void ExtractJpg(string file, string outputPath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode);
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
