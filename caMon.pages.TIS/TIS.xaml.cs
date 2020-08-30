using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Shapes;

using TR;
using TR.BIDSSMemLib;

namespace caMon.pages.TIS
{
    /// <summary>
    /// TIS.xaml の相互作用ロジック
    /// </summary>
    public partial class TIS : Page
    {
        /// <summary> ループタイマー </summary>
        DispatcherTimer timer = new DispatcherTimer();
        int timerInterval = 300;
        int TimeOld = 0;
        caMonIF camonIF;
        Rectangle[,] rectangles;
        Label[,] labels;
        int[] panel;

        public TIS(caMonIF arg_camonIF)
        {
            InitializeComponent();

            camonIF = arg_camonIF;

            SharedFuncs.SML.SMC_BSMDChanged += SMemLib_BIDSSMemChanged;
            SharedFuncs.SML.SMC_OpenDChanged += SMemLib_OpenChanged;
            SharedFuncs.SML.SMC_PanelDChanged += SMemLib_PanelChanged;
            SharedFuncs.SML.SMC_SoundDChanged += SMemLib_SoundChanged;

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            timer.Start();
            rectangles = new Rectangle[3, 12]
            {
                {r11,r13,r15,r17,r19,r111,r113,r115,r117,r119,r121,r123},
                {r31,r33,r35,r37,r39,r311,r313,r315,r317,r319,r321,r323},
                {r51,r53,r55,r57,r59,r511,r513,r515,r517,r519,r521,r523}
            };
            labels = new Label[3, 12]
            {
                {l00,l02,l04,l06,l08,l010,l012,l014,l016,l018,l020,l022},
                {l20,l22,l24,l26,l28,l210,l212,l214,l216,l218,l220,l222},
                {l40,l42,l44,l46,l48,l410,l412,l414,l416,l418,l420,l422}
            };
            panel = new int[256];
        }

        bool BIDSSMemIsEnabled = false;
        int TimeVal = -1;
        int bNotch = -1;
        int pNotch = -1;
        int KeyPosition = -1;

        /// <summary> 
        /// SharedMemに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_BIDSSMemChanged(object sender, ValueChangedEventArgs<BIDSSharedMemoryData> e)
        {
            BIDSSMemIsEnabled = e.NewValue.IsEnabled;

            TimeVal = e.NewValue.StateData.T;
        }

        /// <summary> 
        /// Openに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_OpenChanged(object sender, ValueChangedEventArgs<OpenD> e)
        {
        }

        /// <summary> 
        /// Panelに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_PanelChanged(object sender, ValueChangedEventArgs<int[]> p)
        {
            panel = p.NewValue;
            bNotch = p.NewValue[51];
            pNotch = p.NewValue[66];
            KeyPosition = p.NewValue[92];
        }

        /// <summary> 
        /// Soundに更新があったときに呼ばれる関数
        /// </summary>
        private void SMemLib_SoundChanged(object sender, ValueChangedEventArgs<int[]> s)
        {
        }

        /// <summary> 
        /// タイマで呼ばれる関数
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            if (!BIDSSMemIsEnabled)
            {
            }

            if (TimeVal < TimeOld)
                Task.Delay(10);

            TimeOld = TimeVal;

            if (Smooth.IsChecked == true && timerInterval == 300)
            {
                timerInterval = 0;
                TimerStart();
            }
            if (Smooth.IsChecked == false && timerInterval != 300)
            {
                timerInterval = 300;
                TimerStart();
            }

                DispNotches();
                DispRoute();
                DispFormDoor();
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
        /// 戻るボタン
        /// </summary>
        private void NextP(object sender, RoutedEventArgs e) => camonIF.BackToHomeDo();

        /// <summary>非常表示で使用する色</summary>
        static readonly Color colorEmergency = Colors.Red;
        /// <summary>注意表示で使用する色</summary>
        static readonly Color colorBrake = Colors.Orange;
        /// <summary>通知表示で使用する色</summary>
        static readonly Color colorNotice = Colors.LightGreen;
        /// <summary>有効表示で使用する色</summary>
        static readonly Color colorActive = Colors.Aquamarine;

