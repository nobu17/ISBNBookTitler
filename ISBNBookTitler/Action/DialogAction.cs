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
    public class DialogAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // イベント引数とContextを取得する
            var args = parameter as InteractionRequestedEventArgs;
            var ctx = args.Context as Confirmation;
            // ContextのConfirmedに結果を格納する
            ctx.Confirmed = MessageBox.Show(args.Context.Content.ToString(), args.Context.Title, MessageBoxButton.OKCancel) == MessageBoxResult.OK;

            // コールバックを呼び出す
            args.Callback();
        }
    }
}
