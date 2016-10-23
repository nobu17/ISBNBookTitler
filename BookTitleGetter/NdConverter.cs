﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTitleGetter
{
    public class NdcConverter
    {
        private static Dictionary<string, string> CodeList = new Dictionary<string, string>()
        {
            { "00","総記" },
            { "01","図書館、図書館学" },
            { "02","図書、書誌学" },
            { "03","図書館、百科事典" },
            { "04","一般論文集、一般講演集" },
            { "05","逐次刊行物" },
            { "06","団体" },
            { "07","ジャーナリズム、新聞" },
            { "08","叢書、全集、選集" },
            { "09","貴重書、郷土資料、その他の特別コレクション" },

            { "10","哲学" },
            { "11","哲学各論" },
            { "12","東洋思想" },
            { "13","西洋哲学" },
            { "14","心理学" },
            { "15","倫理学、道徳" },
            { "16","宗教" },
            { "17","神道" },
            { "18","仏教" },
            { "19","キリスト教" },

            { "20","歴史" },
            { "21","日本史" },
            { "22","アジア史、東洋史" },
            { "23"," ヨーロッパ史、西洋史" },
            { "24","アフリカ史" },
            { "25","北アメリカ史" },
            { "26","南アメリカ史" },
            { "27","オセアニア史、両極地方史" },
            { "28","伝記" },
            { "29","地理、地誌、紀行" },

            { "30","社会科学" },
            { "31","政治" },
            { "32","法律" },
            { "33","経済" },
            { "34","財政" },
            { "35","統計" },
            { "36","社会" },
            { "37","教育" },
            { "38","風俗習慣、民俗学、民族学" },
            { "39","国防、軍事" },

            { "40","自然科学" },
            { "41","数学" },
            { "42","物理学" },
            { "43","化学" },
            { "44","天文学、宇宙科学" },
            { "45","地球科学、地学" },
            { "46","生物科学、一般生物学" },
            { "47","植物学" },
            { "48","動物学　" },
            { "49","医学、薬学" },

            { "50","技術、工学" },
            { "51","建設工学、土木工事" },
            { "52","建築学" },
            { "53","機械工学、原子力工学" },
            { "54","電気工学、電子工学" },
            { "55","海洋工学、船舶工学、兵器" },
            { "56","金属工学、鉱山工学" },
            { "57","化学工業" },
            { "58","製造工業" },
            { "59","家政学、生活科学" },

            { "60","産業" },
            { "61","農業" },
            { "62","園芸" },
            { "63","蚕糸業" },
            { "64","畜産業、獣医学" },
            { "65","林業" },
            { "66","水産業" },
            { "67","商業" },
            { "68","運輸、交通" },
            { "69","通信事業" },

            { "70","芸術、美術" },
            { "71","彫刻" },
            { "72","絵画、書道" },
            { "726","コミック" },
            { "73","版画" },
            { "74","写真、印刷" },
            { "75","工芸" },
            { "76","音楽、舞踊" },
            { "77","演劇、映画" },
            { "78","スポーツ、体育" },
            { "79","諸芸、娯楽" },

            { "80","言語" },
            { "81","日本語" },
            { "82","中国語、その他の東洋の諸言語" },
            { "83","英語" },
            { "84","ドイツ語" },
            { "85","フランス語" },
            { "86","スペイン語" },
            { "87","イタリア語" },
            { "88","ロシア語" },
            { "89","その他の諸言語" },

            { "90","文学" },
            { "91","日本文学" },
            { "92","中国文学、その他の東洋文学" },
            { "93","英米文学" },
            { "94","ドイツ文学" },
            { "95","フランス文学" },
            { "96","スペイン文学" },
            { "97","イタリア文学" },
            { "98","ロシア、ソヴィエト文学" },
            { "99","その他の諸文学" },
        };

        public static string GetNdcName(string code)
        {
            try
            {
                var threeCode = new string(code.Take(3).ToArray());
                var name = CodeList.FirstOrDefault(x => threeCode.StartsWith(x.Key));
                if (string.IsNullOrEmpty(name.Value))
                {
                    var twoCode = new string(code.Take(2).ToArray());
                    name = CodeList.FirstOrDefault(x => twoCode.StartsWith(x.Key));
                }
                return name.Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
    }
}
