using BookTitleGetter;
using ISBNGetter;
using FileExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using CommonData;

namespace ISBNBookTitler
{
    /// <summary>
    /// 圧縮ファイルから書籍情報を取得するサービス
    /// </summary>
    public class ExtractAndIsbnGetLogic
    {
        private readonly IExtractJPG _pdfImageService;
        private readonly IIsbnGetFromJpeg _isbnGetService;
        private readonly IBookInfoGet _bookInfoGetService;

        public ExtractAndIsbnGetLogic(IExtractJPG pdfImageService, IIsbnGetFromJpeg isbnGetService, IBookInfoGet bookInfoGetService)
        {
            _pdfImageService = pdfImageService;
            _isbnGetService = isbnGetService;
            _bookInfoGetService = bookInfoGetService;
        }

        public ExtractAndIsbnGetLogic() : this (new GostScriptPDFtoJPG(), new IsbnGetFromZBarImage(), new AmazonBookInfoGet())
        { }


        public BookInfo GetBookInfo(string outputPathRoot, string pdfPath, PageMode mode, int pageCount, ReadFileEncodingType encodingMode)
        {
            //作業フォルダの生成
            var tempDir = Path.Combine(outputPathRoot, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            if (Directory.Exists(tempDir))
            {
                try
                {
                    Directory.Delete(tempDir);
                }
                catch (Exception)
                {
                    throw new ArgumentException("一時フォルダの削除に失敗。");
                }
            }
            //作業フォルダの生成
            try
            {
                Directory.CreateDirectory(tempDir);
            }
            catch (Exception)
            {
                throw new ArgumentException("一時フォルダの削除に失敗。");
            }

            try
            {
                //解凍してJPG画像を生成
                _pdfImageService.ExtractJpg(pdfPath, tempDir, mode, pageCount, encodingMode);
                var jpgs = Directory.GetFiles(tempDir, "*.*", SearchOption.AllDirectories ).Where(x => ImageUtil.IsImageFile(x));
                if (!jpgs.Any())
                {
                    throw new ArgumentException("画像検出に失敗しました。エンコーディング設定を変更してみてください。");
                }

                //画像からisbnを探索
                var isbn = string.Empty;
                foreach (var jpg in jpgs)
                {
                    isbn = _isbnGetService.GetIsbn(jpg);
                    if (!string.IsNullOrWhiteSpace(isbn))
                    {
                        return _bookInfoGetService.GetBookInfo(isbn);
                    }
                }
                throw new ArgumentException("画像からISBNを取得できませんでした。");

            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }            
        }

    }
}
