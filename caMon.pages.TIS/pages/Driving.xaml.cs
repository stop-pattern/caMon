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
        enum panelIndex : int
        {
            Regeneration = 52,  /// 回生
        }

        /// <summary> ループタイマー </summary>
        DispatcherTimer timer = new DispatcherTimer();
        int timerInterval = 300;
        bool BIDSSMemIsEnabled = false;
        int brakeNotch;
        int powerNotch;
        int reverserPosition;
        bool constantSpeed;
        bool door = false;

        List<int> panel;
        List<int> sound;

        public Driving(caMonIF arg_camonIF)
        {
            InitializeComponent();

            SharedFuncs.SML.SMC_BSMDChanged += SMemLib_BIDSSMemChanged;
            SharedFuncs.SML.SMC_OpenDChanged += SMemLib_OpenChanged;
            SharedFuncs.SML.SMC_PanelDChanged += SMemLib_PanelChanged;
            SharedFuncs.SML.SMC_SoundDChanged += SMemLib_SoundChanged;

            panel = new List<int>();
            sound = new List<int>();
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
            panel = new List<int>(p.NewValue);
        }

        /// <summary> 
        /// Soundに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_SoundChanged(object sender, ValueChangedEventArgs<int[]> s)
        {
            sound = new List<int>(s.NewValue);
        }

        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            /// 接続
            Online.Visibility = BIDSSMemIsEnabled ? Visibility.Visible : Visibility.Collapsed;
            Offline.Visibility = !BIDSSMemIsEnabled ? Visibility.Visible : Visibility.Collapsed;

            /// 回生
            Regeneration.Visibility = panel[(int)panelIndex.Regeneration] != 0 ? Visibility.Visible : Visibility.Collapsed;

            /// 定速
            ConstantSpeed.Visibility = constantSpeed ? Visibility.Visible : Visibility.Collapsed;
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
