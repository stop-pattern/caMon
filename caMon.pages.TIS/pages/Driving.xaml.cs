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
            Regeneration = 52,      /// <summary> 回生 </summary>
            Key = 92,               /// <summary> マスコンキー </summary>
            TrainKind = 152,        /// <summary> 列車種別表示 </summary>
            ServiceNumber10 = 153,  /// <summary> 運行番号表示(10の桁) </summary>
            ServiceNumber1 = 154,   /// <summary> 運行番号表示(1の桁) </summary>
            Max = 256               /// <summary> 最大値 </summary>
        }

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
        /// 5号線・大井町線用
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


        DispatcherTimer timer = new DispatcherTimer();  /// <summary> ループタイマー </summary>
        int timerInterval = 300;
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
                    if (brakeNotch == 0) handle = powerNotch == 0 ? "MNU - OFF" : "MNU - P" + powerNotch.ToString();
                    else handle = brakeNotch == spec.B + 1 ? "MNU - EB" : "MNU - B" + brakeNotch.ToString();
                }
                Handle.Text = handle;

                /// 回生
                Regeneration.Visibility = panel[(int)panelIndex.Regeneration] != 0 ? Visibility.Visible : Visibility.Collapsed;

                /// 定速
                ConstantSpeed.Visibility = constantSpeed ? Visibility.Visible : Visibility.Collapsed;

                /// マスコンキー
                KeyDisplay.Visibility = Visibility.Visible;
                Key.Text = keyKind[panel[(int)panelIndex.Key]];

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
