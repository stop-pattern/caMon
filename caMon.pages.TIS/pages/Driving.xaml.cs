using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;

using TR;
using TR.BIDSSMemLib;

namespace caMon.pages.TIS.pages
{
    /// <summary>
    /// Driving.xaml の相互作用ロジック
    /// </summary>
    public partial class Driving : Page
    {
        /// <summary>
        /// panelのインデックス
        /// </summary>
        enum panelIndex : uint
        {
            OdoMeter1000 = 13,      /// <summary> 駅間走行距離(1kmの桁) </summary>
            OdoMeter100 = 14,       /// <summary> 駅間走行距離(0.1kmの桁) </summary>
            OdoMeter10 = 15,        /// <summary> 駅間走行距離(0.01kmの桁) </summary>
            BrakeNotch = 51,        /// <summary> ブレーキ指令計 </summary>
            Regeneration = 52,      /// <summary> 回生 </summary>
            PowerNotch = 66,        /// <summary> 力行表示灯 </summary>
            Key = 92,               /// <summary> マスコンキー </summary>
            TrainKind = 152,        /// <summary> 列車種別表示 </summary>
            ServiceNumber10 = 153,  /// <summary> 運行番号表示(10の桁) </summary>
            ServiceNumber1 = 154,   /// <summary> 運行番号表示(1の桁) </summary>
            StationNow_old = 167,   /// <summary> 駅名表示(旧方式停車時) </summary>
            StationLast_old = 168,  /// <summary> 駅名表示(旧方式走行時自駅) </summary>
            StationNext_old = 169,  /// <summary> 駅名表示(旧方式走行時至駅) </summary>
            Destination = 172,      /// <summary> 行先表示 </summary>
            StationNow_TOB = 236,   /// <summary> 駅名表示(東武各線停車時) </summary>
            StationLast_TOB = 237,  /// <summary> 駅名表示(東武各線走行時自駅) </summary>
            StationNext_TOB = 238,  /// <summary> 駅名表示(東武各線走行時至駅) </summary>
            StationNow_SEB = 239,   /// <summary> 駅名表示(西武各線停車時) </summary>
            StationLast_SEB = 240,  /// <summary> 駅名表示(西武各線走行時自駅) </summary>
            StationNext_SEB = 241,  /// <summary> 駅名表示(西武各線走行時至駅) </summary>
            StationNow_TKK = 245,   /// <summary> 駅名表示(東急各線停車時) </summary>
            StationLast_TKK = 246,  /// <summary> 駅名表示(東急各線走行時自駅) </summary>
            StationNext_TKK = 247,  /// <summary> 駅名表示(東急各線走行時至駅) </summary>
            StationNow_TRTA = 248,  /// <summary> 駅名表示(営団各線停車時) </summary>
            StationLast_TRTA = 249, /// <summary> 駅名表示(営団各線走行時自駅) </summary>
            StationNext_TRTA = 250, /// <summary> 駅名表示(営団各線走行時至駅) </summary>
            Max = 256               /// <summary> 最大値 </summary>
        }

        /// <summary>
        /// 駅名表示
        /// 
        /// </summary>
        readonly List<Tuple<panelIndex, panelIndex, panelIndex>> stations = new List<Tuple<panelIndex, panelIndex, panelIndex>>
        {
            new Tuple<panelIndex, panelIndex, panelIndex>(panelIndex.StationNow_old, panelIndex.StationLast_old, panelIndex.StationNext_old),
            new Tuple<panelIndex, panelIndex, panelIndex>(panelIndex.StationNow_TOB, panelIndex.StationLast_TOB, panelIndex.StationNext_TOB),
            new Tuple<panelIndex, panelIndex, panelIndex>(panelIndex.StationNow_SEB, panelIndex.StationLast_SEB, panelIndex.StationNext_SEB),
            new Tuple<panelIndex, panelIndex, panelIndex>(panelIndex.StationNow_TKK, panelIndex.StationLast_TKK, panelIndex.StationNext_TKK),
            new Tuple<panelIndex, panelIndex, panelIndex>(panelIndex.StationNow_TRTA, panelIndex.StationLast_TRTA, panelIndex.StationNext_TRTA),
        };

