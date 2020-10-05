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

            // コントロール生成
            customIndicators = new List<CustomIndicator>{
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White),
                new CustomIndicator("試験", Colors.Orange, Colors.White, Colors.White)
            };
            // 上から順に置いていく
            for (int i = 0; i < customIndicators.Count; i++)
            {
                customIndicators[i].SetValue(Grid.RowProperty, i);
            }

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

            }
        }

    }
}
