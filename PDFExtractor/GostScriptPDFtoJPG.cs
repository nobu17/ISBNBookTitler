using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtractor
{
    /// <summary>
    /// GostScriptによる画像抽出
    /// </summary>
    public class GostScriptPDFtoJPG : IExtractJPG
    {
        private const int Timeout = 1000000;

        private const string GetPageCountCmmand = @" -q -dNODISPLAY -c ({0})(r) file runpdfbegin pdfpagecount = quit";

        private const string ConvertJpgCommand = @" -dSAFER -dBATCH -dNOPAUSE -sDEVICE=jpeg -dJPEGQ=100 -dQFactor=1.0 -dDisplayFormat=16#30804 -r150 -dFirstPage={0} -dLastPage={1} -sOutputFile=""{2}"" ""{3}""";

        /// <summary>
        /// GostScriptの実行パス
        /// </summary>
        public string GsExePath { get; set; }

        public void ExtractJpg(string file, string outputPath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode)
        {
            //ページ数の取得
            var page = GetPageCount(file);
            if(page > 0)
            {
                //開始終了ページの取得
                var targetPair = PageUtil.GetPagePair(mode, page, pageCount);
                //取得したページから画像変換処理
                foreach(var pair in targetPair.Select((x, i) => new { x = x, i = i }))
                {
                    ConvertJpg(file, outputPath, pair.x.StartPage, pair.x.EndPage, pair.i.ToString());
                }
            }
        }

        private int ConvertJpg(string file, string outputPath, int startPage, int endPage, string fileNameHeader)
        {
            try
            {
                //Processオブジェクトを作成する
                using (var p = new System.Diagnostics.Process())
                {
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(file);
                    p.StartInfo.FileName = GetPath(GsExePath);
                    p.StartInfo.Arguments = string.Format(ConvertJpgCommand, startPage, endPage, Path.Combine(outputPath, fileNameHeader+"image%04d.jpg"), Path.GetFileName(file));
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = false;
                    p.StartInfo.CreateNoWindow = true;

                    //起動する
                    p.Start();

                    var page = p.StandardOutput.ReadToEnd();

                    p.WaitForExit(Timeout);

                    return p.ExitCode;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }


        private int GetPageCount(string file)
        {
            try
            {

                //Processオブジェクトを作成する
                using (var p = new System.Diagnostics.Process())
                {
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(file);
                    p.StartInfo.FileName = GetPath(GsExePath);
                    p.StartInfo.Arguments = string.Format(GetPageCountCmmand, GetPath(Path.GetFileName(file)));
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = false;
                    p.StartInfo.CreateNoWindow = true;

                    //起動する
                    p.Start();

                    var page = p.StandardOutput.ReadToEnd();

                    p.WaitForExit(Timeout);

                    int intPage = 0;
                    //キャスト
                    if (int.TryParse(page, out intPage))
                    {
                        return intPage;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch(Exception e)
            {
                return -1;
            }

        }

        private string GetPath(string path)
        {
            return string.Format(@"""{0}""", path);
        }
    }
}