        /// <summary>
        /// 鍵種別
        /// </summary>
        readonly List<String> keyKind = new List<String>
        {
            "", /// 切
            "地下鉄",
            "東武",
            "東急・横高",
            "西武",
            "相鉄",
            "ＪＲ",
            "小田急",
            "東葉"
        };

        /// <summary>
        /// 列車種別（一般）
        /// F系統は+20
        /// </summary>
        readonly List<Tuple<String, Color>> trainKind = new List<Tuple<String, Color>>
        {
            new Tuple<String, Color>("", Colors.White),
            new Tuple<String, Color>("普　通", Colors.White),
            new Tuple<String, Color>("急　行", Colors.Red),
            new Tuple<String, Color>("快　速", Colors.Orange),
            new Tuple<String, Color>("区間準急", Colors.LawnGreen),
            new Tuple<String, Color>("通勤準急", Colors.Red),
            new Tuple<String, Color>("準　急", Colors.LawnGreen),
            new Tuple<String, Color>("特　急", Colors.DeepSkyBlue),
            new Tuple<String, Color>("土休急行", Colors.Red),
            new Tuple<String, Color>("通勤急行", Colors.Red),
            new Tuple<String, Color>("臨　時", Colors.White),
            new Tuple<String, Color>("各　停", Colors.White),
            new Tuple<String, Color>("平日急行", Colors.Red),
            new Tuple<String, Color>("直　通", Colors.LawnGreen),
            new Tuple<String, Color>("快速急行", Colors.Aqua),
            new Tuple<String, Color>("ライナー", Colors.White),
            new Tuple<String, Color>("通勤特急", Colors.Aqua),
            new Tuple<String, Color>("Ｇ各停", Colors.LawnGreen),
            new Tuple<String, Color>("区間急行", Colors.Red),
            new Tuple<String, Color>("区間快速", Colors.Aqua),
            new Tuple<String, Color>("", Colors.White),
            new Tuple<String, Color>("Ｆ普通", Colors.White),
            new Tuple<String, Color>("Ｆ急行", Colors.Red),
            new Tuple<String, Color>("Ｆ快速", Colors.Yellow),
            new Tuple<String, Color>("Ｆ区間準急", Colors.Green),
            new Tuple<String, Color>("Ｆ通勤準急", Colors.Red),
            new Tuple<String, Color>("Ｆ準急", Colors.Green),
            new Tuple<String, Color>("Ｆ特急", Colors.Blue),
            new Tuple<String, Color>("Ｆ土休急行", Colors.Red),
            new Tuple<String, Color>("Ｆ通勤急行", Colors.Red),
            new Tuple<String, Color>("Ｆ臨時", Colors.White),
            new Tuple<String, Color>("Ｆ各停", Colors.White),
            new Tuple<String, Color>("Ｆ平日急行", Colors.Red),
            new Tuple<String, Color>("Ｆ直通", Colors.Green),
            new Tuple<String, Color>("Ｆ快速急行", Colors.Blue),
            new Tuple<String, Color>("Ｆライナー", Colors.White),
            new Tuple<String, Color>("Ｆ通勤特急", Colors.Blue),
            new Tuple<String, Color>("ＦＧ各停", Colors.Green),
            new Tuple<String, Color>("Ｆ区間急行", Colors.Red),
            new Tuple<String, Color>("Ｆ区間快速", Colors.Blue),
            new Tuple<String, Color>("", Colors.White)
        };

        /// <summary>
        /// 列車種別（特殊）
        /// 5号線・大井町線
        /// </summary>
        readonly List<String> trainKindSpecial = new List<String>
        {
            "",
            "普　通",
            "急　行",
            "快　速",
            "通　快",
            "Ａ快速",
            "準　急",
            "東　快",
            "土休急行",
            "通勤急行",
            "試運転",
            "Ｂ各停",
            "平日急行",
            "直　通",
            "快速急行",
            "ライナー",
            "通勤特急",
            "Ｇ各停",
            "区間急行",
            "区間快速",
            "",
        };

