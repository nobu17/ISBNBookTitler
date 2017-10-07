using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData;
using System.Net.Http;
using BookTitleGetter.OpenBDData;
using System.Runtime.Serialization.Json;

namespace BookTitleGetter
{
    /// <summary>
    /// OpenBD APIの書籍情報取得
    /// </summary>
    public class OpenBDBookInfoGet : IBookInfoGet
    {
        private const string UrlBase = @"https://api.openbd.jp/v1/get?isbn={0}";

        public BookInfo GetBookInfo(string isbn13)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(string.Format(UrlBase, isbn13));
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        using (var content = response.Result.Content)
                        {
                            //API結果のJSONをデシリアライズ
                            var serializer = new DataContractJsonSerializer(typeof(List<HanmotoDataRoot>));
                            List<HanmotoDataRoot> infoc = (List<HanmotoDataRoot>)serializer.ReadObject(content.ReadAsStreamAsync().Result);
                            if(infoc.Any() && infoc[0] != null)
                            {
                                //戻り値にマッピング
                                return GetBookInfoFromHanmatoData(infoc[0]);
                            }
                        }
                    }

                }
            }catch(Exception)
            { }

            return null;
        }

        /// <summary>
        /// HonmatoDataから共通データの整形
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private BookInfo GetBookInfoFromHanmatoData(HanmotoDataRoot root)
        {
            var info = new BookInfo();
            try
            {
                info.Title = root.HanmotoData.DescriptiveDetail.TitleDetail.TitleElement.TitleText.Content;
            }
            catch(Exception)
            {}

            try
            {
                info.Publisher = root.HanmotoData.PublishingDetail.Imprint.ImprintName;
            }
            catch (Exception)
            { }

            try
            {
                info.Date = root.HanmotoData.PublishingDetail.PublishingDates.FirstOrDefault(x => x.PublishingDateRole == "01").Date ?? string.Empty;
            }
            catch (Exception)
            { }

            try
            {
                var auths = root.HanmotoData.DescriptiveDetail.Contributor.OrderBy(x => x.SequenceNumber).Select(x => x.PersonName.Content.Trim());
                info.Author = string.Join(",", auths);
            }
            catch (Exception)
            { }

            //コードをジャンルに変換
            try
            {
                info.Genre = GetGenreFromFormSubjects(root.HanmotoData.DescriptiveDetail.Subjects);
                //見つからなければサイズから推定
                if(string.IsNullOrWhiteSpace(info.Genre))
                {
                    info.Genre = ProductFormConverter.GetGenreStringFromFormCode(root.HanmotoData.DescriptiveDetail.ProductFormDetail);
                }
            }
            catch (Exception)
            {}

            //ない情報はサマリーで補正
            try
            {
                info = GetModifiedData(info, root.Summary);
            }
            catch (Exception)
            { }

            return info;
        }


        private string GetGenreFromFormSubjects(List<Subject> subjects)
        {
            if(subjects == null)
            {
                return string.Empty;
            }
            //まずはCコードを探す
            var cCode = subjects.FirstOrDefault(x => x.SubjectSchemeIdentifier == "78");
            if(cCode != null)
            {
                return CCodeConverter.GetBookGenreString(cCode.SubjectCode);
            }
            else
            {
                //なければジャンルコード
                var gCode = subjects.FirstOrDefault(x => x.SubjectSchemeIdentifier == "79");
                if(gCode != null)
                {
                    return OpenBDGenreCodeConverter.GetBookGenreString(gCode.SubjectCode);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private BookInfo GetModifiedData(BookInfo info, Summary summary)
        {
            if(string.IsNullOrWhiteSpace(info.Author))
            {
                info.Author = summary.Author.Replace("／著", "");
            }
            if(string.IsNullOrWhiteSpace(info.Date))
            {
                info.Date = summary.Pubdate.Replace("-", "");
            }
            if (string.IsNullOrWhiteSpace(info.Title))
            {
                info.Title = summary.Title;
            }

            return info;
        }
    }
}
