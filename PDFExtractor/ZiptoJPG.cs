using Common;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtractor
{
    public class ZiptoJPG : IExtractJPG
    {
        public void ExtractJpg(string file, string outputPath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode)
        {
            //モードに応じたエンコーディング
            var encList = GetEncodingByMode(encodingMode);

            foreach(var enc in encList)
            {
                var option = new ReadOptions();
                option.Encoding = enc;
                //読み込み
                using (var zip = ZipFile.Read(file, option))
                {
                    var entries = zip.Entries.Where(x => ImageUtil.IsImageFile(x.FileName));
                    //var entries = zip.Where(x => x.FileName.ToLower().EndsWith(".jpg") || x.FileName.ToLower().EndsWith(".jpeg"));
                    //画像ファイル数=ページ数と換算する
                    var count = entries.Count();
                    if (count > 0)
                    {
                        //開始終了ページの取得
                        var targetPair = PageUtil.GetPagePair(mode, count, pageCount);
                        //取得したページから画像取り出す
                        foreach (var pair in targetPair.Select((x, i) => new { x = x, i = i }))
                        {
                            //対象のファイルを解凍する
                            foreach (var img in entries.Skip(pair.x.StartPage).Take((pair.x.EndPage - pair.x.StartPage)))
                            {
                                img.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                            }
                        }
                        //一度読み込めれば次のエンコードは実施しない
                        break;
                    }
                }
            }
        }

        private IEnumerable<Encoding> GetEncodingByMode(ReadFileEncodingType encodingMode)
        {
            var encList = new List<Encoding>();
            if (encodingMode == ReadFileEncodingType.S_JIS)
            {
                encList.Add(Encoding.GetEncoding("shift-jis"));
            }
            else if (encodingMode == ReadFileEncodingType.UTF_8)
            {
                encList.Add(new UTF8Encoding(false));
            }
            else if (encodingMode == ReadFileEncodingType.UTF_8AndS_JIS)
            {
                encList.Add(new UTF8Encoding(false));
                encList.Add(Encoding.GetEncoding("shift-jis"));
            }

            return encList;
        }
    }
}