        /// <summary>
        /// 列車行先（一般）
        /// 2・8・11・13号線
        /// </summary>
        readonly List<String> trainDestination = new List<String>
        {
            "",
            "浅　草",
            "業平橋",
            "竹ノ塚",
            "北越谷",
            "北春日部",
            "東武動物公園",
            "久　喜",
            "館　林",
            "太　田",
            "南栗橋",
            "新栃木",
            "成　増",
            "志　木",
            "上福岡",
            "川越市",
            "森林公園",
            "小川町",
            "寄　居",
            "池　袋",
            "",
            "豊島園",
            "保　谷",
            "清　瀬",
            "所　沢",
            "西武球場前",
            "小手指",
            "飯　能",
            "西武秩父",
            "西武新宿",
            "田　無",
            "小　平",
            "新所沢",
            "本川越",
            "西武遊園地",
            "拝　島",
            "",
            "和光市",
            "小竹向原",
            "新線池袋",
            "新木場",
            "新宿三丁目",
            "半蔵門",
            "清澄白河",
            "押上",
            "北千住",
            "南千住",
            "霞ヶ関",
            "中目黒",
            "渋　谷",
            "",
            "奥　沢",
            "武蔵小杉",
            "元住吉",
            "日　吉",
            "菊　名",
            "元町・中華街",
            "桜木町",
            "二子玉川",
            "溝の口",
            "大井町",
            "鷺　沼",
            "長津田",
            "中央林間",
            "こどもの国",
            "",
            "新横浜",
            "横　浜",
            "",
            "",
            "",
            "",
            "",
            "",
        };

        /// <summary>
        /// 列車行先（特殊）
        /// 5号線・9号線
        /// </summary>
        readonly List<String> trainDestinationSpecial = new List<String>
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "上　野",
            "松　戸",
            "柏",
            "我孫子",
            "取　手",
            "",
            "",
            "",
            "",
            "唐木田",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "代々木上原",
            "綾　瀬",
            "北綾瀬",
            "中　野",
            "東陽町",
            "妙　典",
            "西船橋",
            "八千代緑が丘",
            "東葉勝田台",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "三　鷹",
            "",
            "",
            "",
            "津田沼",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
        };

