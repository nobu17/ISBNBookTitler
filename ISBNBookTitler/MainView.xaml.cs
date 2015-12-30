using BookTitleGetter;
using ISBNGetter;
using PDFExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISBNBookTitler
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            //this.DataContext = new IsbnBookLogic();

            //var outDir = @"C:\Users\nobu17\Music\FILES";
            //var df = new GostScriptPDFtoJPG();
            //df.GsExePath = @"C:\Program Files\gs\gs9.18\bin\gswin64c.exe";
            //df.ExtractJpgFromPdf(@"C:\Program Files\gs\gs9.18\bin\input.pdf", outDir, PageMode.Both, 3);

            //var files = Directory.GetFiles(outDir, "*.jpg");
            //var isbn = new IsbnGetFromZBarImage();
            //isbn.ZBarExePath = @"C:\Program Files (x86)\ZBar\bin\zbarimg.exe";
            //foreach (var file in files)
            //{
            //    var re = isbn.GetIsbn(file);
            //    if(!string.IsNullOrWhiteSpace(re))
            //    {
            //        var utl = new AmazonBookInfoGet();

            //        utl.GetBookInfo(re);
            //    }

            //}


        }

    }
}
