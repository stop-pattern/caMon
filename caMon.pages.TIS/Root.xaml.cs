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

namespace caMon.pages.TIS
{
    /// <summary>
    /// Root.xaml の相互作用ロジック
    /// </summary>
    public partial class Root : Page
    {
        /// <summary>ループタイマー</summary>
        DispatcherTimer timer = new DispatcherTimer();
        /// <summary>  </summary>
        caMonIF camonIF;
        /// <summary>ループ間隔[ms]</summary>
        int timerInterval = 300;
        /// <summary>BIDS Shared Memoryの状態</summary>
        bool BIDSSMemIsEnabled = false;
        /// <summary>Bve5から渡される情報</summary>
        BIDSSharedMemoryData bve5;
        /// <summary>OpenBveから渡される情報</summary>
        OpenD obve;
        /// <summary>panelの状態</summary>
        int[] panel;
        /// <summary>soundの状態</summary>
        int[] sound;


        public Root(caMonIF arg_camonIF)
        {
            InitializeComponent();

            camonIF = arg_camonIF;

            SharedFuncs.SML.SMC_BSMDChanged += SMemLib_BIDSSMemChanged;
            SharedFuncs.SML.SMC_OpenDChanged += SMemLib_OpenChanged;
            SharedFuncs.SML.SMC_PanelDChanged += SMemLib_PanelChanged;
            SharedFuncs.SML.SMC_SoundDChanged += SMemLib_SoundChanged;

            panel = new int[256];
            panel = new int[256];

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
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

        /// <summary> 
        /// mod選択画面へ戻る
        /// </summary>
        private void back_home(object sender, RoutedEventArgs e) => camonIF.BackToHomeDo();

        /// <summary> 
        /// アプリケーションを終了
        /// </summary>
        private void close_app(object sender, RoutedEventArgs e) => camonIF.CloseAppDo();

        /// <summary> 
        /// mainFrameにPage.xamlの内容を描画
        /// </summary>
        private void Page__Show(object sender, RoutedEventArgs e) => mainFrame.Source = new Uri(@"Pages\Page.xaml", UriKind.Relative);


        /// <summary> 
        /// SharedMemに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_BIDSSMemChanged(object sender, ValueChangedEventArgs<BIDSSharedMemoryData> e)
        {
            BIDSSMemIsEnabled = e.NewValue.IsEnabled;
            bve5 = e.NewValue;
        }

        /// <summary> 
        /// Openに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_OpenChanged(object sender, ValueChangedEventArgs<OpenD> e)
        {
            obve = e.NewValue;
        }

        /// <summary> 
        /// Panelに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_PanelChanged(object sender, ValueChangedEventArgs<int[]> p)
        {
            panel = p.NewValue;
        }

        /// <summary> 
        /// Soundに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_SoundChanged(object sender, ValueChangedEventArgs<int[]> s)
        {
            sound = s.NewValue;
        }


        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
        }
    }
}
