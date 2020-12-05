using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using TR;
using TR.BIDSSMemLib;

namespace caMon.pages.TIS.pages.indicator
{
    /// <summary>
    /// SEB.xaml の相互作用ロジック
    /// </summary>
    public partial class SEB : Page
    {
        /// <summary>
        /// 列車種別
        /// https://twitter.com/MV__74/status/927453018663948288
        /// </summary>
        readonly Dictionary<uint, string> TrainKinds = new Dictionary<uint, string>()
        {
            { 0, " ―― " },
            { 1, "回送" },
            { 2, "快急" },
            { 3, "" },
            { 4, "急行" },
            { 5, "快速" },
            { 6, "" },
            { 7, "" },
            { 8, "準急" },
            { 9, "特急" },
            { 10, "地下快急" },  // Fライナー快急
            { 11, "" },
            { 12, "" },
            { 13, "" },
            { 14, "" },
            { 15, "各停" },
            //{ , "通勤急行" },
            //{ , "通勤準急" },
            //{ , "区間準急" },
            //{ , "通勤快速" },
            //{ , "貨物" },
            { 80, "臨時A" },  // 推測
            { 81, "臨時B" },
            { 82, "臨時C" },  // 推測
            { 83, "臨時D" },  // 推測
            { 84, "臨時E" },  // 推測
            { 85, "臨時F" },  // 推測
            { 86, "臨時G" },  // 推測
            { 90, "回A" },   // 推測
            { 91, "回B" },
            { 92, "回C" },   // 推測
            { 93, "回D" },   // 推測
            { 94, "回E" },   // 推測
            { 95, "回F" },   // 推測
            { 96, "回G" },
        };

        /// <summary>
        /// 列車行先
        /// https://twitter.com/MV__74/status/927453048980488192
        /// </summary>
        readonly Dictionary<uint, string> TrainDestinations = new Dictionary<uint, string>()
        {
            { 0, " ―― " },
            { 100, "池袋" },
            //{ , "東長崎" },
            { 110, "練馬" },  // 推測
            //{ , "練馬高野台" },
            { 115, "石神井公園" },
            { 120, "保谷" },
            { 121, "ひばりヶ丘" },
            { 130, "清瀬" },
            { 140, "所沢" },  // 推測
            { 145, "西所沢" },
            { 150, "小手指" },
            //{ , "狭山ヶ丘" },
            { 154, "入間市" },
            { 155, "仏子" },
            { 160, "飯能" },
            { 165, "武蔵丘" },
            //{ , "高麗" },
            //{ , "吾野" },
            { 179, "横瀬" },
            { 180, "西武秩父" },
            { 200, "豊島園" },
            //{ , "下山口" },
            { 215, "西武球場前" },
            //{ , "小竹向原" },
            //{ , "池袋(Y)" },
            //{ , "豊洲" },
            { 300, "新木場" },
            //{ , "池袋(F)" },
            //{ , "新宿三丁目" },
            //{ , "渋谷" },
            //{ , "武蔵小杉" },
            //{ , "元住吉" },
            //{ , "菊名" },
            //{ , "横浜" },
            { 340, "元町・中華街" },
            { 400, "西武新宿" },
            { 410, "" },
            { 420, "上石神井" },
            //{ , "" },
            { 425, "田無" },
            { 430, "" },
            { 435, "東村山" },
            { 440, "所沢" },
            { 450, "南入曽" },
            //{ , "狭山市" },
            //{ , "南大塚" },
            { 460, "本川越" },
            { 530, "国分寺" },
            { 540, "小川" },  // 推測
            { 550, "拝島" },
            //{ , "武蔵境" },
            //{ , "是政" },
            //{ , "白糸台" },
        };
        
        /// <summary>
        /// 列車種別（一般）
        /// F系統は+20
        /// </summary>
        readonly List<Tuple<String, Color>> TrainKind = new List<Tuple<String, Color>>
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
        /// 列車行先（一般）
        /// 2・8・11・13号線
        /// </summary>
        readonly List<String> TrainDestination = new List<String>
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


        /// <summary>ループタイマー</summary>
        static readonly DispatcherTimer timer = new DispatcherTimer();
        /// <summary>ループ間隔[ms]</summary>
        readonly int timerInterval = 10;
        /// <summary>BIDS Shared Memoryの状態</summary>
        bool BIDSSMemIsEnabled = false;
        /// <summary>Bve5から渡される情報</summary>
        BIDSSharedMemoryData bve5;
        /// <summary>panelの状態</summary>
        public static List<int> panel = new List<int>();


        public SEB()
        {
            InitializeComponent();

            SharedFuncs.SML.SMC_BSMDChanged += SMemLib_BIDSSMemChanged;
            SharedFuncs.SML.SMC_PanelDChanged += SMemLib_PanelChanged;

            panel = new List<int>();

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
            bve5 = e.NewValue;
        }

        /// <summary> 
        /// Panelに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_PanelChanged(object sender, ValueChangedEventArgs<int[]> p)
        {
            panel = new List<int>(p.NewValue);
        }

        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            if (BIDSSMemIsEnabled && panel?.Count > 0)
            {
                switch (panel[192])
                {
                    default:
                    case 0: // 範囲外
                        PlatformDoorGreen.Status = true;
                        PlatformDoorGreen.Visibility = Visibility.Visible;
                        PlatformDoorWhite.Visibility = Visibility.Hidden;
                        break;
                    case 1: // 範囲内
                        PlatformDoorGreen.Status = !toBool(panel[181]);
                        PlatformDoorGreen.Visibility = Visibility.Visible;
                        PlatformDoorWhite.Visibility = Visibility.Hidden;
                        break;
                    case 2: // ドア開
                        PlatformDoorGreen.Visibility = Visibility.Hidden;
                        PlatformDoorWhite.Visibility = Visibility.Visible;
                        break;
                }

                AtsNormal.Status = toBool(panel[46]);
                Braking.Status = toBool(panel[47]);
                Stop.Status = toBool(panel[253]);
                Failure.Status = false;
                SpeedLimit.Status = toBool(panel[49]);
                Confirm.Status = toBool(panel[48]);
                EmerBrake.Status = (panel[55] == 9);

                AtcService.Status = toBool(panel[26]);
                AtcEmergency.Status = toBool(panel[25]);
                EmrDrive.Status = false;
                Atc.Status = toBool(panel[20]);
                Inside.Status = toBool(panel[33]);
                NotInstitutionalized.Status = toBool(panel[30]);

                PlatformDoorInterlocking.Status = panel[155] == 1 ? true : false;
                PlatformDoorNotInterlocking.Status = panel[155] == 2 ? true : false;

                CrimpingBrake.Status = toBool(panel[176]);
                //NonRegenerative.Status = toBool(panel[171]);    //廃止
                HighBeam.Status = toBool(panel[50]);

                Tasc.Status = toBool(panel[136]);
                TascControl.Status = toBool(panel[138]);

                trainKind.Text = TrainKind[panel[152]].Item1; // 種別
                trainDestination.Text = TrainDestination[panel[172]]; // 行先
                int num = panel[62] * 1000 + panel[63] * 100 + panel[64] * 10 + panel[65];
                trainNumber.Text = num.ToString();
            }
        }

        /// <summary>
        /// int -> bool
        /// </summary>
        /// <param name="arg"><int>input</param>
        /// <returns></returns>
        private bool toBool(int arg)
        {
            if (arg == 0) { return false; }
            return true;
        }
    }
}
