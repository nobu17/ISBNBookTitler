using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtractor
{
    public class ZiptoJPG : IExtractJPG
    {
        public void ExtractJpg(string file, string outputPath, PageMode mode, int pageCount)
        {
            var option = new ReadOptions();
            option.Encoding = new UTF8Encoding(false);
            //読み込み
            using (var zip = ZipFile.Read(file, option))
            {
                var entries = zip.Where(x => x.FileName.ToLower().EndsWith(".jpg") || x.FileName.ToLower().EndsWith(".jpeg"));
                //画像ファイル数=ページ数と換算する
                var count = entries.Count();
                if(count > 0)
                {
                    //開始終了ページの取得
                    var targetPair = PageUtil.GetPagePair(mode, count, pageCount);
                    //取得したページから画像取り出す
                    foreach (var pair in targetPair.Select((x, i) => new { x = x, i = i }))
                    {
                        //対象のファイルを解凍する
                        foreach(var img in entries.Skip(pair.x.StartPage).Take((pair.x.EndPage - pair.x.StartPage)))
                        {
                            img.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
            }
        }
    }
}
