using CommonData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler.Data
{
    public class WpfPDFFileChangeSetting : ValidatableBindableBase
    {
        public WpfPDFFileChangeSetting()
        {
            Title = "@[t]";
            Author = "@[a]";
            SubTitle = "";
            CreationDate = "@[d]";
            Keyword = "";
        }

        /// <summary>
        /// タイトル
        /// </summary>
        private string _title;
        public string Title
        {
            get { return _title; }
            set { this.SetProperty(ref this._title, value); }
        }

        /// <summary>
        /// 作成者
        /// </summary>
        private string _author;
        public string Author
        {
            get { return _author; }
            set { this.SetProperty(ref this._author, value); }
        }

        /// <summary>
        /// サブタイトル
        /// </summary>
        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { this.SetProperty(ref this._subTitle, value); }
        }


        /// <summary>
        /// 作成日
        /// </summary>
        private string _creationDate;
        public string CreationDate
        {
            get { return _creationDate; }
            set { this.SetProperty(ref this._creationDate, value); }
        }

        /// <summary>
        /// キーワード
        /// </summary>
        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }
            set { this.SetProperty(ref this._keyword, value); }
        }


        public PDFFileChangeSetting GetChangeSetting()
        {
            var data = new PDFFileChangeSetting();
            data.Author = this.Author;
            data.Title = this.Title;
            data.SubTitle = this.SubTitle;
            data.CreationDate = this.CreationDate;
            data.Keyword = this.Keyword;

            return data;
        }
    }
}
