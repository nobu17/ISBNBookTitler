using CommonData;
using ISBNBookTitler.Data;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace ISBNBookTitler
{
    public class OpenFileInfoSettingViewAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // イベント引数とContextを取得する
            var args = parameter as InteractionRequestedEventArgs;
            var ctx = args.Context as OpenFileInfoSettingViewActionResult;

            //ダイアログを表示する
            var view = new FileInfoSettingView();
            var model = new FileInfoSettingViewModel();
            model.FileNameSetting = ctx.FileNameSetting;
            model.Setting = ctx.Setting;
            view.DataContext = model;

            view.ShowDialog();

            var result = view.DataContext as FileInfoSettingViewModel;
            if (result.IsConfirmed)
            {
                ctx.Setting = result.Setting;
                ctx.FileNameSetting = result.FileNameSetting;
            }
            else
            {
                //キャンセル時には何も設定しない
                ctx.Setting = null;
                ctx.FileNameSetting = string.Empty;
            }

            // コールバックを呼び出す
            args.Callback();
        }

        public class OpenFileInfoSettingViewActionResult : Notification, INotification
        {
            public OpenFileInfoSettingViewActionResult() { }

            public WpfPDFFileChangeSetting Setting { get; set; }
            public string FileNameSetting { get; set; }
    }

    }
}
