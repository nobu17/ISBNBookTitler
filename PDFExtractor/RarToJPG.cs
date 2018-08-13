using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace FileExtractor
{
    /// <summary>
    /// RAR形式の展開
    /// </summary>
    public class RarToJPG : IExtractJPG
    {
        public void ExtractJpg(string file, string outputPath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode)
        {
            //モードに応じたエンコーディング
            var encList = GetEncodingByMode(encodingMode);
            foreach (var enc in encList)
            {
                var option = new ReaderOptions();
                option.ArchiveEncoding = enc;
                //解凍
                using (var archive = RarArchive.Open(file, option))
                {
                    var entries = archive.Entries.Where(x => !x.IsDirectory && ImageUtil.IsImageFile(x.Key)).OrderBy(x => x.Key);
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
                            foreach (var img in entries.Skip((pair.x.StartPage - 1)).Take((pair.x.EndPage - (pair.x.StartPage - 1))))
                            {
                                img.WriteToDirectory(outputPath, new ExtractionOptions()
                                {
                                    ExtractFullPath = false,
                                    Overwrite = true
                                });

                            }
                        }
                        //一度読み込めれば次のエンコードは実施しない
                        break;
                    }
                }
            }
        }

        private IEnumerable<ArchiveEncoding> GetEncodingByMode(ReadFileEncodingType encodingMode)
        {
            var encList = new List<ArchiveEncoding>();
            if (encodingMode == ReadFileEncodingType.S_JIS)
            {
                var tEnc = new ArchiveEncoding() { Default = Encoding.GetEncoding("shift-jis") };
                encList.Add(tEnc);
            }
            else if (encodingMode == ReadFileEncodingType.UTF_8)
            {
                var tEnc = new ArchiveEncoding() { Default = new UTF8Encoding(false) };
                encList.Add(tEnc);
            }
            else if (encodingMode == ReadFileEncodingType.UTF_8AndS_JIS)
            {
                var tEnc = new ArchiveEncoding() { Default = new UTF8Encoding(false) };
                encList.Add(tEnc);
                tEnc = new ArchiveEncoding() { Default = Encoding.GetEncoding("shift-jis") };
                encList.Add(tEnc);
             }

            return encList;
        }
    }
}