        /// <summary>
        /// 走行位置表示駅名
        /// </summary>
        readonly List<List<String>> displayStations = new List<List<String>>
        {
            /// <summary>
            /// ダミー
            /// </summary>
            new List<String>
            {
                "",
            },
            /// <summary>
            /// 東武(10)
            /// </summary>
            new List<String>
            {
                "",
                "",
                "浅草",
                "業平橋",
                "曳舟",
                "東向島",
                "鐘ヶ淵",
                "堀切",
                "牛田",
                "北千住",
                "小菅",
                "五反野",
                "梅島",
                "西新井",
                "竹ノ塚",
                "谷塚",
                "草加",
                "松原団地",
                "新田",
                "蒲生",
                "新越谷",
                "越谷",
                "北越谷",
                "大袋",
                "せんげん台",
                "武里",
                "一ノ割",
                "春日部",
                "北春日部",
                "姫宮",
                "東武動物公園",
                "和戸",
                "久喜",
                "",
                "東武動物公園",
                "杉戸高野台",
                "幸手",
                "南栗橋",
                "",
                "",
                "",
                "",
                "池袋",
                "北池袋",
                "下板橋",
                "大山",
                "中板橋",
                "ときわ台",
                "上板橋",
                "東武練馬",
                "下赤塚",
                "成増",
                "和光市",
                "朝霞",
                "朝霞台",
                "志木",
                "柳瀬川",
                "みずほ台",
                "鶴瀬",
                "ふじみ野",
                "上福岡",
                "新河岸",
                "川越",
                "川越市",
                "霞ヶ関",
                "鶴ヶ島",
                "若葉",
                "坂戸",
                "北坂戸",
                "高坂",
                "東松山",
                "森林公園",
                "つきのわ",
                "武蔵嵐山",
                "小川町",
                "",
                "",
                "",
                "",
                "",
                "",
            },
            /// <summary>
            /// 西武(20)
            /// </summary>
            new List<String>
            {
				"",
                "",
                "小竹向原",
				"新桜台",
				"練馬",
                "",
                "池袋",
				"椎名町",
				"東長崎",
				"江古田",
				"桜台",
				"練馬",
				"中村橋",
				"富士見台",
				"練馬高野台",
				"石神井公園",
				"大泉学園",
				"保谷",
				"ひばりヶ丘",
				"東久留米",
				"清瀬",
				"秋津",
				"所沢",
				"西所沢",
				"小手指",
				"狭山ヶ丘",
				"武蔵藤沢",
				"稲荷山公園",
				"入間市",
				"仏子",
				"元加治",
				"飯能",
                "",
                "",
                "",
                "西武新宿",
				"高田馬場",
				"下落合",
				"中井",
				"新井薬師前",
				"沼袋",
				"野方",
				"都立家政",
				"鷺ノ宮",
				"下井草",
				"井荻",
				"上井草",
				"上石神井",
				"武蔵関",
				"東伏見",
				"西武柳沢",
				"田無",
				"花小金井",
				"小平",
				"久米川",
				"東村山",
				"所沢",
				"航空公園",
				"新所沢",
				"入曽",
				"狭山市",
				"新狭山",
				"南大塚",
				"本川越",
                "",
                "",
                "",
                "小平",
				"萩山",
				"小川",
				"東大和市",
				"玉川上水",
				"武蔵砂川",
				"西武立川",
				"拝島",
                "",
                "",
                "",
                "",
                "",
                "",
            },
            /// <summary>
            /// 東急(50)
            /// 
            /// </summary>
            new List<String>
            {
                "",
                "",
                "渋谷",
                "代官山",
                "中目黒",
                "祐天寺",
                "学芸大学",
                "都立大学",
                "自由が丘",
                "田園調布",
                "多摩川",
                "新丸子",
                "武蔵小杉",
                "元住吉",
                "日吉",
                "綱島",
                "大倉山",
                "菊名",
                "妙蓮寺",
                "白楽",
                "東白楽",
                "反町",
                "横浜",
                "新高島",
                "みなとみらい",
                "馬車道",
                "日本大通",
                "元町・中華街",
                "",
                "長津田",
                "恩田",
                "こどもの国",
                "",
                "横浜",
                "高島町",
                "桜木町",
                "",
                "白金高輪",
                "白金台",
                "目黒",
                "不動前",
                "武蔵小山",
                "西小山",
                "洗足",
                "大岡山",
                "奥沢",
                "田園調布",
                "",
                "渋谷",
                "池尻大橋",
                "三軒茶屋",
                "駒沢大学",
                "桜新町",
                "用賀",
                "二子玉川",
                "二子新地",
                "高津",
                "溝の口",
                "梶が谷",
                "宮崎台",
                "宮前平",
                "鷺沼",
                "たまプラーザ",
                "あざみ野",
                "江田",
                "市が尾",
                "藤が丘",
                "青葉台",
                "田奈",
                "長津田",
                "つくし野",
                "すずかけ台",
                "南町田",
                "つきみ野",
                "中央林間",
                "",
                "",
                "",
                "",
                "",
                "",
            },
            /// <summary>
            /// 営団(40)
            /// 
            /// </summary>
            new List<String>
            {
				"",
                "",
                "和光市",
				"地下鉄成増",
				"地下鉄赤塚",
				"平和台",
				"氷川台",
				"小竹向原",
				"千川（副）",
				"要町（副）",
				"池袋（副）",
				"雑司が谷",
				"西早稲田",
				"東新宿",
				"新宿三丁目",
				"北参道",
				"明治神宮前",
				"渋谷",
                "",
                "小竹向原",
				"千川（有）",
				"要町（有）",
				"池袋（有）",
				"東池袋",
				"護国寺",
				"江戸川橋",
				"飯田橋",
				"市ヶ谷",
				"麹町",
				"永田町",
				"桜田門",
				"有楽町",
				"銀座一丁目",
				"新富町",
				"月島",
				"豊洲",
				"辰巳",
				"新木場",
                "",
                "渋谷",
				"表参道",
				"青山一丁目",
				"永田町",
				"半蔵門",
				"九段下",
				"神保町",
				"大手町",
				"三越前",
				"水天宮前",
				"清澄白河",
				"住吉",
				"錦糸町",
				"押上",
				"曳舟",
                "",
                "北千住",
				"南千住",
				"三ノ輪",
				"入谷",
				"上野",
				"仲御徒町",
				"秋葉原",
				"小伝馬町",
				"人形町",
				"茅場町",
				"八丁堀",
				"築地",
				"東銀座",
				"銀座",
				"日比谷",
				"霞ヶ関",
				"神谷町",
				"六本木",
				"広尾",
				"恵比寿",
				"中目黒",
                "",
                "",
                "",
                "",
                "",
            },
            /// <summary>
            /// 営団(40)
            /// 
            /// </summary>
            new List<String>
            {
				"",
                "",
                "西船橋",
				"船橋",
				"東船橋",
				"津田沼",
				"",
				"",
				"",
				"",
				"",
				"三鷹",
				"吉祥寺",
				"西荻窪",
				"荻窪",
				"阿佐ヶ谷",
				"高円寺",
				"中野",
                "",
                "中野",
				"落合",
				"高田馬場",
				"早稲田",
				"神楽坂",
				"飯田橋",
				"九段下",
				"竹橋",
				"大手町",
				"日本橋",
				"茅場町",
				"門前仲町",
				"木場",
				"東陽町",
				"南砂町",
				"西葛西",
				"葛西",
				"浦安",
				"南行徳",
				"行徳",
				"妙典",
				"原木中山",
				"西船橋",
				"東海神",
				"飯山満",
				"北習志野",
				"船橋日大前",
				"八千代緑が丘",
				"八千代中央",
				"村上",
				"東葉勝田台",
                "",
                "",
                "",
                "",
                "",
                "",
                "北綾瀬",
				"綾瀬",
				"北千住",
				"町屋",
				"西日暮里",
				"千駄木",
				"根津",
				"湯島",
				"新御茶ノ水",
				"大手町",
				"二重橋前",
				"日比谷",
				"霞ヶ関",
				"国会議事堂前",
				"赤坂",
				"乃木坂",
				"表参道",
				"明治神宮前",
				"代々木公園",
				"代々木上原",
                "",
                "",
                "",
                "",
                "",
            },
            /// <summary>
            /// 営団・東急
            /// 7号線
            /// </summary>
            new List<String>
            {
                "",
                "",
                "",
                "",
                "浦和美園",
                "東川口",
                "戸塚安行",
                "新井宿",
                "鳩ケ谷",
                "南鳩ヶ谷",
                "川口元郷",
                "赤羽岩淵",
                "志茂",
                "王子神谷",
                "王子",
                "西ヶ原",
                "駒込",
                "本駒込",
                "東大前",
                "後楽園",
                "飯田橋",
                "市ヶ谷",
                "四ツ谷",
                "永田町",
                "溜池山王",
                "六本木一丁目",
                "麻布十番",
                "白金高輪",
                "白金台",
                "目黒",
                "不動前",
                "武蔵小山",
                "西小山",
                "洗足",
                "大岡山",
                "奥沢",
                "田園調布",
                "多摩川",
                "新丸子",
                "武蔵小杉",
                "元住吉",
                "日吉",
                "綱島",
                "大倉山",
                "菊名",
                "妙蓮寺",
                "白楽",
                "東白楽",
                "反町",
                "横浜",
                "新高島",
                "みなとみらい",
                "馬車道",
                "日本大通",
                "元町・中華街",
            },
            /// <summary>
            /// 営団・東急
            /// 11号線
            /// </summary>
            new List<String>
            {
                "南栗橋",
                "幸手",
                "杉戸高野台",
                "久喜",
                "和戸",
                "東武動物公園",
                "姫宮",
                "北春日部",
                "春日部",
                "一ノ割",
                "武里",
                "せんげん台",
                "大袋",
                "北越谷",
                "越谷",
                "新越谷",
                "草加",
                "西新井",
                "北千住",
                "曳舟",
                "押上",
                "錦糸町",
                "住吉",
                "清澄白河",
                "水天宮前",
                "三越前",
                "大手町",
                "神保町",
                "九段下",
                "半蔵門",
                "永田町",
                "青山一丁目",
                "表参道",
                "渋谷",
                "池尻大橋",
                "三軒茶屋",
                "駒沢大学",
                "桜新町",
                "用賀",
                "二子玉川",
                "二子新地",
                "高津",
                "溝の口",
                "梶が谷",
                "宮崎台",
                "宮前平",
                "鷺沼",
                "たまプラーザ",
                "あざみ野",
                "江田",
                "市が尾",
                "藤が丘",
                "青葉台",
                "田奈",
                "長津田",
                "つくし野",
                "すずかけ台",
                "南町田",
                "つきみ野",
                "中央林間",
            },
            /// <summary>
            /// 東急
            /// 池上線・多摩川線
            /// </summary>
            new List<String>
            {
                "",
                "",
                "",
                "",
                "五反田",
                "大崎広小路",
                "戸越銀座",
                "荏原中延",
                "旗の台",
                "長原",
                "洗足池",
                "石川台",
                "雪が谷大塚",
                "御嶽山",
                "久が原",
                "千鳥町",
                "池上",
                "蓮沼",
                "蒲田",
                "矢口渡",
                "武蔵新田",
                "下丸子",
                "鵜の木",
                "沼部",
                "多摩川",
                "",
                "",
                "",
                "多摩川",
                "沼部",
                "鵜の木",
                "下丸子",
                "武蔵新田",
                "矢口渡",
                "蒲田",
                "蓮沼",
                "池上",
                "千鳥町",
                "久が原",
                "御嶽山",
                "雪が谷大塚",
                "石川台",
                "洗足池",
                "長原",
                "旗の台",
                "荏原中延",
                "戸越銀座",
                "大崎広小路",
                "五反田",
            },
        };

