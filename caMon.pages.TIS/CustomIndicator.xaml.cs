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

namespace caMon.pages.TIS
{
    /// <summary>
    /// CustomIndicator.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomIndicator : UserControl
    {
        protected readonly Color _c = Colors.Red;

        /// <summary> ループタイマー </summary>
        readonly DispatcherTimer timer = new DispatcherTimer();
        /// <summary> タイマー周期 </summary>
        const int timerInterval = 10;

        /// <summary> 表示状態 </summary>
        //public bool Status { get; set; } = false;
        /// <summary> 表示内容 </summary>
        //public string Text { get; set; } = "";
        /// <summary> 表示色（背景） </summary>
        //public Brush color_back { get; set; } = new SolidColorBrush(Colors.Transparent);
        /// <summary> 表示色（文字） </summary>
        public Brush color_text { get; set; } = new SolidColorBrush(Colors.Transparent);
        /// <summary> 表示色（無効時） </summary>
        public Brush color_disable { get; set; } = new SolidColorBrush(Colors.Transparent);
        /// <summary> 表示状態(差分取得用) </summary>
        protected bool previous { get; set; } = false;

        /// <summary>
        /// 背景色
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor",
                                        typeof(Brush),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnBackgroundColorChanged)));
        /// <summary>
        /// 背景色
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        /// <summary> 値変更時に呼ばれるコールバック関数 </summary>
        private static void OnBackgroundColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // オブジェクトを取得して処理する
            CustomIndicator ctrl = obj as CustomIndicator;
            if (ctrl != null)
            {
                //ctrl.BackgroundColor = ctrl.BackgroundColor;
            }
        }

        /// <summary>
        /// 前景色
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor",
                                        typeof(Brush),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(OnForegroundColorChanged)));
        /// <summary>
        /// 前景色
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public Brush ForegroundColor
        {
            get { return (Brush)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }
        /// <summary> 値変更時に呼ばれるコールバック関数 </summary>
        private static void OnForegroundColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // オブジェクトを取得して処理する
            CustomIndicator ctrl = obj as CustomIndicator;
            if (ctrl != null)
            {
            }
        }

        /// <summary>
        /// 表示文字
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof(string),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata("Text", new PropertyChangedCallback(OnTextChanged)));
        /// <summary>
        /// 表示文字
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary> 値変更時に呼ばれるコールバック関数 </summary>
        private static void OnTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // オブジェクトを取得して処理する
            CustomIndicator ctrl = obj as CustomIndicator;
            if (ctrl != null)
            {
            }
        }

        /// <summary>
        /// 表示状態
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status",
                                        typeof(bool),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnStatusChanged)));
        /// <summary>
        /// 表示状態
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        /// <summary> 値変更時に呼ばれるコールバック関数 </summary>
        private static void OnStatusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // オブジェクトを取得して処理する
            CustomIndicator ctrl = obj as CustomIndicator;
            if (ctrl != null)
            {
                ctrl.SetDisplay(ctrl.Status);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomIndicator()
        {
            InitializeComponent();

            previous = !Status;

            CheckChange(true);

            // this.DataContext = new { dispText = text };

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
        }

        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            CheckChange();
        }

        /// <summary>
        /// 状態変更
        /// </summary>
        /// <param name="update">強制表示更新</param>
        /// <returns>実行結果</returns>
        public int CheckChange(bool update = false)
        {
            if (previous != Status || update)
            {
                previous = Status;
                SetDisplay(Status);
                return 0;
            }
            //else return 1;
            return -1;
        }

        /// <summary>
        /// 表示変更
        /// </summary>
        protected void SetDisplay(bool disp)
        {
            if (disp)
            {   // 有効
                On.Visibility = Visibility.Visible;
                Off.Visibility = Visibility.Collapsed;
            }
            else
            {   // 無効
                On.Visibility = Visibility.Collapsed;
                Off.Visibility = Visibility.Visible;
            }
        }
    }
}
