using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using Microsoft.Practices.Prism.ViewModel;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace ISBNBookTitler
{
    public class MainViewModel : ValidatableBindableBase
    {
        /// <summary>
        /// 画面内用保存xml
        /// </summary>
        private const string SettingXmlPath = @".\Setting.xml";

        public MainViewModel(IsbnBookLogic isbnModel)
        {
            IsbnModel = isbnModel;
            _messageBoxRequest = new InteractionRequest<Confirmation>();
            _openFileDialogRequest = new InteractionRequest<OpenFileDialogAction.OpenFileDialogActionResult>();
        }

        public MainViewModel() : this (new IsbnBookLogic())
        {

        }

        private IsbnBookLogic _isbnModel;

        public IsbnBookLogic IsbnModel
        {
            get { return _isbnModel; }
            set { this.SetProperty(ref this._isbnModel, value); }
        }

        private InteractionRequest<OpenFileDialogAction.OpenFileDialogActionResult> _openFileDialogRequest;
        public IInteractionRequest OpenFileDialogRequest
        {
            get { return _openFileDialogRequest; }
        }

        private InteractionRequest<Confirmation> _messageBoxRequest;
        public IInteractionRequest MessageBoxRequest
        {
            get { return _messageBoxRequest; }
        }

        /// <summary>
        /// 処理中のマスクの可視性
        /// </summary>
        private Visibility _maskVisibility = Visibility.Collapsed;
        public Visibility MaskVisibility
        {
            get { return _maskVisibility; }
            set { this.SetProperty(ref this._maskVisibility, value); }
        }

        /// <summary>
        /// 画面の操作可否
        /// </summary>
        private bool _isWindowEnabled = true;
        public bool IsWindowEnabled
        {
            get { return _isWindowEnabled; }
            set { this.SetProperty(ref this._isWindowEnabled, value); }
        }

        #region 画面内用の保存

        private void Load()
        {
            try
            {
                if (System.IO.File.Exists(SettingXmlPath))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(IsbnBookLogic));
                    //読み込むファイルを開く
                    using (var sw = new System.IO.StreamReader(SettingXmlPath, new System.Text.UTF8Encoding(false)))
                    {
                        //シリアル化し、XMLファイルに保存する
                        IsbnModel = (IsbnBookLogic)serializer.Deserialize(sw);
                    }
                }
            }
            catch(Exception)
            { }
        }

        private void Save()
        {
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(IsbnBookLogic));
                using (var sw = new System.IO.StreamWriter(SettingXmlPath, false, new System.Text.UTF8Encoding(false)))
                {
                    //シリアル化し、XMLファイルに保存する
                    serializer.Serialize(sw, IsbnModel);
                }
            }
            catch (Exception)
            { }
        }

        #endregion

        #region command

        /// <summary>
        /// 画面ロード完了時に画面内容を復元する
        /// </summary>
        private DelegateCommand _loadCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return this._loadCommand ?? (this._loadCommand = new DelegateCommand(() =>
                {
                    Load();
                }));
            }
        }

        /// <summary>
        /// 画面を閉じる際に内容を保存する
        /// </summary>
        private DelegateCommand<CancelEventArgs> _closingCommand;
        public ICommand ClosingCommand
        {
            get
            {
                return this._closingCommand ?? (this._closingCommand = new DelegateCommand<CancelEventArgs>((e) =>
                {
                    Save();
                    e.Cancel = false;
                }));
            }
        }

        /// <summary>
        /// GostScriptのパス参照コマンド
        /// </summary>
        private DelegateCommand _gostscriptOpenFileCommand;
        public ICommand GostscriptOpenFileCommand
        {
            get
            {
                return this._gostscriptOpenFileCommand ?? (this._gostscriptOpenFileCommand = new DelegateCommand(() =>
                {
                    _openFileDialogRequest.Raise(
                        new OpenFileDialogAction.OpenFileDialogActionResult(),
                        (res) =>
                        {
                            if (!string.IsNullOrWhiteSpace(res.FileName))
                                IsbnModel.GostscriptPath = res.FileName;
                        });
                }));
            }
        }

        /// <summary>
        /// Zbarのパス参照コマンド
        /// </summary>
        private DelegateCommand _zbarImgOpenFileCommand;
        public ICommand ZbarImgOpenFileCommand
        {
            get
            {
                return this._zbarImgOpenFileCommand ?? (this._zbarImgOpenFileCommand = new DelegateCommand(() =>
                {
                    _openFileDialogRequest.Raise(
                        new OpenFileDialogAction.OpenFileDialogActionResult(),
                        (res) =>
                        {
                            if(!string.IsNullOrWhiteSpace(res.FileName))
                                IsbnModel.ZbarimgPath = res.FileName;
                        });
                }));
            }
        }

        private DelegateCommand<DragEventArgs> _dropCommand;
        public ICommand DropCommand
        {
            get
            {
                return this._dropCommand ?? (this._dropCommand = new DelegateCommand<DragEventArgs>(
                    (e) =>
                    {
                        if(_isbnModel.IsCanReadFile())
                        {
                            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                            if (files != null)
                            {
                                var res = _isbnModel.SetFiles(files);
                                if (!res)
                                {
                                    ShowMessage("エラー", "pdfファイルをドラッグしてください。");
                                }
                                else
                                {
                                    SetWindowEnabled(false);
                                    Task.Factory.StartNew(() =>
                                    {
                                        _isbnModel.StartConvert();
                                    }).ContinueWith((a)=>
                                    {
                                        SetWindowEnabled(true);
                                    });

                                }

                            }
                        }
                        else
                        {
                            ShowMessage("エラー", "設定後にドラッグしてください。");
                        }
                    }
                ));
            }
        }


        private DelegateCommand<DragEventArgs> _previewDragOverCommand;
        public ICommand PreviewDragOverCommand
        {
            get {
                return this._previewDragOverCommand ?? (this._previewDragOverCommand = new DelegateCommand<DragEventArgs>((e)=> 
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                        e.Effects = DragDropEffects.Copy;
                    else
                        e.Effects = DragDropEffects.None;
                    e.Handled = true;
                } ));
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

        private void SetWindowEnabled(bool state)
        {
            IsWindowEnabled = state;
            if(state)
            {
                MaskVisibility = Visibility.Collapsed;
            }
            else
            {
                MaskVisibility = Visibility.Visible;
            }
        }
    }
}
