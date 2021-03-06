﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookTitleGetter;
using System.IO;
using BookUtil;
using ISBNBookTitler.Data;
using CommonData;
using Common;

namespace ISBNBookTitler.Logic
{
    /// <summary>
    /// ファイルのリネーム
    /// </summary>
    public class BookRenameService : IBookRenameService
    {
        public ConvertInfo RenameBookFile(string file, BookInfo renameInfo, string renameRule)
        {
            var repName = string.Empty;
            if (File.Exists(file) && !string.IsNullOrWhiteSpace(renameRule))
            {
                try
                {
                    var ext = Path.GetExtension(file);
                    var basedir = Path.GetDirectoryName(file);
                    var repParam = RenameInfo.GetReplaceParam(renameInfo);
                    repName = CommonStringReplace.GetReplaceString(renameRule, repParam) + ext;

                    //禁則文字は_置換する
                    var invalidChars = System.IO.Path.GetInvalidFileNameChars();
                    foreach(var ire in invalidChars)
                    {
                        repName = repName.Replace(ire, '_');
                    }
                    //重複時にはカウントアップ
                    var savePath = FileUtil.GetUniqueFilename(Path.Combine(basedir, repName));
                    repName = Path.GetFileName(savePath);

                    File.Move(file, savePath);
                    return new ConvertInfo() { BeforeFileName = file, AfterFileName = repName, Message = "", IsReaNameSuccess = true, AfterFullPath = savePath };
                }
                catch(Exception e)
                {
                    return new ConvertInfo() { BeforeFileName = file, AfterFileName = repName, Message =e.ToString() , IsReaNameSuccess = false };
                }
            }
            return new ConvertInfo() { BeforeFileName = file, AfterFileName = repName, Message = "リネーム失敗", IsReaNameSuccess = false };
        }

    }
}
