using CommonData;
using ISBNBookTitler.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISBNBookTitler
{
    public class FileInfoSettingViewModel : ValidatableBindableBase
    {
        public FileInfoSettingViewModel()
        {
            _closeRequest = new InteractionRequest<Confirmation>();
            _messageBoxRequest = new InteractionRequest<Confirmation>();
            Setting = new WpfPDFFileChangeSetting();
            FileNameSetting = string.Empty;
            DisplayRule = RenameInfo.GetRuleName();
        }

        #region prop

        /// <summary>
        /// OKボタンを押下したか
        /// </summary>
        private bool _isConfirmed;
        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            set { this.SetProperty(ref this._isConfirmed, value); }
        }

        /// <summary>
        /// PDFファイルの変更ルール
        /// </summary>
        private WpfPDFFileChangeSetting _setting;
        public WpfPDFFileChangeSetting Setting
        {
            get { return _setting; }
            set { this.SetProperty(ref this._setting, value); }
        }

        /// <summary>
        /// ファイル名ルール
        /// </summary>
        private string _fileNameSetting;
        [CustomValidation(typeof(FileInfoSettingViewModel), "CheckFileName")]
        public string FileNameSetting
        {
            get { return _fileNameSetting; }
            set
            {
                this.SetProperty(ref this._fileNameSetting, value);
                this.ValidateProperty(value);
            }
        }

        /// <summary>
        /// 表示エラールール
        /// </summary>
        private string _displayRule;
        public string DisplayRule
        {
            get { return _displayRule; }
            set
            {
                this.SetProperty(ref this._displayRule, value);
            }
        }

        #endregion

        #region request

        private InteractionRequest<Confirmation> _closeRequest;
        public IInteractionRequest CloseRequest
        {
            get { return _closeRequest; }
        }

        private InteractionRequest<Confirmation> _messageBoxRequest;
        public IInteractionRequest MessageBoxRequest
        {
            get { return _messageBoxRequest; }
        }

        #endregion

        #region command

        /// <summary>
        /// OKボタン押下コマンド
        /// </summary>
        private DelegateCommand _okCommand;
        public ICommand OKCommand
        {
            get
            {
                return this._okCommand ?? (this._okCommand = new DelegateCommand(() =>
                {
                    if (!HasErrors && !Setting.HasErrors)
                    {
                        IsConfirmed = true;
                        _closeRequest.Raise(new Confirmation(),
                               (res) =>
                               {
                               });
                    }
                    else
                    {
                        ShowMessage("エラー", "不正な入力があります。");
                    }
                }));
            }
        }

        /// <summary>
        /// Candelボタン押下コマンド
        /// </summary>
        private DelegateCommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return this._cancelCommand ?? (this._cancelCommand = new DelegateCommand(() =>
                {
                    IsConfirmed = false;
                    _closeRequest.Raise(new Confirmation(),
                           (res) =>
                           {
                           });
                }));
            }
        }

        #endregion

        private void ShowMessage(string title, string content)
        {
            _messageBoxRequest.Raise(
                new Confirmation { Title = title, Content = content },
                // コールバックで続きの処理をやる
                res =>
                {
                });
        }

        public static ValidationResult CheckFileName(string path, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new ValidationResult("ファイル名を入力してください。");
            }

            //置換パラメータがなければ無効
            var keys = RenameInfo.GetKeys();
            if (!keys.Any(x => path.Contains(x)))
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
