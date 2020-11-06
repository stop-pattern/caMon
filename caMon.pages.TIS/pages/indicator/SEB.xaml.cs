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
