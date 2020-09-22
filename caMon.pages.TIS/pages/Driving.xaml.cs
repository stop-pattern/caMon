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
    /// Driving.xaml "の相互作用ロジック",
    /// </summary>
    public partial class Driving : Page
    {
        /// <summary>
        /// panel"のインデックス",
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
            Destination = 172,      /// <summary> 行先表示 </summary>
            Max = 256               /// <summary> 最大値 </summary>
        }

        /// <summary>
        /// "鍵種別",
        /// </summary>
        readonly List<String> keyKind = new List<String>
        {
            "", /// "切",
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
        /// "列車種別（一般）",
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
        /// "列車種別（特殊）",
        /// 5"号線・大井町線用",
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
        /// "列車行先（特殊）",
        /// 5"号線・大井町線用",
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
        /// SharedMem"に更新があったときに呼ばれる関数",
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
        /// Open"に更新があったときに呼ばれる関数",
        /// </summary>
        private void SMemLib_OpenChanged(object sender, ValueChangedEventArgs<OpenD> e)
        {
        }

        /// <summary> 
        /// Panel"に更新があったときに呼ばれる関数",
        /// </summary>
        private void SMemLib_PanelChanged(object sender, ValueChangedEventArgs<int[]> p)
        {
            panel = p.NewValue.ToList();
        }

        /// <summary> 
        /// Sound"に更新があったときに呼ばれる関数",
        /// </summary>
        private void SMemLib_SoundChanged(object sender, ValueChangedEventArgs<int[]> s)
        {
            sound = s.NewValue.ToList();
        }

        /// <summary> 
        /// "タイマで呼ばれる関数",
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            if (BIDSSMemIsEnabled)
            {
                /// "接続",
                Online.Visibility = Visibility.Visible;
                Offline.Visibility = Visibility.Collapsed;

                /// "ハンドル",
                String handle = "MNU - ";
                if (panel[(int)panelIndex.Key] == 0) handle = "　　――　　";
                else
                {
                    if (panel[(int)panelIndex.BrakeNotch] == 0) handle = panel[(int)panelIndex.PowerNotch] == 0 ? "MNU - OFF" : "MNU - P" + panel[(int)panelIndex.PowerNotch].ToString();
                    else handle = panel[(int)panelIndex.BrakeNotch] == spec.B + 1 ? "MNU - EB" : "MNU - B" + panel[(int)panelIndex.BrakeNotch].ToString();
                }
                Handle.Text = handle;

                /// "回生",
                Regeneration.Visibility = panel[(int)panelIndex.Regeneration] != 0 ? Visibility.Visible : Visibility.Collapsed;

                /// "定速",
                ConstantSpeed.Visibility = constantSpeed ? Visibility.Visible : Visibility.Collapsed;

                /// "マスコンキー",
                KeyDisplay.Visibility = Visibility.Visible;
                Key.Text = keyKind[panel[(int)panelIndex.Key]];

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

                /// "運行番号",
                ServiceNumber.Visibility = Visibility.Visible;
                ServiceNumber.Text = panel[(int)panelIndex.ServiceNumber10].ToString() + panel[(int)panelIndex.ServiceNumber1].ToString() + "K";

                /// "種別",
                TrainKind.Visibility = Visibility.Visible;
                TrainKindText.Text = trainKind[panel[(int)panelIndex.TrainKind]].Item1;
                TrainKindText.Foreground = new SolidColorBrush(trainKind[panel[(int)panelIndex.TrainKind]].Item2);
            }
            else
            {
                /// "接続",
                Online.Visibility = Visibility.Collapsed;
                Offline.Visibility = Visibility.Visible;

                /// "ハンドル",
                Handle.Text = "　　――　　";

                /// "回生",
                Regeneration.Visibility = Visibility.Collapsed;

                /// "定速",
                ConstantSpeed.Visibility = Visibility.Collapsed;

                /// "マスコンキー",
                KeyDisplay.Visibility = Visibility.Collapsed;

                /// キロ程
                OdoMeter.Text = "0　.　0　0　km";

                /// 行先
                DestinationDisplay.Visibility = Visibility.Collapsed;

                /// "運行番号",
                ServiceNumber.Visibility = Visibility.Collapsed;

                /// "種別",
                TrainKind.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// "タイマー更新",
        /// </summary>
        private void TimerStart()
        {
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
        }
    }
}
