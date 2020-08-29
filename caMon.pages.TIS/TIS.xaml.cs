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
			rectangles= new Rectangle[3, 12]
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
		}

		bool BIDSSMemIsEnabled = false;
		float SpeedAbsVal = float.NaN;
		float BCPresVal = float.NaN;
		float MRPresVal = float.NaN;
		int BNumVal = -1;
		int BMaxVal = -1;
		int EBPosVal = -1;
		int ATSCheckBPosVal = -1;
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

			SpeedAbsVal = Math.Abs(e.NewValue.StateData.V);
			BCPresVal = e.NewValue.StateData.BC;
			MRPresVal = e.NewValue.StateData.MR;

			//bNotch = e.NewValue.HandleData.B;
			//pNotch = e.NewValue.HandleData.P;

			BNumVal = e.NewValue.HandleData.B;
			BMaxVal = e.NewValue.SpecData.B;
			EBPosVal = BMaxVal + 1;

			ATSCheckBPosVal = e.NewValue.SpecData.A;


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
				SpeedAbsVal = 0;
				BCPresVal = 0;
				MRPresVal = 700;
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
		static readonly Color colorNotice = Colors.Lime;
		/// <summary>有効表示で使用する色</summary>
		static readonly Color colorActive = Colors.Aquamarine;

		/// <summary>
		/// ノッチ表示更新
		/// </summary>
		private void DispNotches()
		{
			if(KeyPosition == 0)
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
						break;
					/// TOB
					case 2:
						comp.Content = "東武";
						comp.Visibility = Visibility.Visible;
						break;
					/// TKK
					case 3:
						comp.Content = "東急";
						//comp.Content = "東急・横高";
						comp.Visibility = Visibility.Visible;

						labels[0, 0].Content = "非常運転";
						rectangles[0, 0].Visibility = Visibility.Visible;
						labels[0, 0].Visibility = Visibility.Visible;

						labels[0, 3].Content = "ATC";
						ChangeDisplay(rectangles[0, 3], labels[0, 3], colorBrake, Convert.ToBoolean(SharedFuncs.SML.PanelA[21]));
						rectangles[0, 3].Visibility = Visibility.Visible;
						labels[0, 3].Visibility = Visibility.Visible;
						break;
					/// SEB
					case 4:
						comp.Content = "西武";
						comp.Visibility = Visibility.Visible;
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
				label.Foreground = new SolidColorBrush(Colors.Black);
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
