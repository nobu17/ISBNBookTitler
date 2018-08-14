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
    public class ZiptoJPG : BaseObject, IExtractJPG
    {
        public void ExtractJpg(string file, string outputPath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode)
        {
            Info(string.Format("ZIP解凍開始 file={0},outputPath={1}, mode={2}, pageCount={3}, encodingMode={4}", file, outputPath, mode, pageCount, encodingMode));
            //モードに応じたエンコーディング
            var encList = GetEncodingByMode(encodingMode);

            foreach(var enc in encList)
            {
                Info(string.Format("enc={0}", enc));
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
                            foreach (var img in entries.Skip(pair.x.StartPage - 1).Take((pair.x.EndPage - (pair.x.StartPage - 1))))
                            {
                                img.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                            }
                        }
                        //一度読み込めれば次のエンコードは実施しない
                        break;
                    }
                    else
                    {
                        Error(string.Format("圧縮ファイル内数が0以下 {0}", count));
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
