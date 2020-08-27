using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using TR;

namespace caMon.pages.TIS
{
    /// <summary>
    /// TIS.xaml の相互作用ロジック
    /// </summary>
    public partial class TIS : Page
	{
		caMonIF camonIF;
		public TIS(caMonIF arg_camonIF)
		{
			InitializeComponent();

			camonIF = arg_camonIF;
		}

		private void NextP(object sender, RoutedEventArgs e) => camonIF.BackToHomeDo();
	}
}
