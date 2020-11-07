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

using System.Linq;
using System.Windows.Threading;

using TR;
using TR.BIDSSMemLib;

namespace caMon.pages.TIS.pages.driving
{
    /// <summary>
    /// TKK.xaml の相互作用ロジック
    /// </summary>
    public partial class TKK : Page
    {

        /// <summary>
        /// panelのインデックス
        /// </summary>
        enum panelIndex : uint
        {
            BrakeNotch = 51,        /// <summary> ブレーキ指令計 </summary>
            Regeneration = 52,      /// <summary> 回生 </summary>
            PowerNotch = 66,        /// <summary> 力行表示灯 </summary>
            Key = 92,               /// <summary> マスコンキー </summary>
            TascC = 138,            /// <summary> TASC制御 </summary>
            TrainKind = 152,        /// <summary> 列車種別表示 </summary>
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


        readonly DispatcherTimer timer = new DispatcherTimer();  /// <summary> ループタイマー </summary>
        const int timerInterval = 10;
        bool BIDSSMemIsEnabled = false;
        bool IsDoorClosed = false;
        bool DoorFlag = false;
        Spec spec;

        List<int> panel = new List<int>();
        List<int> sound = new List<int>();
        public TKK()
        {
            InitializeComponent();

            SharedFuncs.SML.SMC_BSMDChanged += SMemLib_BIDSSMemChanged;
            SharedFuncs.SML.SMC_OpenDChanged += SMemLib_OpenChanged;
            SharedFuncs.SML.SMC_PanelDChanged += SMemLib_PanelChanged;
            SharedFuncs.SML.SMC_SoundDChanged += SMemLib_SoundChanged;

            DoorFlag = true;

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

            if (e.OldValue.IsDoorClosed != e.NewValue.IsDoorClosed)
            {
                DoorFlag = true;
                IsDoorClosed = e.NewValue.IsDoorClosed;
            }

            spec = e.NewValue.SpecData;
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
            if (BIDSSMemIsEnabled && panel?.Count > 0/* && sound?.Count > 0*/)
            {
                /// ハンドル
                String handle;
                if (panel[(int)panelIndex.Key] == 0) handle = "　　――　　";
                else
                {
                    if (panel[(int)panelIndex.BrakeNotch] == 0) handle = panel[(int)panelIndex.PowerNotch] == 0 ? "MNU - OFF" : "MNU - P" + panel[(int)panelIndex.PowerNotch].ToString();
                    else handle = panel[(int)panelIndex.BrakeNotch] == 9 /*spec.B + 1*/ ? "MNU - EB" : "MNU - B" + panel[(int)panelIndex.BrakeNotch].ToString();
                }
                Handle.Text = handle;

                /// TASC
                TascBrake.Visibility = panel[(int)panelIndex.TascC] != 0 ?  Visibility.Visible : Visibility.Collapsed;

                /// マスコンキー
                KeyDisplay.Visibility = Visibility.Visible;
                Key.Text = keyKind[panel[(int)panelIndex.Key]];

                /// 種別
                TrainKind.Visibility = Visibility.Visible;
                TrainKindText.Text = trainKind[panel[(int)panelIndex.TrainKind]].Item1;
                TrainKindText.Foreground = new SolidColorBrush(trainKind[panel[(int)panelIndex.TrainKind]].Item2);

                /// ドア
                if (DoorFlag)
                {
                    doorl0.Background = GetBrush(IsDoorClosed);
                    doorl1.Background = GetBrush(IsDoorClosed);
                    doorl2.Background = GetBrush(IsDoorClosed);
                    doorl3.Background = GetBrush(IsDoorClosed);
                    doorl4.Background = GetBrush(IsDoorClosed);
                    doorl5.Background = GetBrush(IsDoorClosed);
                    doorl6.Background = GetBrush(IsDoorClosed);
                    doorl7.Background = GetBrush(IsDoorClosed);
                    doorl8.Background = GetBrush(IsDoorClosed);
                    doorl9.Background = GetBrush(IsDoorClosed);
                    doorr0.Background = GetBrush(IsDoorClosed);
                    doorr1.Background = GetBrush(IsDoorClosed);
                    doorr2.Background = GetBrush(IsDoorClosed);
                    doorr3.Background = GetBrush(IsDoorClosed);
                    doorr4.Background = GetBrush(IsDoorClosed);
                    doorr5.Background = GetBrush(IsDoorClosed);
                    doorr6.Background = GetBrush(IsDoorClosed);
                    doorr7.Background = GetBrush(IsDoorClosed);
                    doorr8.Background = GetBrush(IsDoorClosed);
                    doorr9.Background = GetBrush(IsDoorClosed);
                    DoorFlag = false;
                }
            }
            else
            {
                /// ハンドル
                Handle.Text = "　　――　　";

                /// TASC
                TascBrake.Visibility = Visibility.Collapsed;

                /// マスコンキー
                KeyDisplay.Visibility = Visibility.Collapsed;

                /// 種別
                TrainKind.Visibility = Visibility.Collapsed;

                /// ドア
                Brush doorClose = new SolidColorBrush(Colors.White);
                doorl0.Background = GetBrush(true);
                doorl1.Background = GetBrush(true);
                doorl2.Background = GetBrush(true);
                doorl3.Background = GetBrush(true);
                doorl4.Background = GetBrush(true);
                doorl5.Background = GetBrush(true);
                doorl6.Background = GetBrush(true);
                doorl7.Background = GetBrush(true);
                doorl8.Background = GetBrush(true);
                doorl9.Background = GetBrush(true);
                doorr0.Background = GetBrush(true);
                doorr1.Background = GetBrush(true);
                doorr2.Background = GetBrush(true);
                doorr3.Background = GetBrush(true);
                doorr4.Background = GetBrush(true);
                doorr5.Background = GetBrush(true);
                doorr6.Background = GetBrush(true);
                doorr7.Background = GetBrush(true);
                doorr8.Background = GetBrush(true);
                doorr9.Background = GetBrush(true);
            }
        }

        /// <summary>
        /// ドア色変更
        /// </summary>
        /// <param name="arg">isDoorClosed?</param>
        /// <returns></returns>
        Brush GetBrush(bool arg)
        {
            Brush doorOpen = new SolidColorBrush(Colors.HotPink);
            Brush doorClose = new SolidColorBrush(Colors.White);
            return arg ? doorClose : doorOpen;
        }
    }
}
