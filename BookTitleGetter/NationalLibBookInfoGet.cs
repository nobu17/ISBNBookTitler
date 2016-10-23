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
        private const string BaseURL = @"http://iss.ndl.go.jp/api/opensearch?isbn={0}&maximumRecords=1";

        public BookInfo GetBookInfo(string isbn13)
        {
            var task = GetBookInfoAsync(isbn13);
            task.Wait();
            return task.Result;
        }

        private async Task<BookInfo> GetBookInfoAsync(string isbn)
        {
            var url = string.Format(BaseURL, isbn);

            var client = new HttpClient();

            var res = await client.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();

                XNamespace dc = @"http://purl.org/dc/elements/1.1/";
                XNamespace dcndl = @"http://ndl.go.jp/dcndl/terms/";

                var doc = XDocument.Parse(result);
                var ele = doc.Elements().FirstOrDefault().Elements("channel").FirstOrDefault().Elements("item").FirstOrDefault();

                var bookinfo = new BookInfo();
                bookinfo.Title = ele.Element("title").Value;
                bookinfo.Author = ele.Element("author").Value;
                bookinfo.Publisher = ele.Element(dc + "publisher").Value;

                var ndc = ele.Elements(dc + "subject").FirstOrDefault(x => x.ToString().Contains("dcndl:NDC9"));
                if (ndc != null)
                {
                    //分類名称を取得
                    bookinfo.Genre = NdcConverter.GetNdcName(ndc.Value);
                }

                return bookinfo;
            }

            return null;
        }
    }
}
