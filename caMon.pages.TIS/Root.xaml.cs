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
        static readonly DispatcherTimer timer = new DispatcherTimer();
        /// <summary>  </summary>
        readonly caMonIF camonIF;
        /// <summary>ループ間隔[ms]</summary>
        readonly int timerInterval = 300;
        /// <summary>BIDS Shared Memoryの状態</summary>
        bool BIDSSMemIsEnabled = false;
        /// <summary>Bve5から渡される情報</summary>
        BIDSSharedMemoryData bve5;
        /// <summary>OpenBveから渡される情報</summary>
        OpenD obve;
        /// <summary>panelの状態</summary>
        public static List<int> panel = new List<int>();
        /// <summary>soundの状態</summary>
        public static List<int> sound = new List<int>();
        /// <summary>表示状態</summary>
        PageStatus status = new PageStatus();

        /// <summary>
        /// 表示状態
        /// </summary>
        enum PageStatus
        {
            CannotLoadPages = -3,
            SMemDisable = -2,
            Unknown = -1,
            Root = 0,
            Indicator,
            Driver,
        }


        public Root(caMonIF arg_camonIF)
        {
            InitializeComponent();

            camonIF = arg_camonIF;

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
        private void Back_home(object sender, RoutedEventArgs e) => camonIF.BackToHomeDo();

        /// <summary> 
        /// アプリケーションを終了
        /// </summary>
        private void Close_app(object sender, RoutedEventArgs e) => camonIF.CloseAppDo();

        /// <summary> 
        /// mainFrameにPage.xamlの内容を描画
        /// </summary>
        private void Page__Show(object sender, RoutedEventArgs e) => mainFrame.Source = new Uri(@"Pages\Page.xaml", UriKind.Relative);


        /// <summary> 
        /// SharedMemに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_BIDSSMemChanged(object sender, ValueChangedEventArgs<BIDSSharedMemoryData> e)
        {
            if (!e.NewValue.IsEnabled) status = PageStatus.SMemDisable;
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
            switch (status)
            {
                case PageStatus.CannotLoadPages:
                    mainFrameCover.Visibility = Visibility.Visible;
                    textMessage.Text = "ページ表示エラー\nmainFrameが表示できません\nrootページのみを表示しています";
                    contentLabel.Content = "◆　表 示 エ ラ ー　◆";
                    mainFrame.Visibility = Visibility.Collapsed;
                    break;
                case PageStatus.SMemDisable:
                    mainFrameCover.Visibility = Visibility.Visible;
                    textMessage.Text = "BIDS Shared Memory\n接続未検出\nBIDSSMemを接続してください";
                    contentLabel.Content = "◆　接 続 エ ラ ー　◆";
                    mainFrame.Visibility = Visibility.Collapsed;
                    break;
                case PageStatus.Unknown:
                    mainFrameCover.Visibility = Visibility.Visible;
                    textMessage.Text = "不明なエラー";
                    contentLabel.Content = "◆　エ ラ ー　◆";
                    mainFrame.Visibility = Visibility.Collapsed;
                    break;
                default:
                    mainFrameCover.Visibility = Visibility.Collapsed;
                    textMessage.Text = "表示する画面を選択してください";
                    contentLabel.Content = "◆　Ｔ　Ｉ　Ｓ　◆";
                    mainFrame.Visibility = Visibility.Visible;
                    break;
                case PageStatus.Root:
                    mainFrameCover.Visibility = Visibility.Collapsed;
                    textMessage.Visibility = Visibility.Collapsed;
                    contentLabel.Content = "◆　Ｔ　Ｉ　Ｓ　◆";
                    mainFrame.Visibility = Visibility.Visible;
                    break;
                case PageStatus.Indicator:
                    mainFrameCover.Visibility = Visibility.Collapsed;
                    textMessage.Visibility = Visibility.Collapsed;
                    contentLabel.Content = "◆　" + PageStatus.Indicator + "　◆";
                    mainFrame.Visibility = Visibility.Visible;
                    mainFrame.Source = new Uri(@"Pages\Indicator.xaml", UriKind.Relative);
                    break;
                case PageStatus.Driver:
                    mainFrameCover.Visibility = Visibility.Collapsed;
                    textMessage.Visibility = Visibility.Collapsed;
                    contentLabel.Content = "◆　" + PageStatus.Driver + "　◆";
                    mainFrame.Visibility = Visibility.Visible;
                    mainFrame.Source = new Uri(@"Pages\Driving.xaml", UriKind.Relative);
                    break;
            }
        }


        private void Indicator_Click(object sender, RoutedEventArgs e)
        {
            status = PageStatus.Indicator;
            Botton_Reset();
            Indicator.Foreground = new SolidColorBrush(Colors.White);
            Indicator.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void Driving_Click(object sender, RoutedEventArgs e)
        {
            status = PageStatus.Driver;
            Botton_Reset();
            Driving.Foreground = new SolidColorBrush(Colors.White);
            Driving.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void Botton_Reset()
        {
            Indicator.IsChecked = false;
            Indicator.Foreground = new SolidColorBrush(Colors.Black);
            Indicator.Background = new SolidColorBrush(Colors.White);
            Driving.IsChecked = false;
            Driving.Foreground = new SolidColorBrush(Colors.Black);
            Driving.Background = new SolidColorBrush(Colors.White);
        }
    }
}
