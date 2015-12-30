using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using PDFExtractor;
using ISBNGetter;
using System.Reflection;
using System.IO;
using BookTitleGetter;
using ISBNBookTitler.Logic;
using System.Collections.ObjectModel;
using ISBNBookTitler.Data;
using System.Xml.Serialization;
using System;

namespace ISBNBookTitler
{
    public class IsbnBookLogic : ValidatableBindableBase
    {
        /// <summary>
        /// 一時フォルダパス
        /// </summary>
        private const string TempDirPath = @"Temp";

        private PdfIsbnGetLogic _pdfIsbnGet;
        private IBookRenameService _renameService;

        private List<string> _pdfFiles;

        public IsbnBookLogic()
        {
            GostscriptPath = string.Empty;
            ZbarimgPath = string.Empty;
            FileName = @"(@[g])[@[a]]@[t]";
            PagePattern = PageMode.Start;
            PageCount = 1;
        }

        #region prop

        /// <summary>
        /// gostscriptのパス
        /// </summary>
        private string _gostscriptPath;
        [CustomValidation(typeof(IsbnBookLogic), "CheckExist")]
        public string GostscriptPath
        {
            get { return _gostscriptPath; }
            set
            {
                this.SetProperty(ref this._gostscriptPath, value);
                this.ValidateProperty(value);
            }
        }

        /// <summary>
        /// zbarimgのパス
        /// </summary>
        private string _zbarimgPath;
        [CustomValidation(typeof(IsbnBookLogic), "CheckExist")]
        public string ZbarimgPath
        {
            get { return _zbarimgPath; }
            set
            {
                this.SetProperty(ref this._zbarimgPath, value);
                this.ValidateProperty(value);
            }
        }

        private string _fileName;
        [CustomValidation(typeof(IsbnBookLogic), "CheckFileName")]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                this.SetProperty(ref this._fileName, value);
                this.ValidateProperty(value);
            }
        }

        /// <summary>
        /// 画像読み込みページのパターン
        /// </summary>
        private PageMode _pagePattern;
        public PageMode PagePattern
        {
            get { return _pagePattern; }
            set
            {
                this.SetProperty(ref this._pagePattern, value);
            }
        }

        /// <summary>
        /// 読み込みページ数
        /// </summary>
        private int _pageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                this.SetProperty(ref this._pageCount, value);
            }
        }

        /// <summary>
        /// 変換結果
        /// </summary>
        private ObservableCollection<ConvertInfo> _convertResult;
        [XmlIgnore]
        public ObservableCollection<ConvertInfo> ConvertResult
        {
            get { return _convertResult; }
            set
            {
                this.SetProperty(ref this._convertResult, value);
            }
        }
        

        #endregion

        /// <summary>
        /// ファイルが読み込める状態かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsCanReadFile()
        {
            return !HasErrors;
        }

        /// <summary>
        /// 読み込みファイルを設定
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool SetFiles(string[] files)
        {
            var pdf = files.Where(x => x.EndsWith(".pdf"));
            if(pdf.Any())
            {
                _pdfFiles = pdf.ToList();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartConvert()
        {
            var converResult = new ObservableCollection<ConvertInfo>();
            //一時フォルダをEXE直下のパスに変換
            var tempDirAct = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), TempDirPath);

            //サービスを初期化
            InitService();
            
            foreach(var pdf in _pdfFiles)
            {
                try
                {
                    //書籍情報の取得
                    var bookinfo = _pdfIsbnGet.GetBookInfo(tempDirAct, pdf, PagePattern, PageCount);
                    //リネーム
                    if (bookinfo != null)
                    {
                        var res = _renameService.RenameBookFile(pdf, bookinfo, FileName);
                        converResult.Add(res);
                    }
                    else
                    {
                        converResult.Add(new ConvertInfo() { BeforeFileName = pdf, AfterFileName = string.Empty, Message = "書籍情報の取得に失敗" });
                    }
                }
                catch(Exception e)
                {
                    converResult.Add(new ConvertInfo() { BeforeFileName = pdf, AfterFileName = string.Empty, Message = "書籍情報の取得中に例外が発生:" + e.Message });
                }
            }
            ConvertResult = converResult;
        }
        
        /// <summary>
        /// 各種サービスを初期化
        /// </summary>
        protected virtual void InitService()
        {
            _renameService = new BookRenameService();

            //GostScriptによる画像変換
            var pdfconv = new GostScriptPDFtoJPG();
            pdfconv.GsExePath = GostscriptPath;
            //zbarによる画像読み込み
            var isbnimage = new IsbnGetFromZBarImage();
            isbnimage.ZBarExePath = ZbarimgPath;
            //amazonページから本の読み取り
            var amazon = new AmazonBookInfoGet();

            _pdfIsbnGet = new PdfIsbnGetLogic(pdfconv, isbnimage, amazon);
        }

        /// <summary>
        /// 検証
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ValidationResult CheckExist(string path, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new ValidationResult("EXEのパスを入力してください。");
            }
            else if (!System.IO.File.Exists(path))
            {
                return new ValidationResult("EXEが存在しません。");
            }
            else if (!path.ToLower().EndsWith(".exe"))
            {
                return new ValidationResult("無効なパスです。");
            }
            else return ValidationResult.Success;
        }

        public static ValidationResult CheckFileName(string path, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new ValidationResult("ファイル名を入力してください。");
            }

            //置換パラメータがなければ無効
            var keys = RenameInfo.GetKeys();
            if(!keys.Any(x => path.Contains(x)))
            {
                return new ValidationResult("最低でも1つの置換パラメータを入れてください。");
            }

            var invalidChars = System.IO.Path.GetInvalidFileNameChars();

            if (path.IndexOfAny(invalidChars) >= 0)
            {
                return new ValidationResult("ファイル名に指定不可能な文字列が入力されています。");
            }

            else return ValidationResult.Success;
        }



    }
}