        /// <summary>
        /// ノッチ表示更新
        /// </summary>
        private void DispNotches()
        {
            if (KeyPosition == 0)
            {
                ChangeDisplay(R_EB, L_EB, Colors.Transparent);
                ChangeDisplay(R_B7, L_B7, Colors.Transparent);
                ChangeDisplay(R_B6, L_B6, Colors.Transparent);
                ChangeDisplay(R_B5, L_B5, Colors.Transparent);
                ChangeDisplay(R_B4, L_B4, Colors.Transparent);
                ChangeDisplay(R_B3, L_B3, Colors.Transparent);
                ChangeDisplay(R_B2, L_B2, Colors.Transparent);
                ChangeDisplay(R_B1, L_B1, Colors.Transparent);
                ChangeDisplay(R_N0, L_N0, Colors.Transparent);
                ChangeDisplay(R_P1, L_P1, Colors.Transparent);
                ChangeDisplay(R_P2, L_P2, Colors.Transparent);
                ChangeDisplay(R_P3, L_P3, Colors.Transparent);
                ChangeDisplay(R_P4, L_P4, Colors.Transparent);
                return;
            }

            if (bNotch >= 8) ChangeDisplay(R_EB, L_EB, colorEmergency, true);
            else ChangeDisplay(R_EB, L_EB, Colors.Transparent);

            if (bNotch >= 7) ChangeDisplay(R_B7, L_B7, colorBrake, true);
            else ChangeDisplay(R_B7, L_B7, Colors.Transparent);
            if (bNotch >= 6) ChangeDisplay(R_B6, L_B6, colorBrake, true);
            else ChangeDisplay(R_B6, L_B6, Colors.Transparent);
            if (bNotch >= 5) ChangeDisplay(R_B5, L_B5, colorBrake, true);
            else ChangeDisplay(R_B5, L_B5, Colors.Transparent);
            if (bNotch >= 4) ChangeDisplay(R_B4, L_B4, colorBrake, true);
            else ChangeDisplay(R_B4, L_B4, Colors.Transparent);
            if (bNotch >= 3) ChangeDisplay(R_B3, L_B3, colorBrake, true);
            else ChangeDisplay(R_B3, L_B3, Colors.Transparent);
            if (bNotch >= 2) ChangeDisplay(R_B2, L_B2, colorBrake, true);
            else ChangeDisplay(R_B2, L_B2, Colors.Transparent);
            if (bNotch >= 1) ChangeDisplay(R_B1, L_B1, colorBrake, true);
            else ChangeDisplay(R_B1, L_B1, Colors.Transparent);

            ChangeDisplay(R_N0, L_N0, colorNotice, true);

            if (pNotch >= 1) ChangeDisplay(R_P1, L_P1, colorActive, true);
            else ChangeDisplay(R_P1, L_P1, Colors.Transparent);
            if (pNotch >= 2) ChangeDisplay(R_P2, L_P2, colorActive, true);
            else ChangeDisplay(R_P2, L_P2, Colors.Transparent);
            if (pNotch >= 3) ChangeDisplay(R_P3, L_P3, colorActive, true);
            else ChangeDisplay(R_P3, L_P3, Colors.Transparent);
            if (pNotch >= 4) ChangeDisplay(R_P4, L_P4, colorActive, true);
            else ChangeDisplay(R_P4, L_P4, Colors.Transparent);
        }

        /// <summary>
        /// 線区関係
        /// </summary>
        private void DispRoute()
        {
            if (KeyPosition != 0)
            {
                switch (KeyPosition)
                {
                    /// TRTA
                    case 1:
                        comp.Content = "地下鉄";
                        comp.Visibility = Visibility.Visible;

                        labels[0, 0].Content = "非常運転";
                        rectangles[0, 0].Visibility = Visibility.Visible;
                        labels[0, 0].Visibility = Visibility.Visible;

                        labels[0, 1].Content = "ＡＴＯ";
                        ChangeDisplay(rectangles[0, 1], labels[0, 1], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[146]));
                        rectangles[0, 1].Visibility = Visibility.Visible;
                        labels[0, 1].Visibility = Visibility.Visible;

                        labels[0, 2].Content = "ＡＴＣ";
                        ChangeDisplay(rectangles[0, 2], labels[0, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[19]));
                        rectangles[0, 2].Visibility = Visibility.Visible;
                        labels[0, 2].Visibility = Visibility.Visible;

                        labels[0, 3].Content = "構　内";
                        ChangeDisplay(rectangles[0, 3], labels[0, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[31]));
                        rectangles[0, 3].Visibility = Visibility.Visible;
                        labels[0, 3].Visibility = Visibility.Visible;

                        labels[0, 4].Content = "非　設";
                        ChangeDisplay(rectangles[0, 4], labels[0, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[29]));
                        rectangles[0, 4].Visibility = Visibility.Hidden;
                        labels[0, 4].Visibility = Visibility.Hidden;

                        labels[0, 5].Content = "";
                        rectangles[0, 5].Visibility = Visibility.Hidden;
                        labels[0, 5].Visibility = Visibility.Hidden;

                        labels[0, 6].Content = "";
                        rectangles[0, 6].Visibility = Visibility.Hidden;
                        labels[0, 6].Visibility = Visibility.Hidden;

                        labels[0, 7].Content = "";
                        rectangles[0, 7].Visibility = Visibility.Hidden;
                        labels[0, 7].Visibility = Visibility.Hidden;

                        labels[0, 8].Content = "";
                        rectangles[0, 8].Visibility = Visibility.Hidden;
                        labels[0, 8].Visibility = Visibility.Hidden;

                        labels[0, 9].Content = "";
                        rectangles[0, 9].Visibility = Visibility.Hidden;
                        labels[0, 9].Visibility = Visibility.Hidden;

                        labels[0, 10].Content = "";
                        rectangles[0, 10].Visibility = Visibility.Hidden;
                        labels[0, 10].Visibility = Visibility.Hidden;

                        labels[0, 11].Content = "";
                        rectangles[0, 11].Visibility = Visibility.Hidden;
                        labels[0, 11].Visibility = Visibility.Hidden;

                        labels[1, 0].Content = "ＡＴＣ非常";
                        ChangeDisplay(rectangles[1, 0], labels[1, 0], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[22]));
                        rectangles[1, 0].Visibility = Visibility.Visible;
                        labels[1, 0].Visibility = Visibility.Visible;

                        labels[1, 1].Content = "ＡＴＣ常用";
                        ChangeDisplay(rectangles[1, 1], labels[1, 1], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[23]));
                        rectangles[1, 1].Visibility = Visibility.Visible;
                        labels[1, 1].Visibility = Visibility.Visible;

