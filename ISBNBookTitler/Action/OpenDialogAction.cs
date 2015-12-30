using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;

namespace ISBNBookTitler
{
    public class OpenFileDialogAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // イベント引数とContextを取得する
            var args = parameter as InteractionRequestedEventArgs;
            var ctx = args.Context as OpenFileDialogActionResult;
            // ContextのConfirmedに結果を格納する
            var fDialog = new OpenFileDialog();
            fDialog.Filter = "exeファイル(*.exe)|*.exe";

            //ダイアログを表示する
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                ctx.FileName = fDialog.FileName;
            }

            // コールバックを呼び出す
            args.Callback();
        }

        public class OpenFileDialogActionResult : Notification, INotification
        {
            public OpenFileDialogActionResult() { FileName = string.Empty; }

            public string FileName { get; set; }
        }

    }
}