        DispatcherTimer timer = new DispatcherTimer();  /// <summary> ループタイマー </summary>
        int timerInterval = 10;
        bool BIDSSMemIsEnabled = false;
        int brakeNotch;
        int powerNotch;
        int reverserPosition;
        bool constantSpeed;
        bool door = false;
        Spec spec;

        List<int> panel = new List<int>();
        List<int> sound = new List<int>();


        public Driving()
        {
            InitializeComponent();

            SharedFuncs.SML.SMC_BSMDChanged += SMemLib_BIDSSMemChanged;
            SharedFuncs.SML.SMC_OpenDChanged += SMemLib_OpenChanged;
            SharedFuncs.SML.SMC_PanelDChanged += SMemLib_PanelChanged;
            SharedFuncs.SML.SMC_SoundDChanged += SMemLib_SoundChanged;

            panel = new List<int>();
            sound = new List<int>();

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
        }

        /// <summary> 
        /// SharedMemに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_BIDSSMemChanged(object sender, ValueChangedEventArgs<BIDSSharedMemoryData> e)
        {
            BIDSSMemIsEnabled = e.NewValue.IsEnabled;

            brakeNotch = e.NewValue.HandleData.B;
            powerNotch = e.NewValue.HandleData.P;
            reverserPosition = e.NewValue.HandleData.R;
            switch (e.NewValue.HandleData.C)
            {
                case 1:
                    constantSpeed = true;
                    break;
                case 2:
                    constantSpeed = false;
                    break;
                case 0:
                default:
                    break;
            }

            spec = e.NewValue.SpecData;

            door = e.NewValue.IsDoorClosed;
        }

