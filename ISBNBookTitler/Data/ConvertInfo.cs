using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler.Data
{
    public class ConvertInfo
    {
        public string BeforeFileName { get; set; }

        public string AfterFileName { get; set; }

        public string Message { get; set; }

        public bool IsReaNameSuccess { get; set; }

        public bool IsFileInfoChangeSuccess { get; set; }

        public string IsReNameSuccessString
        {
            get
            {
                return GetResultString(IsReaNameSuccess);
            }
        }

        public string IsFileInfoChangeSuccessString
        {
            get
            {
                return GetResultString(IsFileInfoChangeSuccess);
            }
        }

        private static string GetResultString(bool isSuccess)
        {
            if (isSuccess)
            {
                return "成功";
            }
            else
            {
                return "失敗";
            }
        }
    }
}
