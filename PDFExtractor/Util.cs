using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtractor
{
    internal class PageUtil
    {
        /// <summary>
        /// モードに応じたページペアを取得
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="maxPage">ファイルの実際の最大ページ</param>
        /// <param name="pageCount">抜き出すページ数</param>
        /// <returns></returns>
        public static List<PagePair> GetPagePair(PageMode mode, int maxPage, int pageCount)
        {
            var pair = new List<PagePair>();
            //開始から
            if (mode == PageMode.Start | mode == PageMode.Both)
            {
                var endPage = pageCount > maxPage ? maxPage : pageCount;
                pair.Add(new PagePair(1, endPage));
            }
            //末尾から
            if (mode == PageMode.End | mode == PageMode.Both)
            {
                var startPage = maxPage - pageCount;
                if (startPage > 0)
                {
                    pair.Add(new PagePair(startPage, maxPage));
                }
            }

            return pair;
        }
    }

    internal class PagePair
    {
        public PagePair(int startPage, int endPage)
        {
            StartPage = startPage;
            EndPage = endPage;
        }

        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}
