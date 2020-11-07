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
    /// TKK.xaml の相互作用ロジック
    /// </summary>
    public partial class TKK : Page
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


        public TKK()
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

                EmrDrive.Status = toBool(panel[31]);
                Tasc.Status = toBool(panel[136]);
                Atc.Status = toBool(panel[21]);
                Shunting.Status = toBool(panel[32]);
                Inside.Status = toBool(panel[31]);
                NotInstitutionalized.Status = toBool(panel[27]) || toBool(panel[28]);

                AtcEmergency.Status = toBool(panel[22]);
                AtcService.Status = toBool(panel[23]);
                TascControl.Status = toBool(panel[138]);
                SnowResistant.Status = toBool(panel[176]);
                //NonRegenerative.Status = toBool(panel[171]);    //廃止
                EmrBrake.Status = (panel[55] == 9);

                PlatformDoorInterlocking.Status = panel[155] == 1 ? true : false;
                PlatformDoorNotInterlocking.Status = panel[155] == 2 ? true : false;

                HighBeam.Status = toBool(panel[18]);
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
