using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Shapes;

using TR;

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

			bNotch = e.NewValue.HandleData.B;
			pNotch = e.NewValue.HandleData.P;

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

		/// <summary>EBノッチ表示で使用する色</summary>
		static readonly Color EBBase_on = Colors.Red;
		/// <summary>Bノッチ表示で使用する色</summary>
		static readonly Color BBase_on = Colors.Orange;
		/// <summary>Nノッチ表示で使用する色</summary>
		static readonly Color NBase_on = Colors.Lime;
		/// <summary>Pノッチ表示で使用する色</summary>
		static readonly Color PBase_on = Colors.Aquamarine;

		/// <summary>
		/// ノッチ表示更新
		/// </summary>
		private void DispNotches()
		{
			if(KeyPosition == 0)
			{
				ChangeNotches(R_EB, L_EB, Colors.Transparent);
				ChangeNotches(R_B7, L_B7, Colors.Transparent);
				ChangeNotches(R_B6, L_B6, Colors.Transparent);
				ChangeNotches(R_B5, L_B5, Colors.Transparent);
				ChangeNotches(R_B4, L_B4, Colors.Transparent);
				ChangeNotches(R_B3, L_B3, Colors.Transparent);
				ChangeNotches(R_B2, L_B2, Colors.Transparent);
				ChangeNotches(R_B1, L_B1, Colors.Transparent);
				ChangeNotches(R_N0, L_N0, Colors.Transparent);
				ChangeNotches(R_P1, L_P1, Colors.Transparent);
				ChangeNotches(R_P2, L_P2, Colors.Transparent);
				ChangeNotches(R_P3, L_P3, Colors.Transparent);
				ChangeNotches(R_P4, L_P4, Colors.Transparent);
				return;
			}

			if (bNotch >= 8) ChangeNotches(R_EB, L_EB, EBBase_on, true);
			else ChangeNotches(R_EB, L_EB, Colors.Transparent);

			if (bNotch >= 7) ChangeNotches(R_B7, L_B7, BBase_on, true);
			else ChangeNotches(R_B7, L_B7, Colors.Transparent);
			if (bNotch >= 6) ChangeNotches(R_B6, L_B6, BBase_on, true);
			else ChangeNotches(R_B6, L_B6, Colors.Transparent);
			if (bNotch >= 5) ChangeNotches(R_B5, L_B5, BBase_on, true);
			else ChangeNotches(R_B5, L_B5, Colors.Transparent);
			if (bNotch >= 4) ChangeNotches(R_B4, L_B4, BBase_on, true);
			else ChangeNotches(R_B4, L_B4, Colors.Transparent);
			if (bNotch >= 3) ChangeNotches(R_B3, L_B3, BBase_on, true);
			else ChangeNotches(R_B3, L_B3, Colors.Transparent);
			if (bNotch >= 2) ChangeNotches(R_B2, L_B2, BBase_on, true);
			else ChangeNotches(R_B2, L_B2, Colors.Transparent);
			if (bNotch >= 1) ChangeNotches(R_B1, L_B1, BBase_on, true);
			else ChangeNotches(R_B1, L_B1, Colors.Transparent);

			ChangeNotches(R_N0, L_N0, NBase_on, true);

			if (pNotch >= 1) ChangeNotches(R_P1, L_P1, PBase_on, true);
			else ChangeNotches(R_P1, L_P1, Colors.Transparent);
			if (pNotch >= 2) ChangeNotches(R_P2, L_P2, PBase_on, true);
			else ChangeNotches(R_P2, L_P2, Colors.Transparent);
			if (pNotch >= 3) ChangeNotches(R_P3, L_P3, PBase_on, true);
			else ChangeNotches(R_P3, L_P3, Colors.Transparent);
			if (pNotch >= 4) ChangeNotches(R_P4, L_P4, PBase_on, true);
			else ChangeNotches(R_P4, L_P4, Colors.Transparent);
		}

		/// <summary>
		/// ノッチ表示更新
		/// </summary>
		/// <param name="rectangle">変更する長方形</param>
		/// <param name="label">変更する文字</param>
		/// <param name="color">塗りつぶし色</param>
		/// <param name="status">状態</param>
		private void ChangeNotches(Rectangle rectangle, Label label, Color color, bool status = false)
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
