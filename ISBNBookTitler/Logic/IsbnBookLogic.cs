﻿using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using FileExtractor;
using ISBNGetter;
using System.Reflection;
using System.IO;
using BookTitleGetter;
using ISBNBookTitler.Logic;
using System.Collections.ObjectModel;
using ISBNBookTitler.Data;
using System.Xml.Serialization;
using System;
using Common;
using CommonData;
using FileInfoChange;

namespace ISBNBookTitler
{
    public class IsbnBookLogic : ValidatableBindableBase
    {
        /// <summary>
        /// 一時フォルダパス
        /// </summary>
        private const string TempDirPath = @"Temp";

        private IFiledBookInfoGetService _filedBookInfoGetService;
        private IBookRenameService _renameService;
        private IFileInfoChange _fileInfoChangeService;

        private List<string> _convertFiles;

        public IsbnBookLogic()
        {
            GostscriptPath = string.Empty;
            ZbarimgPath = string.Empty;
            FileName = @"(@[g])[@[a]]@[t]";
            PagePattern = PageMode.Start;
            PageCount = 1;
            BookInfoGetService = BookInfoGetServiceType.Amazon;
            ReadFileEncoding = ReadFileEncodingType.UTF_8;
            DisplayMessageDuringProcess = string.Empty;
            FileChangeSetting = new WpfPDFFileChangeSetting();
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
        /// 書籍情報取得サービス
        /// </summary>
        private BookInfoGetServiceType _bookInfoGetService;
        public BookInfoGetServiceType BookInfoGetService
        {
            get { return _bookInfoGetService; }
            set
            {
                this.SetProperty(ref this._bookInfoGetService, value);
            }
        }


        /// <summary>
        /// 画像読み込みエンコーディング(ZIP)
        /// </summary>
        private ReadFileEncodingType _readFileEncoding;
        public ReadFileEncodingType ReadFileEncoding
        {
            get { return _readFileEncoding; }
            set
            {
                this.SetProperty(ref this._readFileEncoding, value);
            }
        }

        /// <summary>
        /// ファイルの命名設定
        /// </summary>
        private WpfPDFFileChangeSetting _fileChangeSetting;
        public WpfPDFFileChangeSetting FileChangeSetting
        {
            get { return _fileChangeSetting; }
            set
            {
                this.SetProperty(ref this._fileChangeSetting, value);
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

        /// <summary>
        /// 処理中の進行メッセージ
        /// </summary>
        private string _displayMessageDuringProcess;
        [XmlIgnore]
        public string DisplayMessageDuringProcess
        {
            get { return _displayMessageDuringProcess; }
            set
            {
                this.SetProperty(ref this._displayMessageDuringProcess, value);
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
            var targetFiles = files.Where(x => FileUtil.IsTargetFile(x));
            if (targetFiles.Any())
            {
                _convertFiles = targetFiles.ToList();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 変換を実施.結果はConvertResultプロパティに設定
        /// </summary>
        public void StartConvert()
        {
            var converResult = new ObservableCollection<ConvertInfo>();
            //一時フォルダをEXE直下のパスに変換
            var tempDirAct = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), TempDirPath);

            //サービスを初期化
            InitService();

            foreach(var perFile in _convertFiles.Select((x, i)=> new { File = x, index = i }))
            {
                var file = perFile.File;
                //進捗メッセージを設定
                DisplayMessageDuringProcess = string.Format("{0}/{1}", perFile.index, _convertFiles.Count());
                try
                {
                    //書籍情報の取得
                    var bookinfo = _filedBookInfoGetService.GetBookInfo(tempDirAct, file, PagePattern, PageCount, ReadFileEncoding);
                     
                    if (bookinfo != null)
                    {
                        //リネーム
                        var res = _renameService.RenameBookFile(file, bookinfo, FileName);
                        //リネームに成功したらファイル情報の付与
                        if(res.IsReaNameSuccess)
                        {
                            var res2 = _fileInfoChangeService.ChangeFIleInfo(res.AfterFullPath, bookinfo, FileChangeSetting.GetChangeSetting());
                            //結果の結合
                            res.IsFileInfoChangeSuccess = res2.IsSuccess;
                            if (!string.IsNullOrWhiteSpace(res2.Message))
                            {
                                res.Message += " pdfファイル情報の付与に失敗:" + res2.Message;
                            }
                        }
    
                        converResult.Add(res);
                    }
                    else
                    {
                        converResult.Add(new ConvertInfo() { BeforeFileName = file, AfterFileName = string.Empty, Message = "書籍情報の取得に失敗" });
                    }
                }
                catch(Exception e)
                {
                    Error(string.Format("変換中に例外が発生 {0}", file), e);
                    converResult.Add(new ConvertInfo() { BeforeFileName = file, AfterFileName = string.Empty, Message = "書籍情報の取得中に例外が発生:" + e.Message });
                }
            }
            ConvertResult = converResult;
        }

        
        
        /// <summary>
        /// 各種サービスを初期化
        /// </summary>
        protected virtual void InitService()
        {
            //ファイルのリネームサービス
            _renameService = new BookRenameService();
            //ファイルの情報変更
            _fileInfoChangeService = new FileInfoChangeService();

            //書籍情報取得サービスの選択
            var bookInfoServ = ComboBoxBookInfoType.PageItems.FirstOrDefault(x => x.SelectService == BookInfoGetService);

            //ファイルから書籍情報取得サービス
            _filedBookInfoGetService = new FiledBookInfoGetService();
            _filedBookInfoGetService.InitService(GostscriptPath, ZbarimgPath, bookInfoServ.Service);
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
