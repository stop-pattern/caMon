using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
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
        enum panelIndex : int
        {
            Regeneration = 52,  /// 回生
        }

        /// <summary>
        /// 列車種別（一般）
        /// F系統は+30
        /// </summary>
        Dictionary<uint, string> trainKind = new Dictionary<uint, string>
        {
            {0, "" },
            {1, "普　通" },
            {2, "急　行" },
            {3, "快　速" },
            {4, "区間準急" },
            {5, "通勤準急" },
            {6, "準　急" },
            {7, "特　急" },
            {8, "土休急行" },
            {9, "通勤急行" },
            {10, "臨　時" },
            {11, "各　停" },
            {12, "平日急行" },
            {13, "直　通" },
            {14, "快速急行" },
            {15, "ライナー" },
            {16, "通勤特急" },
            {17, "Ｇ各停" },
            {18, "区間急行" },
            {19, "区間快速" },
            {20, "" },
            {31, "Ｆ普通" },
            {32, "Ｆ急行" },
            {33, "Ｆ快速" },
            {34, "Ｆ区間準急" },
            {35, "Ｆ通勤準急" },
            {36, "Ｆ準急" },
            {37, "Ｆ特急" },
            {38, "Ｆ土休急行" },
            {39, "Ｆ通勤急行" },
            {40, "Ｆ臨時" },
            {41, "Ｆ各停" },
            {42, "Ｆ平日急行" },
            {43, "Ｆ直通" },
            {44, "Ｆ快速急行" },
            {45, "Ｆライナー" },
            {46, "Ｆ通勤特急" },
            {47, "ＦＧ各停" },
            {48, "Ｆ区間急行" },
            {49, "Ｆ区間快速" },
            {50, "" },
        };

        /// <summary>
        /// 列車種別（特殊）
        /// 5号線・大井町線用
        /// </summary>
        Dictionary<uint, string> trainKindSpecial = new Dictionary<uint, string>
        {
            {0, "" },
            {1, "普　通" },
            {2, "急　行" },
            {3, "快　速" },
            {4, "通　快" },
            {5, "Ａ快速" },
            {6, "準　急" },
            {7, "東　快" },
            {8, "土休急行" },
            {9, "通勤急行" },
            {10, "試運転" },
            {11, "Ｂ各停" },
            {12, "平日急行" },
            {13, "直　通" },
            {14, "快速急行" },
            {15, "ライナー" },
            {16, "通勤特急" },
            {17, "Ｇ各停" },
            {18, "区間急行" },
            {19, "区間快速" },
            {20, "" },
            {21, "" },
        };

        /// <summary> ループタイマー </summary>
        DispatcherTimer timer = new DispatcherTimer();
        int timerInterval = 300;
        bool BIDSSMemIsEnabled = false;
        int brakeNotch;
        int powerNotch;
        int reverserPosition;
        bool constantSpeed;
        bool door = false;

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

                /// 回生
                Regeneration.Visibility = panel[(int)panelIndex.Regeneration] != 0 ? Visibility.Visible : Visibility.Collapsed;

                /// 定速
                ConstantSpeed.Visibility = constantSpeed ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                /// 接続
                Online.Visibility = Visibility.Collapsed;
                Offline.Visibility = Visibility.Visible;

                /// 回生
                Regeneration.Visibility = Visibility.Collapsed;

                /// 定速
                ConstantSpeed.Visibility = Visibility.Collapsed;
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
