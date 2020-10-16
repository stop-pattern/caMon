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
        public bool status { get; set; } = false;
        /// <summary> 表示内容 </summary>
        public string text { get; set; } = "";
        /// <summary> 表示色（背景） </summary>
        public Brush color_back { get; set; } = new SolidColorBrush(Colors.Transparent);
        /// <summary> 表示色（文字） </summary>
        public Brush color_text { get; set; } = new SolidColorBrush(Colors.Transparent);
        /// <summary> 表示色（無効時） </summary>
        public Brush color_disable { get; set; } = new SolidColorBrush(Colors.Transparent);
        /// <summary> 表示状態(差分取得用) </summary>
        protected bool previous { get; set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomIndicator()
        {
            InitializeComponent();

            previous = !status;

            front.Text = this.text;
            back.Background = this.Background;
            back.BorderThickness = new Thickness(0);

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
            if (previous != status || update)
            {
                previous = status;
                SetDisplay(status);
                return 0;
            }
            //else return 1;
            return -1;
        }

        /// <summary>
        /// 表示状態変更
        /// </summary>
        /// <param name="st">表示状態</param>
        /// <param name="update">強制表示更新</param>
        /// <returns>実行結果</returns>
        public int ChangeStatus(bool st = false, bool update = false)
        {
            if (st != status || update)
            {
                status = st;
                SetDisplay(st);
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
            front.Text = this.text;
            if (disp)
            {   // 有効
                //this.Style = (Style)(this.Resources["true"]);
                back.Background = this.Background;
                back.BorderBrush = new SolidColorBrush(Colors.Transparent);
                back.BorderThickness = new Thickness(0);
                front.Foreground = color_text;
            }
            else
            {   // 無効
                //this.Style = (Style)(this.Resources["false"]);
                back.Background = this.Background;
                back.BorderBrush = this.Foreground;
                back.BorderThickness = new Thickness(5);
                front.Foreground = this.Foreground;
            }
        }
    }
}