                        labels[1, 2].Content = "耐雪ﾌﾞﾚｰｷ";
                        ChangeDisplay(rectangles[1, 2], labels[1, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[176]));
                        rectangles[1, 2].Visibility = Visibility.Visible;
                        labels[1, 2].Visibility = Visibility.Visible;

                        labels[1, 3].Content = "回生開放";
                        //ChangeDisplay(rectangles[1, 3], labels[1, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[171]));    // 廃止
                        rectangles[1, 3].Visibility = Visibility.Visible;
                        labels[1, 3].Visibility = Visibility.Visible;

                        labels[1, 4].Content = "";
                        rectangles[1, 4].Visibility = Visibility.Hidden;
                        labels[1, 4].Visibility = Visibility.Hidden;

                        labels[1, 5].Content = "";
                        rectangles[1, 5].Visibility = Visibility.Hidden;
                        labels[1, 5].Visibility = Visibility.Hidden;

                        labels[1, 6].Content = "";
                        rectangles[1, 6].Visibility = Visibility.Hidden;
                        labels[1, 6].Visibility = Visibility.Hidden;

                        labels[1, 7].Content = "";
                        rectangles[1, 7].Visibility = Visibility.Hidden;
                        labels[1, 7].Visibility = Visibility.Hidden;

                        labels[1, 8].Content = "";
                        rectangles[1, 8].Visibility = Visibility.Hidden;
                        labels[1, 8].Visibility = Visibility.Hidden;

                        labels[1, 9].Content = "";
                        rectangles[1, 9].Visibility = Visibility.Hidden;
                        labels[1, 9].Visibility = Visibility.Hidden;

                        labels[1, 10].Content = "";
                        rectangles[1, 10].Visibility = Visibility.Hidden;
                        labels[1, 10].Visibility = Visibility.Hidden;

                        labels[1, 11].Content = "";
                        rectangles[1, 11].Visibility = Visibility.Hidden;
                        labels[1, 11].Visibility = Visibility.Hidden;

                        labels[2, 0].Content = "ﾎｰﾑﾄﾞｱ連動";
                        labels[2, 0].FontSize = 40;
                        ChangeDisplay(rectangles[2, 0], labels[2, 0], colorNotice, (SharedFuncs.SML.PanelA[155] == 1));
                        rectangles[2, 0].Visibility = Visibility.Visible;
                        labels[2, 0].Visibility = Visibility.Visible;

                        labels[2, 1].Content = "ﾎｰﾑﾄﾞｱ非連動";
                        labels[2, 1].FontSize = 40;
                        ChangeDisplay(rectangles[2, 1], labels[2, 1], colorBrake, (SharedFuncs.SML.PanelA[155] == 2));
                        rectangles[2, 1].Visibility = Visibility.Visible;
                        labels[2, 1].Visibility = Visibility.Visible;

                        labels[2, 2].Content = "ｽﾃｯﾌﾟ支障";
                        rectangles[2, 2].Visibility = Visibility.Visible;
                        labels[2, 2].Visibility = Visibility.Visible;

                        labels[2, 3].Content = "ﾎｰﾑﾄﾞｱ支障";
                        rectangles[2, 3].Visibility = Visibility.Visible;
                        labels[2, 3].Visibility = Visibility.Visible;

                        labels[2, 4].Content = "";
                        rectangles[2, 4].Visibility = Visibility.Hidden;
                        labels[2, 4].Visibility = Visibility.Hidden;

                        labels[2, 5].Content = "起動試験";
                        rectangles[2, 5].Visibility = Visibility.Visible;
                        labels[2, 5].Visibility = Visibility.Visible;

                        labels[2, 6].Content = "";
                        rectangles[2, 6].Visibility = Visibility.Hidden;
                        labels[2, 6].Visibility = Visibility.Hidden;

                        labels[2, 7].Content = "";
                        rectangles[2, 7].Visibility = Visibility.Hidden;
                        labels[2, 7].Visibility = Visibility.Hidden;

                        labels[2, 8].Content = "";
                        rectangles[2, 8].Visibility = Visibility.Hidden;
                        labels[2, 8].Visibility = Visibility.Hidden;

                        labels[2, 9].Content = "";
                        rectangles[2, 9].Visibility = Visibility.Hidden;
                        labels[2, 9].Visibility = Visibility.Hidden;

                        labels[2, 10].Content = "";
                        rectangles[2, 10].Visibility = Visibility.Hidden;
                        labels[2, 10].Visibility = Visibility.Hidden;

                        labels[2, 11].Content = "";
                        rectangles[2, 11].Visibility = Visibility.Hidden;
                        labels[2, 11].Visibility = Visibility.Hidden;
                        break;

                    /// TOB
                    case 2:
                        comp.Content = "東武";
                        comp.Visibility = Visibility.Visible;

                        labels[0, 0].Content = "ATS非常運転";
                        labels[0, 0].FontSize = 40;
                        rectangles[0, 0].Visibility = Visibility.Visible;
                        labels[0, 0].Visibility = Visibility.Visible;

                        labels[0, 1].Content = "ATC非常運転";
                        labels[0, 1].FontSize = 40;
                        rectangles[0, 1].Visibility = Visibility.Visible;
                        labels[0, 1].Visibility = Visibility.Visible;

                        labels[0, 2].Content = "ＴＡＳＣ";
                        ChangeDisplay(rectangles[0, 2], labels[0, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[136]));
                        rectangles[0, 2].Visibility = Visibility.Visible;
                        labels[0, 2].Visibility = Visibility.Visible;