        /// <summary> 
        /// Openに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_OpenChanged(object sender, ValueChangedEventArgs<OpenD> e)
        {
        }

        /// <summary> 
        /// Panelに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_PanelChanged(object sender, ValueChangedEventArgs<int[]> p)
        {
            panel = p.NewValue.ToList();
        }

        /// <summary> 
        /// Soundに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_SoundChanged(object sender, ValueChangedEventArgs<int[]> s)
        {
            sound = s.NewValue.ToList();
        }

        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            if (BIDSSMemIsEnabled)
            {
                /// 接続
                Online.Visibility = Visibility.Visible;
                Offline.Visibility = Visibility.Collapsed;

                /// ハンドル
                String handle = "MNU - ";
                if (panel[(int)panelIndex.Key] == 0) handle = "　　――　　";
                else
                {
                    if (panel[(int)panelIndex.BrakeNotch] == 0) handle = panel[(int)panelIndex.PowerNotch] == 0 ? "MNU - OFF" : "MNU - P" + panel[(int)panelIndex.PowerNotch].ToString();
                    else handle = panel[(int)panelIndex.BrakeNotch] == 9 /*spec.B + 1*/ ? "MNU - EB" : "MNU - B" + panel[(int)panelIndex.BrakeNotch].ToString();
                }
                Handle.Text = handle;

                /// 回生
                Regeneration.Visibility = panel[(int)panelIndex.Regeneration] != 0 ? Visibility.Visible : Visibility.Collapsed;

                /// 定速
                ConstantSpeed.Visibility = constantSpeed ? Visibility.Visible : Visibility.Collapsed;

                /// マスコンキー
                KeyDisplay.Visibility = Visibility.Visible;
                Key.Text = keyKind[panel[(int)panelIndex.Key]];

                /// 区間
                for (int i = 0; i < stations.Count; i++)
                {
                    if (panel[(int)stations[i].Item1] != 0)     /// 停車表示
                    {
                        StationNow.Text = displayStations[i][panel[(int)stations[i].Item1]];
                        StationNow.Visibility = Visibility.Visible;
                        StationLast.Visibility = Visibility.Collapsed;
                        StationNext.Visibility = Visibility.Collapsed;
                        StationArrow.Visibility = Visibility.Collapsed;
                    }
                    else                                        /// 走行表示
                    {
                        StationLast.Text = displayStations[i][panel[(int)stations[i].Item2]]; ;
                        StationNext.Text = displayStations[i][panel[(int)stations[i].Item3]]; ;
                        StationNow.Visibility = Visibility.Collapsed;
                        StationLast.Visibility = Visibility.Visible;
                        StationNext.Visibility = Visibility.Visible;
                        StationArrow.Visibility = Visibility.Visible;
                    }
                }

                /// キロ程
                String odo = panel[(int)panelIndex.OdoMeter1000].ToString() + "　.　" +
                    panel[(int)panelIndex.OdoMeter100].ToString() + "　" +
                    panel[(int)panelIndex.OdoMeter10].ToString() + "　" + "km";
                OdoMeter.Text = odo;

                /// 行先
                String dist = trainDestination[panel[(int)panelIndex.Destination]];
                if (dist != "")
                {
                    DestinationDisplay.Visibility = Visibility.Visible;
                    Destination.Text = dist;
                }
                else DestinationDisplay.Visibility = Visibility.Collapsed;

                /// 運行番号
                ServiceNumber.Visibility = Visibility.Visible;
                ServiceNumber.Text = panel[(int)panelIndex.ServiceNumber10].ToString() + panel[(int)panelIndex.ServiceNumber1].ToString() + "K";

                /// 種別
                TrainKind.Visibility = Visibility.Visible;
                TrainKindText.Text = trainKind[panel[(int)panelIndex.TrainKind]].Item1;
                TrainKindText.Foreground = new SolidColorBrush(trainKind[panel[(int)panelIndex.TrainKind]].Item2);
            }
            else
            {
                /// 接続
                Online.Visibility = Visibility.Collapsed;
                Offline.Visibility = Visibility.Visible;

                /// ハンドル
                Handle.Text = "　　――　　";

                /// 回生
                Regeneration.Visibility = Visibility.Collapsed;

                /// 定速
                ConstantSpeed.Visibility = Visibility.Collapsed;

                /// マスコンキー
                KeyDisplay.Visibility = Visibility.Collapsed;

                /// 区間
                StationNow.Visibility = Visibility.Collapsed;
                StationLast.Visibility = Visibility.Collapsed;
                StationNext.Visibility = Visibility.Collapsed;
                StationArrow.Visibility = Visibility.Collapsed;

                /// キロ程
                OdoMeter.Text = "0　.　0　0　km";

                /// 行先
                DestinationDisplay.Visibility = Visibility.Collapsed;

                /// 運行番号
                ServiceNumber.Visibility = Visibility.Collapsed;

                /// 種別
                TrainKind.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// タイマー更新
        /// </summary>
        private void TimerStart()
        {
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
        }
    }
}
