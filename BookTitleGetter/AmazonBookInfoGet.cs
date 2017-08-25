using Sgml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookTitleGetter
{
    /// <summary>
    /// AmazonのHTML情報から書籍情報を取得する
    /// </summary>
    public class AmazonBookInfoGet : IBookInfoGet
    {
        private const string UrlBase = @"http://www.amazon.co.jp/dp/{0}";

        public BookInfo GetBookInfo(string isbn13)
        {
            var title = string.Empty;
            var genre = string.Empty;
            var author = string.Empty;
            var pub = string.Empty;
            var date = string.Empty;
            //URL用にISBN13をISBN10に変換する
            var isbn10 = BookUtil.IsbnConverter.GetIsbn10FromIsbn13(isbn13);
            //URL整形
            using (var stream = new WebClient().OpenRead(string.Format(UrlBase, isbn10)))
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                //HTMLをLinqToXMLで操作するためにパース
                var xml = ParseHtml(sr);

                var titlenode = xml.Descendants("span").Where(x => (string)x.Attribute("id") == "productTitle").FirstOrDefault();
                if(titlenode != null)
                {
                    title = titlenode.Value.Trim();
                    //ジャンル
                    genre = titlenode.Parent.Descendants("span").Skip(1).First().Value.Trim();
                    //出版年月日
                    date = titlenode.Parent.Descendants("span").Skip(2).First().Value.Replace("&ndash;", "").Trim();
                }
                //著者は変則的なのtitleから取り出す
                var hedInfo = xml.Descendants("title").FirstOrDefault();
                if(hedInfo != null)
                {
                    var str = hedInfo.Value;
                    //パターン1
                    if(str.Contains("：") && str.Contains(":"))
                    {
                        var splited = str.Split(':');
                        if(splited.Count() >= 2)
                        {
                            author = splited[1].Trim();
                        }
                    }
                    if (str.Contains("|") && str.Contains("|"))
                    {
                        var splited = str.Split('|');
                        author = splited[1].Trim();
                    }
                }

                //出版社
                try
                {
                    var detailNodes = xml.Descendants("div").Where(x => (string)x.Attribute("id") == "detail_bullets_id").FirstOrDefault();
                    var des = ((detailNodes.Elements().Descendants("div").Where(x => (string)x.Attribute("class") == "content").FirstOrDefault()).Descendants("li").FirstOrDefault(x => x.Value.Contains("出版社"))).Value.Replace("出版社:", "").TakeWhile(x => x != '(');
                    pub = new string(des.ToArray()).Trim();
                }
                catch (Exception)
                {
                }

            }

            var bookInfo = new BookInfo();
            bookInfo.Author = author;
            bookInfo.Genre = genre;
            bookInfo.Title = title;
            bookInfo.Publisher = pub;
            bookInfo.Date = date;

            return bookInfo;
        }

        static XDocument ParseHtml(TextReader reader)
        {
            using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower })
            {
                sgmlReader.InputStream = reader;
                return XDocument.Load(sgmlReader);
            }
        }
    }
}
