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
        /// <summary>
        /// 背景色
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor",
                                        typeof(Brush),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        /// <summary>
        /// 背景色
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// 前景色
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor",
                                        typeof(Brush),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White)));
        /// <summary>
        /// 前景色
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public Brush ForegroundColor
        {
            get { return (Brush)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        /// <summary>
        /// 文字色
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor",
                                        typeof(Brush),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black)));
        /// <summary>
        /// 文字色
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public Brush TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// 表示文字
        /// 依存プロパティ
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof(string),
                                        typeof(CustomIndicator),
                                        new FrameworkPropertyMetadata("Text"));
        /// <summary>
        /// 表示文字
        /// ラッパー(CLI用プロパティ)
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
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
