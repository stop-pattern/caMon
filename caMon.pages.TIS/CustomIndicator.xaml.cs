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
        const int timerInterval = 100;

        /// <summary> 表示状態 </summary>
        public bool status { get; set; }
        /// <summary> 表示内容 </summary>
        public string text { get; set; }
        /// <summary> 表示色（背景） </summary>
        public Color color_back { get; set; }
        /// <summary> 表示色（文字） </summary>
        public Color color_text { get; set; }
        /// <summary> 表示色（無効時） </summary>
        public Color color_disable { get; set; }
        /// <summary> 表示状態(差分取得用) </summary>
        protected bool previous;

        public CustomIndicator()
        {
            InitializeComponent();

            status = false;
            text = "";
            color_back = Colors.Red;
            color_text = Colors.Black;
            color_disable = Colors.White;
            previous = !status;

            front.Text = this.text;
            back.Background = new SolidColorBrush(this.color_back);
            back.BorderThickness = new Thickness(0);

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="str">表示内容</param>
        /// <param name="b">表示色（背景）</param>
        /// <param name="t">表示色（文字）</param>
        /// <param name="d">表示色（無効時）</param>
        /// <param name="st">表示状態</param>
        public CustomIndicator(string str = "", Color b = default, Color t = default, Color d = default, bool st = false)
        {
            InitializeComponent();

            status = st;
            text = str;
            color_back = b;     // Colors.Red
            color_text = t;     // Colors.Black
            color_disable = d;  // Colors.White
            previous = !status;

            ChangeStatus(st, true);

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
        /// 表示状態変更
        /// </summary>
        /// <param name="update">強制表示更新</param>
        /// <returns>実行結果</returns>
        public int CheckChange(bool st = false, bool update = false)
        {
            if (previous != status || update)
            {
                previous = status;
                if (status) setEnable();
                else setDisable();
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
                if (st) setEnable();
                else setDisable();
                return 0;
            }
            //else return 1;
            return -1;
        }

        /// <summary>
        /// 有効表示
        /// </summary>
        protected void setEnable()
        {
            front.Text = this.text;
            //this.Style = (Style)(this.Resources["true"]);
            back.Background = new SolidColorBrush(color_back);
            back.BorderBrush = new SolidColorBrush(Colors.Transparent);
            back.BorderThickness = new Thickness(0);
            front.Foreground = new SolidColorBrush(color_text);
        }

        /// <summary>
        /// 無効表示
        /// </summary>
        protected void setDisable()
        {
            front.Text = this.text;
            //this.Style = (Style)(this.Resources["false"]);
            back.Background = new SolidColorBrush(Colors.Transparent);
            back.BorderBrush = new SolidColorBrush(color_disable);
            back.BorderThickness = new Thickness(5);
            front.Foreground = new SolidColorBrush(color_disable);
        }
    }
}
