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
using caMon.pages.TIS;

namespace caMon.pages.TIS
{
    /// <summary>
    /// notch.xaml の相互作用ロジック
    /// </summary>
    public partial class notch : Page
    {
        readonly DispatcherTimer timer = new DispatcherTimer();  /// <summary> ループタイマー </summary>
        int timerInterval = 10;
        bool BIDSSMemIsEnabled = false;
        int brakeNotch;
        int powerNotch;
        int reverserPosition;
        bool door = false;
        Spec spec;

        List<int> panel = new List<int>();

        List<CustomIndicator> customIndicators;

        public notch()
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

            brakeNotch = e.NewValue.HandleData.B;
            powerNotch = e.NewValue.HandleData.P;
            reverserPosition = e.NewValue.HandleData.R;

            spec = e.NewValue.SpecData;

            door = e.NewValue.IsDoorClosed;
        }

        /// <summary> 
        /// Panelに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_PanelChanged(object sender, ValueChangedEventArgs<int[]> p)
        {
            panel = p.NewValue.ToList();
        }

        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            if (BIDSSMemIsEnabled && panel?.Count > 0/* && sound?.Count > 0*/)
            {
                if (panel[92] != 0 && panel[56] == 0)
                {   // 通常時
                    ind_off.Status = false;
                    ind_e.Status = (panel[55] >= 9) ? true : false;
                    ind_b8.Status = (panel[55] >= 8) ? true : false;
                    ind_b7.Status = (panel[55] >= 7) ? true : false;
                    ind_b6.Status = (panel[55] >= 6) ? true : false;
                    ind_b5.Status = (panel[55] >= 5) ? true : false;
                    ind_b4.Status = (panel[55] >= 4) ? true : false;
                    ind_b3.Status = (panel[55] >= 3) ? true : false;
                    ind_b2.Status = (panel[55] >= 2) ? true : false;
                    ind_b1.Status = (panel[55] >= 1) ? true : false;
                    ind_n.Status = (panel[55] == 0 || panel[66] == 0) ? true : false;
                    ind_p1.Status = (panel[66] >= 1) ? true : false;
                    ind_p2.Status = (panel[66] >= 2) ? true : false;
                    ind_p3.Status = (panel[66] >= 3) ? true : false;
                    ind_p4.Status = (panel[66] >= 4) ? true : false;
                    ind_p5.Status = (panel[66] >= 5) ? true : false;
                }
                else
                {   // 鍵抜取
                    ind_off.Status = true;
                    ind_e.Status = false;
                    ind_b8.Status = false;
                    ind_b7.Status = false;
                    ind_b6.Status = false;
                    ind_b5.Status = false;
                    ind_b4.Status = false;
                    ind_b3.Status = false;
                    ind_b2.Status = false;
                    ind_b1.Status = false;
                    ind_n.Status = false;
                    ind_p1.Status = false;
                    ind_p2.Status = false;
                    ind_p3.Status = false;
                    ind_p4.Status = false;
                    ind_p5.Status = false;
                }
            }
        }

    }
}
