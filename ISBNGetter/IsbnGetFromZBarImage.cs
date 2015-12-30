using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNGetter
{
    public class IsbnGetFromZBarImage : IIsbnGetFromJpeg
    {
        private const int Timeout = 1000000;

        private const string IsbnHeader = "ISBN-13:";

        private const string GetIsbnCommand = @" -Sisbn13.enable ""{0}""";

        public string ZBarExePath { get; set; }


        public string GetIsbn(string imagePath)
        {
            try
            {
                //Processオブジェクトを作成する
                using (var p = new System.Diagnostics.Process())
                {
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(imagePath);
                    p.StartInfo.FileName = GetPath(ZBarExePath);
                    p.StartInfo.Arguments = string.Format(GetIsbnCommand, imagePath);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = false;
                    p.StartInfo.CreateNoWindow = true;

                    //起動する
                    p.Start();

                    var res = p.StandardOutput.ReadToEnd();

                    p.WaitForExit(Timeout);

                    if(!string.IsNullOrWhiteSpace(res))
                    {
                        var sIndex = res.IndexOf(IsbnHeader);
                        if(sIndex >= 0)
                        {
                            var endIndex = res.IndexOf(Environment.NewLine, sIndex);
                            var isbn = res.Skip(sIndex).Take(endIndex - sIndex).ToArray();
                            return new string(isbn).Replace(IsbnHeader, string.Empty);
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private string GetPath(string path)
        {
            return string.Format(@"""{0}""", path);
        }
    }
}
