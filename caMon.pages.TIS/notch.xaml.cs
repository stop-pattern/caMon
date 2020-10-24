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
        bool updateFlag = false;

        List<int> panel = new List<int>();

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
            updateFlag = true;
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
            if (updateFlag)
            {
                if (8 > spec.J)
                {
                    ind_b8.Visibility = Visibility.Hidden;
                    if (7 > spec.J)
                    {
                        ind_b7.Visibility = Visibility.Hidden;
                        if (6 > spec.J)
                        {
                            ind_b6.Visibility = Visibility.Hidden;
                            if (5 > spec.J)
                            {
                                ind_b5.Visibility = Visibility.Hidden;
                                if (4 > spec.J)
                                {
                                    ind_b4.Visibility = Visibility.Hidden;
                                    if (3 > spec.J)
                                    {
                                        ind_b3.Visibility = Visibility.Hidden;
                                        if (2 > spec.J)
                                        {
                                            ind_b2.Visibility = Visibility.Hidden;
                                            if (1 > spec.J)
                                            {
                                                ind_b1.Visibility = Visibility.Hidden;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (5 > spec.P)
                {
                    ind_p5.Visibility = Visibility.Hidden;
                    if (4 > spec.P)
                    {
                        ind_p4.Visibility = Visibility.Hidden;
                        if (3 > spec.P)
                        {
                            ind_p3.Visibility = Visibility.Hidden;
                            if (2 > spec.P)
                            {
                                ind_p2.Visibility = Visibility.Hidden;
                                if (1 > spec.P)
                                {
                                    ind_p1.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                    }
                }
                updateFlag = false;
            }

            if (BIDSSMemIsEnabled && panel?.Count > 0/* && sound?.Count > 0*/)
            {
                if (false)  // 通常
                {
                    ind_off.Status = false;
                    ind_e.Status = (brakeNotch >= 9) ? true : false;
                    ind_b8.Status = (brakeNotch >= 8) ? true : false;
                    ind_b7.Status = (brakeNotch >= 7) ? true : false;
                    ind_b6.Status = (brakeNotch >= 6) ? true : false;
                    ind_b5.Status = (brakeNotch >= 5) ? true : false;
                    ind_b4.Status = (brakeNotch >= 4) ? true : false;
                    ind_b3.Status = (brakeNotch >= 3) ? true : false;
                    ind_b2.Status = (brakeNotch >= 2) ? true : false;
                    ind_b1.Status = (brakeNotch >= 1) ? true : false;
                    ind_n.Status = (brakeNotch == 0 || powerNotch == 0) ? true : false;
                    ind_p1.Status = (powerNotch >= 1) ? true : false;
                    ind_p2.Status = (powerNotch >= 2) ? true : false;
                    ind_p3.Status = (powerNotch >= 3) ? true : false;
                    ind_p4.Status = (powerNotch >= 4) ? true : false;
                    ind_p5.Status = (powerNotch >= 5) ? true : false;
                }
                // メトロ
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
