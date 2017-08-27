using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookTitleGetter
{
    public class NationalLibBookInfoGet : IBookInfoGet
    {
        private const string BaseURL = @"http://iss.ndl.go.jp/api/sru?operation=searchRetrieve&version=1.2&maximumRecords=1&query=isbn%20%3d%20%22{0}%22&recordSchema=dcndl_simple";

        public BookInfo GetBookInfo(string isbn13)
        {
            var task = GetBookInfoAsync(isbn13);
            task.Wait();
            return task.Result;
        }

        private async Task<BookInfo> GetBookInfoAsync(string isbn13)
        {
            var bookinfo = new BookInfo();

            //URL用にISBN13をISBN10に変換する
            var isbn10 = BookUtil.IsbnConverter.GetIsbn10FromIsbn13(isbn13);
            var url = string.Format(BaseURL, isbn10);

            var client = new HttpClient();

            var res = await client.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();

                var doc = XDocument.Parse(result);

                var record = doc.Descendants().Where(x => x.Name.LocalName == "record").FirstOrDefault().Descendants().Where(x => x.Name.LocalName == "recordData").FirstOrDefault();

                bookinfo.Title = record.Descendants().Where(x => x.Name.LocalName == "title").FirstOrDefault().Value;

                try
                {
                    bookinfo.Author = record.Descendants().Where(x => x.Name.LocalName == "creator").FirstOrDefault().Value.Replace(" 著", "").Trim();
                }
                catch (Exception) { }

                try
                {
                    bookinfo.Publisher = record.Descendants().Where(x => x.Name.LocalName == "publisher").FirstOrDefault().Value.Trim();
                }
                catch (Exception) { }

                try
                {
                    bookinfo.Date = record.Descendants().Where(x => x.Name.LocalName == "issued").FirstOrDefault().Value.Trim();
                }
                catch (Exception) { }

                var cc222 = record.Descendants().Where(x => x.Name.LocalName == "subject" && x.ToString().Contains("dcndl:NDC9")).FirstOrDefault();
                //ジャンルをNDC9より取り出す
                try
                {
                    var ndlc = record.Descendants().Where(x => x.Name.LocalName == "subject" && x.ToString().Contains("dcndl:NDC9")).FirstOrDefault().Value.Trim();
                    //頭2桁のみ使用
                    bookinfo.Genre = NdcConverter.GetNdcName(ndlc);
                }
                catch (Exception) { }

                return bookinfo;
            }

            return null;
        }
    }
}
