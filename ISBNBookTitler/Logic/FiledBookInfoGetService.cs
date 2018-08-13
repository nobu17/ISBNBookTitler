using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookTitleGetter;
using Common;
using CommonData;
using FileExtractor;
using ISBNGetter;

namespace ISBNBookTitler.Logic
{
    /// <summary>
    /// ファイルから書籍情報を取得する
    /// </summary>
    public class FiledBookInfoGetService : IFiledBookInfoGetService
    {
        private ExtractAndIsbnGetLogic _pdfIsbnGet;
        private ExtractAndIsbnGetLogic _zipIsbnGet;
        private ExtractAndIsbnGetLogic _rarIsbnGet;

        /// <summary>
        /// ファイルから書籍情報を取得します
        /// </summary>
        /// <param name="outputPathRoot"></param>
        /// <param name="filePath"></param>
        /// <param name="mode"></param>
        /// <param name="pageCount"></param>
        /// <param name="encodingMode"></param>
        /// <returns></returns>
        public BookInfo GetBookInfo(string outputPathRoot, string filePath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode)
        {
            var extractAndInfoGetService = GetExtractLogicByFile(filePath);
            return extractAndInfoGetService.GetBookInfo(outputPathRoot, filePath, mode, pageCount, encodingMode);
        }

        /// <summary>
        /// ファイルパスに応じた展開ロジックを取得します
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ExtractAndIsbnGetLogic GetExtractLogicByFile(string filePath)
        {
            if(FileUtil.IsPdfFile(filePath))
            {
                return _pdfIsbnGet;
            }
            else if(FileUtil.IsZipFile(filePath))
            {
                return _zipIsbnGet;
            }
            else if (FileUtil.IsRarFile(filePath))
            {
                return _rarIsbnGet;
            }
            else
            {
                throw new ArgumentException("入力されたファイルに対応するサービスが見つかりません。"+ filePath);
            }
        }

        /// <summary>
        /// 各種サービスを初期化します
        /// </summary>
        /// <param name="ghostScriptPath"></param>
        /// <param name="zbarImagePath"></param>
        /// <param name="bookinfoGetService"></param>
        public void InitService(string ghostScriptPath, string zbarImagePath, IBookInfoGet bookinfoGetService)
        {
            //GostScriptによる画像変換
            var pdfconv = new GostScriptPDFtoJPG();
            pdfconv.GsExePath = ghostScriptPath;
            //zbarによる画像読み込み
            var isbnimage = new IsbnGetFromZBarImage();
            isbnimage.ZBarExePath = zbarImagePath;

            //PDF解凍
            _pdfIsbnGet = new ExtractAndIsbnGetLogic(pdfconv, isbnimage, bookinfoGetService);

            //ZIP解凍
            var zipExtract = new ZiptoJPG();
            _zipIsbnGet = new ExtractAndIsbnGetLogic(zipExtract, isbnimage, bookinfoGetService);

            //RAR解凍
            var rarExtract = new RarToJPG();
            _rarIsbnGet = new ExtractAndIsbnGetLogic(rarExtract, isbnimage, bookinfoGetService);
        }

    }
}