                        labels[0, 3].Content = "東武ATS";
                        ChangeDisplay(rectangles[0, 3], labels[0, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[41]));
                        rectangles[0, 3].Visibility = Visibility.Visible;
                        labels[0, 3].Visibility = Visibility.Visible;

                        labels[0, 4].Content = "ＡＴＣ";
                        ChangeDisplay(rectangles[0, 4], labels[0, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[74]));
                        rectangles[0, 4].Visibility = Visibility.Visible;
                        labels[0, 4].Visibility = Visibility.Visible;

                        labels[0, 5].Content = "入　換";
                        ChangeDisplay(rectangles[0, 5], labels[0, 5], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[75]));
                        rectangles[0, 5].Visibility = Visibility.Visible;
                        labels[0, 5].Visibility = Visibility.Visible;

                        labels[0, 6].Content = "次駅停車";
                        ChangeDisplay(rectangles[0, 6], labels[0, 6], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[252]));
                        rectangles[0, 6].Visibility = Visibility.Visible;
                        labels[0, 6].Visibility = Visibility.Visible;

                        labels[0, 7].Content = "";
                        rectangles[0, 7].Visibility = Visibility.Hidden;
                        labels[0, 7].Visibility = Visibility.Hidden;

                        labels[0, 8].Content = "ハイビーム";
                        ChangeDisplay(rectangles[0, 8], labels[0, 8], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[18]));
                        rectangles[0, 8].Visibility = Visibility.Visible;
                        labels[0, 8].Visibility = Visibility.Visible;

                        labels[0, 9].Content = "";
                        rectangles[0, 9].Visibility = Visibility.Hidden;
                        labels[0, 9].Visibility = Visibility.Hidden;

                        labels[0, 10].Content = "";
                        rectangles[0, 10].Visibility = Visibility.Hidden;
                        labels[0, 10].Visibility = Visibility.Hidden;

                        labels[0, 11].Content = "";
                        rectangles[0, 11].Visibility = Visibility.Hidden;
                        labels[0, 11].Visibility = Visibility.Hidden;

                        labels[1, 0].Content = "保安非常";
                        ChangeDisplay(rectangles[1, 0], labels[1, 0], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[76]));
                        rectangles[1, 0].Visibility = Visibility.Visible;
                        labels[1, 0].Visibility = Visibility.Visible;

                        labels[1, 1].Content = "保安常用";
                        ChangeDisplay(rectangles[1, 1], labels[1, 1], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[77]));
                        rectangles[1, 1].Visibility = Visibility.Visible;
                        labels[1, 1].Visibility = Visibility.Visible;

                        labels[1, 2].Content = "TASC制御";
                        ChangeDisplay(rectangles[1, 2], labels[1, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[138]));
                        rectangles[1, 2].Visibility = Visibility.Visible;
                        labels[1, 2].Visibility = Visibility.Visible;

                        labels[1, 3].Content = "耐雪ﾌﾞﾚｰｷ";
                        ChangeDisplay(rectangles[1, 3], labels[1, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[176]));
                        rectangles[1, 3].Visibility = Visibility.Visible;
                        labels[1, 3].Visibility = Visibility.Visible;

                        labels[1, 4].Content = "回生開放";
                        //ChangeDisplay(rectangles[1, 4], labels[1, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[171]));	// 廃止
                        rectangles[1, 4].Visibility = Visibility.Visible;
                        labels[1, 4].Visibility = Visibility.Visible;

                        labels[1, 5].Content = "非常ﾌﾞﾚｰｷ";
                        ChangeDisplay(rectangles[1, 5], labels[1, 5], colorEmergency, (SharedFuncs.SML.PanelA[51] >= 8));
                        rectangles[1, 5].Visibility = Visibility.Visible;
                        labels[1, 5].Visibility = Visibility.Visible;

                        labels[1, 6].Content = "制御開放";
                        rectangles[1, 6].Visibility = Visibility.Visible;
                        labels[1, 6].Visibility = Visibility.Visible;

                        labels[1, 7].Content = "";
                        rectangles[1, 7].Visibility = Visibility.Hidden;
                        labels[1, 7].Visibility = Visibility.Hidden;

                        labels[1, 8].Content = "情報伝送故障";
                        labels[1, 8].FontSize = 40;
                        rectangles[1, 8].Visibility = Visibility.Visible;
                        labels[1, 8].Visibility = Visibility.Visible;

                        labels[1, 9].Content = "ＴＩＳ試験";
                        rectangles[1, 9].Visibility = Visibility.Visible;
                        labels[1, 9].Visibility = Visibility.Visible;

                        labels[1, 10].Content = "確認";
                        rectangles[1, 10].Visibility = Visibility.Hidden;
                        labels[1, 10].Visibility = Visibility.Hidden;

                        labels[1, 11].Content = "";
                        rectangles[1, 11].Visibility = Visibility.Hidden;
                        labels[1, 11].Visibility = Visibility.Hidden;

                        labels[2, 0].Content = "ﾎｰﾑﾄﾞｱ連動";
                        labels[2, 0].FontSize = 40;
                        ChangeDisplay(rectangles[2, 0], labels[2, 0], colorNotice, (SharedFuncs.SML.PanelA[155] == 1));
                        rectangles[2, 0].Visibility = Visibility.Visible;
                        labels[2, 0].Visibility = Visibility.Visible;

                        labels[2, 1].Content = "ﾎｰﾑﾄﾞｱ非連動";
                        labels[2, 1].FontSize = 40;
                        ChangeDisplay(rectangles[2, 1], labels[2, 1], colorBrake, (SharedFuncs.SML.PanelA[155] == 2));
                        rectangles[2, 1].Visibility = Visibility.Visible;
                        labels[2, 1].Visibility = Visibility.Visible;

                        labels[2, 2].Content = "";
                        rectangles[2, 2].Visibility = Visibility.Hidden;
                        labels[2, 2].Visibility = Visibility.Hidden;

                        labels[2, 3].Content = "";
                        rectangles[2, 3].Visibility = Visibility.Hidden;
                        labels[2, 3].Visibility = Visibility.Hidden;

                        labels[2, 4].Content = "";
                        rectangles[2, 4].Visibility = Visibility.Hidden;
                        labels[2, 4].Visibility = Visibility.Hidden;

                        labels[2, 5].Content = "起動試験";
                        rectangles[2, 5].Visibility = Visibility.Visible;
                        labels[2, 5].Visibility = Visibility.Visible;

                        labels[2, 6].Content = "";
                        rectangles[2, 6].Visibility = Visibility.Hidden;
                        labels[2, 6].Visibility = Visibility.Hidden;

                        labels[2, 7].Content = "";
                        rectangles[2, 7].Visibility = Visibility.Hidden;
                        labels[2, 7].Visibility = Visibility.Hidden;

                        labels[2, 8].Content = "";
                        rectangles[2, 8].Visibility = Visibility.Hidden;
                        labels[2, 8].Visibility = Visibility.Hidden;

                        labels[2, 9].Content = "";
                        rectangles[2, 9].Visibility = Visibility.Hidden;
                        labels[2, 9].Visibility = Visibility.Hidden;

                        labels[2, 10].Content = "";
                        rectangles[2, 10].Visibility = Visibility.Hidden;
                        labels[2, 10].Visibility = Visibility.Hidden;

                        labels[2, 11].Content = "";
                        rectangles[2, 11].Visibility = Visibility.Hidden;
                        labels[2, 11].Visibility = Visibility.Hidden;
                        break;

                    /// TKK
                    case 3:
                        comp.Content = "東急";
                        //comp.Content = "東急・横高";
                        comp.Visibility = Visibility.Visible;

                        labels[0, 0].Content = "非常運転";
                        rectangles[0, 0].Visibility = Visibility.Visible;
                        labels[0, 0].Visibility = Visibility.Visible;

                        labels[0, 1].Content = "";
                        rectangles[0, 1].Visibility = Visibility.Hidden;
                        labels[0, 1].Visibility = Visibility.Hidden;

                        labels[0, 2].Content = "ＴＡＳＣ";
                        ChangeDisplay(rectangles[0, 2], labels[0, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[136]));
                        rectangles[0, 2].Visibility = Visibility.Visible;
                        labels[0, 2].Visibility = Visibility.Visible;

                        labels[0, 3].Content = "ＡＴＣ";
                        ChangeDisplay(rectangles[0, 3], labels[0, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[21]));
                        rectangles[0, 3].Visibility = Visibility.Visible;
                        labels[0, 3].Visibility = Visibility.Visible;

                        labels[0, 4].Content = "入　換";
                        ChangeDisplay(rectangles[0, 4], labels[0, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[32]));
                        rectangles[0, 4].Visibility = Visibility.Visible;
                        labels[0, 4].Visibility = Visibility.Visible;

                        labels[0, 5].Content = "構　内";
                        rectangles[0, 5].Visibility = Visibility.Visible;
                        labels[0, 5].Visibility = Visibility.Visible;

                        labels[0, 6].Content = "非　設";
                        ChangeDisplay(rectangles[0, 6], labels[0, 6], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[28]));
                        rectangles[0, 6].Visibility = Visibility.Visible;
                        labels[0, 6].Visibility = Visibility.Visible;

                        labels[0, 7].Content = "";
                        rectangles[0, 7].Visibility = Visibility.Hidden;
                        labels[0, 7].Visibility = Visibility.Hidden;

                        labels[0, 8].Content = "ハイビーム";
                        ChangeDisplay(rectangles[0, 8], labels[0, 8], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[18]));
                        rectangles[0, 8].Visibility = Visibility.Visible;
                        labels[0, 8].Visibility = Visibility.Visible;

                        labels[0, 9].Content = "";
                        rectangles[0, 9].Visibility = Visibility.Hidden;
                        labels[0, 9].Visibility = Visibility.Hidden;

                        labels[0, 10].Content = "";
                        rectangles[0, 10].Visibility = Visibility.Hidden;
                        labels[0, 10].Visibility = Visibility.Hidden;

                        labels[0, 11].Content = "";
                        rectangles[0, 11].Visibility = Visibility.Hidden;
                        labels[0, 11].Visibility = Visibility.Hidden;

                        labels[1, 0].Content = "ＡＴＣ非常";
                        ChangeDisplay(rectangles[1, 0], labels[1, 0], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[22]));
                        rectangles[1, 0].Visibility = Visibility.Visible;
                        labels[1, 0].Visibility = Visibility.Visible;

                        labels[1, 1].Content = "ＡＴＣ常用";
                        ChangeDisplay(rectangles[1, 1], labels[1, 1], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[23]));
                        rectangles[1, 1].Visibility = Visibility.Visible;
                        labels[1, 1].Visibility = Visibility.Visible;

                        labels[1, 2].Content = "TASC制御";
                        ChangeDisplay(rectangles[1, 2], labels[1, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[138]));
                        rectangles[1, 2].Visibility = Visibility.Visible;
                        labels[1, 2].Visibility = Visibility.Visible;

                        labels[1, 3].Content = "耐雪ﾌﾞﾚｰｷ";
                        ChangeDisplay(rectangles[1, 3], labels[1, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[176]));
                        rectangles[1, 3].Visibility = Visibility.Visible;
                        labels[1, 3].Visibility = Visibility.Visible;

                        labels[1, 4].Content = "回生開放";
                        //ChangeDisplay(rectangles[1, 4], labels[1, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[171]));	// 廃止
                        rectangles[1, 4].Visibility = Visibility.Visible;
                        labels[1, 4].Visibility = Visibility.Visible;

                        labels[1, 5].Content = "非常ﾌﾞﾚｰｷ";
                        ChangeDisplay(rectangles[1, 5], labels[1, 5], colorEmergency, (SharedFuncs.SML.PanelA[51] >= 8));
                        rectangles[1, 5].Visibility = Visibility.Visible;
                        labels[1, 5].Visibility = Visibility.Visible;

                        labels[1, 6].Content = "制御開放";
                        rectangles[1, 6].Visibility = Visibility.Visible;
                        labels[1, 6].Visibility = Visibility.Visible;

                        labels[1, 7].Content = "";
                        rectangles[1, 7].Visibility = Visibility.Hidden;
                        labels[1, 7].Visibility = Visibility.Hidden;

                        labels[1, 8].Content = "情報伝送故障";
                        labels[1, 8].FontSize = 40;
                        rectangles[1, 8].Visibility = Visibility.Visible;
                        labels[1, 8].Visibility = Visibility.Visible;

                        labels[1, 9].Content = "ＴＩＳ試験";
                        rectangles[1, 9].Visibility = Visibility.Visible;
                        labels[1, 9].Visibility = Visibility.Visible;

                        labels[1, 10].Content = "";
                        rectangles[1, 10].Visibility = Visibility.Hidden;
                        labels[1, 10].Visibility = Visibility.Hidden;

                        labels[1, 11].Content = "";
                        rectangles[1, 11].Visibility = Visibility.Hidden;
                        labels[1, 11].Visibility = Visibility.Hidden;

                        labels[2, 0].Content = "ﾎｰﾑﾄﾞｱ連動";
                        labels[2, 0].FontSize = 40;
                        ChangeDisplay(rectangles[2, 0], labels[2, 0], colorNotice, (SharedFuncs.SML.PanelA[155] == 1));
                        rectangles[2, 0].Visibility = Visibility.Visible;
                        labels[2, 0].Visibility = Visibility.Visible;

                        labels[2, 1].Content = "ﾎｰﾑﾄﾞｱ非連動";
                        labels[2, 1].FontSize = 40;
                        ChangeDisplay(rectangles[2, 1], labels[2, 1], colorBrake, (SharedFuncs.SML.PanelA[155] == 2));
                        rectangles[2, 1].Visibility = Visibility.Visible;
                        labels[2, 1].Visibility = Visibility.Visible;

                        labels[2, 2].Content = "";
                        rectangles[2, 2].Visibility = Visibility.Hidden;
                        labels[2, 2].Visibility = Visibility.Hidden;

                        labels[2, 3].Content = "ﾎｰﾑﾄﾞｱ支障";
                        rectangles[2, 3].Visibility = Visibility.Visible;
                        labels[2, 3].Visibility = Visibility.Visible;

                        labels[2, 4].Content = "";
                        rectangles[2, 4].Visibility = Visibility.Hidden;
                        labels[2, 4].Visibility = Visibility.Hidden;

                        labels[2, 5].Content = "起動試験";
                        rectangles[2, 5].Visibility = Visibility.Visible;
                        labels[2, 5].Visibility = Visibility.Visible;

                        labels[2, 6].Content = "";
                        rectangles[2, 6].Visibility = Visibility.Hidden;
                        labels[2, 6].Visibility = Visibility.Hidden;

                        labels[2, 7].Content = "";
                        rectangles[2, 7].Visibility = Visibility.Hidden;
                        labels[2, 7].Visibility = Visibility.Hidden;

                        labels[2, 8].Content = "";
                        rectangles[2, 8].Visibility = Visibility.Hidden;
                        labels[2, 8].Visibility = Visibility.Hidden;

                        labels[2, 9].Content = "";
                        rectangles[2, 9].Visibility = Visibility.Hidden;
                        labels[2, 9].Visibility = Visibility.Hidden;

                        labels[2, 10].Content = "";
                        rectangles[2, 10].Visibility = Visibility.Hidden;
                        labels[2, 10].Visibility = Visibility.Hidden;

                        labels[2, 11].Content = "";
                        rectangles[2, 11].Visibility = Visibility.Hidden;
                        labels[2, 11].Visibility = Visibility.Hidden;
                        break;

                    /// SEB
                    case 4:
                        comp.Content = "西武";
                        comp.Visibility = Visibility.Visible;

                        labels[0, 0].Content = "ＡＴＳ正常";
                        ChangeDisplay(rectangles[0, 0], labels[0, 0], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[46]));
                        rectangles[0, 0].Visibility = Visibility.Visible;
                        labels[0, 0].Visibility = Visibility.Visible;

                        labels[0, 1].Content = "動　作";
                        ChangeDisplay(rectangles[0, 1], labels[0, 1], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[47]));
                        rectangles[0, 1].Visibility = Visibility.Visible;
                        labels[0, 1].Visibility = Visibility.Visible;

                        labels[0, 2].Content = "停　車";
                        ChangeDisplay(rectangles[0, 2], labels[0, 2], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[253]));
                        rectangles[0, 2].Visibility = Visibility.Visible;
                        labels[0, 2].Visibility = Visibility.Visible;

                        labels[0, 3].Content = "故　障";
                        ChangeDisplay(rectangles[0, 3], labels[0, 3], colorEmergency, false);
                        rectangles[0, 3].Visibility = Visibility.Visible;
                        labels[0, 3].Visibility = Visibility.Visible;

                        labels[0, 4].Content = "速　制";
                        ChangeDisplay(rectangles[0, 4], labels[0, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[49]));
                        rectangles[0, 4].Visibility = Visibility.Visible;
                        labels[0, 4].Visibility = Visibility.Visible;

                        labels[0, 5].Content = "確　認";
                        ChangeDisplay(rectangles[0, 5], labels[0, 5], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[48]));
                        rectangles[0, 5].Visibility = Visibility.Visible;
                        labels[0, 5].Visibility = Visibility.Visible;

                        labels[0, 6].Content = "";
                        rectangles[0, 6].Visibility = Visibility.Hidden;
                        labels[0, 6].Visibility = Visibility.Hidden;

                        labels[0, 7].Content = "圧着ﾌﾞﾚｰｷ";
                        ChangeDisplay(rectangles[0, 7], labels[0, 7], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[176]));
                        rectangles[0, 7].Visibility = Visibility.Visible;
                        labels[0, 7].Visibility = Visibility.Visible;

                        labels[0, 8].Content = "回生解放";
                        //ChangeDisplay(rectangles[0, 8], labels[0, 8], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[171]));  // 廃止
                        rectangles[0, 8].Visibility = Visibility.Visible;
                        labels[0, 8].Visibility = Visibility.Visible;

                        labels[0, 9].Content = "";
                        rectangles[0, 9].Visibility = Visibility.Hidden;
                        labels[0, 9].Visibility = Visibility.Hidden;

                        labels[0, 10].Content = "";
                        rectangles[0, 10].Visibility = Visibility.Hidden;
                        labels[0, 10].Visibility = Visibility.Hidden;

                        labels[0, 11].Content = "";
                        rectangles[0, 11].Visibility = Visibility.Hidden;
                        labels[0, 11].Visibility = Visibility.Hidden;

                        labels[1, 0].Content = "ＡＴＣ常用";
                        ChangeDisplay(rectangles[1, 0], labels[1, 0], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[26]));
                        rectangles[1, 0].Visibility = Visibility.Visible;
                        labels[1, 0].Visibility = Visibility.Visible;

                        labels[1, 1].Content = "ＡＴＣ非常";
                        ChangeDisplay(rectangles[1, 1], labels[1, 1], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[25]));
                        rectangles[1, 1].Visibility = Visibility.Visible;
                        labels[1, 1].Visibility = Visibility.Visible;

                        labels[1, 2].Content = "非常運転";
                        rectangles[1, 2].Visibility = Visibility.Visible;
                        labels[1, 2].Visibility = Visibility.Visible;

                        labels[1, 3].Content = "ＡＴＣ";
                        ChangeDisplay(rectangles[1, 3], labels[1, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[20]));
                        rectangles[1, 3].Visibility = Visibility.Visible;
                        labels[1, 3].Visibility = Visibility.Visible;

                        labels[1, 4].Content = "構　内";
                        ChangeDisplay(rectangles[1, 4], labels[1, 4], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[33]));
                        rectangles[1, 4].Visibility = Visibility.Visible;
                        labels[1, 4].Visibility = Visibility.Visible;

                        labels[1, 5].Content = "非　設";
                        ChangeDisplay(rectangles[1, 5], labels[1, 5], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[30]));
                        rectangles[1, 5].Visibility = Visibility.Visible;
                        labels[1, 5].Visibility = Visibility.Visible;

                        labels[1, 6].Content = "";
                        rectangles[1, 6].Visibility = Visibility.Hidden;
                        labels[1, 6].Visibility = Visibility.Hidden;

                        labels[1, 7].Content = "ＴＡＳＣ";
                        ChangeDisplay(rectangles[1, 7], labels[1, 7], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[136]));
                        rectangles[1, 7].Visibility = Visibility.Visible;
                        labels[1, 7].Visibility = Visibility.Visible;

                        labels[1, 8].Content = "ＴＡＳＣ制御";
                        ChangeDisplay(rectangles[1, 8], labels[1, 8], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[138]));
                        labels[1, 8].FontSize = 40;
                        rectangles[1, 8].Visibility = Visibility.Visible;
                        labels[1, 8].Visibility = Visibility.Visible;

                        labels[1, 9].Content = "";
                        rectangles[1, 9].Visibility = Visibility.Hidden;
                        labels[1, 9].Visibility = Visibility.Hidden;

                        labels[1, 10].Content = "";
                        rectangles[1, 10].Visibility = Visibility.Hidden;
                        labels[1, 10].Visibility = Visibility.Hidden;

                        labels[1, 11].Content = "";
                        rectangles[1, 11].Visibility = Visibility.Hidden;
                        labels[1, 11].Visibility = Visibility.Hidden;

                        labels[2, 0].Content = "ﾎｰﾑﾄﾞｱ連動";
                        labels[2, 0].FontSize = 40;
                        ChangeDisplay(rectangles[2, 0], labels[2, 0], colorNotice, (SharedFuncs.SML.PanelA[155] == 1));
                        rectangles[2, 0].Visibility = Visibility.Visible;
                        labels[2, 0].Visibility = Visibility.Visible;

                        labels[2, 1].Content = "ﾎｰﾑﾄﾞｱ非連動";
                        labels[2, 1].FontSize = 40;
                        ChangeDisplay(rectangles[2, 1], labels[2, 1], colorBrake, (SharedFuncs.SML.PanelA[155] == 2));
                        rectangles[2, 1].Visibility = Visibility.Visible;
                        labels[2, 1].Visibility = Visibility.Visible;

                        labels[2, 2].Content = "ｽﾃｯﾌﾟ支障";
                        rectangles[2, 2].Visibility = Visibility.Visible;
                        labels[2, 2].Visibility = Visibility.Visible;

                        labels[2, 3].Content = "ﾎｰﾑﾄﾞｱ支障";
                        rectangles[2, 3].Visibility = Visibility.Visible;
                        labels[2, 3].Visibility = Visibility.Visible;

                        labels[2, 4].Content = "";
                        rectangles[2, 4].Visibility = Visibility.Hidden;
                        labels[2, 4].Visibility = Visibility.Hidden;

                        labels[2, 5].Content = "起動試験";
                        rectangles[2, 5].Visibility = Visibility.Visible;
                        labels[2, 5].Visibility = Visibility.Visible;

                        labels[2, 6].Content = "";
                        rectangles[2, 6].Visibility = Visibility.Hidden;
                        labels[2, 6].Visibility = Visibility.Hidden;

                        labels[2, 7].Content = "";
                        rectangles[2, 7].Visibility = Visibility.Hidden;
                        labels[2, 7].Visibility = Visibility.Hidden;

                        labels[2, 8].Content = "";
                        rectangles[2, 8].Visibility = Visibility.Hidden;
                        labels[2, 8].Visibility = Visibility.Hidden;

                        labels[2, 9].Content = "";
                        rectangles[2, 9].Visibility = Visibility.Hidden;
                        labels[2, 9].Visibility = Visibility.Hidden;

                        labels[2, 10].Content = "";
                        rectangles[2, 10].Visibility = Visibility.Hidden;
                        labels[2, 10].Visibility = Visibility.Hidden;

                        labels[2, 11].Content = "";
                        rectangles[2, 11].Visibility = Visibility.Hidden;
                        labels[2, 11].Visibility = Visibility.Hidden;
                        break;

                    /// SOT
                    case 5:
                        comp.Content = "相鉄";
                        comp.Visibility = Visibility.Visible;
                        break;

                    /// SOT
                    case 6:
                        comp.Content = "JR";
                        comp.Visibility = Visibility.Visible;
                        break;

                    /// SOT
                    case 7:
                        comp.Content = "小田急";
                        comp.Visibility = Visibility.Visible;
                        break;

                    /// TOY
                    case 8:
                        comp.Content = "東葉";
                        comp.Visibility = Visibility.Visible;
                        break;
                }
            }
            else
            {
                comp.Content = "";
                comp.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// FD表示
        /// </summary>
        private void DispFormDoor()
        {
            Rectangle rectangle = FD_r;
            Label label = FD_l;
            if (SharedFuncs.SML.PanelA[155] == 0)   // 非連動判定
            {
                rectangle.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Hidden;
                return;
            }
            if (SharedFuncs.SML.PanelA[192] == 2)   // ドア状態
            {
                rectangle.Stroke = new SolidColorBrush(Colors.White);
                label.Foreground = new SolidColorBrush(Colors.White);
                label.Background = new SolidColorBrush(Colors.Transparent);
                rectangle.Visibility = Visibility.Visible;
                label.Visibility = Visibility.Visible;
                return;
            }
            switch (SharedFuncs.SML.PanelA[181])    // 定位
            {
                case 0:
                    rectangle.Stroke = new SolidColorBrush(Colors.Transparent);
                    label.Foreground = new SolidColorBrush(Colors.Black);
                    label.Background = new SolidColorBrush(colorNotice);
                    rectangle.Visibility = Visibility.Hidden;
                    label.Visibility = Visibility.Visible;
                    break;
                case 1:
                    rectangle.Stroke = new SolidColorBrush(colorNotice);
                    label.Foreground = new SolidColorBrush(colorNotice);
                    label.Background = new SolidColorBrush(Colors.Transparent);
                    rectangle.Visibility = Visibility.Visible;
                    label.Visibility = Visibility.Visible;
                    break;
                default:
                    rectangle.Visibility = Visibility.Hidden;
                    label.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="rectangle">変更する長方形</param>
        /// <param name="label">変更する文字</param>
        /// <param name="color">塗りつぶし色</param>
        /// <param name="status">状態</param>
        private void ChangeDisplay(Rectangle rectangle, Label label, Color color, bool status = false)
        {
            if (status)
            {
                rectangle.Fill = new SolidColorBrush(color);
                rectangle.Stroke = new SolidColorBrush(Colors.Transparent);
                if (color != colorEmergency) label.Foreground = new SolidColorBrush(Colors.Black);
                else label.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                rectangle.Fill = new SolidColorBrush(Colors.Transparent);
                rectangle.Stroke = new SolidColorBrush(Colors.White);
                label.Foreground = new SolidColorBrush(Colors.White);
            }
        }

    }
}
