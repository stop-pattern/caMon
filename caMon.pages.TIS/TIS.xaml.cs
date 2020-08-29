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

		/// <summary> 
		/// SMLに更新があったときに呼ばれる関数
		/// </summary>
		private void SMemLib_BIDSSMemChanged(object sender, ValueChangedEventArgs<BIDSSharedMemoryData> e)
		{
			BIDSSMemIsEnabled = e.NewValue.IsEnabled;

			SpeedAbsVal = Math.Abs(e.NewValue.StateData.V);
			BCPresVal = e.NewValue.StateData.BC;
			MRPresVal = e.NewValue.StateData.MR;

			BNumVal = e.NewValue.HandleData.B;
			BMaxVal = e.NewValue.SpecData.B;
			EBPosVal = BMaxVal + 1;

			ATSCheckBPosVal = e.NewValue.SpecData.A;


			TimeVal = e.NewValue.StateData.T;
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

			DispNotches((int)SpeedAbsVal);
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
		static readonly Color NBase_on = Colors.LimeGreen;
		/// <summary>Pノッチ表示で使用する色</summary>
		static readonly Color PBase_on = Colors.Aquamarine;

		/// <summary>
		/// ノッチ表示更新
		/// </summary>
		/// <param name="num">ノッチ段数</param>
		private void DispNotches(int num)
		{
			if (num == 0)
			{
				ChangeNotches(R_N0, L_N0, NBase_on, true);
			}
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
